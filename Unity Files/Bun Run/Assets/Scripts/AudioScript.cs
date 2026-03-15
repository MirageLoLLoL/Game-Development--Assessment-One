using UnityEngine;
using UnityEngine.Audio;

public class AudioScript : MonoBehaviour
{
    public AudioSource sound;
    public AudioResource[] footsteps;
    public AudioResource[] jump;
    // Update is called once per frame
    private void Awake()
    {
        sound = GetComponent<AudioSource>();
    }
    public void PlayFootstep()
    {
        AudioResource chosenSound = footsteps[Random.Range(0, footsteps.Length)];
        sound.resource = chosenSound;
        sound.Play();
    }
    public void PlayJump()
    {
        AudioResource chosenSound = jump[Random.Range(0, jump.Length)];
        sound.resource = chosenSound;
        sound.Play();
    }
}
