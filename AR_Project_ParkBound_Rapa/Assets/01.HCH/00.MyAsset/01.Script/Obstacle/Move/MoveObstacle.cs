using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    private WaitForSeconds waitSeconds, startWaitSeconds;

    [SerializeField] float moveSpeed, delayTime, startDelayTime;
    float rate = 0f;

    [SerializeField] Vector3[] targetTrArray;

    void Start()
    {
        startWaitSeconds = new WaitForSeconds(startDelayTime);
        waitSeconds = new WaitForSeconds(delayTime);

        //CoroutineManager.Instance.CoroutineDictionary.Add("MoveCoroutine", CoroutineManager.Instance.StartCoroutine(MoveCycle(moveSpeed, targetTrArray)));
        CoroutineManager.Instance.StartCoroutine(MoveCycle(moveSpeed, targetTrArray));
    }

    IEnumerator Move(float _speed, Vector3 targetTr)
    {
        
        Vector3 startPos = transform.position;

        while(rate <= 1)
        {
            rate += _speed * Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetTr, rate);
            yield return null;
        }
    }

    IEnumerator MoveCycle(float _speed, Vector3[] targetTr)
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
