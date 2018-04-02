using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dispHP : MonoBehaviour {

    GameObject player;
    // Update is called once per frame


    private void Start()
    {
        player = GameObject.Find("Hero");
        //player.GetComponent<PlayerController>();
        gameObject.GetComponent<Text>().text = "Your current health is " + "10!"; // default value?;
    }

    void Update () {
        gameObject.GetComponent<Text>().text = "Your current health is " + player.GetComponent<PlayerController>().curHP() + "!" ;
    }
}
