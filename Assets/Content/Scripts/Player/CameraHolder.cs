using UnityEngine;

public class CameraHolder : MonoBehaviour
{
public Transform cameraPositon;
    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPositon.position;
    }
}
