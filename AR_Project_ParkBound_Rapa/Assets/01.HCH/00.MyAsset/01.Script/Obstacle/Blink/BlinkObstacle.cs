using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkObstacle : MonoBehaviour
{
    [SerializeField, Tooltip("���ø��� ��ȯ�Ǳ������ ���� �ð�")] float visibleTime = 1.0f;
    [SerializeField, Tooltip("�񰡽ø��� ��ȯ�Ǳ������ ���� �ð�")] float invisibleTime = 1.0f;
    [SerializeField, Tooltip("ù ����Ŭ�� Ȱ��ȭ �Ǳ� �� ��� �ð�")] float startWaitTime = 1.0f;
    [SerializeField, Tooltip("�񰡽ø�尡 �Ǳ� �� ��¦ �����̴� ����Ʈ�� ���� �ӵ�")] float previewBlinkSpeed = 3f;

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
            float rateTemp = 0;                                                                              // ��ȭ�� �ӽ� ����
            float currAlpha = material.color.a;                                                              // �ڷ�ƾ ���� �� ���� ��
            float targetAlpha = i % 2 == 1 ? 1 : 0;                                                      // ��ǥ ��ȭ ���� ��

            //print(previewBlinkSpeed * 0.01f); // ���� ����Բ� ���庼 �� -> 0.03�� �����ߵǴµ� �߰����� 0.05�� ����

            while (rateTemp < 1)
            {
                rateTemp += previewBlinkSpeed * Time.deltaTime;
                float alpha = Mathf.Lerp(currAlpha, targetAlpha, rateTemp);
                material.color = new Color(material.color.r, material.color.g, material.color.b, alpha);    // ���� ���� ��ȭ�� �ش�
                yield return null;
            }
        }
    }

    IEnumerator BurnTimeToBlink()
    {
        for (int i = 0; i < 6; i++)
        {
            float rateTemp = 0;                                                                              // ��ȭ�� �ӽ� ����

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
