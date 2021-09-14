using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCheckGuide : MonoBehaviour
{
    [SerializeField] RectTransform imageRectTr;

    Vector2 originImageRect;
    [SerializeField] float yOffset = 0;

    void Start()
    {
        originImageRect = imageRectTr.position;
        StartCoroutine(ImageLerp());
    }

    IEnumerator ImageLerp()
    {
        float rate = 0;
        while(true)
        {
            Vector2 lerpPos = imageRectTr.position;
            while(rate < 1)
            {
                rate += Time.deltaTime;
                lerpPos.y = Mathf.Lerp(originImageRect.y + yOffset, originImageRect.y, rate);
                imageRectTr.position = lerpPos;
                yield return new WaitForEndOfFrame();
            }
            while(rate > 0)
            {
                rate -= Time.deltaTime;
                lerpPos.y = Mathf.Lerp(originImageRect.y + yOffset, originImageRect.y, rate);
                imageRectTr.position = lerpPos;
                yield return new WaitForEndOfFrame();
            }
            //yield return null;
        }
    }
}
