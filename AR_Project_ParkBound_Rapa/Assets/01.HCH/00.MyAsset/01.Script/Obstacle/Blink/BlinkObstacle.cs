using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkObstacle : MonoBehaviour
{
    [SerializeField, Tooltip("가시모드로 전환되기까지의 지연 시간")] float visibleTime = 1.0f;
    [SerializeField, Tooltip("비가시모드로 전환되기까지의 지연 시간")] float invisibleTime = 1.0f;
    [SerializeField, Tooltip("첫 사이클이 활성화 되기 전 대기 시간")] float startWaitTime = 1.0f;
    [SerializeField, Tooltip("비가시모드가 되기 전 살짝 깜빡이는 이펙트의 점멸 속도")] float previewBlinkSpeed = 3f;

    Material material;

    int blinkCount = 0;
    bool isVisible = true;

    void Start()
    {
        CoroutineManager.Instance.StartCoroutine(BlinkCycle());
        material = GetComponent<MeshRenderer>().material;
    }

    IEnumerator BlinkCycle()
    {
        yield return new WaitForSeconds(startWaitTime);
        yield return StartCoroutine(PreviewBlink());

        while (true)
        {
            Blink();

            VisibleTimeCheck();
            yield return VisibleTimeCheck();

            CheckBlinkCount();
            if (blinkCount == 1)
            {
                yield return CoroutineManager.Instance.StartCoroutine(BurnTimeToBlink());
            }
            else
            {
                yield return CoroutineManager.Instance.StartCoroutine(PreviewBlink());
            }
        }
    }

    IEnumerator PreviewBlink()
    {
        for (int i = 0; i < 6; i++)
        {
            float rateTemp = 0;                                                                              // 변화율 임시 변수
            float currAlpha = material.color.a;                                                              // 코루틴 시작 전 알파 값
            float targetAlpha = i % 2 == 1 ? 1 : 0;                                                      // 목표 변화 알파 값

            //print(previewBlinkSpeed * 0.01f); // 내일 강사님께 여쭤볼 것 -> 0.03이 찍혀야되는데 중간마다 0.05가 찍힘

            while (rateTemp < 1)
            {
                rateTemp += previewBlinkSpeed * Time.deltaTime;
                float alpha = Mathf.Lerp(currAlpha, targetAlpha, rateTemp);
                material.color = new Color(material.color.r, material.color.g, material.color.b, alpha);    // 알파 값만 변화를 준다
                yield return null;
            }
        }
    }

    IEnumerator BurnTimeToBlink()
    {
        for (int i = 0; i < 6; i++)
        {
            float rateTemp = 0;                                                                              // 변화율 임시 변수

            while (rateTemp < 1)
            {
                rateTemp += previewBlinkSpeed * Time.deltaTime;
                yield return null;
            }
        }
    }

    void Blink()
    {
        VisibleChange();
        gameObject.SetActive(isVisible);
    }

    WaitForSeconds VisibleTimeCheck() => isVisible ? new WaitForSeconds(invisibleTime) : new WaitForSeconds(visibleTime);

    void VisibleChange() => isVisible = !isVisible;

    void CheckBlinkCount() => blinkCount = blinkCount == 0 ? 1 : 0;
}
