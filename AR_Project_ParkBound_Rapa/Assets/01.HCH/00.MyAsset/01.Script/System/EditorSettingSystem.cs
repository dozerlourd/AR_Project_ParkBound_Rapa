using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorSettingSystem : SceneObject<EditorSettingSystem>
{
    #region 변수

    [SerializeField] GameObject[] ARObjects;
    [SerializeField] GameObject[] editorObjects;

    #endregion

    void Start()
    {
        #region 전처리기

//에디터에 따른 기본 설정 변경
#if UNITY_EDITOR
        foreach (GameObject obj in ARObjects) obj.SetActive(false);
        foreach (GameObject obj in editorObjects) obj.SetActive(true);
#elif UNITY_ANDROID
        foreach (GameObject obj in ARObjects) obj.SetActive(true);
        foreach (GameObject obj in editorObjects) obj.SetActive(false);
#endif

        #endregion
    }
}
