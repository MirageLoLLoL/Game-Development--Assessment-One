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
        //Replace with load back to menu when that's done
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#elif UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX
        Application.Quit();
#elif !UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX_ || UNITY_STANDALONE_OSX
        Application.Quit();
#endif
        Application.Quit();
    }
}
