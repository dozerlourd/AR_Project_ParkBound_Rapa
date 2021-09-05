using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == 1 << 7)
        {
            col.transform.SetParent(gameObject.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }
}
