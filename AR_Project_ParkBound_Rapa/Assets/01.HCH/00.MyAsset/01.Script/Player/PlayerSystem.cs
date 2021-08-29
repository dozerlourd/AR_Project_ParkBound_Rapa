using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSystem : SceneObject<PlayerSystem>
{
    #region 변수

    [SerializeField] float InitDelayTime = 1f;

    [SerializeField] GameObject _player;
    Animator _animator;
    SpriteRenderer _spriteRenderer;
    Collider _collider;

    Vector3 _initPos;
    Vector3 _originPos;

    #endregion

    #region 속성

    public GameObject Player => _player;
    public Animator Animator => _animator = _animator ? _animator : Player.GetComponent<Animator>();
    public SpriteRenderer SpriteRenderer => _spriteRenderer = _spriteRenderer ? _spriteRenderer : Player.GetComponent<SpriteRenderer>();
    public Collider Collider => _collider = _collider ? _collider : Player.GetComponent<Collider>();

    public Vector3 InitPosition => _initPos = OriginPos;

    public Vector3 OriginPos
    {
        get => _originPos;
        set
        {
            _originPos = GameObject.FindGameObjectWithTag("T_OriginPos").transform.position != null ?
            GameObject.FindGameObjectWithTag("T_OriginPos").transform.position :
            Vector3.zero;
            print(GameObject.FindGameObjectWithTag("T_OriginPos"));
        }
    }

    #endregion

    #region 유니티 생명주기

    private void Start()
    {
        OriginPos = OriginPos;
    }

    #endregion

    #region 구현부

    public IEnumerator PlayerDie()
    {
        // SetActive를 false로 변경한다
        // 일정 시간 이후 초기화 위치로 되돌아간다
        // 다시 SetActive를 true로 변경한다
        Player.SetActive(false);
        Player.transform.position = InitPosition;

        yield return new WaitForSeconds(InitDelayTime);
        Player.SetActive(true);
    }

    #endregion
}
