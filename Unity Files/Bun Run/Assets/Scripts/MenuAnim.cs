using UnityEngine;

public class MenuAnim : MonoBehaviour
{
    public Vector2 startPosition, goalPosition;
    public Vector2 currentPosition;
    public bool isHidden;
    public int moveSpeed;
    public void OpenUp()
    {
        isHidden =! isHidden;
    }
    private void FixedUpdate()
    {
        if (isHidden)
        {
            print("showing");
            currentPosition = Vector2.Lerp(currentPosition, goalPosition, Time.deltaTime * moveSpeed);
            transform.position = currentPosition;
        }
        else
        {
            print("hiding");
            currentPosition = Vector2.Lerp(currentPosition, startPosition, Time.deltaTime * moveSpeed);
            transform.position = currentPosition;
        }
    }
}
