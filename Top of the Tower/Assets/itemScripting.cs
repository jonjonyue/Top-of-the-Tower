using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemScripting : MonoBehaviour {

    public int numOfItems;
	// Use this for initialization
	void Start () {
        numOfItems = gameObject.transform.childCount;
        //foreach (Transform child in transform)
        //{
        //    if (child != transform[0])
        //    {
        //        //child is your child transform
        //    }
        //}
        Debug.Log(numOfItems);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
