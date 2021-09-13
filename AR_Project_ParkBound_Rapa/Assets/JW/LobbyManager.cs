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

    // 방장이 되어서 방을 만들고 입장하는 기능
    public void CreatMyRoom()
    {
        // 방제가 없거나, 방 입장 인원수 값이 0인 경우에는 함수를 종료한다.
        if (roomNameField.text.Length < 1 || int.Parse(roomPlayerCount.text) < 1)
        {
            return;
        }

        // 방을 생성하기 위한 기본 정보를 설정한다.
        RoomOptions myRoom = new RoomOptions();
        myRoom.IsOpen = true;
        myRoom.IsVisible = true;
        myRoom.MaxPlayers = byte.Parse(roomPlayerCount.text);

        // 생성하려는 방의 설정대로 생성 또는 참가한다.
        PhotonNetwork.CreateRoom(roomNameField.text, myRoom);
        //PhotonNetwork.JoinOrCreateRoom(roomNameField.text, myRoom, TypedLobby.Default);
    }

    // 방이 성공적으로 생성되었을 때 실행되는 함수
    public override void OnCreatedRoom()
    {
        print("방 생성 및 입장 완료!");
        PhotonNetwork.LoadLevel("PlayScene");
    }

    // 방을 생성하는 것에 실패하였을 때 실행되는 함수
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        logText.text = string.Format("방 생성을 실패하였습니다! ({0:d}) {1:s}", (int)returnCode, message);
    }

    // 입력한 정보에 맞는 방에 입장하는 기능
    public void JoinSelectedRoom()
    {
        if (roomNameField.text.Length < 1 || int.Parse(roomPlayerCount.text) < 1)
        {
            return;
        }

        PhotonNetwork.JoinRoom(roomNameField.text);
    }

    // 방 입장에 성공했을 때
    public override void OnJoinedRoom()
    {
        print("방에 성공적으로 입장했습니다.");
        PhotonNetwork.LoadLevel("PlayScene");
    }

    // 방 입장에 실패했을 때
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        logText.text = string.Format("방 입장을 실패하였습니다! ({0:d}) {1:s}", (int)returnCode, message);
    }


    // 같은 로비에 있는 방 리스트가 변경될 때마다 실행되는 콜백 함수
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // 1. 현재 생성된 룸 정보 목록을 지운다.
        for (int i = 0; i < contentRoot.childCount; i++)
        {
            Destroy(contentRoot.GetChild(i).gameObject);
        }

        // 2. 전달받은 룸 리스트를 변수에 저장한다.
        for (int i = 0; i < roomList.Count; i++)
        {
            // 딕셔너리 변수에 같은 이름의 룸이 이미 있다면...
            if (allRooms.ContainsKey(roomList[i].Name))
            {
                // 그 룸이 서버에서 제거될 룸이라면 딕셔너리에서 제거한다.
                if (roomList[i].RemovedFromList)
                {
                    allRooms.Remove(roomList[i].Name);
                }
                else
                {
                    // 원래 있던 키의 값 부분을 갱신한다.
                    allRooms[roomList[i].Name] = roomList[i];
                }
            }
            // 그렇지 않다면, 딕셔너리에 추가한다.
            else
            {
                // 로비로 처음 왔을 때를 대비해서 제거 대상인 룸은 딕셔너리에 포함하지 않는다.
                if (!roomList[i].RemovedFromList)
                {
                    allRooms.Add(roomList[i].Name, roomList[i]);
                }
            }
        }

        // 3. 변수에 있는 값을 이용해서 룸 정보 목록을 새로 만든다.
        foreach (RoomInfo newRoom in allRooms.Values)
        {
            GameObject go = Instantiate(roomPrefab, contentRoot);

            go.GetComponentInChildren<Text>().text =
                $"{newRoom.Name}\t({newRoom.PlayerCount}/{newRoom.MaxPlayers})";

            // 생성된 프리팹에 함수를 바인딩한다.
            // 1. 프리팹에 있는 이벤트 트리거 컴포넌트를 가져온다.
            EventTrigger selectTrigger = go.GetComponent<EventTrigger>();

            // 2. 함수를 연결할 바인더를 생성하고 함수를 연결한다.
            EventTrigger.Entry myEntry = new EventTrigger.Entry();
            myEntry.callback.AddListener(SelectedRoomInfo);

            // 3. 엔트리 리스트에다가 위에서 만든 엔트리를 추가한다.
            selectTrigger.triggers.Add(myEntry);
        }
    }

    // 생성된 방을 마우스로 선택했을 때 실행될 함수
    public void SelectedRoomInfo(BaseEventData data)
    {
        // 방제 읽어오기
        string roomFullName = data.selectedObject.GetComponentInChildren<Text>().text;
        string[] partialName = roomFullName.Split('(');
        roomNameField.text = partialName[0];

        // 총 인원수 읽어오기
        string[] playerCounts = partialName[1].Split('/');
        string playerCount = playerCounts[1].Remove(playerCounts[1].Length - 1);
        roomPlayerCount.text = playerCount;
    }

}