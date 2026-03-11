using UnityEngine;

public class Stare : MonoBehaviour
{
    public GameObject player;
    void Update()
    {
        transform.LookAt(player.transform.position);
    }
}
