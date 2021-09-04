using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RotateObstacle : MonoBehaviour
{
    [SerializeField] protected float rotSpeed, limitAngle, delayTime, startDelayTime;
    protected float IncreaseValue;

    [SerializeField] protected Vector3 rotDir;

    protected void Start()
    {
        //CoroutineManager.Instance.CoroutineDictionary.Add("RotateCoroutine", CoroutineManager.Instance.StartCoroutine(RotateCycle()));
        CoroutineManager.Instance.StartCoroutine(RotateCycle());
    }

    protected virtual IEnumerator Rotate(float _speed, float _angle) { yield return null; }
    protected virtual IEnumerator Rotate(float _speed) { yield return null; }

    protected abstract IEnumerator RotateCycle();
}
