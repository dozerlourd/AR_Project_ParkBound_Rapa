using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
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

    //캐릭터 회전 변수
    //float ry;

    public float rotSpeed = 200;

    Vector3 dir;
    [SerializeField] float gravity = -9.8f;
    float yVelocity = 0;
    int jumpCount = 1;
    CharacterController cc;
    Animator anim;
    int jumpHash = 0;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        jumpHash = Animator.StringToHash("Base Layer.JumpStart");

        UISystem.Instance.jumpType = UISystem.JumpType.Grounded;
    }

    // Update is called once per frame
    void Update()
    {
      
        //점프키를 눌렀을 때 점프력을 넣어준다.

        if (jumpCount > 0)
        {

            if (Input.GetButtonDown("Jump") || UISystem.Instance.jumpType == UISystem.JumpType.JumpStart)
            {
                print("점프키 "+ UISystem.Instance.jumpType);
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
                else if (jumpClickTime > 0.3f && jumpClickTime <= 1.0f)
                {
                   
                    //anim.SetTrigger("JumpEnd");
                }

            }
            if (Input.GetButtonUp("Jump") || UISystem.Instance.jumpType == UISystem.JumpType.JumpExit)
            {
                anim.SetTrigger("JumpEnd");
              
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
        Vector3 heading = Camera.main.transform.localRotation * Vector3.forward;

        //카메라 시선에도 바뀌지 않도록 y값은 0으로 만든다.
        heading.y = 0;
        heading = heading.normalized;

    //if(ControllerSystem.Instance.horizontal_InputDirection.x !=0)
    //    {
    //        transform.Rotate(0, -rotSpeed * Time.deltaTime, 0);
    //    }

        dir = heading * ControllerSystem.Instance.vertical_InputDirection.y * playerSpeed * Time.deltaTime;
        dir += Quaternion.Euler(0, 90, 0) * heading * Time.deltaTime * ControllerSystem.Instance.horizontal_InputDirection.x * playerSpeed;

        //ry += rotSpeed * ControllerSystem.Instance.horizontal_InputDirection.y * Time.deltaTime;
        //transform.eulerAngles = new Vector3(0, ry, 0);
  
        float jy = ControllerSystem.Instance.vertical_InputDirection.y * 2;
        float jx = ControllerSystem.Instance.horizontal_InputDirection.x * 2;

        //조이스틱 방향에 따라 캐릭터의 Rotation 값을 다르게 한다.
        //요소: Rotation 값, 회전 속도, 조이스틱 방향 값
        //1. 만약 조이스틱 방향이 왼쪽을 향한다면,


        anim.SetFloat("Horizontal", jx);
        anim.SetFloat("Vertical", jy);


        if (yVelocity > maxJump)
        {
            yVelocity = maxJump;
        }

        dir.y = yVelocity;
        cc.Move(dir);
    }
}

