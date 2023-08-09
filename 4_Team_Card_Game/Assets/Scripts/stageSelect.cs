using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class stageSelect : MonoBehaviour
{
    public GameObject alert;

    void Start()
    {
        if (PlayerPrefs.HasKey("level") == false && PlayerPrefs.HasKey("stageLevel") == false)
        {
            PlayerPrefs.SetInt("level", 0);
            PlayerPrefs.SetInt("stageLevel", 0);
        }

        if (PlayerPrefs.GetInt("level") == 0)
        {
            stageLock("stage2");
            stageLock("stage3");
        } else if (PlayerPrefs.GetInt("level") == 1)
        {
            stageLock("stage3");
        }

        if (PlayerPrefs.HasKey("bestScore"))
        {
            PlayerPrefs.SetFloat("bestScore", 0f);
        }
    }
    public void s1()
    {
        PlayerPrefs.SetInt("stageLevel", 1);
        SceneManager.LoadScene("MainScene");
    }
    public void s2()
    {
        if (1 <= PlayerPrefs.GetInt("level"))
        {
            PlayerPrefs.SetInt("stageLevel", 2);
            SceneManager.LoadScene("MainScene");
        } else
        {
            alertActive();
        }
    }
    public void s3()
    {
        if (2 <= PlayerPrefs.GetInt("level"))
        {
            PlayerPrefs.SetInt("stageLevel", 3);
            SceneManager.LoadScene("MainScene");
        } else
        {
            alertActive();
        }
    }
    void alertActive()
    {
        alert.SetActive(true);
        Invoke("closeAlert", 1f);
    }

    void closeAlert()
    {
        alert.SetActive(false);
    }

    void stageLock(string stage)
    {
        GameObject.Find(stage).GetComponent<Image>().color = Color.gray;
    }
}
