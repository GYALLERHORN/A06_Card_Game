using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseUI : MonoBehaviour, IPointerClickHandler
{
    // ���� UI ��ũ��Ʈ

    bool isTimeStop = false;
    public bool IsTimeStop { get { return isTimeStop; } }

    [SerializeField]
    Sprite StartImage; // �ν����Ϳ��� �־��ֽø� �˴ϴ�.
    Sprite PauseImage; // ���� ���۽� ��������Ʈ�̰� �ڵ����� �Ҵ�˴ϴ�.

    void Awake()
    {
        PauseImage = GetComponent<Image>().sprite;
    }

    public void OnPointerClick(PointerEventData eventData) // ��ư ���۳�Ʈ ����� �� (IPointer �������̽� �˻��ϸ� ����)
    {
        if (isTimeStop == false && gameManager.I.IsGameing == true)
        {
            GetComponent<Image>().sprite = StartImage;
            isTimeStop = true;
            Time.timeScale = 0;
            DestroyClickEffect();
            gameManager.I.endPanel.SetActive(true);
            transform.Find("DontCheat").gameObject.SetActive(true); // endPanel << �̰ɷε� ����� �������ٸ� �����ϼŵ� �˴ϴ� ��������Ʈ�� �ӽö� �����ϼŵ��˴ϴ�.
        }
        else if (isTimeStop == true && gameManager.I.IsGameing == true)
        {
            GetComponent<Image>().sprite = PauseImage;
            isTimeStop = false;
            Time.timeScale = 1.0f;
            gameManager.I.endPanel.SetActive(false);
            transform.Find("DontCheat").gameObject.SetActive(false); // endPanel << �̰ɷε� ����� �������ٸ� �����ϼŵ� �˴ϴ� ��������Ʈ�� �ӽö� �����ϼŵ��˴ϴ�.
        }
    }

    void DestroyClickEffect()
    {
        Transform[] ClickEffectChildArr = gameManager.I.ClickEffects.GetComponentsInChildren<Transform>();
        for (int num = 1; num < ClickEffectChildArr.Length; num++)
        {
            Debug.Log(ClickEffectChildArr[num].name);
            Destroy(ClickEffectChildArr[num].gameObject);
        }
    }
}
