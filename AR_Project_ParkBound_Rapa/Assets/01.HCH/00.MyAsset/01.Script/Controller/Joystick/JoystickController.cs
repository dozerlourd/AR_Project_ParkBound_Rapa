

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JHS
{
    /// <summary>
    /// 조이스틱 컨트롤 제어
    /// </summary>
    public class JoystickController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        #region 변수

        [SerializeField] RectTransform m_stickTr;
        [SerializeField] RectTransform m_backgroundTr;
        float m_radius; // 반지름

        bool m_isControlJoystick;

        Vector2 m_initPos;

        #endregion

        #region 속성

        /// <summary>
        /// 조이스틱 입력 방향(에디터 상일 시 키보드 입력)
        /// </summary>
        public Vector2 InputDirection { get; private set; }

        #endregion

        #region 유니티 생명주기

        void Awake()
        {
            m_radius = m_backgroundTr.rect.width * 0.5f;
            m_initPos = m_backgroundTr.localPosition;
        }

        void Update()
        {
            if (m_isControlJoystick) return;

            //if (Input.GetAxis("Horizontal") <= 0.4f) InputDirection = new Vector2(0, Input.GetAxis("Vertical"));
            InputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        #endregion

        #region 콜백 함수

        // 반지름을 넘어서지 않는 선에서 입력 위치로 스틱을 위치시키며 입력 방향을 노멀라이즈하여 대입
        public void OnPointerDown(PointerEventData eventData)
        {
            m_isControlJoystick = true;
            //m_backgroundTr.position = eventData.position;

            Vector2 direction = eventData.position - (Vector2)m_backgroundTr.position;
            direction = Vector2.ClampMagnitude(direction, m_radius);

            m_stickTr.localPosition = direction;

            InputDirection = new Vector2(direction.x / 200, direction.y / 200);
        }

        // 반지름을 넘어서지 않는 선에서 입력 위치로 스틱을 위치시키며 입력 방향을 노멀라이즈하여 대입
        public void OnDrag(PointerEventData eventData)
        {
            Vector2 direction = eventData.position - (Vector2)m_backgroundTr.position;
            direction = Vector2.ClampMagnitude(direction, m_radius);

            m_stickTr.localPosition = direction;

            InputDirection = new Vector2(Mathf.Clamp(direction.x / 200, -1, 1), Mathf.Clamp(direction.y / 200, -0.5f, 0.5f));
            //InputDirection = new Vector2(direction.x, direction.y);
        }

        // 스틱을 원래 위치로 돌려놓고 입력 방향 또한 초기화
        public void OnPointerUp(PointerEventData eventData)
        {
            InputDirection = Vector2.zero;
            m_stickTr.localPosition = Vector2.zero;
            //m_backgroundTr.localPosition = m_initPos;

            m_isControlJoystick = false;
        }

        #endregion
    }
}
