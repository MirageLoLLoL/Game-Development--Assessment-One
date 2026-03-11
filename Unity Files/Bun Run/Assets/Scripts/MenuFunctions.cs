using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    bool switcher;
    public GameObject menuA;
    public GameObject menuB;
    public void SwitchMenu()
    {
        menuB.SetActive(true);
        menuA.SetActive(false);
    }
    public void PlayLevel()
    {
        SceneManager.LoadScene("TutorialLevel");
    }
    public void End()
    {
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
