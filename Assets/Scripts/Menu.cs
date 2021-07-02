using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    public Text CoinsText;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.gm.Load();
        UpdateCoins(GameManager.gm.coins);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCoins(int Coins)
    {


        CoinsText.text = Coins.ToString();
    }

    public void StartRun()
    {
        GameManager.gm.StartRun();
    }

    public void GenerateCoins()
    {
        UpdateCoins(GameManager.gm.coins);
    }
}
