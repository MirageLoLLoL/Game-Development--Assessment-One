using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartExitLevel : MonoBehaviour
{
    public GameObject buttons;
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TutorialLevel");
    }
    public void ExitLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
