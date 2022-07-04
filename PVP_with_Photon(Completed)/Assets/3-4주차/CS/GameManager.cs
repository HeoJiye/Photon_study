using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using UnityEngine.UI;

// 에셋에서 Photon Pun2 Free 임포트
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks {
    public InputField nicknameInput;

    public GameObject[] view;

    public GameObject Player;
    
    void Awake() {
        ChangeView(0);
        PhotonNetwork.ConnectUsingSettings(); // 네트워크 연결
    }

    public void GameStart() {
        if(PhotonNetwork.InLobby) {
            ChangeView(2);

            RoomOptions option = new RoomOptions();
            option.MaxPlayers = 20;
            PhotonNetwork.JoinOrCreateRoom("ROOM", option, null);

            PhotonNetwork.LocalPlayer.NickName = nicknameInput.text;
         }
    }

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();
    public override void OnJoinedLobby() => ChangeView(1);

    public override void OnJoinedRoom() {
        Player = PhotonNetwork.Instantiate("Player", new Vector3(0f, 0f, 0f), Quaternion.identity);

        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        cp["HP"] = 6;
        cp["SCORE"] = 0;
        
        PhotonNetwork.LocalPlayer.SetCustomProperties(cp);
    }

    void ChangeView(int num) {
        for(int i = 0; i < 3; i++) {
            if(i == num) view[i].SetActive(true);
            else view[i].SetActive(false);
        }
    }
}
