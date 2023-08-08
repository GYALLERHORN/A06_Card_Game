using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.IO;

public class gameManager : MonoBehaviour
{
    public static gameManager I;

    public GameObject card;
    public GameObject firstCard;
    public GameObject secondCard;

    public GameObject endTxt;
    public Text timeTxt;
    public Text matchingTxt;

    public int matching = 0; // mathing number
    float time = 30.0f;


    void Awake()
    {
        I = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;

        int[] rtans = new int[] { 0, 0, 1, 1, 2, 2, 3, 3};
        rtans = rtans.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        for (int i = 0; i < 8; i++)
        {
            GameObject newCard = Instantiate(card);
            newCard.transform.parent = GameObject.Find("cards").transform;

            float x = i % 4 * 1.4f - 2.1f;
            float y = i / 4 * 1.4f - 3.0f;

            newCard.transform.position = new Vector3(x, y, 0);

            string rtanName = "rtan" + rtans[i].ToString();
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(rtanName);
        }
    }

    public void IsMatched()
    {
        string firstCardImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        if (firstCardImage == secondCardImage)
        {
            //card matching system
            matching += 1;
            matchingTxt.text = "성공 횟수 : " + matching.ToString();

            firstCard.GetComponent<card>().DestroyCard();
            secondCard.GetComponent<card>().DestroyCard();

            int leftCards = GameObject.Find("cards").transform.childCount;
            if (leftCards == 2)
            {
                Invoke("GameEnd", 0.5f);
            }
        }
        else
        {
            firstCard.GetComponent<card>().CloseCard();
            secondCard.GetComponent<card>().CloseCard();
        }

        firstCard = null;
        secondCard = null;
    }

    void Update()
    {
        time -= Time.deltaTime;

        timeTxt.text = time.ToString("N2");
        if(timeTxt.text == "0.00")
        {
            GameEnd();

        }
    }

    void GameEnd()
    {
        Time.timeScale = 0.0f;
        endTxt.SetActive(true);
        time = 0f;
        
    }
}
