using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dispSlayed : MonoBehaviour {

    // Use this for initialization
    GameObject player;
    // Update is called once per frame


    private void Start()
    {
        player = GameObject.Find("Hero");
        //player.GetComponent<PlayerController>();
        gameObject.GetComponent<Text>().text = "You have slayed " + "0" + " enermy(ies)!"; // default value?;
    }

    void Update()
    {
        gameObject.GetComponent<Text>().text = "You have slayed " + player.GetComponent<PlayerController>().slayed() + " enermy(ies)!";
    }
}
