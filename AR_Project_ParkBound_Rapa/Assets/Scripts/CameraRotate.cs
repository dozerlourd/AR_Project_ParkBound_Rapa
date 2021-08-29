using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float rotSpeed = 200;
    float mx = 0;

    void Update()
    {
        mx += Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(new Vector3(0, mx, 0));
    }
}
