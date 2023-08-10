using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class card : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip draw;

    void Start()
    {
    }

    public void OpenCard()
    {
        if (gameManager.I.canOpen == false) // 카드를 클릭했지만 이미 뒤집힌 두 카드의 매치 판정중이니 false
        {
            return; // OpenCard 미실행
        }
        if (gameManager.I.IsStartAniOff == false)
        {
            return;
        }
        if (Time.timeScale == 0)
        {
            return;
        }

        gameManager.I.numOfMatcing++;
        audioSource.PlayOneShot(draw);

        GetComponent<Animator>().SetTrigger("IsSelect"); // 애니메이션 생성

        transform.Find("front").gameObject.SetActive(true);
        transform.Find("back").gameObject.SetActive(false);

        if (gameManager.I.firstCard == null)
        {
            gameManager.I.firstCard = gameObject;
            gameManager.I.StartCountDown();
        }
        else
        {
            gameManager.I.secondCard = gameObject;
            gameManager.I.IsMatched();
        }
    }

    public void DestroyCard()
    {
        Invoke("DestroyCardInvoke", 0.5f);
    }
    public void DestroyCardInvoke()
    {
        Destroy(gameObject);
        gameManager.I.canOpen = true; // 매치 판정 이후 두 카드의 동작이 진행된 후 다시 카드 뒤집기를 가능하게 원위치
    }
    public void CloseCard()
    {
        Invoke("CloseCardInvoke", 0.5f);

    }
    public void CloseCardInvoke()
    {
        gameManager.I.countDownGO.SetActive(false);
        transform.Find("front").gameObject.SetActive(false);
        transform.Find("back").gameObject.SetActive(true);
        transform.Find("back").gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
        gameManager.I.canOpen = true; // 매치 판정 이후 두 카드의 동작이 진행된 후 다시 카드 뒤집기를 가능하게 원위치
    }

}
