using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;
using UnityEditor.Experimental.RestService;
using System;
using Unity.VisualScripting;

public class gameManager : MonoBehaviour
{
    public static gameManager I;

    public GameObject card;
    public GameObject firstCard;
    public GameObject secondCard;
    public GameObject countDownGO;
    public GameObject touchEffect;


    GameObject PauseUI;
    [SerializeField]
    GameObject MatchText;

    public GameObject endPanel;
    public Text maxScoreTxt;
    public Text scoreTxt; // 기록이 아닌 점수 텍스트
    public Text timeTxt;
    public Text matchingTxt;
    public Text numOfMatchingTxt;
    public GameObject timePenalty; // 카드 두개가 다를 때 시간 까는 패널티

    public GameObject ClickEffects; // 클릭시 나오는 이펙트 부모


    public Animator anim; // timeTxt 애니메이션 전환
    public AudioSource audioSource; // GM오디오소스
    public AudioClip matchedSound; // 카드 두개가 일치할 때 소리
    public AudioClip unmatchedSound; // 카드 두개가 다를 때 소리

    bool ShowHint = false;

    public int matching = 0; // mathing number
    public int numOfMatcing = 0; //매칭 시도 횟수
    public Text NOM; //매칭 시도 횟수 텍스트 - 혹시 몰라 만들어둠 아직 안쓰임
    public int score = 0;

    public bool IsStartAniOff = false; //  !중요! 시작 애니메이션 끝나기 전에는 이벤트 발생 안되게 방어코드 작성
    public bool IsGameing = false; // 게임 중일때 true , 게임 중인지 아닐지 판단 필요할 때 사용하세용

    float time;

    public bool canOpen = true; // 카드가 두개 뒤집혔을 때 다른 카드를 뒤집을 수 있는가?

    void Awake()
    {
        I = this;
        PauseUI = GameObject.Find("Canvas").transform.Find("PauseBtn").gameObject;

    }

    // Start is called before the first frame update
    void Start()
    {
        IsGameing = true;
        

        List<GameObject> cardsArr = new List<GameObject>(); // 시작 애니메이션

        // time 이 게임 시작시 초기화될 수 있게 start로 옮김
        time = 30f;
        Time.timeScale = 1.0f;
        ShowHint = false;

        int[] members = new int[] { 0, 0, 1, 1, 2, 2, 3, 3 };
        
        if (PlayerPrefs.GetInt("stageLevel") == 2)
        {
            members = new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5 };
        }
        else if (PlayerPrefs.GetInt("stageLevel") == 3)
        {
            members = new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        }
        
        members = members.OrderBy(item => UnityEngine.Random.Range(-1.0f, 1.0f)).ToArray();

