using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBoardRepair : MonoBehaviour
{
    [Tooltip("�� �信 ��θ� ���� ������ �����ݴϴ�.")]
    public bool debugLine = true;
    [Tooltip("���� ��ǥ�� �������� ������ ��� ��ǥ")]
    public Vector3[] relativeMovePoint;   //Ŀ���� �����Ϳ��� ���� ��ü�� ������ ��� ������ ������ ��������.
    [Tooltip("���� ���۰� ���ÿ� �����̰� �մϴ�. false�� ��� ĳ���Ͱ� �ö�Ÿ�� �����Դϴ�.")]
    public bool awakeStart = true;
    [Tooltip("���� ���� ���� ó�� �����̱���� ��� �ð�")]
    public float awakeWaitTime = 0;
    [Tooltip("�̵� �ӵ�")]
    public float speed = 3;
    [Tooltip("������ ��� �ð�")]
    public float waitTime = 0;
    [Tooltip("���� �������� �����ϰ� �����Ǳ������ ��� �ð�")]
    public float destroyWaitTime = 1.0f;
    [Tooltip("����� �ð�")]
    public float respawnTime = 1.0f;
    [Tooltip("���� ���� ����")]
    public bool jumpInertia = false;

    private float jumpInertiaValue; //jumpInertia�� true�� �� �����¿� �������� ��

    private int cur = 1;        //���� ������ ��� ��ȣ
    [SerializeField] bool back = false;  //���� �������� ��� �ǵ��� ���� ������
    private bool movingOn = false;  //���� �����̰� �ִ���. awakeStart�� false �� �ʿ�

    private bool playerCheck = false;   //�÷��̾� ĳ���Ͱ� ž���ߴ���
    private GameObject player;      //�÷��̾� ĳ���͸� ã�� ������Ʈ���� ����
    private PlayerMove playerMove;
    private CharacterController playerCC;

    Vector3[] Pos;      //relativeMovePoint���� ���� ��ȯ�� ���� ������ǥ�� ������ �ִ� �迭

    Vector3 firstPos = Vector3.zero;    //OnDrawGizmos���� ���. ���� ���� ���¸� �ľ��ϰ� �ʱ� ��ǥ�� ����

    void Awake()
    {
        if (relativeMovePoint.Length <= 0)   //���� �̵���ΰ� �ԷµȰ� ���ٸ� �ش� ��ũ��Ʈ ������Ʈ�� ����
        {
            Destroy(this);
            return;
        }

        player = GameObject.Find("Player"); //�÷��̾� ĳ���͸� ã�´�.
        if (player) //�ִٸ� ������Ʈ ����
        {
            playerMove = player.GetComponent<PlayerMove>();
            playerCC = player.GetComponent<CharacterController>();
        }

        Pos = new Vector3[relativeMovePoint.Length + 1];   //+1�� �ϴ� ������ ���� ��ǥ���� �����ؾ� �ϱ� ����
        Pos[0] = firstPos = transform.position;  //���� ��ǥ�� ����. firstPos�� OnDrawGizmos�� ���� ����.
        for (int i = 1; i < relativeMovePoint.Length + 1; i++)  //0���� �̹� ä�����Ƿ� 1������ ä���.
        {
            Pos[i] = Pos[i - 1] + transform.TransformDirection(relativeMovePoint[i - 1]);   //������Ʈ�� ������ ����Ͽ� �����ǥ�� ���� ���� ���� ��ǥ�� ���Ѵ�.
        }
        if (awakeStart)     //awakeStart�� true��� �ٷ� �����̰� �Ѵ�.
        {
            movingOn = true;
            StartCoroutine(Move());
        }
    }

    IEnumerator Move()
    {
        WaitForFixedUpdate delay = new WaitForFixedUpdate();  //�ش� �ڵ�� ����� ����Ͽ����� �ִ��� ���� ���� ������ ���� 30���������� ���� FixedUpdateȯ�濡�� �����Ǿ��ִ�.
                                                              //������ yield return null�� �ᵵ �������.
                                                              //�׸��� ���⼭ ���� ������ ���ִ� ������ �������� ���̱� ���Կ� �ִ�.

        if (awakeStart && awakeWaitTime != 0)   //awakeStart�� true�� ���� ������ �ִ�. ������ ������� ������ ��ó �ٸ� �����̴� ������Ʈ��� �����ڷ� �����̰� �� �� ����.
            yield return new WaitForSeconds(awakeWaitTime);
        while (true)
        {
            if (transform.position == Pos[cur]) //Pos[cur]�迭 ��� ������ �����ϸ� ����
            {
                if (!back)  //�ǵ��ƿ��°� �ƴ϶��
                {
                    if (++cur == Pos.Length)    //cur ���� 1 �ø���. �ٵ� �� ���� Pos.Length�� �����ϸ� ���� �������� �����ߴٴ� ���̴�.
                    {
                        if (!awakeStart)    //���⿡ �����ߴٴ� ���� �÷��̾ ž���Ͽ� �����̱� ������ ������Ʈ�� ���� �������� �����ߴٴ� ���̴�.
                        {
                            Invoke("DestroyWait", destroyWaitTime); //Invoke�� ���� ���ð� ����ŭ ��� �� �Լ� ����
                            this.enabled = false;                   //�׸��� ������ �ش� �ڵ尡 �ʿ� �����Ƿ� ��Ȱ��ȭ ��Ų��.
                            yield break;                            //�ڷ�ƾ�� ����
                        }
                        else                    //awakeStart�� true���
                        {
                            back = true;        //back�� true�� ����� �ڷ� ���ٴ� ���� �˸���.
                            cur = cur - 2;      //�׸��� ���� ��ǥ�� Ŀ���� �ű��.
                        }
                    }
                }
                else    //back�� true��� ����
                {
                    if (--cur == -1)    //��� ���� ��θ� Ž���ϵ� ���� ���� ����Ű�� �ٽ� ó������ �ǵ��ƿԴٴ� ���̴�
                    {
                        back = false;   //�ٽ� back�� false�� ����� ���������� �̵��ϰ� �����.
                        cur = cur + 2;  //Ŀ���� �ٽ� ���� ����Ų��.
                    }
                }
                if (waitTime != 0)
                    yield return new WaitForSeconds(waitTime);  //������ ���ð��� �ִٸ� �׸�ŭ ��ٸ���
                else
                    yield return delay;     //������ �ٷ� ���� ���������� �ѱ��.
            }
            else       //�������� �����Ѱ� �ƴ϶�� �̵� ���̴�.
            {
                Vector3 prevPos = transform.position;  //�̵� ���� ��ǥ�� �ӽ÷� �����Ѵ�.

                transform.position = Vector3.MoveTowards(transform.position, Pos[cur], speed * Time.deltaTime); //�������� ���� �̵��Ѵ�.
                if (playerCheck)
                {
                    if (playerMove.dir.y < 0)  //�÷��̾ ���� ������ �ƴ� ����� Ÿ�� �ִٸ�
                    {
                        playerCC.Move((transform.position - prevPos) *transform.lossyScale.x);    //�̵� �� ��ǥ���� �̵� ���� ��ǥ�� ����ŭ �÷��̾� ĳ���͸� �̵���Ų��.
                                                                        //�̷��� ��Ȯ�� ������Ʈ�� ���� �����δ�.
                        playerCC.Move(new Vector3(0, -2.0f * Time.deltaTime, 0) * transform.lossyScale.x);   //�̰� ������ ������ isGrounded�� ����� Ȱ��ȭ���� �ʴ´�.
                        if (jumpInertia)    //���� ���� ������ true���
                            jumpInertiaValue = ((Pos[cur] - transform.position).normalized * speed).y;  //deltatime�� ������ ���� �״���� �ӵ����� y���� ���Ѵ�.
                                                                                                        //�Ʒ� OnTriggerExit()�Լ��� ���� �÷��̾ ������ �����ٸ� �� ���� ������ ������ ������ �ް� �Ѵ�.
                    }
                }
                yield return delay;
            }
        }
    }

    //�����̴� ��θ� ���信 �׷��ش�., ǻ 
    void OnDrawGizmos()
    {
        if (!debugLine || relativeMovePoint.Length <= 0)    //�ν����� â���� üũ�� ������ ������ �׸��� �ʴ´�.
            return;
        Vector3 t1, t2; //�ӽ� ��ǥ
        if (firstPos == Vector3.zero)   //firstPos�� Awake���� ���� ������Ʈ�� ��ǥ�� �ʱ�ȭ�ȴ�. ��, Vector3.zero�� ������ ���۵��� �ʾҴٴ� �Ҹ�
            t1 = t2 = transform.position;   //������ ���۵Ǳ� ���̶�� �ڽ��� ���� ��ǥ�� �ӽð��� �ִ´�.
        else
            t1 = t2 = firstPos;         //������ ���۵Ǿ��ٸ� firstPos�� ����ִ� �ʱ� ��ġ���� �ӽð��� �ִ´�.
                                        //�̷��� ������ ������Ʈ�� �������� ��� ���� ��θ� �׷��ֱ� ���Կ� �ִ�.
        for (int i = 0; i < relativeMovePoint.Length; i++) //�Էµ� ����� ������ŭ �ݺ�.
        {
            t2 += transform.TransformDirection(relativeMovePoint[i]);    //�ι�° �ӽ� ��ǥ�� �Էµ� ��� ����� ����ŭ �����༭ ������ ��ǥ�� �־��ش�.
            if (0 < i)                                                      //ù��° �ӽ� ��ǥ�� �ι�° �ӽ� ��ǥ�� ������ǥ�� ���� �Ѵ�.
                t1 += transform.TransformDirection(relativeMovePoint[i - 1]);   //��, 0��°�� ��� i-1 �迭�� �����Ƿ� �����Ѵ�.
            Debug.DrawLine(t1, t2, Color.red);      //�� ��ǥ�� ��� �׷��ָ� ��� ��ΰ� ���信 �׷�����.
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("T_Player"))   //�÷��̾��� �߿� �پ��ִ� �ݶ��̴�
        {
            playerCheck = true;         //�÷��̾ ž��
            if (!movingOn && playerMove.dir.y < 1) //���� ���� �����̰� �÷��̾ ���� ���� �ƴ϶��
            {
                movingOn = true;
                StartCoroutine("Move"); //���
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("T_Player"))
        {
            playerCheck = false;
            if (jumpInertia)    //���� ������ �����ִٸ� ������ ���ص� ���� �����Ѵ�.
            {
                playerMove.yVelocity += jumpInertiaValue;
            }
        }
    }

    private void DestroyWait()  //������ ������������ �����Ͽ� ������� �� �� ȣ��Ǵ� �Լ�
    {
        this.gameObject.SetActive(false);   //���� �ؾ��ϹǷ� ��Ȱ��ȭ�� �����ش�.
        Invoke("Respawn", respawnTime);     //����� ���ð���ŭ Invoke�Լ��� ����Ų��.
    }
    private void Respawn()  //����� �ʱ�ȭ. ��� ���� �ʱ�ȭ ��Ű�� �ٽ� Ȱ��ȭ ��Ų��.
    {
        cur = 1;
        transform.position = firstPos;
        back = false;
        movingOn = false;
        playerCheck = false;
        this.enabled = true;
        this.gameObject.SetActive(true);
    }
}