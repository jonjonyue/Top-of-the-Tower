using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackCollider : MonoBehaviour {

	void Start() {
	}

	List<GameObject> nearEnemy = new List<GameObject>();

	void OnTriggerEnter(Collider col)
	{
		if (gameObject.tag == "PlayerAttack" && col.gameObject.tag == "Enemy") {
			nearEnemy.Add (col.gameObject);
		} else if (gameObject.tag == "EnemyAttack" && col.gameObject.tag == "Player") {
			nearEnemy.Add (col.gameObject);
		}
	}
	void OnTriggerExit(Collider col)
	{
		if (gameObject.tag == "PlayerAttack" && col.gameObject.tag == "Enemy") {
			nearEnemy.Remove (col.gameObject);
		} else if (gameObject.tag == "EnemyAttack" && col.gameObject.tag == "Player") {
			nearEnemy.Remove (col.gameObject);
		}
	}

	public IEnumerator Attack () {

		var colors = new Color[2];

		for(int i = 0; i < nearEnemy.Count;i++)
		{
			int damage = 2;
			colors [1] = nearEnemy[i].GetComponent<SpriteRenderer>().color;
			colors [0] = Color.red;
			//				print("attacking");
			character cha = (character)nearEnemy[i].GetComponent<character> ();
			yield return new WaitForSeconds (.5f);
			nearEnemy[i].GetComponent<Renderer>().material.color = colors[0];
			yield return new WaitForSeconds(0.25f);
			nearEnemy[i].GetComponent<Renderer>().material.color = colors[1];
			cha.takeDamage (damage);
		}
	}
}
