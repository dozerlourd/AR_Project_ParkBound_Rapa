using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      Vector3 dir =  Camera.main.transform.TransformDirection(Vector3.right);

        transform.position += dir * Time.deltaTime;
    }
}
