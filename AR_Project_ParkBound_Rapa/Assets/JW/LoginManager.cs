using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LoginManager : MonoBehaviourPunCallbacks
{
    public InputField nickNameField;
    public string clientVersion = "1.0.0";


    void Start()
    {

    }

    public void ConnectServer()
    {
        if (nickNameField.text.Length < 1)
        {
            return;
        }

        // 접속 닉네임(아이디)
        PhotonNetwork.NickName = nickNameField.text;

        // 클라이언트 버전
        PhotonNetwork.GameVersion = clientVersion;

        // 씬을 다른 플레이어와 자동으로 동기화하도록 설정한다.
        PhotonNetwork.AutomaticallySyncScene = true;

        // 포톤 서버 셋팅에 작성한대로 접속을 시도한다.
        PhotonNetwork.ConnectUsingSettings();
    }

    // 네임 서버에 접속했을 때 실행되는 함수
    public override void OnConnected()
    {
        print("네임 서버에 접속했습니다.");
    }

    // 마스터 서버에 접속했을 때 실행되는 함수(로비로 들어가려는 준비 단계)
    public override void OnConnectedToMaster()
    {
        print("마스터 서버(kr)에 접속했습니다.");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    // 요청한 로비에 접속했을 때 실행되는 함수
    public override void OnJoinedLobby()
    {
        // 로비 씬으로 전환한다.
        PhotonNetwork.LoadLevel("LobbyScene");
    }
}