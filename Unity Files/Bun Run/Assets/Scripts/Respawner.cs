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
    public FadeController fadeController;
    public Color blackout;
    public GameObject burnEffectPrefab;
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
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForFixedUpdate(); //Prevents respawn method from not working randomly
        if (!movement.isIn) //Checks after the wait to see if player is still outside of the boundaries and not just moving between them
        {
            yield return StartCoroutine(RespawnSequence());
        }
    }

    private IEnumerator RespawnSequence()
    {
        // add burning effect
        // Instantiate(burnEffectPrefab, movement.transform.position, Quaternion.identity);
        var burnEffect = Instantiate(burnEffectPrefab, movement.transform.position, Quaternion.identity);
        burnEffect.transform.SetParent(movement.transform);

        yield return new WaitForSeconds(3f);
        //fade to black
        yield return StartCoroutine(fadeController.Fade(0f, 1f, 1f));
        //stop momentum
        animator.rb.linearVelocity = Vector3.zero;
        //teleport player
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
        //destroy fire
        Destroy(burnEffect, 0f);
        // fade back from black
        yield return StartCoroutine(fadeController.Fade(1f, 0f, 1f));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(RespawnSequence());
        }
    }
}
