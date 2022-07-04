using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class PlayerMove : MonoBehaviourPunCallbacks, IPunObservable {
    PhotonView PV;

    Transform trans;
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer renderer;

    public Text NicknameView;

    public AudioManager am;

    public float movePower;
    public float jumpPower;
    public float maxSpeed;

    Vector3 curPos;

    void Awake() {
        trans = gameObject.GetComponent<Transform>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        renderer = gameObject.GetComponent<SpriteRenderer>();

        PV = gameObject.GetComponent<PhotonView>();

        am.sound[2].Play();

        if(PV.IsMine) { // 내가 소환했나?
            NicknameView.text = PhotonNetwork.LocalPlayer.NickName;
            NicknameView.color = Color.green;
        }
        else {
            NicknameView.text = PV.Owner.NickName; // PV.Owner => Player 객체를 반환
            NicknameView.color = Color.red;
        }
    }

    void Update() {
        if(PV.IsMine) {
            Move();
            Jump();
            Shot();

            // renderer.flipX 동기화
            PV.RPC("Sync_FlipX", RpcTarget.All, renderer.flipX);
        }
        // PV.IsMine == false
        else trans.position = curPos;
    }
    [PunRPC]
    void Sync_FlipX(bool flipX) {
        renderer.flipX = flipX;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(trans.position);
        }
        else {
            curPos = (Vector3)stream.ReceiveNext();
        }
    }
        // 총알 발사
    void Shot() {
        if(Input.GetKeyDown(KeyCode.X)) {
            am.sound[3].Play();

            // 총알 생성
            GameObject bullet =  PhotonNetwork.Instantiate("bullet", trans.position, Quaternion.identity);
            
            // 총알 위치 조정
            Rigidbody2D bullet_rigid = bullet.GetComponent<Rigidbody2D>();

            if(renderer.flipX) {
                bullet.transform.Translate(-0.8f, -0.15f, 0f);
                bullet_rigid.AddForce(new Vector3(-10f, 0f, 0f), ForceMode2D.Impulse);
            }
            else {
                bullet.transform.Translate(0.8f, -0.15f, 0f);
                bullet_rigid.AddForce(new Vector3(10f, 0f, 0f), ForceMode2D.Impulse);
            }
        }
    }


    // 플레이어 이동과 점프
    void Move() {
        float direction = Input.GetAxis("Horizontal");
        
        rigid.AddForce(Vector3.right * direction * movePower, ForceMode2D.Force);

        if(rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if(rigid.velocity.x < maxSpeed * (-1))
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        if(trans.position.y < -15f) {
            am.sound[2].Play();
            trans.position = new Vector3(0f, 0f, 0f);
        }

        if(direction != 0) {
            anim.SetBool("isWalking", true);

            if(direction > 0)
                renderer.flipX = false;
            else
                renderer.flipX = true;
        }
        else
            anim.SetBool("isWalking", false);
    }
    void Jump() {
        if(Input.GetKeyDown(KeyCode.Z)) {
            am.sound[4].Play();

            rigid.AddForce(Vector3.up * jumpPower, ForceMode2D.Impulse);
            anim.SetTrigger("Jumping");
        }
    }
}
