using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviourPun, IPunObservable
{

    public float playerSpeed = 5.0f;


    //���� ���� ���� �ð� 
    public float jumpPower = 1f;

    //���� ���� ����

    public float minJump = 1.0f;
    public float midJump = 2.0f;
    public float maxJump = 3.0f;

    //���� �ð� ���� ����
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
        //���� ī�޶� on/off
        // myCam.SetActive(photonView.IsMine);

        anim = transform.GetChild(0).GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        UISystem.Instance.jumpType = UISystem.JumpType.Grounded;
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            //����Ű�� ������ �� �������� �־��ش�.
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
                    //�����ð��� 
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

            //��ǥ: ī�޶� ���� ���� ������ �������� ĳ������ �̵�Ű(W,A,S,D)�� �����Ѵ�. 
            yVelocity += gravity * Time.deltaTime;

            //ĳ���Ͱ� �ٴڿ� ���� ���� �߷� ������ �ȵǵ���
            //if (cc.collisionFlags == CollisionFlags.Below)
            if (cc.isGrounded)
            {
                yVelocity = 0;

                jumpCount = 1;

            }

            //���� ���͸� �����Ѵ�.

            jy = ControllerSystem.Instance.vertical_InputDirection.y * 2;
            jx = ControllerSystem.Instance.horizontal_InputDirection.x * 2;

            Vector3 dir = new Vector3(jx, 0, jy);

            dir.Normalize();

            dir = Camera.main.transform.TransformDirection(dir);

            //���̽�ƽ ���⿡ ���� ĳ������ Rotation ���� �ٸ��� �Ѵ�.
            //���: Rotation ��, ȸ�� �ӵ�, ���̽�ƽ ���� ��
            //1. ���� ���̽�ƽ ������ ������ ���Ѵٸ�,


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

    // �������̽� �߻� �Լ��� �����Ѵ�.
    // ������ �ִ� �ڽ��� �����͸� ������ �����ϰų�, �ٸ� Ŭ���̾�Ʈ�� ���� �����͸� �����κ��� �޴� �ݹ� �Լ�
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // ������ ����
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(jx);
            stream.SendNext(jy);
        }
        // ������ ����
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

