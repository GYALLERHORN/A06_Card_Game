using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseUI : MonoBehaviour, IPointerClickHandler
{
    // ���� UI ��ũ��Ʈ

    bool IsTimeStop = false; // ������� ���Ǵ� �Ұ� 

    [SerializeField]
    Sprite StartImage; // �ν����Ϳ��� �־��ֽø� �˴ϴ�.
    Sprite PauseImage; // ���� ���۽� ��������Ʈ�̰� �ڵ����� �Ҵ�˴ϴ�.

    void Awake()
    {
        PauseImage = GetComponent<Image>().sprite;
    }

    public void OnPointerClick(PointerEventData eventData) // ��ư ���۳�Ʈ ����� �� (IPointer �������̽� �˻��ϸ� ����)
    {
        if (IsTimeStop == false && gameManager.I.IsGameing == true)
        {
            GetComponent<Image>().sprite = StartImage;
            IsTimeStop = true;
            Time.timeScale = 0;
            gameManager.I.endPanel.SetActive(true);
            transform.Find("DontCheat").gameObject.SetActive(true); // endPanel << �̰ɷε� ����� �������ٸ� �����ϼŵ� �˴ϴ� ��������Ʈ�� �ӽö� �����ϼŵ��˴ϴ�.
        }
        else if (IsTimeStop == true && gameManager.I.IsGameing == true)
        {
            GetComponent<Image>().sprite = PauseImage;
            IsTimeStop = false;
            Time.timeScale = 1.0f;
            gameManager.I.endPanel.SetActive(false);
            transform.Find("DontCheat").gameObject.SetActive(false); // endPanel << �̰ɷε� ����� �������ٸ� �����ϼŵ� �˴ϴ� ��������Ʈ�� �ӽö� �����ϼŵ��˴ϴ�.
        }
    }
}
