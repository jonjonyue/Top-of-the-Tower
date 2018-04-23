using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class SpawnScript : MonoBehaviour {

    GameObject item;
    public Sprite sprite;
    public RuntimeAnimatorController rac;
	public int itemIndex;

    //List<string> itemString;
    //List<string> itemController;
	SpriteRenderer spriteRenderer;
	Animator anim;
	//int itemIndex;
	BoxCollider col;
	bool hasBeenUsed = false;

	public Transform spawnPoint;
	public float amplitude = 0.5f;
	public float frequency = 1f;

	// Position Storage Variables
	Vector3 posOffset = new Vector3 ();
	Vector3 tempPos = new Vector3 ();

    void Start () {
		// Store the starting position & rotation of the object
		posOffset = transform.position;

		spriteRenderer = GetComponent<SpriteRenderer> ();
		anim = GetComponent<Animator> ();
		col = GetComponent<BoxCollider> ();

        //itemString = new List<string>();
        //itemController = new List<string>();
        //itemString.Add("items/Banana/banana.png");

        //itemController.Add("items/Banana/banana_0.controller");

        updateItem();
    }
	
	// Update is called once per frame
	void Update () {
		// Float up/down with a Sin()
		tempPos = posOffset;
		tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;

		transform.position = tempPos;

		/*
		 * Pickup functionality
		 */
		if (!hasBeenUsed) {
			Collider[] cols = Physics.OverlapBox (col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask ("Hitbox"));

			foreach (Collider c in cols) {
				if (c.tag == "Player") {
					PlayerController player = c.gameObject.GetComponentInParent<PlayerController> ();

                    // switch on the item type
                    switch (itemIndex)
                    {
                        case 1: // banana
                            player.heal(player.maxHealth/5);
                            break;
                        case 2: // pill 
                            player.tempAttackUp(Time.time);
                            break;
                        case 3: // ring - defense
                            player.speed = player.speed + 1;
                            break;
                        case 4: // potion
                            player.heal(player.maxHealth/2);
                            break;
                        case 5: // sword
                            player.strength = player.strength + 1;
                            break;
                        //case 6: // tstar abandoned
                        //    break;
                    }



					
					hasBeenUsed = true;

					/* FIXME */
					// Add to inventory here
					gameObject.SetActive (false);
				}
			}
		}
    }

    void updateItem()
    {
		
        //itemIndex = Random.Range(0, 0);
        //spriteRenderer.sprite = AssetDatabase.LoadAssetAtPath("Assets/" + itemString[itemIndex], typeof(Sprite)) as Sprite;
        //anim.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath("Assets/" + itemController[itemIndex], typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        spriteRenderer.sprite = sprite;
        anim.runtimeAnimatorController = rac;
    }
}
