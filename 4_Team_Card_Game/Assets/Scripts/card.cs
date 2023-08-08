using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenCard()
    {
        GetComponent<Animator>().SetTrigger("IsSelect"); // 애니메이션 재생
        transform.Find("front").gameObject.SetActive(true);
        transform.Find("back").gameObject.SetActive(false);

        if (gameManager.I.firstCard == null)
        {
            gameManager.I.firstCard = gameObject;
        }
        else
        {
            gameManager.I.secondCard = gameObject;
            gameManager.I.IsMatched();
        }
    }

    public void DestroyCard()
    {
        Invoke("DestroyCardInvoke",0.5f);
    }
    public void DestroyCardInvoke()
    {
        Destroy(gameObject);
    }
    public void CloseCard()
    {
        Invoke("CloseCardInvoke",0.5f);
    }
    public void CloseCardInvoke()
    {
        transform.Find("front").gameObject.SetActive(false);
        transform.Find("back").gameObject.SetActive(true);
        transform.Find("back").gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
    }
}
