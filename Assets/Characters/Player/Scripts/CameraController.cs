using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    public Transform target;
    public float Yaw { get; private set; }
    public float Pitch { get; private set; }
    public float mouseSensitivity = 3;
    public float distanceAway = 3f;
    public float smooth = 2f;
    public float camDepthSmooth = 2f;
    public void IniCamera(Transform target)
    {
        this.target = target;
        transform.position = target.position;
    }
    Vector3 disPos;

    void Start()
    {
        disPos = new Vector3(0, 4, -3);
    }

    void Update()
    {

        UpdateRotation();
        UpdatePosition();
    }

    //void LateUpdate()
    //{
    
    //    transform.position = Vector3.Lerp(transform.position, disPos, Time.deltaTime * smooth);
    //    transform.LookAt(target.position);
    //}
    private void UpdateRotation()
    {
        Yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        Pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        Pitch = Mathf.Clamp(Pitch,18, 80);
        transform.rotation = Quaternion.Euler(Pitch, Yaw, 0);
    }

    private void UpdatePosition()
    {
        // Get rotation based on Yaw and Pitch
        Quaternion rotation = Quaternion.Euler(Pitch, Yaw, 0);

        // Compute desired position using the rotation
        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * distanceAway);

        transform.position = desiredPosition;
        //transform.LookAt(target.position);
    }
}
