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
    
    public Transform mainPlane;
    void Start()
    {
        // Setup pool bins
        foreach (TrackChunk prefab in randomChunks)
        {
            if (!poolDictionary.ContainsKey(prefab))
                poolDictionary.Add(prefab, new Queue<TrackChunk>());
            SeedPool(prefab, 1);
        }
        if (!poolDictionary.ContainsKey(startChunk))
            poolDictionary.Add(startChunk, new Queue<TrackChunk>());

        TrackChunk firstChunk = GetFromPool(startChunk, Vector3.zero, Quaternion.identity);
        activeChunks.Add(firstChunk);
        lastExitPoint = firstChunk.exitPoint;
        // Fill remaining slots
        for (int i = 1; i < concurrentChunks; i++)
        {
            SpawnNextChunk();
        }
    }

    void Update()
    {
        if (activeChunks.Count < concurrentChunks) return;

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
        TrackChunk prefab = randomChunks[randomIndex];
        TrackChunk newChunk = GetFromPool(prefab, lastExitPoint.position, lastExitPoint.rotation);
        activeChunks.Add(newChunk);
        lastExitPoint = newChunk.exitPoint;

        // locate the main plane 0.1 units below the new chunks exit point
        if (mainPlane != null)
        {
            mainPlane.position = lastExitPoint.position + new Vector3(0, 0.4f, 0);
        }
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

    private TrackChunk GetFromPool( TrackChunk prefab, Vector3 targetEntryPosition, Quaternion spawnRotation)
    {
        TrackChunk chunk;
        // debug log the current count of each chunk type in the pool for debugging purposes

        if (poolDictionary[prefab].Count > 0)
        {
            chunk = poolDictionary[prefab].Dequeue();
        }
        else
        {
            // if there is no chunk with that type left get another one randomly from the pool
            int fallbackIndex = Random.Range(0, randomChunks.Length);
            while (poolDictionary[randomChunks[fallbackIndex]].Count == 0)
            {
                fallbackIndex = Random.Range(0, randomChunks.Length);
            }
            chunk = poolDictionary[randomChunks[fallbackIndex]].Dequeue();
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