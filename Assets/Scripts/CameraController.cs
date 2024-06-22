using System.Collections;
using System.Collections.Generic;
// using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] int ySens;
    [SerializeField] int xSens;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;


    public Transform camOrientation;

    float rotX;
    float rotY;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

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
        rotY += mouseX;

        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);

        //rotate the camera on the x-axis
        transform.rotation = Quaternion.Euler(rotX, rotY, 0);

        //get camera orientation
        camOrientation.rotation = Quaternion.Euler(0, rotY, 0);

    }
}
