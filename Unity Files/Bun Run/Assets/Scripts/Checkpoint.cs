using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public RespawnManager respawnManager;
    public GameObject self;
    private void Awake()
    {
        self = gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        respawnManager.storedLocation = transform.position;
        respawnManager.checkpointCount += 1;
        Destroy(self);
    }
}
