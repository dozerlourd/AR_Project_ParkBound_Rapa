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
        // ������ ���� ����� ���� �⺻ ������ �Ѵ�.(���۷� ����)
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 30;

        // �÷��̾ �����Ѵ�.
        Vector2 randomPos = Random.insideUnitCircle * radius;
        location = new Vector3(randomPos.x, 1, randomPos.y);
        //Invoke("CreatingPlayer", 1.0f);
    }

    void Update()
    {

        // ESC Ű�� ������ �������� ���� ���� ����
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
        // ����, �����̾��ٸ� ���� ������ �ٸ� �������� �ѱ�� ����.
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CountOfPlayersInRooms > 1)
        {
            PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerListOthers[0]);
        }

        // ���� ���� ������.
        PhotonNetwork.LeaveRoom();
    }

    public override void OnConnectedToMaster()
    {
        print("������ ������ �Ծ��");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }


    public override void OnJoinedLobby()
    {
        print("�κ�� �ٽ� �Ծ��");
        PhotonNetwork.LoadLevel("NetworkLobbyScene");
    }

    // �������� ������ �������� ���� �ݹ�
    public override void OnDisconnected(DisconnectCause cause)
    {
        print("�������� ������ ���������ϴ�. :" + cause.ToString());
        StartCoroutine(Kick());
    }

    IEnumerator Kick()
    {
        for (int i = 0; i < 3; i++)
        {
            print(string.Format("{0:d}�� �ڿ� �α��� ������ ��ȯ�˴ϴ�.", 3 - i));
            yield return new WaitForSeconds(1);
        }

        PhotonNetwork.LoadLevel("LoginScene");
    }

    //public void CreatingPlayer()
    //{
    //    PhotonNetwork.Instantiate(player, location, Quaternion.identity);
    //}
    #region ���̵� ��

    //void GuidedSceneSetting() => CurrentScene = Scenes.GuidedScene;

    #endregion

    #region �κ� ��

    //void LobbySceneSetting() => CurrentScene = Scenes.LobbyScene;

    #endregion

    #region �÷��� ��

   // void PlaySceneSetting() => CurrentScene = playScene;

    #endregion

    #region ������

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
