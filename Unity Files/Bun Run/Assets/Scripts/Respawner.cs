using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        animator.outOfBounds = false;
    }
    private void OnTriggerExit(Collider other)
    {
        movement.isIn = false;
        animator.outOfBounds = true;
        if (movement.isIn == false)
        {
            StartCoroutine(DeathTimer());
        }
    }

    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(3f);
        if (respawnManager.checkpointCount == 0)
        {
            animator.rb.linearVelocity = Vector3.zero;
            movement.transform.position = startSpawn.transform.position;
        }
        else
        {
            animator.rb.linearVelocity = Vector3.zero;
            movement.transform.position = respawnManager.storedLocation;
        }
    }
}
