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

    // Active list — index 0 is oldest, last index is newest
    private List<TrackChunk> activeChunks = new List<TrackChunk>();
    private Transform lastExitPoint;

    // Pool bins keyed by prefab type
    private Dictionary<TrackChunk, Queue<TrackChunk>> poolDictionary = new Dictionary<TrackChunk, Queue<TrackChunk>>();

    void Start()
    {
        // Setup pool bins
        foreach (TrackChunk prefab in randomChunks)
        {
            if (!poolDictionary.ContainsKey(prefab))
                poolDictionary.Add(prefab, new Queue<TrackChunk>());
        }
        if (!poolDictionary.ContainsKey(startChunk))
            poolDictionary.Add(startChunk, new Queue<TrackChunk>());

        // Spawn start chunk — entryPoint aligns to world origin so car at (0,1,0) lands on it
        TrackChunk firstChunk = GetFromPool(startChunk, Vector3.zero, Quaternion.identity);
        activeChunks.Add(firstChunk);
        lastExitPoint = firstChunk.exitPoint;

        Debug.Log($"Start: randomChunks count = {randomChunks.Length}, startChunk = {startChunk}, firstChunk entryPoint = {firstChunk.entryPoint}, firstChunk exitPoint = {firstChunk.exitPoint}");

        // Fill remaining slots
        for (int i = 1; i < concurrentChunks; i++)
        {
            Debug.Log($"Spawning chunk {i}...");
            SpawnNextChunk();
            Debug.Log($"Spawned chunk {i}, activeChunks count = {activeChunks.Count}");
        }
    }

    void Update()
    {
        if (activeChunks.Count < 2) return;

        // Recycle oldest chunk when car has passed the second-to-last chunk's exit.
        // This keeps the chunk the car just left alive until it exits the next one.
        TrackChunk triggerChunk = activeChunks[activeChunks.Count - 2];
        Vector3 exitToPlayer = player.position - triggerChunk.exitPoint.position;
        float dot = Vector3.Dot(exitToPlayer, triggerChunk.exitPoint.forward);

        if (dot >= 0f)
        {
            RecycleOldestChunk();
            SpawnNextChunk();
        }
    }

    private void SpawnNextChunk()
    {
        int randomIndex = Random.Range(0, randomChunks.Length);
        TrackChunk prefabToSpawn = randomChunks[randomIndex];
        TrackChunk newChunk = GetFromPool(prefabToSpawn, lastExitPoint.position, lastExitPoint.rotation);
        activeChunks.Add(newChunk);
        lastExitPoint = newChunk.exitPoint;
    }

    private void RecycleOldestChunk()
    {
        TrackChunk chunkToRecycle = activeChunks[0];
        activeChunks.RemoveAt(0);
        ReturnToPool(chunkToRecycle);
    }

    // ==========================================
    //              POOLING LOGIC
    // ==========================================

    private TrackChunk GetFromPool(TrackChunk prefabType, Vector3 targetEntryPosition, Quaternion spawnRotation)
    {
        TrackChunk chunk;

        if (poolDictionary[prefabType].Count > 0)
        {
            chunk = poolDictionary[prefabType].Dequeue();
        }
        else
        {
            chunk = Instantiate(prefabType);
            chunk.originalPrefab = prefabType;
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

    private void ReturnToPool(TrackChunk chunkToSleep)
    {
        chunkToSleep.gameObject.SetActive(false);
        poolDictionary[chunkToSleep.originalPrefab].Enqueue(chunkToSleep);
    }
}