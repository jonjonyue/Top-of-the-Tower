using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackCollider : MonoBehaviour
{

	public character Char;

	List<GameObject> nearEnemy = new List<GameObject> ();

	void OnTriggerEnter (Collider col)
	{
		if (gameObject.tag == "PlayerAttack" && col.gameObject.tag == "Enemy") {
			nearEnemy.Add (col.gameObject);
		} else if (gameObject.tag == "EnemyAttack" && col.gameObject.tag == "Player") {
//			print ("Entered range for player");
			nearEnemy.Add (col.gameObject);
		}
	}

	void OnTriggerExit (Collider col)
	{
		if (gameObject.tag == "PlayerAttack" && col.gameObject.tag == "Enemy") {
			nearEnemy.Remove (col.gameObject);
		} else if (gameObject.tag == "EnemyAttack" && col.gameObject.tag == "Player") {
			nearEnemy.Remove (col.gameObject);
		}
	}

	public IEnumerator Attack ()
	{

		gameObject.SetActive (true);

		Char.attacking = true;

		var colors = new Color[2];

//		Debug.Log ("Starting Attack");

		for (int i = 0; i < nearEnemy.Count; i++) {
			int damage = 2;
//			if (Object.Equals (nearEnemy [i], null)) {
//				print ("Target is not null");
			colors [1] = nearEnemy [i].GetComponent<SpriteRenderer> ().color;
			colors [0] = Color.red;
			//				print("attacking");
			character cha = nearEnemy [i].GetComponent<character> ();
			yield return new WaitForSeconds (.4f);
			nearEnemy [i].GetComponent<SpriteRenderer> ().color = colors [0];
			yield return new WaitForSeconds (0.25f);
			nearEnemy [i].GetComponent<SpriteRenderer> ().color = colors [1];
			cha.takeDamage (damage);
			gameObject.SetActive (false);
//			}
		}
//		Debug.Log ("Done Attacking");
		Char.attacking = false;
	}
}
