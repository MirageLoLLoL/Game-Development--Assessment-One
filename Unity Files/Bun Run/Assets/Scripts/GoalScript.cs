using UnityEngine;

public class GoalScript : MonoBehaviour
{
    public Movement movement;
    public ProgressionTimer timer;
    private void OnTriggerEnter(Collider other)
    {
        movement.isDone = true;
        timer.isOver = true;
    }
}
