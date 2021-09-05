using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObstacleByVector : MoveByVector
{
    protected override IEnumerator Move(float _speed, Vector3 targetTr)
    {
        
        Vector3 startPos = transform.position;

        while(rate <= 1)
        {
            rate += _speed * Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetTr, rate);
            yield return null;
        }
    }

    protected override IEnumerator MoveCycle(float _speed, Vector3[] targetTr)
    {
        yield return startWaitSeconds;

        while (true)
        {
            for (int i = 0; i < targetTr.Length; i++)
            {
                yield return StartCoroutine(Move(_speed, targetTr[i]));

                rate = 0;
                yield return waitSeconds;
            }
        }
    }
}
