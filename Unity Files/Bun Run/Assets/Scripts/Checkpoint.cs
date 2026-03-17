using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public RespawnManager respawnManager;
    public GameObject self;
    public GameObject particleParent;
    private void Awake()
    {
        self = gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        respawnManager.storedLocation = transform.position;
        respawnManager.checkpointCount += 1;
        
        ParticleSystem[] particles = particleParent.GetComponentsInChildren<ParticleSystem>();
        // detach particles 
        particleParent.transform.parent = null;
        
        foreach (ParticleSystem ps in particles)
        {
            ps.Play();
        }
        
        Destroy(particleParent, 5f);
        Destroy(self);
    }
}
