﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapCollis : MonoBehaviour {

    public GameObject TrapNeedle;
    public PlayerController pc;
    public bool trapActivate;
    int count = 0;
    //count starts at 0 

    MeshRenderer needleMR;

    // Use this for initialization
    void Start()
    {
        //  boulderMR = Boulder.GetComponent<MeshRenderer>();

        //  boulderMR.enabled = false;
        needleMR = TrapNeedle.GetComponent<MeshRenderer>();
        needleMR.enabled = false;
    }

    private void OnCollisionEnter(Collision col)
    {
        Debug.Log("coll detected");
        if ((col.gameObject.tag == "Player") && (count < 1))
        {
            trapActivate = true;
            needleMR.enabled = true;
            Debug.Log(count);
            Debug.Log("collided w needle");
            //boulderMR.enabled = true;
            pc.takeDamage(2);
            count = 1;
        }
        else if (col.gameObject.tag == "Player")
        {
            Debug.Log("second count" + count);
            needleMR.enabled = false;
            // boulderMR.enabled = false;
        }
        //count++;
    }
}
