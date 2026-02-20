using UnityEngine;
using UnityEngine.UIElements;

public class BallRoll : MonoBehaviour
{
    public Rigidbody ballRb;
    public Transform cameraBase;
    public LayerMask ground;

    private float horizontalInput, verticalInput;
    private float mouseHorizontal, mouseVertical, yaw;
    bool jump;
    [SerializeField]
    float jumpPower;
    public float speedH = 2.0f;
    public float turnSpeed = 5.0f;
    private Vector3 inputVector;

    public float acceleration, topSpeed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ballRb.transform.parent = null;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        GetInput();
        CamControls();
        transform.position = ballRb.transform.position;
    }

    void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        mouseHorizontal = Input.GetAxisRaw("Mouse X");
        jump = Input.GetKey(KeyCode.Space);

        //Movement Vector Normalization
        inputVector = Vector3.zero;
        inputVector += cameraBase.forward * verticalInput;
        inputVector += cameraBase.right * horizontalInput;
        inputVector.Normalize();
        inputVector *= acceleration;

        if (jump && Grounded())
        {
            ballRb.AddForce(Vector3.up * jumpPower);
        }
    }

    void CamControls()
    {
        yaw += speedH * mouseHorizontal * turnSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + yaw, transform.eulerAngles.z);
        if (mouseHorizontal == 0)
        {
            if (yaw > 0)
            {
                yaw -= Time.deltaTime;
            }

            if (yaw < 0)
            {
                yaw += Time.deltaTime;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //MovementInput
        if (verticalInput < 0 || verticalInput > 0 || horizontalInput < 0 || horizontalInput > 0)
        {
            ballRb.AddForce(inputVector);

            //Velocity Caching
            Vector3 cachedVelocity = ballRb.linearVelocity;
            float yVelocity = ballRb.linearVelocity.y;

            //Speed Cap, ignoring Y speed
            cachedVelocity.y = 0;
            cachedVelocity = Vector3.ClampMagnitude(cachedVelocity, topSpeed);
            cachedVelocity.y = yVelocity;

            ballRb.linearVelocity = cachedVelocity;
        }
    }

    bool Grounded()
    {
        RaycastHit groundLayer;
        return (Physics.Raycast(transform.position, -transform.up, out groundLayer, 1f, ground));
    }
}
