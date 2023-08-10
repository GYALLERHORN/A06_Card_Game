using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class audioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip startSceneBgm;
    public AudioClip mainSceneBgm;
    public string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "MainScene")
        {
            audioSource.clip = mainSceneBgm;
            audioSource.Play(); 
        }
        else
        {
            audioSource.clip = startSceneBgm;
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
