using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class UIManager : MonoBehaviour
{
    public Image[] lifeHearts;
    public Text coinText;
    public GameObject gameOverPanel;
    public GameObject MagnetPanel;
    public GameObject X2Panel;
    public Text scoreText;


    public void UpdateLifes(int lives)
    {
        for (int i = 0; i < lifeHearts.Length; i++)
        {
            if (lives > i)
            {
                lifeHearts[i].color = Color.white;
            }
            else
            {
                lifeHearts[i].color = Color.black;
            }

        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Updatecoins(int coin)
    {
        coinText.text = coin.ToString();
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score + "m";
    }

}
