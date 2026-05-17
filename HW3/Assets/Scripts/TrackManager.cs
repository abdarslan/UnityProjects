using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    [Header("Track Prefabs")]
    public TrackChunk startChunk;
    public TrackChunk[] randomChunks;

    [Header("Spawning Rules")]
    public Transform player;
    public int concurrentChunks = 3;
    public float chunkDropPerSpawn = 0.01f;

    // Active list — index 0 is oldest, last index is newest
    private List<TrackChunk> activeChunks = new List<TrackChunk>();
    private Transform lastExitPoint;

    // Guard: don't recycle until the player has genuinely passed the first chunk's exit.
    // Prevents immediate recycling in frame 1 when a curved chunk's exit faces back toward spawn.
    private bool _playerPassedFirstExit = false;

    [Header("Angle Limit")]
    [Tooltip("Maximum accumulated yaw (degrees) before straight/counter-curve chunks are preferred.")]
    public float maxAccumulatedAngle = 225f;
    // Yaw turn angle per prefab type, computed once at startup from entryPoint→exitPoint.
    private Dictionary<TrackChunk, float> _chunkAngles = new Dictionary<TrackChunk, float>();
    // Running sum of turn angles across all currently active chunks.
    private float _accumulatedAngle = 0f;

    // Pool bins keyed by prefab type
    private Dictionary<TrackChunk, Queue<TrackChunk>> poolDictionary = new Dictionary<TrackChunk, Queue<TrackChunk>>();
    void Start()
    {
        if (startChunk == null)
        {
            Debug.LogError("TrackManager requires a startChunk assigned.");
            enabled = false;
            return;
        }

        if (randomChunks == null || randomChunks.Length == 0)
        {
            Debug.LogError("TrackManager requires at least one random chunk assigned.");
            enabled = false;
            return;
        }

        // Setup pool bins
        if (!poolDictionary.ContainsKey(startChunk))
            poolDictionary.Add(startChunk, new Queue<TrackChunk>());
        SeedPool(startChunk, 1);

        foreach (TrackChunk prefab in randomChunks)
        {
            if (!poolDictionary.ContainsKey(prefab))
                poolDictionary.Add(prefab, new Queue<TrackChunk>());
            int seedCount = Mathf.Max(1, Mathf.CeilToInt((concurrentChunks - 1f) / randomChunks.Length));
            SeedPool(prefab, seedCount);
        }

        // Pre-compute the yaw turn angle for every prefab once — just a SignedAngle call per type.
        _chunkAngles[startChunk] = Vector3.SignedAngle(
            startChunk.entryPoint.forward, startChunk.exitPoint.forward, Vector3.up);
        foreach (TrackChunk prefab in randomChunks)
            _chunkAngles[prefab] = Vector3.SignedAngle(
                prefab.entryPoint.forward, prefab.exitPoint.forward, Vector3.up);

        TrackChunk firstChunk = GetFromPool(startChunk, Vector3.zero, Quaternion.identity);
        if (firstChunk == null)
        {
            enabled = false;
            return;
        }
        activeChunks.Add(firstChunk);
        _accumulatedAngle += _chunkAngles[startChunk];
        lastExitPoint = firstChunk.exitPoint;
        // Fill remaining slots
        for (int i = 1; i < concurrentChunks; i++)
        {
            if (!SpawnNextChunk())
            {
                break;
            }
        }
    }

    void Update()
    {
        if (activeChunks.Count < concurrentChunks) return;

        // Wait until the player has actually driven past the oldest chunk's exit before
        // enabling recycling. This prevents a curved chunk at index 1 from making dot >= 0
        // in frame 1 and recycling the start chunk before the player moves.
        if (!_playerPassedFirstExit)
        {
            TrackChunk firstChunk = activeChunks[0];
            Vector3 toPlayer = player.position - firstChunk.exitPoint.position;
            if (Vector3.Dot(toPlayer, firstChunk.exitPoint.forward) < 0f)
                return;
            _playerPassedFirstExit = true;
        }

        // Recycle oldest chunk when car has passed the second-to-last chunk's exit.
        // This keeps the chunk the car just left alive until it exits the next one.
        TrackChunk triggerChunk = activeChunks[activeChunks.Count - concurrentChunks + 1];
        Vector3 exitToPlayer = player.position - triggerChunk.exitPoint.position;
        float dot = Vector3.Dot(exitToPlayer, triggerChunk.exitPoint.forward);

        if (dot >= 0f)
        {
            if (SpawnNextChunk())
            {
                RecycleOldestChunk();
            }
        }
    }

    private bool SpawnNextChunk()
    {
        // Walk from a random starting index and pick the first candidate whose addition
        // keeps |accumulatedAngle| within the limit. If every candidate would exceed it,
        // fall back to whichever adds the smallest absolute angle.
        int startIndex = Random.Range(0, randomChunks.Length);
        TrackChunk selectedPrefab = null;
        TrackChunk fallbackPrefab = randomChunks[startIndex];
        float fallbackBestAbs = float.MaxValue;

        for (int i = 0; i < randomChunks.Length; i++)
        {
            TrackChunk candidate = randomChunks[(startIndex + i) % randomChunks.Length];
            float projected = _accumulatedAngle + _chunkAngles[candidate];

            if (Mathf.Abs(projected) <= maxAccumulatedAngle)
            {
                selectedPrefab = candidate;
                break;
            }
            float absAngle = Mathf.Abs(_chunkAngles[candidate]);
            if (absAngle < fallbackBestAbs)
            {
                fallbackBestAbs = absAngle;
                fallbackPrefab = candidate;
            }
        }

        if (selectedPrefab == null)
            selectedPrefab = fallbackPrefab;

        Vector3 nextEntryPosition = lastExitPoint.position + Vector3.down * chunkDropPerSpawn;
        TrackChunk newChunk = GetFromPool(selectedPrefab, nextEntryPosition, lastExitPoint.rotation);
        if (newChunk == null)
            return false;
        
        _accumulatedAngle += _chunkAngles[newChunk.originalPrefab];
        activeChunks.Add(newChunk);
        lastExitPoint = newChunk.exitPoint;
        return true;
    }
    private void RecycleOldestChunk()
    {
        TrackChunk chunkToRecycle = activeChunks[0];
        _accumulatedAngle -= _chunkAngles[chunkToRecycle.originalPrefab];
        activeChunks.RemoveAt(0);
        ReturnToPool(chunkToRecycle);
    }

    // ==========================================
    //              POOLING LOGIC
    // ==========================================

    private TrackChunk GetFromPool( TrackChunk prefab, Vector3 targetEntryPosition, Quaternion spawnRotation)
    {
        if (!poolDictionary.TryGetValue(prefab, out Queue<TrackChunk> pool))
        {
            Debug.LogError($"No pool exists for chunk prefab {prefab.name}.");
            return null;
        }

        TrackChunk chunk = null;

        if (pool.Count > 0)
        {
            chunk = pool.Dequeue();
        }
        else
        {
            // If the requested type is empty, fall back to any available random chunk.
            int attempts = 0;
            do
            {
                int fallbackIndex = Random.Range(0, randomChunks.Length);
                TrackChunk fallbackPrefab = randomChunks[fallbackIndex];
                if (poolDictionary.TryGetValue(fallbackPrefab, out Queue<TrackChunk> fallbackPool) && fallbackPool.Count > 0)
                {
                    chunk = fallbackPool.Dequeue();
                    break;
                }

                attempts++;
            }
            while (attempts < randomChunks.Length * 2);

            if (chunk == null)
            {
                Debug.LogError($"No available chunks left to spawn for prefab {prefab.name}.");
                return null;
            }
        }

        // Position chunk so its entryPoint lands exactly at targetEntryPosition.
        // Set rotation first, then zero position so entryPoint.position reflects the rotated offset,
        // then shift the chunk so entryPoint ends up at the target.
        chunk.transform.rotation = spawnRotation;
        chunk.transform.position = Vector3.zero;
        chunk.transform.position = targetEntryPosition - chunk.entryPoint.position;
        chunk.gameObject.SetActive(true);

        return chunk;
    }
    private void SeedPool(TrackChunk prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            TrackChunk chunk = Instantiate(prefab);
            chunk.originalPrefab = prefab;
            chunk.gameObject.SetActive(false);
            poolDictionary[prefab].Enqueue(chunk);
        }
    }
    private void ReturnToPool(TrackChunk chunkToSleep)
    {
        chunkToSleep.gameObject.SetActive(false);
        poolDictionary[chunkToSleep.originalPrefab].Enqueue(chunkToSleep);
    }
}