using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviourPun, IPunObservable
{

    public float playerSpeed = 5.0f;


    //점프 강도 적용 시간 
    public float jumpPower = 1f;

    //점프 강도 종류

    public float minJump = 1.0f;
    public float midJump = 2.0f;
    public float maxJump = 3.0f;

    //점프 시간 측정 지수
    float jumpClickTime;
    bool isJump;

    float ry;
    float jy;
    float jx;
    float rey;
    float rex;

    public float rotSpeed = 200;

    public Vector3 dir;
    [SerializeField] float gravity = -9.8f;
    public float yVelocity = 0;
    int jumpCount = 1;

    Animator anim;
    CharacterController cc;
    public Text myName;

    Vector3 otherPosition;
    Quaternion otherRotation;



    void Start()
    {
        if (photonView.IsMine == true)
        {
            myName.text = PhotonNetwork.NickName;
        }
        else
        {
            myName.text = photonView.Owner.NickName;
        }
        myName.color = photonView.IsMine ? Color.green : Color.yellow;
        //메인 카메라 on/off
        // myCam.SetActive(photonView.IsMine);

        anim = transform.GetChild(0).GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        UISystem.Instance.jumpType = UISystem.JumpType.Grounded;
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            //점프키를 눌렀을 때 점프력을 넣어준다.
            myName.transform.forward = Camera.main.transform.forward;

            if (jumpCount > 0)
            {

                if (Input.GetButtonDown("Jump") || UISystem.Instance.jumpType == UISystem.JumpType.JumpStart)
                {
                    jumpClickTime = 0;
                    isJump = true;
                    anim.SetTrigger("Jump");
                    jumpCount--;
                    UISystem.Instance.jumpType = UISystem.JumpType.JumpStay;
                }

                if (Input.GetButton("Jump") || UISystem.Instance.jumpType == UISystem.JumpType.JumpStay)
                {
                    //점프시간을 
                    //jumpClickTime += Time.deltaTime;
                    if (jumpClickTime > 0 && jumpClickTime <= 0.04f)
                    {
                        yVelocity = minJump * Time.deltaTime;
                    }
                    else if (jumpClickTime > 0.04f && jumpClickTime <= 0.2f)
                    {
                        yVelocity = midJump * Time.deltaTime;

                    }
                    else if (jumpClickTime > 0.2f && jumpClickTime <= 0.3f)
                    {
                        yVelocity = maxJump * Time.deltaTime;

                    }

                    else if (yVelocity > maxJump)
                    {
                        yVelocity = maxJump * Time.deltaTime;
                    }
                }
                if (Input.GetButtonUp("Jump") || UISystem.Instance.jumpType == UISystem.JumpType.JumpExit)
                {
                    isJump = false;
                    print(isJump);

                    if (yVelocity > 0)
                    {
                        yVelocity = 0;
                    }
                }
            }

            if (isJump)
            {
                jumpClickTime += Time.deltaTime;
            }
            else
            {
                anim.SetTrigger("JumpEnd");
                print(jumpClickTime);
                jumpClickTime = 0;
                UISystem.Instance.jumpType = UISystem.JumpType.Grounded;
            }

            //목표: 카메라를 보는 정면 방향을 기준으로 캐릭터의 이동키(W,A,S,D)를 설정한다. 
            yVelocity += gravity * Time.deltaTime;

            //캐릭터가 바닥에 있을 때는 중력 적용이 안되도록
            //if (cc.collisionFlags == CollisionFlags.Below)
            if (cc.isGrounded)
            {
                yVelocity = 0;

                jumpCount = 1;

            }

            //정면 벡터를 설정한다.

            jy = ControllerSystem.Instance.vertical_InputDirection.y * 2;
            jx = ControllerSystem.Instance.horizontal_InputDirection.x * 2;

            Vector3 dir = new Vector3(jx, 0, jy);

            dir.Normalize();

            dir = Camera.main.transform.TransformDirection(dir);

            //조이스틱 방향에 따라 캐릭터의 Rotation 값을 다르게 한다.
            //요소: Rotation 값, 회전 속도, 조이스틱 방향 값
            //1. 만약 조이스틱 방향이 왼쪽을 향한다면,


            anim.SetFloat("Horizontal", jx);
            anim.SetFloat("Vertical", jy);


            dir.y = yVelocity;
            cc.Move(dir * transform.lossyScale.x * Time.deltaTime * playerSpeed);
            Vector3 playerAngle = dir * 1 / Time.deltaTime;
            playerAngle.y = 0;
            if (jx != 0 || jy != 0)
            {
                transform.forward = playerAngle;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, otherPosition, Time.deltaTime * 50);
            transform.rotation = Quaternion.Lerp(transform.rotation, otherRotation, Time.deltaTime * 50);
            anim.SetFloat("Horizontal", rex);
            anim.SetFloat("Vertical", rey);
        }
    }

    // 인터페이스 추상 함수를 구현한다.
    // 변경이 있는 자신의 데이터를 서버에 전송하거나, 다른 클라이언트가 보낸 데이터를 서버로부터 받는 콜백 함수
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 데이터 전송
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(jx);
            stream.SendNext(jy);
        }
        // 데이터 수신
        else if (stream.IsReading)
        {
            otherPosition = (Vector3)stream.ReceiveNext();
            //print(photonView.Owner.NickName + ": " + otherPosition.ToString());
            otherRotation = (Quaternion)stream.ReceiveNext();
            rex = (float)stream.ReceiveNext();
            rey = (float)stream.ReceiveNext();

        }
    }



}

