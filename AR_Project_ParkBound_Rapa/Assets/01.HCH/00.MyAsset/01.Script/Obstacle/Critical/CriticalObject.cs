using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "T_Player")
        {
            StartCoroutine(PlayerSystem.Instance.PlayerDie());
        }
    }
}
