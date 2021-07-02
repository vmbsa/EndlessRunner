using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Random = UnityEngine.Random;
using System;

[Serializable]
public class PlayerData
{
    public int coins;

}

public class GameManager : MonoBehaviour
{

    public static GameManager gm;

    public int coins;

    private string filePath;

    private void Awake()
    {
        if(gm == null)
        {
            gm = this;
        }else if(gm != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        filePath = Application.persistentDataPath + "/playerInfo.dat";

        if (File.Exists(filePath))
            Load();

    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath);

        PlayerData data = new PlayerData();

        data.coins = coins;
        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(filePath,FileMode.Open);

        PlayerData data = (PlayerData)bf.Deserialize(file);
        file.Close();

        coins = data.coins;
        print(coins.ToString());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartRun()
    {
        SceneManager.LoadScene(1);
    }

    public void EndRun()
    {
        SceneManager.LoadScene("Menu");
    }

    
}
