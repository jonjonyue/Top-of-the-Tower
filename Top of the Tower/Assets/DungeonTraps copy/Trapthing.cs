using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapThing: MonoBehaviour
{
    public GameObject Boulder;
    public GameObject TrapNeedle;
    public Collider col;
    public PlayerController pc;
    public bool trapActivate;
    int count = 0;
    //count starts at 0 

    MeshRenderer boulderMR;
    MeshRenderer needleMR;

    // Use this for initialization
    void Start()
    {
      //  boulderMR = Boulder.GetComponent<MeshRenderer>();

      //  boulderMR.enabled = false;
        needleMR = TrapNeedle.GetComponent<MeshRenderer>();
        needleMR.enabled = false;
    }

   private void Update()
    {
        
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox")); //colliders in green box
        

        foreach (Collider c in cols)
        {
            if ((c.tag == "Player") && (count<1))
            {
                trapActivate = true;
  
                Debug.Log(count);
                Debug.Log("collided w needle");
                //boulderMR.enabled = true;
                pc.takeDamage(2);
                count = 1;
            }
            else if (c.tag == "Player")
            {
                Debug.Log("second count" + count);
                needleMR.enabled = false;
               // boulderMR.enabled = false;
            }
            //count++;
            
        }
    } 
    private void OnCollisionEnter(Collision col)
    {
        if ((col.gameObject.tag == "Player") && (count < 1))
        {
            trapActivate = true;

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
