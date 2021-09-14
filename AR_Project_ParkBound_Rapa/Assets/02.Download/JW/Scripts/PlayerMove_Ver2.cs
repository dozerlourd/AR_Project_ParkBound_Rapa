using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove_Ver2 : MonoBehaviour
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

    public float rotSpeed = 200;

    public Vector3 dir;
    [SerializeField] float gravity = -9.8f;
    public float yVelocity = 0;
    int jumpCount = 1;

    Animator anim;
    CharacterController cc;

    [SerializeField] Transform platformTr;

    void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        UISystem.Instance.jumpType = UISystem.JumpType.Grounded;
    }

    void Update()
    {
        //����Ű�� ������ �� �������� �־��ش�.

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
                    yVelocity = minJump;
                }
                else if (jumpClickTime > 0.04f && jumpClickTime <= 0.2f)
                {
                    yVelocity = midJump;
                    
                }
                else if (jumpClickTime > 0.2f && jumpClickTime <= 0.3f)
                {
                    yVelocity = maxJump;
                    
                }

                else if (yVelocity > maxJump)
                {
                    yVelocity = maxJump;
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
        
        float jy = ControllerSystem.Instance.vertical_InputDirection.y * 2;
        float jx = ControllerSystem.Instance.horizontal_InputDirection.x * 2;

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
        transform.right = playerAngle;
    }
}

