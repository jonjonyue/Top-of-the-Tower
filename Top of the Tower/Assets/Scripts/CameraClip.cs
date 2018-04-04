using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClip : MonoBehaviour {

    Collider col;
    LinkedList<GameObject> list = new LinkedList<GameObject>();

	// Use this for initialization
	void Start () {
        col = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Environment"));
        LinkedList<GameObject> bufferList = new LinkedList<GameObject>();

        foreach (Collider c in cols)
        {
            bufferList.AddLast(c.gameObject);
            if(!list.Contains(c.gameObject))
                list.AddLast(c.gameObject);
        }

        if (list.Count != 0)
        {
            foreach (GameObject g in list)
            {
                if (!Equals(bufferList.Find(g), null))
                    g.GetComponent<Renderer>().enabled = false;
                else
                    g.GetComponent<Renderer>().enabled = true;
            }
        }   
	}
}
