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

    [SerializeField]
    GameObject MatchText;

    public GameObject endTxt;
    public Text timeTxt;
    public Text matchingTxt;
    public GameObject timePenalty;


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

        int[] rtans = new int[] { 0, 0, 1, 1, 2, 2, 3, 3 };
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

            MakeMatchText("세글자");

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

            MakeMatchText("실패");

            firstCard.GetComponent<card>().CloseCard();
            secondCard.GetComponent<card>().CloseCard();

            time -= 3.0f;
            GameObject penalty = Instantiate(timePenalty);
            Destroy(penalty, 0.5f);
        }

        firstCard = null;
        secondCard = null;
    }

    void Update()
    {
        time -= Time.deltaTime;

        timeTxt.text = time.ToString("N2");
        if (time <= 0.0f)
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

    // 카드 매치 시도시 텍스트 출력
    // target = 텍스트 나올 오브젝트, text = 나올 글자
    // 이미지 변경후 이름 나오게 수정
    void MakeMatchText(string text)
    {
        string name = SelectName(text);

        GameObject firstCardText = Instantiate(MatchText, firstCard.transform.position, Quaternion.identity);
        GameObject secondCardText = Instantiate(MatchText, secondCard.transform.position, Quaternion.identity);
        firstCardText.transform.Find("Text").gameObject.GetComponent<Text>().text = name;
        secondCardText.transform.Find("Text").gameObject.GetComponent<Text>().text = name;
    }

    // 이름 구별 함수 (아직 미완성)
    string SelectName(string text)
    {
        string name;

        switch (text)
        {
            case "0":
                name = "김호연";
                break;
            case "1":
                name = "김진성";
                break;
            case "2":
                name = "곽민규";
                break;
            case "3":
                name = "노재우";
                break;
            default:
                name = "로그봐";
                //Debug.Log("이름 입력 실패");
                break;
        }
        return name;
    }
}
