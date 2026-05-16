using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackChunk : MonoBehaviour
{
    [Tooltip("Drag the EntryPoint child object here - at the start face of the track, +Z facing direction of travel")]
    public Transform entryPoint;

    [Tooltip("Drag the ExitPoint child object here - at the end face of the track, +Z facing direction of travel")]
    public Transform exitPoint;
    
    // Hidden because the TrackManager handles this automatically
    [HideInInspector] 
    public TrackChunk originalPrefab; 
}