using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class CinematicVideo : MonoBehaviour
{
    [SerializeField]
    private int nextScene;
    private VideoPlayer videoPlayer;
    private bool skipped=false;
    private bool fadeStart=false;
    
     [SerializeField]
    private Image blackFade;
    [SerializeField]
    private float fadeDuration=.5f;
    [SerializeField]
    private bool fadeEnable=true;
    void Start()
    {
        skipped=false;
        fadeStart=false;
        videoPlayer=GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnLoopPointReached;
    }

    private void OnLoopPointReached(VideoPlayer source)
    {
        Debug.Log("cinematic ended");
        OnVideoEnded();
    }

    private void OnVideoEnded()
    {
        Debug.Log("skipped cinematic");
        if (fadeEnable)
        {
            fadeStart=true;
        }
        else
        {
            SceneManager.LoadScene(nextScene);
        }
    }
    void Update()
    {

        if (fadeStart)
        {
            if (blackFade.color.a>=1f)
            {
            SceneManager.LoadScene(nextScene);
            }
            else
            {
            Color fadeColor = blackFade.color;    
            fadeColor.a+= (Time.deltaTime/fadeDuration);
            blackFade.color=fadeColor;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && !skipped)
        {
            skipped=true;
            OnVideoEnded();
        }
    }
}
