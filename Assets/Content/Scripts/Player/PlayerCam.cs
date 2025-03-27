using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sens;

    public Transform orientation;
    
    float xrotation;
    float yrotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sens;

        yrotation += mouseX;
        xrotation -= mouseY;
        xrotation = Mathf.Clamp(xrotation, -90f, 90f);

        //rotate camera
        transform.rotation = Quaternion.Euler(xrotation, yrotation, 0f);
        orientation.rotation = Quaternion.Euler(0f, yrotation, 0f);
    }
}
