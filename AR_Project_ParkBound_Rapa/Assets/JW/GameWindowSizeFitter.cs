using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWindowSizeFitter : MonoBehaviour
{
    public int screenWidth = 480;
    public int screenHeight = 320;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(screenWidth, screenHeight, FullScreenMode.Windowed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
