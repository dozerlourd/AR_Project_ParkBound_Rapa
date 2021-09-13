using JHS;
using UnityEngine;

public class ControllerSystem : SceneObject<ControllerSystem>
{
    #region 변수

    [SerializeField] JoystickController m_horizontal_joystickController;
    [SerializeField] JoystickController m_vertical_joystickController;

    #endregion

    #region 공개 속성

    public Vector2 vertical_InputDirection => m_vertical_joystickController.InputDirection;
    public Vector2 horizontal_InputDirection => m_horizontal_joystickController.InputDirection;

    #endregion
}
