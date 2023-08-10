using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseUI : MonoBehaviour, IPointerClickHandler
{
    // 퍼즈 UI 스크립트

    bool isTimeStop = false;
    public bool IsTimeStop { get { return isTimeStop; } }

    [SerializeField]
    Sprite StartImage; // 인스펙터에서 넣어주시면 됩니다.
    Sprite PauseImage; // 게임 시작시 스프라이트이고 자동으로 할당됩니다.

    void Awake()
    {
        PauseImage = GetComponent<Image>().sprite;
    }

    public void OnPointerClick(PointerEventData eventData) // 버튼 컴퍼넌트 비슷한 거 (IPointer 인터페이스 검색하면 나옴)
    {
        if (isTimeStop == false && gameManager.I.IsGameing == true)
        {
            GetComponent<Image>().sprite = StartImage;
            isTimeStop = true;
            Time.timeScale = 0;
            DestroyClickEffect();
            gameManager.I.endPanel.SetActive(true);
            transform.Find("DontCheat").gameObject.SetActive(true); // endPanel << 이걸로도 충분히 가려진다면 삭제하셔도 됩니다 스프라이트도 임시라서 변경하셔도됩니다.
        }
        else if (isTimeStop == true && gameManager.I.IsGameing == true)
        {
            GetComponent<Image>().sprite = PauseImage;
            isTimeStop = false;
            Time.timeScale = 1.0f;
            gameManager.I.endPanel.SetActive(false);
            transform.Find("DontCheat").gameObject.SetActive(false); // endPanel << 이걸로도 충분히 가려진다면 삭제하셔도 됩니다 스프라이트도 임시라서 변경하셔도됩니다.
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
