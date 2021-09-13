using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    #region 변수

    Dictionary<string, Coroutine> coroutineDictionary = new Dictionary<string, Coroutine>();

    #endregion

    #region 속성

    public static CoroutineManager Instance { get; private set; }

    public Dictionary<string, Coroutine> CoroutineDictionary { get; private set; }

    #endregion

    #region 유니티 생명주기

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion
}
