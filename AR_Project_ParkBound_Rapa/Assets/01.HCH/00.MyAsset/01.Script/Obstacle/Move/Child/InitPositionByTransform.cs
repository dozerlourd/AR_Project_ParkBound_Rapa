using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPositionByTransform : MoveByTransform
{
    [SerializeField] Transform initPos;

    new void Start()
    {
        startWaitSeconds = new WaitForSeconds(startDelayTime);
        waitSeconds = new WaitForSeconds(delayTime);

        //CoroutineManager.Instance.CoroutineDictionary.Add("MoveCoroutine", CoroutineManager.Instance.StartCoroutine(MoveCycle(moveSpeed, targetTrArray)));
        CoroutineManager.Instance.StartCoroutine(MoveCycle(moveSpeed, targetTrArray));
    }

    protected override IEnumerator Move(float _speed, Transform targetTr)
    {
        Vector3 dir = targetTr.position - transform.position;

        while (Vector3.Distance(targetTr.position, transform.position) >= 0.1f)
        {
            transform.position += dir.normalized * _speed * Time.deltaTime * transform.lossyScale.x;
            yield return null;
        }
    }

    protected override IEnumerator MoveCycle(float _speed, Transform[] targetTr)
    {
        yield return startWaitSeconds;

        while (true)
        {
            for (int i = 0; i < targetTr.Length; i++)
            {
                yield return StartCoroutine(Move(_speed, targetTr[i]));

                InitPos();
                yield return waitSeconds;
            }
        }
    }

    void InitPos()
    {
        transform.position = initPos.position;
        //print("ÃÊ±âÈ­");
    }
}
