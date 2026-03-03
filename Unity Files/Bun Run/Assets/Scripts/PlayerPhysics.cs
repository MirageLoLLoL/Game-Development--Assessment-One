using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerPhysics : MonoBehaviour
{
    [Header("Objects")]
    public Rigidbody motorsphere;
    public Transform playerObj;
    public Transform charaObj;
    public Transform characterRotation;
    public Transform cameraRotation;
    public Animator characterAnimator;

    [Header("Input")]
    private float verticalInput, horiztontalInput;
    private bool jumpInput;

    [Header("Speed/AccelerationAttributes")]
    public int verticalAcceleration, horizontalAcceleration;
    public int jumpCount;

    [Header("States")]
    public bool isGrounded, falling;
    public float speedMeasure, ascentMeasure;

    [Header("Ground/WallCalculation")]
    public LayerMask groundLayer, gravityLayer;
    private float rotateSpeed = 250f;
    private float rotateStep;
    private Quaternion gravRotation, currentRotation;
    private ConstantForce gravity;
    public float gravityForce, jumpPower;

    private void Start()
    {
        motorsphere.transform.parent = null; //Removes the motorsphere from the Player heirarchy, allowing for the additions of forces without odd side effects
    }

    private void Update()
    {
        transform.position = motorsphere.transform.position; //Moves player to the rigidbody
        GetInput();

        if (jumpInput && isGrounded)
        {
            motorsphere.AddForce(Vector3.up * jumpPower);
        }

    }
    private void FixedUpdate()
    {
        GroundAngleCalc();
        Gravity();
        Movement();
        SpeedMeasure();
        AnimationController();
        if (isGrounded)
        {
            jumpCount = 2;
            motorsphere.linearDamping = 1;
        }
        else
        {
            motorsphere.linearDamping = 0;
        }
    }
    void GetInput()
    {
        verticalInput = Input.GetAxisRaw("Vertical");
        horiztontalInput = Input.GetAxis("Horizontal");
        jumpInput = Input.GetButton("Jump");
    }

    void Movement()
    {
        motorsphere.AddForce((cameraRotation.transform.forward * verticalAcceleration) * verticalInput);
        motorsphere.AddForce((cameraRotation.transform.right * horizontalAcceleration) * horiztontalInput);
        characterRotation.transform.forward = motorsphere.linearVelocity; //Faces character towards movement direction
    }
    void GroundAngleCalc()
    {
        //Raycast Ground Check
        RaycastHit ground;
        isGrounded = Physics.Raycast(transform.position, -transform.up, out ground, 0.75f, groundLayer);

        // Shift's player rotation to match the normals of gravity colliders
        RaycastHit gravAngle;
        Physics.Raycast(playerObj.transform.position, -transform.up, out gravAngle, Mathf.Infinity, gravityLayer);
        gravRotation = Quaternion.FromToRotation(playerObj.transform.up, gravAngle.normal) * playerObj.transform.rotation;
        rotateStep = rotateSpeed * Time.deltaTime;
        playerObj.transform.rotation = Quaternion.RotateTowards(playerObj.transform.rotation, gravRotation, rotateStep);

        if (isGrounded)
        {
            //Shifts the child player object to be oriented with the ground
            RaycastHit groundAngle;
            Physics.Raycast(transform.position, -transform.up, out groundAngle, 7.5f, groundLayer);
            currentRotation = Quaternion.FromToRotation(charaObj.transform.up, groundAngle.normal) * charaObj.transform.rotation;
            rotateStep = rotateSpeed * Time.deltaTime;
            charaObj.transform.rotation = Quaternion.RotateTowards(charaObj.transform.rotation, currentRotation, rotateStep);
        }
        else
        {
            charaObj.transform.rotation = Quaternion.RotateTowards(charaObj.transform.rotation, gravRotation, rotateStep);
        }
    }
    void Gravity()
    { 
        motorsphere.AddForce(-playerObj.up * gravityForce);
    }
    void SpeedMeasure()
    {
        speedMeasure = motorsphere.linearVelocity.magnitude;
        falling = motorsphere.linearVelocity.y < 0 ? true : false;  
    }
    void AnimationController()
    {
        if (isGrounded)
        {
            characterAnimator.SetBool("Grounded", true);

            switch (speedMeasure)
            {
                case float i when i > 0f && i <= 1.99f:
                    characterAnimator.SetBool("IsMoving", false);
                    break;
                case float i when i >= 2f && i <= 4.99f:
                    characterAnimator.SetBool("IsMoving", true);
                    characterAnimator.SetFloat("Speed", 0f);
                    break;
                case float i when i >= 5f && i <= 14.99f:
                    characterAnimator.SetFloat("Speed", 0.5f);
                    break;
                case float i when i >= 15f:
                    characterAnimator.SetFloat("Speed", 1f);
                    break;
            }
        }
        else
        {
            characterAnimator.SetBool("Grounded", false);
        }
        if(jumpInput)
        {
            characterAnimator.SetBool("Jump", true);
        }
        else
        {
            characterAnimator.SetBool("Jump", false);
        }
        if(falling)
        {
            characterAnimator.SetBool("Ascending", false);
            characterAnimator.SetBool("Descending", true);
        }
        else
        {
            characterAnimator.SetBool("Ascending", true);
            characterAnimator.SetBool("Descending", false);
        }

    }
}
