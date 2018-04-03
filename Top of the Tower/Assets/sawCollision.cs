using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sawCollision : MonoBehaviour {

    public GameObject SawBlade;    public PlayerController pc;    public bool trapActivate;    int count = 0;

    private void OnCollisionEnter(Collision col)    {        Debug.Log("coll detected");        if ((col.gameObject.tag == "Player") && (count < 1)) //trap only spikes the player once 
        {            trapActivate = true;

            Debug.Log(count);            Debug.Log("collided w saw blade");            pc.takeDamage(2);            count = 1;        }        else if (col.gameObject.tag == "Player")        {            Debug.Log("second attack by trap" + count);        }    }
}
