using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbySceneSystem : SceneObject<LobbySceneSystem>
{
    [SerializeField] Image fadeImage;
    [SerializeField, Range(0.001f, 1)] float fadeSpeed;

    [SerializeField] GameObject lobbyUI;

    [SerializeField] string nextSceneName;

    [SerializeField] int nextSceneIndex;

    void Start()
    {
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
    lobbyUI.SetActive(false);
        StartCoroutine(LobbyFlow());
}

IEnumerator LobbyFlow()
{
    yield return new WaitUntil(() => GameManager.CurrentScene == GameManager.Scenes.LobbyScene);

    yield return new WaitForSeconds(1.0f);
    yield return StartCoroutine(GameManager.FadeOut(fadeImage, fadeSpeed));

    lobbyUI.SetActive(true);
}

public void OnClickPlayButton()
{
    GameManager.ChangeScene(nextSceneIndex);
}
}
