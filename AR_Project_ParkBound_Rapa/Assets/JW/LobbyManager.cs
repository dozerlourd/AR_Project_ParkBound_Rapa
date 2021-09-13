using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField roomNameField;
    public InputField roomPlayerCount;
    public Text logText;
    public GameObject roomPrefab;
    public Transform contentRoot;

    Dictionary<string, RoomInfo> allRooms = new Dictionary<string, RoomInfo>();

    // ������ �Ǿ ���� ����� �����ϴ� ���
    public void CreatMyRoom()
    {
        // ������ ���ų�, �� ���� �ο��� ���� 0�� ��쿡�� �Լ��� �����Ѵ�.
        if (roomNameField.text.Length < 1 || int.Parse(roomPlayerCount.text) < 1)
        {
            return;
        }

        // ���� �����ϱ� ���� �⺻ ������ �����Ѵ�.
        RoomOptions myRoom = new RoomOptions();
        myRoom.IsOpen = true;
        myRoom.IsVisible = true;
        myRoom.MaxPlayers = byte.Parse(roomPlayerCount.text);

        // �����Ϸ��� ���� ������� ���� �Ǵ� �����Ѵ�.
        PhotonNetwork.CreateRoom(roomNameField.text, myRoom);
        //PhotonNetwork.JoinOrCreateRoom(roomNameField.text, myRoom, TypedLobby.Default);
    }

    // ���� ���������� �����Ǿ��� �� ����Ǵ� �Լ�
    public override void OnCreatedRoom()
    {
        print("�� ���� �� ���� �Ϸ�!");
        PhotonNetwork.LoadLevel("PlayScene");
    }

    // ���� �����ϴ� �Ϳ� �����Ͽ��� �� ����Ǵ� �Լ�
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        logText.text = string.Format("�� ������ �����Ͽ����ϴ�! ({0:d}) {1:s}", (int)returnCode, message);
    }

    // �Է��� ������ �´� �濡 �����ϴ� ���
    public void JoinSelectedRoom()
    {
        if (roomNameField.text.Length < 1 || int.Parse(roomPlayerCount.text) < 1)
        {
            return;
        }

        PhotonNetwork.JoinRoom(roomNameField.text);
    }

    // �� ���忡 �������� ��
    public override void OnJoinedRoom()
    {
        print("�濡 ���������� �����߽��ϴ�.");
        PhotonNetwork.LoadLevel("PlayScene");
    }

    // �� ���忡 �������� ��
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        logText.text = string.Format("�� ������ �����Ͽ����ϴ�! ({0:d}) {1:s}", (int)returnCode, message);
    }


    // ���� �κ� �ִ� �� ����Ʈ�� ����� ������ ����Ǵ� �ݹ� �Լ�
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // 1. ���� ������ �� ���� ����� �����.
        for (int i = 0; i < contentRoot.childCount; i++)
        {
            Destroy(contentRoot.GetChild(i).gameObject);
        }

        // 2. ���޹��� �� ����Ʈ�� ������ �����Ѵ�.
        for (int i = 0; i < roomList.Count; i++)
        {
            // ��ųʸ� ������ ���� �̸��� ���� �̹� �ִٸ�...
            if (allRooms.ContainsKey(roomList[i].Name))
            {
                // �� ���� �������� ���ŵ� ���̶�� ��ųʸ����� �����Ѵ�.
                if (roomList[i].RemovedFromList)
                {
                    allRooms.Remove(roomList[i].Name);
                }
                else
                {
                    // ���� �ִ� Ű�� �� �κ��� �����Ѵ�.
                    allRooms[roomList[i].Name] = roomList[i];
                }
            }
            // �׷��� �ʴٸ�, ��ųʸ��� �߰��Ѵ�.
            else
            {
                // �κ�� ó�� ���� ���� ����ؼ� ���� ����� ���� ��ųʸ��� �������� �ʴ´�.
                if (!roomList[i].RemovedFromList)
                {
                    allRooms.Add(roomList[i].Name, roomList[i]);
                }
            }
        }

        // 3. ������ �ִ� ���� �̿��ؼ� �� ���� ����� ���� �����.
        foreach (RoomInfo newRoom in allRooms.Values)
        {
            GameObject go = Instantiate(roomPrefab, contentRoot);

            go.GetComponentInChildren<Text>().text =
                $"{newRoom.Name}\t({newRoom.PlayerCount}/{newRoom.MaxPlayers})";

            // ������ �����տ� �Լ��� ���ε��Ѵ�.
            // 1. �����տ� �ִ� �̺�Ʈ Ʈ���� ������Ʈ�� �����´�.
            EventTrigger selectTrigger = go.GetComponent<EventTrigger>();

            // 2. �Լ��� ������ ���δ��� �����ϰ� �Լ��� �����Ѵ�.
            EventTrigger.Entry myEntry = new EventTrigger.Entry();
            myEntry.callback.AddListener(SelectedRoomInfo);

            // 3. ��Ʈ�� ����Ʈ���ٰ� ������ ���� ��Ʈ���� �߰��Ѵ�.
            selectTrigger.triggers.Add(myEntry);
        }
    }

    // ������ ���� ���콺�� �������� �� ����� �Լ�
    public void SelectedRoomInfo(BaseEventData data)
    {
        // ���� �о����
        string roomFullName = data.selectedObject.GetComponentInChildren<Text>().text;
        string[] partialName = roomFullName.Split('(');
        roomNameField.text = partialName[0];

        // �� �ο��� �о����
        string[] playerCounts = partialName[1].Split('/');
        string playerCount = playerCounts[1].Remove(playerCounts[1].Length - 1);
        roomPlayerCount.text = playerCount;
    }

}