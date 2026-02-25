using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform cameraController;
    public float speed = 6f;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Mouse X");
        float vertical = Input.GetAxisRaw("Mouse Y");
        Vector3 direction = new Vector3(0f, horizontal, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            cameraController.Rotate(direction * speed * Time.deltaTime);
        }
    }
}
