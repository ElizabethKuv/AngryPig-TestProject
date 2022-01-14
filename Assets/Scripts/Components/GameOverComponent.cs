using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverComponent : MonoBehaviour
{
    public static GameObject CanvasTheGameIsOver;
    public static Text TextTheGameIsOver;

    [SerializeField] GameObject canvasTheGameIsOver;
    [SerializeField] Text textTheGameIsOver;

    private void Awake()
    {
        CanvasTheGameIsOver = canvasTheGameIsOver;
        TextTheGameIsOver = textTheGameIsOver;
        CanvasTheGameIsOver.SetActive(false);
    }
    
    public static void Loss()
    {
        TextTheGameIsOver.text = "Oh no...";
        CanvasTheGameIsOver.SetActive(true);
        Time.timeScale = 0;
    }

    public static void Won()
    {
        TextTheGameIsOver.text = "Congrats! You won!";
        CanvasTheGameIsOver.SetActive(true);
        Time.timeScale = 0;
    }
}
