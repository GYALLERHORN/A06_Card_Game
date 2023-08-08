using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchText : MonoBehaviour
{
    [SerializeField]
    float MoveSpeed; // 글자 올라가는 속도
    [SerializeField]
    float MoveMaxPosY; // 올라가는 Y 한계값
    [SerializeField]
    float PosY; // 카드 기준 Y값

    float CheckMovePos = 0;

    Text Text;

    void Awake()
    {
        Text = transform.Find("Text").GetComponent<Text>();
        transform.position += Vector3.up * PosY;
    }

    void Update() // 위치, 투명도, 오브젝트 파괴 조건
    {
        transform.position += Vector3.up * MoveSpeed * Time.deltaTime;
        CheckMovePos += Vector3.up.y * MoveSpeed * Time.deltaTime;
        Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, 1.0f -  (CheckMovePos /MoveMaxPosY ));
        if (CheckMovePos > MoveMaxPosY)
        {
            Destroy(gameObject);
        }
    }
}