        for (int i = 0; i < members.Length; i++)
        {
            GameObject newCard = Instantiate(card);
            cardsArr.Add(newCard); // 시작 애니메이션
            newCard.transform.parent = GameObject.Find("cards").transform;

            float x = i % 4 * 1.4f - 2.1f;
            float y = i / 4 * 1.4f - 3.0f;

            newCard.transform.position = new Vector3(x, y, 0);


            string memberName = "member" + members[i].ToString();
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(memberName);
            newCard.SetActive(false); // 시작 애니메이션 
        }
        StartCoroutine("AppearCard", cardsArr); // 시작 애니메이션

    }


    public void IsMatched()
    {
        canOpen = false; // 매치 판정 중일때는 다른 카드 뒤집기 불가

        string firstCardImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

        
            if (firstCardImage == secondCardImage)
            {
                audioSource.PlayOneShot(matchedSound);


                //card matching system
                matching += 1;
                matchingTxt.text = "성공 횟수 : " + matching.ToString();

                //스코어 부분
                score += 100;

                MakeMatchText(firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name);

                firstCard.GetComponent<card>().DestroyCard();
                secondCard.GetComponent<card>().DestroyCard();

                int leftCards = GameObject.Find("cards").transform.childCount;
                if (leftCards == 2)
                {
                    if (PlayerPrefs.GetInt("stageLevel") > PlayerPrefs.GetInt("level"))
                    {
                        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("stageLevel"));
                    }

                    Invoke("GameEnd", 0.5f);
                }
            }
            else
            {
                audioSource.PlayOneShot(unmatchedSound);


                MakeMatchText("실패");
                score -= 10;

                firstCard.GetComponent<card>().CloseCard();
                secondCard.GetComponent<card>().CloseCard();

                float penaltyReduce = -1.0f;
                timePenalty.transform.Find("Canvas").Find("timePenaltyTxt").gameObject.GetComponent<Text>().text = penaltyReduce.ToString("N0"); ;
                time += penaltyReduce;
                GameObject penalty = Instantiate(timePenalty); // 제한시간에서 3초 까기
                Destroy(penalty, 0.5f);
            }
        
        StopCountDown();
        firstCard = null;
        secondCard = null;
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && PauseUI.GetComponent<PauseUI>().IsTimeStop == false)
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchPosition.z = 0;
            GameObject effect = Instantiate(touchEffect, touchPosition, Quaternion.identity);

            effect.transform.SetParent(ClickEffects.transform);
            effect.name = "ClickEffect";

            Destroy(effect, 1.0f);
        }

        if (IsStartAniOff == true) // 등장 애니메이션 끝났는지 확인하는 조건문
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
    }

    void GameEnd()
    {
        IsGameing = false;
        if (0f > time)
        {
            timeTxt.text = "0.00";
        }
        StopCountDown();
        anim.SetBool("under10seconds", false);


        if (PlayerPrefs.HasKey("bestScore") == false)
        {
            PlayerPrefs.SetFloat("bestScore", time);
        }
        else
        {
            if (time > PlayerPrefs.GetFloat("bestScore"))
            {
                PlayerPrefs.SetFloat("bestScore", time);
            }
        }

        float maxScore = PlayerPrefs.GetFloat("bestScore");
        maxScoreTxt.text = maxScore.ToString("N2");
        numOfMatchingTxt.text = numOfMatcing.ToString();

        // 다시하기 + 스테이지 선택 추가로 endTxt > endPanel 로 변경
        endPanel.SetActive(true);
        scoreTxt.text = score.ToString();
        maxScoreTxt.gameObject.SetActive(true);
        Time.timeScale = 0.0f;
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

    // 이름 구별 함수 스프라이트 이름 절대 지켜!
    string SelectName(string spriteName)
    {
        string name;

        switch (spriteName)
        {
            case "member0":
                name = "김호연";
                break;
            case "member4":
                name = "김호연";
                break;
            case "member1":
                name = "김진성";
                break;
            case "member5":
                name = "김진성";
                break;
            case "member2":
                name = "곽민규";
                break;
            case "member6":
                name = "곽민규";
                break;
            case "member3":
                name = "노재우";
                break;
            case "member7":
                name = "노재우";
                break;
            case "실패":
                name = "실패";
                break;
            default:
                name = "로그봐";
                Debug.Log("이름 입력 실패");
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
            int RandomCard = UnityEngine.Random.Range(0, cards.transform.childCount); // cards.transform.childCount -> cards의 자식 갯수(남은 카드 갯수)
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

    //test 용 함수
    public void dataReset()
    {
        PlayerPrefs.SetInt("level", 0);
        PlayerPrefs.SetInt("stageLevel", 0);
        PlayerPrefs.SetFloat("bestScore", 0f);
    }


    //시작 애니매이션 함수

    IEnumerator AppearCard(List<GameObject> cardsArr)
    {
        IsStartAniOff = false;
        for (int num = 0; num < cardsArr.Count; num++)
        {
            yield return new WaitForSeconds(0.15f); //0.15초 안에 하나씩
            cardsArr[num].SetActive(true); //카드가 활성화
            cardsArr[num].GetComponent<Animator>().SetTrigger("IsAppear"); // 애니메이션도 동작
            if (num == cardsArr.Count-1)
            {
                yield return new WaitForSeconds(0.3f);
                IsStartAniOff = true;
            }
        }
    }
}
