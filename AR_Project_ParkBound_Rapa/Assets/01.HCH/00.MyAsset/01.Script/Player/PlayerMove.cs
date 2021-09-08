using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
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
                if (jumpClickTime > 0 && jumpClickTime <= 0.08f)
                {
                    yVelocity = minJump * Time.deltaTime;
                }
                else if (jumpClickTime > 0.08f && jumpClickTime <= 0.275f)
                {
                    yVelocity = midJump * Time.deltaTime;
                    
                }
                else if (jumpClickTime > 0.275f && jumpClickTime <= 0.45f)
                {
                    yVelocity = maxJump * Time.deltaTime;
                    anim.SetTrigger("JumpEnd");
                }
            }
            if (Input.GetButtonUp("Jump") || UISystem.Instance.jumpType == UISystem.JumpType.JumpExit)
            {
                isJump = false;
                print(isJump);
                if(yVelocity > 0)
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
        Vector3 heading = Camera.main.transform.localRotation * Vector3.forward;

        //ī�޶� �ü����� �ٲ��� �ʵ��� y���� 0���� �����.
        heading.y = 0;
        heading = heading.normalized;

        dir = heading * ControllerSystem.Instance.vertical_InputDirection.y * playerSpeed * Time.deltaTime;
        dir += Quaternion.Euler(0, 90, 0) * heading * Time.deltaTime * ControllerSystem.Instance.horizontal_InputDirection.x * playerSpeed;

        float jy = ControllerSystem.Instance.vertical_InputDirection.y * 2;
        float jx = ControllerSystem.Instance.horizontal_InputDirection.x * 2;

        //���̽�ƽ ���⿡ ���� ĳ������ Rotation ���� �ٸ��� �Ѵ�.
        //���: Rotation ��, ȸ�� �ӵ�, ���̽�ƽ ���� ��
        //1. ���� ���̽�ƽ ������ ������ ���Ѵٸ�,


        anim.SetFloat("Horizontal", jx);
        anim.SetFloat("Vertical", jy);

        if (yVelocity > maxJump)
        {
            yVelocity = maxJump;
        }

        dir.y = yVelocity;
        cc.Move(dir * transform.lossyScale.x);
    }
}

