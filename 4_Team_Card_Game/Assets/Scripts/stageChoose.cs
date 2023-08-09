using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class stageChoose : MonoBehaviour
{
    public void moveToStageSelect()
    {
        SceneManager.LoadScene("StageSelect");
    }
}
