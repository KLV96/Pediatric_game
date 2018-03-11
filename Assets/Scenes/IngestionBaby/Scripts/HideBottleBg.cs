﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBottleBg : MonoBehaviour {

    float timer = 0.0f;
    public Renderer rend;
    public playAnimationOnClick playAnimatinOnClickScript;
    public Babybottle babyBottleScript;

    // Update is called once per frame
    void Update ()
    {
        startTimer();
        hidebgImage();
    }

    void startTimer()
    {
        timer += Time.deltaTime;
    }

    void hidebgImage()
    {
        if (timer > 2)
        {
            rend.enabled = false;
        }

        if( timer > 3)
        {
            playAnimatinOnClickScript.bottleEnabled = true;
            babyBottleScript.bottleEnabled = true;
        }
    }


}