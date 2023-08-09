using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startImage : MonoBehaviour
{
    // Start is called before the first frame update
    public void gotoStageSelect()
    {
        SceneManager.LoadScene("StageSelect");
    }
}
