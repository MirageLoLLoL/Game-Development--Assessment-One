using NUnit.Framework;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public Checkpoint checkpointScript;
    public GameObject[] checkpointObjects;
    public int checkpointCount;
    public Vector3 storedLocation;

    private void Awake()
    {
        if (checkpointScript != null)
        {
            checkpointScript = checkpointObjects[checkpointCount].GetComponent<Checkpoint>();
        }
        else return;
    }
}
