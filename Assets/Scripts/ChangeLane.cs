using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLane : MonoBehaviour
{
   
    public void PositionLane()
    {
        int randomLane = (int)Random.Range(-1, 2); // de -1 1 ele nao faz o  2
        transform.position = new Vector3(randomLane, transform.position.y, transform.position.z);
    }


}
