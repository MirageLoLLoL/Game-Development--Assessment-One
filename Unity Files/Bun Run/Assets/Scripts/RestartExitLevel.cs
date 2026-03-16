using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
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
        SceneManager.LoadScene("MainMenu");
    }
}
