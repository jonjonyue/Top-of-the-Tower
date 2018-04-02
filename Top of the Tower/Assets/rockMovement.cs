using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockMovement : MonoBehaviour {

    public GameObject rockTrap;
    public PlayerController pc;

    private Vector3 start1 = new Vector3(-0.5f,0.5f , -1);
    private Vector3 end1 = new Vector3(-0.5f, 0.5f , -4);
    public float speed = 1.0f;


    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(start1, end1, Mathf.PingPong(Time.time * speed, 1.0f));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("rock collided w player");
            pc.takeDamage(2);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("rock collided w enemy");
        }
 
    }
}
