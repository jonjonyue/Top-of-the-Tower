using System.Collections;using System.Collections.Generic;using UnityEngine;public class TrapCollision : MonoBehaviour
{    public GameObject Boulder;    public Collider col;    public PlayerController pc;    public bool trapActivate;
    int count = 0;
    //count starts at 0 

    MeshRenderer boulderMR;

    // Use this for initialization
    void Start()    {        boulderMR = Boulder.GetComponent<MeshRenderer>();        boulderMR.enabled = false;    }    private void Update()    {                Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));        foreach (Collider c in cols)        {            if ((c.tag == "Player") && (count<1))            {                trapActivate = true;                Debug.Log("collided");                Debug.Log(count);                boulderMR.enabled = true;
                pc.takeDamage(2);
                count = 1;
            }
            else if (c.tag == "Player")
            {
                Debug.Log("second count" + count);
                boulderMR.enabled = false;
            }
            //count++;
            
        }
    }
}