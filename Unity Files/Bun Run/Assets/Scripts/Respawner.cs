using UnityEngine;

public class Respawner : MonoBehaviour
{
    public Movement movement;
    public CharacterAnimator animator;
    public GameObject startSpawn;
    public RespawnManager respawnManager;

    private void OnTriggerEnter(Collider other)
    {
        movement.isIn = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (respawnManager.checkpointCount == 0)
        {
            movement.transform.position = startSpawn.transform.position;
            movement.isIn = false;
            animator.rb.linearVelocity = Vector3.zero;
        }
        else
        {
            movement.isIn = false;
            animator.rb.linearVelocity = Vector3.zero;
            movement.transform.position = respawnManager.storedLocation;
        }
    }
}
