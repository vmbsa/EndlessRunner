using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{


    public GameObject[] obs;
    public Vector2 numberOfObs;
    public List<GameObject> newObs;

    public GameObject coin;
    public Vector2 numberofCoins;
    public List<GameObject> newcoins;
  

    public GameObject magnet;
    public GameObject X2;

    // Start is called before the first frame update
    void Start()
    {
        int newNumberOFObs = (int)Random.Range(numberOfObs.x, numberOfObs.y);
        int newNumberOfCoins = (int)Random.Range(numberofCoins.x, numberofCoins.y);

        for (int i = 0; i < newNumberOFObs; i++)
        {
            newObs.Add(Instantiate(obs[Random.Range(0, obs.Length)], transform));
            newObs[i].SetActive(false);
        }

        for (int i = 0; i < newNumberOfCoins; i++)
        {
            newcoins.Add(Instantiate(coin, transform));
            newcoins[i].SetActive(false);
       
        }

        PositionObs();
        PositionCoins();
        Instantiate(magnet, transform);
        magnet.SetActive(false);
        PositionMagets();
        Instantiate(X2, transform);
        X2.SetActive(false);
        PositionX2();
    }

    void PositionObs()
    {
        for (int i = 0; i < newObs.Count; i++) //usar funcoes
        {
            float posZMin = (297f / newObs.Count) + (297f / newObs.Count) * i;
            float posZMax = (297f / newObs.Count) + (297f / newObs.Count) * i + 1;
            newObs[i].transform.localPosition = new Vector3(0, 0, Random.Range(posZMin, posZMax));
            newObs[i].SetActive(true);
            if (newObs[i].GetComponent<ChangeLane>() != null)
            {
                newObs[i].GetComponent<ChangeLane>().PositionLane();
            }
        }
    }

    void PositionCoins() //usar funcoes
    {
        float minZPos = 10f;
        for (int i = 0; i < newcoins.Count; i++)
        {
            float maxZPos = minZPos + 5f;
            float randomZPos = Random.Range(minZPos, maxZPos);
            newcoins[i].transform.localPosition = new Vector3(transform.position.x, transform.position.y, randomZPos);
            newcoins[i].SetActive(true);
            newcoins[i].GetComponent<ChangeLane>().PositionLane();
            minZPos = randomZPos + 1;
        }
    }

    void PositionMagets()
    {
        float minZPos = 200f;
            float maxZPos = minZPos + 80f;
            float randomZPos = Random.Range(minZPos, maxZPos);
            magnet.transform.localPosition = new Vector3(transform.position.x, transform.position.y, randomZPos);
            magnet.SetActive(true);
            magnet.GetComponent<ChangeLane>().PositionLane();
            minZPos = randomZPos + 200;
    }

    void PositionX2()
    {
        float minZPos = 100f;
        float maxZPos = minZPos + 80f;
        float randomZPos = Random.Range(minZPos, maxZPos);
        X2.transform.localPosition = new Vector3(transform.position.x, transform.position.y, randomZPos);
        X2.SetActive(true);
        X2.GetComponent<ChangeLane>().PositionLane();
        minZPos = randomZPos + 200;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().IncreaseSpeed(); // funcao para aumentar a Velocidade
            transform.position = new Vector3(0, 0, transform.position.z + 297 * 2);
            PositionObs();
            PositionCoins();
            PositionMagets();
            PositionX2();
        }

    }


}
