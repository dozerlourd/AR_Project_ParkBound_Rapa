using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


public class GameManager : MonoBehaviourPunCallbacks
{
   // private const Scenes playScene = Scenes.PlayScene;
    static float deltaTime;
    public float radius = 5.0f;
    //public GameObject Player;
    Vector3 location;
    //public enum Scenes
    //{
    //    GuidedScene = 0,
    //    LobbyScene,
    //    PlayScene = 2
    //}
   // public static Scenes CurrentScene = Scenes.GuidedScene;

    //protected new void Awake()
    //{
    //    base.Awake();
    //    deltaTime = Time.deltaTime;
    //}

     void Start()
    {
        // 서버와 소켓 통신을 위한 기본 설정을 한다.(전송률 설정)
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 30;

        // 플레이어를 생성한다.
        Vector2 randomPos = Random.insideUnitCircle * radius;
        location = new Vector3(randomPos.x, 1, randomPos.y);
        //Invoke("CreatingPlayer", 1.0f);
    }

    void Update()
    {

        // ESC 키를 누르면 서버와의 연결 강제 종료
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        PhotonNetwork.Disconnect();
    //    }

    //    switch (SceneManager.GetActiveScene().buildIndex)
    //    {
    //        case 0: GuidedSceneSetting(); break;

    //        case 1: LobbySceneSetting(); break;

    //        case 2: PlaySceneSetting(); break;

    //        default: break;
    //    }
    }

    public void RoomExit()
    {
        // 만일, 방장이었다면 방장 권한을 다른 유저에게 넘기고 간다.
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CountOfPlayersInRooms > 1)
        {
            PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerListOthers[0]);
        }

        // 현재 방을 나간다.
        PhotonNetwork.LeaveRoom();
    }

    public override void OnConnectedToMaster()
    {
        print("마스터 서버로 왔어욧");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }


    public override void OnJoinedLobby()
    {
        print("로비로 다시 왔어요");
        PhotonNetwork.LoadLevel("NetworkLobbyScene");
    }

    // 서버와의 연결이 끊어졌을 때의 콜백
    public override void OnDisconnected(DisconnectCause cause)
    {
        print("서버와의 연결이 끊어졌습니다. :" + cause.ToString());
        StartCoroutine(Kick());
    }

    IEnumerator Kick()
    {
        for (int i = 0; i < 3; i++)
        {
            print(string.Format("{0:d}초 뒤에 로그인 씬으로 전환됩니다.", 3 - i));
            yield return new WaitForSeconds(1);
        }

        PhotonNetwork.LoadLevel("LoginScene");
    }

    //public void CreatingPlayer()
    //{
    //    PhotonNetwork.Instantiate(player, location, Quaternion.identity);
    //}
    #region 가이드 씬

    //void GuidedSceneSetting() => CurrentScene = Scenes.GuidedScene;

    #endregion

    #region 로비 씬

    //void LobbySceneSetting() => CurrentScene = Scenes.LobbyScene;

    #endregion

    #region 플레이 씬

   // void PlaySceneSetting() => CurrentScene = playScene;

    #endregion

    #region 구현부

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
