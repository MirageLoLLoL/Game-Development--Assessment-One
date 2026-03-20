using UnityEngine;

public class Pause : MonoBehaviour
{
    bool paused = false;
    public GameObject pauseMenu;
    public GameObject settingsWindow;
    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 0f;
                paused = true;
                Cursor.visible = true;
            }
            pauseMenu.SetActive(false);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 1f; 
                paused = false;
                Cursor.visible = false;
            }
            pauseMenu.SetActive(true);
        }
    }
    public void OpenSettings()
    {
        settingsWindow.SetActive(true);
    }
    public void CloseSettings()
        { settingsWindow.SetActive(false); }
    public void ClosePauseWindow()
    {
        paused = false;
        Time.timeScale = 1f;
    }
}
