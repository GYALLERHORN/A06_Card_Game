using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager I;

    public GameObject card;
    public GameObject firstCard;
    public GameObject secondCard;
    float time = 30.0f;
    public Text timeText;

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

    public void isMatched()
    {
        string firstCardImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        if (firstCardImage == secondCardImage)
        {
            firstCard.GetComponent<card>().destroyCard();
            secondCard.GetComponent<card>().destroyCard();

            int leftCards = gameObject.transform.Find("cards").childCount;
            if (leftCards == 2)
            {
                Invoke("gameEnd", 0.5f);
            }
        }
        else
        {
            firstCard.GetComponent<card>().closeCard();
            secondCard.GetComponent<card>().closeCard();
        }

        firstCard = null;
        secondCard = null;
    }

    void Update()
    {
        time -= Time.deltaTime;
        timeText.text = time.ToString("N2");
    }

    void gameEnd()
    {
        Time.timeScale = 0.0f;
    }
}
