using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonstopRotateObstacle : RotateObstacle
{
    protected override IEnumerator Rotate(float _speed)
    {
        while(true)
        {
            IncreaseValue += _speed * Time.deltaTime;
            transform.eulerAngles += rotDir * rotSpeed * Time.deltaTime;
            yield return null;
        }
    }

    protected override IEnumerator RotateCycle()
    {
        while(true)
        {
            yield return StartCoroutine(Rotate(rotSpeed));
            yield return null;
        }
    }
}
