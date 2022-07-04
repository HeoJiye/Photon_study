using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class UIManager : MonoBehaviourPunCallbacks
{
    public Text text;

    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.InRoom) {
            Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
            text.text = "HP: " + cp["HP"] + " SCORE: " + cp["SCORE"];
        }
    }
}
