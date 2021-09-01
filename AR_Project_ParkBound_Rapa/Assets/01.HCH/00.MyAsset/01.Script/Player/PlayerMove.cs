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

    Vector3 dir;
    [SerializeField] float gravity = -9.8f;
    float yVelocity = 0;
    int jumpCount = 1;
    CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //����Ű�� ������ �� �������� �־��ش�.

        if (jumpCount > 0)
        {

            if (Input.GetButtonDown("Jump"))
            {
                jumpClickTime = 0;
                isJump = true;
                print(isJump);
                jumpCount--;
            }

            if (Input.GetButton("Jump"))
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

            }
            if (Input.GetButtonUp("Jump"))
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

        if (yVelocity > maxJump)
        {
            yVelocity = maxJump;
        }

        dir.y = yVelocity;
        cc.Move(dir);
    }
}

