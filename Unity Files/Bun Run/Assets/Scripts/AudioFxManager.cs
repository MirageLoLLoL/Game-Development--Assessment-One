using UnityEngine;

public class AudioFxManager : MonoBehaviour
{
    public static AudioFxManager Instance { get; private set; }

    [SerializeField] private AudioClip gravityChangeFX;
    private AudioSource audioSource;
    // public float gravityFXPitch = 1f;
    void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.7f;
    }

    public void PlayGravityChange()
    {
        if (gravityChangeFX && !audioSource.isPlaying)
        {
            // if (gravityFXPitch > 1f)
            // {
            //     audioSource.pitch = gravityFXPitch;
            // }
            
            audioSource.PlayOneShot(gravityChangeFX);
            // gravityFXPitch += 1.5f;
        }
    }
}
