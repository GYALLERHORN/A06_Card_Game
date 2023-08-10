using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class startImage : MonoBehaviour
{
    
    public void gotoStageSelect()
    {
        SceneManager.LoadScene("StageSelect");
    }
}
