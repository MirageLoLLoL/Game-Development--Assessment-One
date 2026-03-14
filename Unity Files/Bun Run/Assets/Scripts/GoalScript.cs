using UnityEngine;

public class GoalScript : MonoBehaviour
{
    public Movement movement;
    public ProgressionTimer timer;
    public RestartExitLevel retryScript;
    private void OnTriggerEnter(Collider other)
    {
        movement.isDone = true;
        timer.isOver = true;
        retryScript.buttons.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
