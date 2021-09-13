using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogText : MonoBehaviour
{
    Text logText;

    void Start()
    {
        logText = GetComponent<Text>();

#if UNITY_EDITOR
        logText.text = $"유니티 에디터!";
#elif UNITY_ANDROID
        logText.text = $"안드로이드!";
#endif
    }
}
