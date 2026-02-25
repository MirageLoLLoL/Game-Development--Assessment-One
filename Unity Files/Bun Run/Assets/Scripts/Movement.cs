using UnityEngine;
using UnityEngine.UIElements;

public class BallRoll : MonoBehaviour
{
    public Rigidbody ballRb;
    public Transform cameraTransform;
    public LayerMask ground;
    private float horizontalInput, verticalInput;

    private void Start()
    {
        
    }
    private void Update()
    {
        GetInput();
        
    }

    void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
    
}
