using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public Text[] textUI;
    public InputField[] inputFieldUI;

    void Update() {
        textUI[0].text = PhotonNetwork.NetworkClientState.ToString();

        info();
    }


// PhotonNetwork class Reference: https://doc-api.photonengine.com/ko-kr/pun/current/class_photon_network.html


// 0. 서버 접속
    public void Connect() => PhotonNetwork.ConnectUsingSettings(); // 버튼에 연결되는 함수

    public override void OnConnectedToMaster() => print("서버 접속 완료"); // 조건이 되면 호출되는 함수
    

// 1. 로비 접속
    public void JoinLobby() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby() => print("로비 접속 완료");


// 2. 연결 끊기
    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause) => print("연결 끊김");


// 3. 방 만들기
    public void CreateRoom() {
        RoomOptions option = new RoomOptions();
        option.MaxPlayers = 20;
        // RoomOprions: https://doc-api.photonengine.com/ko-kr/pun/current/class_room_options.html

        PhotonNetwork.CreateRoom(inputFieldUI[1].text, option);
    }

    public override void OnCreatedRoom() {
        PhotonNetwork.LocalPlayer.NickName = inputFieldUI[0].text;

        print("방 만들기 완료");
     }

    public override void OnCreateRoomFailed(short returnCode, string message) => print("방 만들기 실패");


 // 4. 방 참가하기
    public void JoinRoom() => PhotonNetwork.JoinRoom(inputFieldUI[1].text);

    public override void OnJoinedRoom() {
        PhotonNetwork.LocalPlayer.NickName = inputFieldUI[0].text;

        print("방 참가 완료");
    }

    public override void OnJoinRoomFailed(short returnCode, string message) => print("방 참가 실패");


// 5. 방 참가/만들기: 입력받은 방 이름이 있으면 참가하고, 없으면 만든다.
    public void JoinOrCreateRoom() {
        RoomOptions option = new RoomOptions();
        option.MaxPlayers = 20;

        PhotonNetwork.JoinOrCreateRoom(inputFieldUI[1].text, option, null);
    }


// 6. 방 랜덤 입장
    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();


// 7. 방 떠나기
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();



// 상세 정보 가져오기
    void info() {
        string message = "";

        if(PhotonNetwork.InRoom) {
            message += "현재 " + PhotonNetwork.CurrentRoom.Name + "에 있습니다.\n";
            message += "현재 방 인원: " + PhotonNetwork.CurrentRoom.PlayerCount + "\n";

            string playerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)  { 
                playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            }

            message += playerStr + "\n";
        }
        else if(PhotonNetwork.InLobby) message += "현재 로비에 있습니다.\n";
        else if(PhotonNetwork.IsConnected) message += "현재 서버에 연결되어 있습니다.\n";

        textUI[1].text = message;
    }
}
