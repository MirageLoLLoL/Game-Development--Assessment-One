using UnityEngine;

public class GravityZoneSwitcher : MonoBehaviour
{
    bool switcher;
    public GameObject[] zoneA;
    public GameObject[] zoneB;
    private void OnTriggerEnter(Collider other)
    {
        switcher =! switcher;
        if (switcher)
        {
            for(int i = 0; i < zoneA.Length; i++)
            {
                zoneA[i].SetActive(false);
            }
            for(int i = 0; i < zoneB.Length; i++)
            {
                zoneB[i].SetActive(true); 
            }
        }
        else
        {
            for (int i = 0;i < zoneA.Length; i++)
            {
                zoneA[i].SetActive(true);
            }
            for (int i = 0; i < zoneB.Length; i++)
            {
                zoneB[i].SetActive(false); 
            }
        }
    }
}
