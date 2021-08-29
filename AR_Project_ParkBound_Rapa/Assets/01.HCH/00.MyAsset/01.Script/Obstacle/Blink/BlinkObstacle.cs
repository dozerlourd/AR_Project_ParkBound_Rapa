using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkObstacle : MonoBehaviour
{
    [SerializeField] float visibleTime = 1.0f, invisibleTime = 1.0f;
    [SerializeField] float nonBlinkTime = 4.0f, startWaitTime = 1.0f;
    [SerializeField] float previewBlinkSpeed = 3f;
    float previewBlinkTime;

    int blinkCount = 0;
    bool isVisible = true;

    void Start() => CoroutineManager.Instance.StartCoroutine(BlinkCycle());

    IEnumerator BlinkCycle()
    {
        yield return new WaitForSeconds(startWaitTime);

        while (true)
        {
            VisibleTimeCheck();
            yield return VisibleTimeCheck();

            //yield return StartCoroutine(PreviewBlink());
            Blink();

            CheckBlinkCount();
            if (blinkCount == 1) continue;
            else yield return new WaitForSeconds(nonBlinkTime);
        }
    }

    IEnumerator PreviewBlink()
    {
        yield return null;
    }

    void Blink()
    {
        VisibleChange();
        gameObject.SetActive(isVisible);
    }

    WaitForSeconds VisibleTimeCheck() => isVisible ? new WaitForSeconds(visibleTime) : new WaitForSeconds(invisibleTime);

    void VisibleChange() => isVisible = !isVisible;

    void CheckBlinkCount() => blinkCount = blinkCount == 0 ? 1 : 0;
}
