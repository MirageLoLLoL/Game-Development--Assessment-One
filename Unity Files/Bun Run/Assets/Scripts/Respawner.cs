using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Respawner : MonoBehaviour
{
    public Movement movement;
    public CharacterAnimator animator;
    public Transform startSpawn;
    public OrbitCamera orbitCamera;
    public RespawnManager respawnManager;
    public Color blackout;
    bool frozen;

    private void OnTriggerEnter(Collider other)
    {
        movement.isIn = true;
        animator.outOfBounds = false;
        orbitCamera.outOfBounds = false;
    }
    private void OnTriggerExit(Collider other)
    {
        movement.isIn = false;
        animator.outOfBounds = true;
        orbitCamera.outOfBounds = true;
        if (movement.isIn == false)
        {
            StartCoroutine(DeathTimer());
        }
    }

    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(2f);
        yield return new WaitForFixedUpdate(); //Prevents respawn method from not working randomly
        if (!movement.isIn) //Checks after the wait to see if player is still outside of the boundaries and not just moving between them
        {
            animator.rb.linearVelocity = Vector3.zero;

            if (respawnManager.checkpointCount <= 0)
            {
                movement.transform.position = startSpawn.transform.position;
                animator.rb.transform.position = startSpawn.transform.position;
                orbitCamera.transform.position = startSpawn.transform.position;
                print("Respawning at start");
            }
            else
            {
                yield return new WaitForSeconds(1.5f);
                movement.transform.position = respawnManager.storedLocation;
                animator.rb.transform.position = respawnManager.storedLocation;
                orbitCamera.transform.position = respawnManager.storedLocation;
                print("Respawning at checkpoint");

            }
        }
    }
}
