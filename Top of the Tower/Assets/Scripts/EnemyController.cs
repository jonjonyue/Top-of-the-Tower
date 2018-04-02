using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyController : character {

	// Enemy Specific Stats
	float aggroDistance;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (!counted)
        {
            if (health <= 0)
            {
                counted = count();
            }
        }
    }

    static bool count()
    {

        if (Monitor.TryEnter(locker))
        {
            killed = killed + 1;
            Monitor.Exit(locker);
            return true;
        }
        else
        {
            return false;
        }      
    }
}
