using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartExitLevel : MonoBehaviour
{
    public GameObject buttons;
    public void RestartLevel()
    {
        SceneManager.LoadScene("TutorialLevel");
    }
    public void ExitLevel()
    {
        //Replace with load back to menu when that's done
        SceneManager.LoadScene("MainMenu");
    }
}
