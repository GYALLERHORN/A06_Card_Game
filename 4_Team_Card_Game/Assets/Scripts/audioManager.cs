using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip startSceneSource;
    public AudioClip mainSceneBgm;


    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = mainSceneBgm;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
