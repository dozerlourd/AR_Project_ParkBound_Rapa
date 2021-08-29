using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : SingletonObject<GameManager>
{
    private const Scenes playScene = Scenes.PlayScene;
    static float deltaTime;
    public enum Scenes
    {
        GuidedScene = 0,
        LobbyScene,
        PlayScene = 2
    }
    public static Scenes CurrentScene = Scenes.GuidedScene;

    protected new void Awake()
    {
        base.Awake();
        deltaTime = Time.deltaTime;
    }

    void Update()
    {
        switch(SceneManager.GetActiveScene().buildIndex)
        {
            case 0: GuidedSceneSetting(); break;

            case 1: LobbySceneSetting(); break;

            case 2: PlaySceneSetting(); break;

            default: break;
        }
    }

    #region °¡ÀÌµå ¾À

    void GuidedSceneSetting() => CurrentScene = Scenes.GuidedScene;

    #endregion

    #region ·Îºñ ¾À

    void LobbySceneSetting() => CurrentScene = Scenes.LobbyScene;

    #endregion

    #region ÇÃ·¹ÀÌ ¾À

    void PlaySceneSetting() => CurrentScene = playScene;

    #endregion

    #region ±¸ÇöºÎ

    public static IEnumerator FadeIn(Image _img, float _speed)
    {
        float temp = 0;
        float tempSpeed = _speed * deltaTime;

        while (temp < 1)
        {
            temp += tempSpeed;
            _img.color = new Color(_img.color.r, _img.color.g, _img.color.b, temp);
            yield return null;
        }
    }

    public static IEnumerator FadeOut(Image _img, float _speed)
    {
        float temp = 0;
        float tempSpeed = _speed * deltaTime;

        while (temp < 1)
        {
            temp += tempSpeed;
            _img.color = new Color(_img.color.r, _img.color.g, _img.color.b, 1 - temp);
            yield return null;
        }
    }

    public static void ChangeScene(int changeSceneIndex)
    {
        SceneManager.LoadScene(changeSceneIndex);
    }

    public static void ChangeScene(string changeSceneName)
    {
        SceneManager.LoadScene(changeSceneName);
    }

    #endregion
}
