using UnityEngine;

public class MenuAnim : MonoBehaviour
{
    public Vector2 startPosition, goalPosition, currentPosition;
    public bool isHidden;
    public int moveSpeed;
    public void OpenUp()
    {
        isHidden = !isHidden;
    }
    private void Update()
    {
        currentPosition = transform.position;
        if (!isHidden)
        {
            print("Showing level select");
            transform.position = Vector2.Lerp(currentPosition, goalPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            print("Hiding level select");
            transform.position = Vector2.Lerp(currentPosition, startPosition, moveSpeed * Time.deltaTime);
        }
    }
        
}
