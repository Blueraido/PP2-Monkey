using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] int ySens;
    [SerializeField] int xSens;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;
    [SerializeField] float FOV;

    float rotX;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Camera.main.fieldOfView = FOV;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * ySens * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * xSens * Time.deltaTime;

        if (invertY)
        {
            rotX += mouseY;
        }
        else
        {
            rotX -= mouseY;
        }
        //clamp the rotX on the x axis

        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);

        //rotate the camera on the x-axis
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        //rotate the player on the y-xis
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
