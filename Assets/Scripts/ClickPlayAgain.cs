﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickPlayAgain : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) { 
            SceneManager.LoadScene("Level3", LoadSceneMode.Single);
        }
    }
}
