using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : RotateObstacle
{
    private WaitForSeconds waitSeconds, startWaitSeconds;

    private new void Start()
    {
        startWaitSeconds = new WaitForSeconds(startDelayTime);
        waitSeconds = new WaitForSeconds(delayTime);
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
            yield return StartCoroutine(Rotate(rotSpeed, limitAngle));
            yield return waitSeconds;
            ClampIncreaseZ();
            ChangeSpeed();
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
