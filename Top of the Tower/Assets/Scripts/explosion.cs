using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{

    public int explosionDamage;

    Animator anim;
    Collider col;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();

        col = GetComponent<Collider>();
        col.enabled = false;
        StartCoroutine(explode());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator explode()
    {
        anim.SetTrigger("spawn");
        float timer = 0;
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            timer += Time.deltaTime;
            if (timer < .8f)
                transform.position = GameObject.Find("Hero").transform.position;
            yield return null;
        }
        col.enabled = true;
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));

        Debug.Log("Explosion hitting " + cols.Length + " targets");

        //ATTACK MECHANICS
        foreach (Collider c in cols)
        {
            if (c.tag == "Player")
            {
                Debug.Log("Hit hero!");
                GameObject.Find("Hero").GetComponent<PlayerController>().takeDamage(explosionDamage);
            }
            if (c.tag == "Enemy") {
                Debug.Log("Hit enemy!");
                SkeletonSpawn s = c.GetComponentInParent<SkeletonSpawn>();
                if (s != null)
                    s.takeDamage(explosionDamage);
            }
        }
    }

}
