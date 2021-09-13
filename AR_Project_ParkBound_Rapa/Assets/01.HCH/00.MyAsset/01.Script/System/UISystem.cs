using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : SceneObject<UISystem>
{
    [SerializeField]
    public enum JumpType
    {
        JumpStart,
        JumpStay,
        JumpExit,
        Grounded
    }
    public JumpType jumpType;

    /// <summary> JumpType Á¾·ù -> JumpStart, JumpStay, JumpExit, Grounded </summary>
    public void ChangeJumpType(string jType) => jumpType = (JumpType)System.Enum.Parse(typeof(JumpType), jType);
}
