using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float moveSpeed = 5.0f;
    public float jumpPower = 10.0f;
    public int jumpCount = 1;
    public Camera cam;
    public float gravity = -9.8f;
    float yVelocity = 0;
    CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        //카메라를 기준으로 플레이어의 이동키를 정면으로 설정한다.
        //1. 현재 카메라의 좌표
        //2.현재 캐릭터의 좌표
        //3.인풋 축 입력 정보(Input.GetAxis)
        //4.전진이동과 측이동을 구분지어줄 Quaternion 회전값


      

        //카메라 정면 좌표 구한다 (카메라 위치 - 정면)
        Vector3 dir = cam.transform.localRotation * Vector3.forward;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        dir.y = 0;
        dir.Normalize();


        if(x != 0 || z != 0)
        {
            cc.Move(dir * moveSpeed * Time.deltaTime);
        }
        

        if (cc.collisionFlags == CollisionFlags.Below)
        //if (cc.isGrounded)
        {
            yVelocity = 0;
            jumpCount = 1;

        }



        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            yVelocity = jumpPower;
            jumpCount--;
        }




    }
}
