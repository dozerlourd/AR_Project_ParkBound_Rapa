using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByTransform : MonoBehaviour
{
    protected WaitForSeconds waitSeconds, startWaitSeconds;

    [SerializeField] protected float moveSpeed, delayTime, startDelayTime;
    protected float rate = 0f;

    [SerializeField] protected Transform[] targetTrArray;

    protected void Start()
    {
        startWaitSeconds = new WaitForSeconds(startDelayTime);
        waitSeconds = new WaitForSeconds(delayTime);

        CoroutineManager.Instance.StartCoroutine(MoveCycle(moveSpeed, targetTrArray));
    }

    protected virtual IEnumerator Move(float _speed, Transform targetTr)
    {
        return null;
    }

    protected virtual IEnumerator MoveCycle(float _speed, Transform[] targetTr)
    {
        return null;
    }
}
