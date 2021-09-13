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

        // ���� �г���(���̵�)
        PhotonNetwork.NickName = nickNameField.text;

        // Ŭ���̾�Ʈ ����
        PhotonNetwork.GameVersion = clientVersion;

        // ���� �ٸ� �÷��̾�� �ڵ����� ����ȭ�ϵ��� �����Ѵ�.
        PhotonNetwork.AutomaticallySyncScene = true;

        // ���� ���� ���ÿ� �ۼ��Ѵ�� ������ �õ��Ѵ�.
        PhotonNetwork.ConnectUsingSettings();
    }

    // ���� ������ �������� �� ����Ǵ� �Լ�
    public override void OnConnected()
    {
        print("���� ������ �����߽��ϴ�.");
    }

    // ������ ������ �������� �� ����Ǵ� �Լ�(�κ�� ������ �غ� �ܰ�)
    public override void OnConnectedToMaster()
    {
        print("������ ����(kr)�� �����߽��ϴ�.");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    // ��û�� �κ� �������� �� ����Ǵ� �Լ�
    public override void OnJoinedLobby()
    {
        // �κ� ������ ��ȯ�Ѵ�.
        PhotonNetwork.LoadLevel("LobbyScene");
    }
}