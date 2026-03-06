using Unity.VisualScripting;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public Animator animator;
    public Movement movement;
    public OrbitCamera orbitCamera;
    public Rigidbody rb;
    public float speedMeasure;
    private bool falling;
    public Transform playerRotation;
    public LayerMask groundLayer;
    public bool outOfBounds;

    private void Update()
    {
        SpeedMeasure();
        AnimationController();
        Face();
    }

    void SpeedMeasure()
    {
        speedMeasure = rb.linearVelocity.magnitude;
        falling = rb.linearVelocity.y < 0;
    }

    void Face()
    {
        transform.rotation = orbitCamera.gravityAlignment;
        if (speedMeasure > 0.5f)
        {
            playerRotation.transform.forward = rb.linearVelocity;
            playerRotation.transform.localEulerAngles = new Vector3(0f, playerRotation.transform.localEulerAngles.y, 0f);
        }
    }
    void AnimationController()
    {
        if (movement.OnGround)
        {
            animator.SetBool("Grounded", true);
            float animatorSpeed = animator.GetFloat("Speed");
            switch (speedMeasure)
            {
                case float i when i > 0f && i <= 0.69f:
                    animator.SetBool("IsMoving", false);
                    break;
                case float i when i >= 0.1f && i <= 4.99f:
                    animator.SetBool("IsMoving", true);
                    animator.SetFloat("Speed", Mathf.Lerp(animatorSpeed, 0f, Time.deltaTime * 1.5f));
                    break;
                case float i when i >= 5f && i <= 17.99f:
                    animator.SetFloat("Speed", Mathf.Lerp(animatorSpeed, 0.5f, Time.deltaTime * 1.5f));
                    break;
                case float i when i >= 18f:
                    animator.SetFloat("Speed", Mathf.Lerp(animatorSpeed, 1f, Time.deltaTime * 1.5f));
                    break;
            }
        }
        else
        {
            animator.SetBool("Grounded", false);
        }
        if (movement.desiredJump)
        {
            animator.SetBool("Jump", true);
        }
        else
        {
            animator.SetBool("Jump", false);
        }
        if (movement.desiredJump)
        {
            animator.SetBool("Ascending", false);
            animator.SetBool("Descending", true);
        }
        else
        {
            animator.SetBool("Ascending", true);
            animator.SetBool("Descending", false);
        }
        if (outOfBounds)
        {
            animator.SetBool("OutOfBounds", true);
        }
        else
        {
            animator.SetBool("OutOfBounds", false);
        }
    }
}
