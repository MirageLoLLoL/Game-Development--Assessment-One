using UnityEngine;

public class GoalScript : MonoBehaviour
{
    public Movement movement;
    public ProgressionTimer timer;
    public RestartExitLevel exitLevel; 

    private void OnTriggerEnter(Collider other)
    {
        movement.isDone = true;
        timer.isOver = true;
        exitLevel.buttons.gameObject.SetActive(true);
        timer.SetBestScore();
        Cursor.visible = true;
    }
}
