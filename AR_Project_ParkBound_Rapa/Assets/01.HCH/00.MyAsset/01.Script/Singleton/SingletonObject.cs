using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonObject<T> : MonoBehaviour where T : SingletonObject<T>
{
    #region 변수

    static T instance;

    #endregion

    #region 공개속성

    public static T Instance => instance = instance ? instance : FindObjectOfType<T>() ?? new GameObject(typeof(T).Name).AddComponent<T>();

    #endregion

    #region 유니티 생명주기

    protected void Awake()
    {
        if (FindObjectsOfType<T>().Length != 1)
        {
            Destroy(gameObject);
        }
        else DontDestroyOnLoad(gameObject);
    }

    #endregion
}
