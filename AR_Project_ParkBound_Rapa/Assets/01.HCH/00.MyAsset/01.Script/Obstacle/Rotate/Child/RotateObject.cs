using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : RotateObstacle
{
    private WaitForSeconds waitSeconds1, waitSeconds2, startWaitSeconds;

    private new void Start()
    {
        startWaitSeconds = new WaitForSeconds(startDelayTime);
        waitSeconds1 = new WaitForSeconds(delayTime1);
        waitSeconds2 = new WaitForSeconds(delayTime2);
        base.Start();
    }

    protected override IEnumerator Rotate(float _speed, float _angle)
    {
        while (IncreaseValue < _angle && IncreaseValue >= 0)
        {
            IncreaseValue += _speed * Time.deltaTime;
            transform.eulerAngles += rotDir * rotSpeed * Time.deltaTime;
            yield return null;
        }
    }

    protected override IEnumerator RotateCycle()
    {
        yield return startWaitSeconds;

        while (true)
        {
            for (int i = 0; i < 2; i++)
            {
                yield return StartCoroutine(Rotate(rotSpeed, limitAngle));
                if(i == 0)
                {
                    yield return waitSeconds1;
                }
                else
                {
                    yield return waitSeconds2;
                }
                
                ClampIncreaseZ();
                ChangeSpeed();
            }
        }
    }

    void ChangeSpeed()
    {
        rotSpeed *= -1;
    }

    void ClampIncreaseZ()
    {
        IncreaseValue = Mathf.Clamp(IncreaseValue, 0, limitAngle - 0.0001f);
    }
}
