using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class BulletMove : MonoBehaviourPunCallbacks, IPunObservable {
    PhotonView PV;

    Transform trans;
    Vector3 curPos;

    void Awake() {
        PV = gameObject.GetComponent<PhotonView>();
        trans = gameObject.GetComponent<Transform>();
    }

    void Update() {
        if(!PV.IsMine) trans.position = curPos;
    }

    // 무언가랑 부딪히면 Destroy
    void OnCollisionEnter2D(Collision2D coll) {
        GameObject other = coll.gameObject;

        if(other.tag == "Player") {
            PhotonView playerPV = other.GetComponent<PhotonView>();

            // 내 총알이 다른 사람한테 맞았다!
            if(PV.IsMine && !playerPV.IsMine) {
                Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;

                int score = (int) cp["SCORE"];
                score += 10;
                cp["SCORE"] = score;

                PhotonNetwork.LocalPlayer.SetCustomProperties(cp);
            }

            // 다른 사람 총알이 나한테 맞았다!
            else if(!PV.IsMine && playerPV.IsMine) {
                Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;

                int hp = (int) cp["HP"];
                hp--;
                cp["HP"] = hp;

                PhotonNetwork.LocalPlayer.SetCustomProperties(cp);

                if(hp <= 0) {
                    Hashtable cp_other = PV.Owner.CustomProperties;

                    int score = (int) cp_other["SCORE"];
                    score += 100;
                    cp_other["SCORE"] = score;

                    PV.Owner.SetCustomProperties(cp_other);
                }
            }
        }
        if(PV.IsMine) PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void DestroyRPC() {
        Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(trans.position);
        }
        else {
            curPos = (Vector3)stream.ReceiveNext();
        }
    }
}
