using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ChattingManager : MonoBehaviourPunCallbacks
{
    public PhotonView PV;

    public Text[] ChatText;
    public InputField ChatInput;
    
    public void Send() {
        PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + ChatInput.text);
        ChatInput.text = "";
    }

    public void KillMessage(string s, string o) {
        PV.RPC("ChatRPC", RpcTarget.All, "<color=red>" + s + "님이 " + o +"님을 죽였습니다.</color>");
    }

    public void GameStart() {
        ChatRPC("<color=yellow>" + PhotonNetwork.NickName + "님이 참가하셨습니다</color>");
    }
    public override void OnPlayerLeftRoom(Player otherPlayer) {
        ChatRPC("<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
    }

    [PunRPC]
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < ChatText.Length; i++)
            if (ChatText[i].text == "")
            {
                isInput = true;
                ChatText[i].text = msg;
                break;
            }
        if (!isInput) // 꽉차면 한칸씩 위로 올림
        {
            for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
            ChatText[ChatText.Length - 1].text = msg;
        }
    }
}
