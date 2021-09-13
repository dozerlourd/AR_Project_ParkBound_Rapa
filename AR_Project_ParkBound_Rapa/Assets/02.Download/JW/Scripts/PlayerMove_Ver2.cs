using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove_Ver2 : MonoBehaviour
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
        //점프키를 눌렀을 때 점프력을 넣어준다.

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
        
        float jy = ControllerSystem.Instance.vertical_InputDirection.y * 2;
        float jx = ControllerSystem.Instance.horizontal_InputDirection.x * 2;

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
        transform.right = playerAngle;
    }
}

