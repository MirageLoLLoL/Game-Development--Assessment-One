using UnityEngine;

public class MenuAnim : MonoBehaviour
{
    public Vector2 startPosition, goalPosition, currentPosition;
    public bool isHidden;
    public int move;
    public void OpenUp()
    {
        
        if (isHidden)
        {
            if (currentPosition != goalPosition)
            {
                move = 1;
            }
            else
            {
                isHidden = false;
                move = 0;
            }
        }
        else
        {
            if (currentPosition != startPosition)
            {
                move = 2;
            }
            else
            {
                isHidden = true;
                move = 0;
            }
        }
    }
    private void Update()
    {
        transform.position = currentPosition;
        switch (move)
        {
            case 0:
                {
                    return;
                }
            case 1:
                {
                    print("moving");
                    Vector2.Lerp(currentPosition, goalPosition, Time.deltaTime * 2);
                    return;
                }
            case 2:
                {
                    print("moving");
                    Vector2.Lerp(currentPosition, startPosition, Time.deltaTime * 2);
                    return;
                }
        }
    }
}
