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

    public void openCard()
    {
        transform.Find("front").gameObject.SetActive(true);
        transform.Find("back").gameObject.SetActive(false);

        if (gameManager.I.firstCard == null)
        {
            gameManager.I.firstCard = gameObject;
        }
        else
        {
            gameManager.I.secondCard = gameObject;
            gameManager.I.isMatched();
        }
    }

    public void destroyCard()
    {
        Invoke("destroyCardInvoke",0.5f);
    }
    public void destroyCardInvoke()
    {
        Destroy(gameObject);
    }
    public void closeCard()
    {
        Invoke("closeCardInvoke",0.5f);
    }
    public void closeCardInvoke()
    {
        transform.Find("front").gameObject.SetActive(false);
        transform.Find("back").gameObject.SetActive(true);
    }
}
