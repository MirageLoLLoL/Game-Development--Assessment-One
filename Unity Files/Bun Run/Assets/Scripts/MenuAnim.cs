using UnityEngine;

public class MenuAnim : MonoBehaviour
{
    public Vector2 startPosition, goalPosition;

    public void OpenUp()
    {
        transform.position = Vector2.Slerp(transform.position, goalPosition);
    }

    public void CloseUp()
    {

    }
}
