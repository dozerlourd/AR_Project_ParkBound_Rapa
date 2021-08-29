using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolderSystem : SceneObject<FolderSystem>
{
    #region 변수

    [SerializeField] Transform dustTr;
    [SerializeField] Transform obstacleTr;
    //[SerializeField] Transform c;
    //[SerializeField] Transform d;

    #endregion

    #region 속성

    public Transform DustTr => dustTr;
    public Transform ObstacleTr => obstacleTr;
    //public Transform C => c;
    //public Transform D => d;

    #endregion
}
