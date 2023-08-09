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
        if (gameManager.I.IsStartAniOff == true)
        {
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
    }

    public void DestroyCard()
    {
        Invoke("DestroyCardInvoke", 0.5f);
    }
    public void DestroyCardInvoke()
    {
        Destroy(gameObject);
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
    }

}
