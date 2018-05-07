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
        transform.SetSiblingIndex(itemIndex);
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
                    GameObject clone = null;
                    // switch on the item type
                    switch (itemIndex)
                    {
                        case 0: // banana
                            player.heal(player.maxHealth/8);
                            break;
                        case 1: // pill 
                            player.tempAttackUp(Time.time);
                            player.strengthText.text = "TEMP " + player.strength;
                            player.statsInfo("Temparoray Attack +1");
                            break;
                        case 2: // ring - defense
                            player.speed = player.speed + .2f;
                            player.speedText.text = "" + player.speed;
                            clone = (GameObject)Instantiate(player.damageNumber, player.transform.position + new Vector3(0, 1, 0), Quaternion.Euler(Vector3.zero), player.transform);
                            clone.GetComponent<FloatingText>().setText("+0.2 Speed");
                            break;
                        case 3: // potion
                            player.heal(player.maxHealth/4);
                            break;
                        case 4: // sword
                            player.strength = player.strength + 1;
                            player.strengthText.text = "" + player.strength;
                            clone = (GameObject)Instantiate(player.damageNumber, player.transform.position + new Vector3(0, 1, 0), Quaternion.Euler(Vector3.zero), player.transform);
                            clone.GetComponent<FloatingText>().setText("+1 Strength");
                            break;
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
