using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByVector : MonoBehaviour
{
    protected WaitForSeconds waitSeconds, startWaitSeconds;

    [SerializeField] protected float moveSpeed, delayTime, startDelayTime;
    protected float rate = 0f;

    [SerializeField] protected Vector3[] targetVectorArray;

    protected void Start()
    {
        startWaitSeconds = new WaitForSeconds(startDelayTime);
        waitSeconds = new WaitForSeconds(delayTime);

        CoroutineManager.Instance.StartCoroutine(MoveCycle(moveSpeed, targetVectorArray));
    }

    protected virtual IEnumerator Move(float _speed, Vector3 targetTr)
    {

        return null;
    }

    protected virtual IEnumerator MoveCycle(float _speed, Vector3[] targetTr)
    {
        return null;
    }
}
