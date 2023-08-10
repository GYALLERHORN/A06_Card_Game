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
        DontDestroyOnLoad(GameObject.Find("alertCanvas"));
        if (PlayerPrefs.HasKey("level") == false && PlayerPrefs.HasKey("stageLevel") == false)
        {
            PlayerPrefs.SetInt("level", 0);
            PlayerPrefs.SetInt("stageLevel", 0);
        }

        if (PlayerPrefs.GetInt("level") == 0)
        {
            Stage2Lock();
            Stage3Lock();
        } 
        else if (PlayerPrefs.GetInt("level") == 1)
        {
            Stage3Lock();
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

    void Stage2Lock()
    {
        GameObject.Find("Canvas").transform.Find("stage2Lock").gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.Find("stage2").gameObject.SetActive(false);
    }

    void Stage3Lock()
    {
        GameObject.Find("Canvas").transform.Find("stage3Lock").gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.Find("stage3").gameObject.SetActive(false);
    }
}
