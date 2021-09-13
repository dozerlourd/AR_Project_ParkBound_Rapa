using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    #region ����

    Dictionary<string, Coroutine> coroutineDictionary = new Dictionary<string, Coroutine>();

    #endregion

    #region �Ӽ�

    public static CoroutineManager Instance { get; private set; }

    public Dictionary<string, Coroutine> CoroutineDictionary { get; private set; }

    #endregion

    #region ����Ƽ �����ֱ�

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
