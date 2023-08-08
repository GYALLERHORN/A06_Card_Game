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
    public GameObject countDownGO;

    [SerializeField]
    GameObject MatchText;

    public GameObject endTxt;
    public Text timeTxt;
    public Text matchingTxt;
    public GameObject timePenalty; // 카드 두개가 다를 때 시간 까는 패널티

    public Animator anim; // timeTxt 애니메이션 전환
    public AudioSource audioSource; // GM오디오소스
    public AudioClip matchedSound; // 카드 두개가 일치할 때 소리
    public AudioClip unmatchedSound; // 카드 두개가 다를 때 소리


    bool ShowHint = false;

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
        ShowHint = false;

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
            audioSource.PlayOneShot(matchedSound);

            //card matching system
            matching += 1;
            matchingTxt.text = "성공 횟수 : " + matching.ToString();

            MakeMatchText("이름");

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
            audioSource.PlayOneShot(unmatchedSound);


            MakeMatchText("실패");

            firstCard.GetComponent<card>().CloseCard();
            secondCard.GetComponent<card>().CloseCard();

            time -= 3.0f;
            GameObject penalty = Instantiate(timePenalty); // 제한시간에서 3초 까기
            Destroy(penalty, 0.5f);
        }

        StopCountDown();
        firstCard = null;
        secondCard = null;
    }

    void Update()
    {
        time -= Time.deltaTime;

        timeTxt.text = time.ToString("N2");
        if (time <= 10.0f)
        {
            anim.SetBool("under10seconds", true);
            ShowMeTheHint();
        }
        if (time <= 0.0f)
        {
            GameEnd();

        }
    }

    void GameEnd()
    {
        anim.SetBool("under10seconds", false);

        Time.timeScale = 0.0f;
        endTxt.SetActive(true);
        time = 0f;

    }



    //카운트 다운

    public Text countDownTxt;

    bool isCountingDown = false; // 카운트다운 중인지 여부를 나타내는 변수

    public void StopCountDown()
    {
        StopAllCoroutines();
        countDownGO.SetActive(false);
        isCountingDown = false;
    }
    public void StartCountDown()
    {
        isCountingDown = true; // 카운트다운 시작
        StartCoroutine(CountDownCoroutine());

    }

    IEnumerator CountDownCoroutine()
    {
        float initialCount = 2f; // 초기 카운트 값
        float count = initialCount;


        while (count > 0 && isCountingDown)
        {
            count -= Time.deltaTime;
            countDownGO.SetActive(true);
            countDownTxt.text = count.ToString("N0") + "초 안에 뒤집으세요!";
            yield return null;
        }

        // 카운트가 0이 되면 카드 다시 뒤집기
        if (count <= 0 && firstCard != null)
        {


            if (firstCard != null && secondCard == null)
            {
                firstCard.GetComponent<card>().CloseCard();
                firstCard = null;
                secondCard = null;
            }

        }

        // 카운트 완료 후 초기화
        count = initialCount;
        countDownGO.SetActive(false);
        countDownTxt.text = count.ToString("N0") + "초 안에 뒤집으세요!";
        isCountingDown = false;
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
            case "실패":
                name = "실패";
                break;
            default:
                name = "로그봐";
                //Debug.Log("이름 입력 실패");
                break;
        }
        return name;
    }


    // 함수가 실핼될 때 힌트를 보여줌
    void ShowMeTheHint()
    {
        if (ShowHint == false)
        {
            ShowHint = true; // 업데이트에서 실행돼서 스위치 달아줌
            GameObject cards = GameObject.Find("cards"); // cards는 card의 부모이기때문에 불러옴
            int RandomCard = Random.Range(0, cards.transform.childCount); // cards.transform.childCount -> cards의 자식 갯수(남은 카드 갯수)
                                                                          // 남은 카드 중에서 힌트를 줄 카드 랜덤 선택

            for (int num = 0; num < cards.transform.childCount; num++) // 카드들을 비교하기위해 사용
            {
                if (cards.transform.GetChild(RandomCard).Find("front").GetComponent<SpriteRenderer>().sprite.name // RandomCard의 스프라이트 이름과
                    == cards.transform.GetChild(num).Find("front").GetComponent<SpriteRenderer>().sprite.name // for문으로 차례대로 카드 스프라이트 이름을 비교
                    && RandomCard != num) // RandomCard와 for문의 카드 번호가 같으면 안됨
                {
                    cards.transform.GetChild(RandomCard).GetComponent<Animator>().SetTrigger("IsHint"); //애니메이션 트리거 작동
                    cards.transform.GetChild(num).GetComponent<Animator>().SetTrigger("IsHint"); // 트리거 = 1회 작동
                    break; // 짝을 찾으면 바로 중단해서 퍼포먼스 상향
                }
            }
        }
    }
}
