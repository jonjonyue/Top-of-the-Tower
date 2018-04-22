using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class character : MonoBehaviour
{
	// Enums
	public enum charType {player, enemy};

	// Informational
	public string charName;

    [Header("Stats")]
	public int health;
	public int defense;
	public int speed;
    public int strength;

	// Code variables
    [HideInInspector] public bool isAlive = true;
    [HideInInspector] public bool attacking = false;

    // GUI
    public Slider healthSlider;
    public GameObject damageNumber;

    protected static int killed = 0;

    protected bool counted = false;

    protected static object locker = new object();

    public int curHP()
    {
        return health;
    }

    virtual public void takeDamage(int damage) {
		health -= damage;

        Debug.Log (gameObject.name + " taking " + damage + " damage.");
		if (health <= 0) {
			Destroy (gameObject);
		}
	}

    virtual public void takeCombatDamage(int damage) {
        damage -= defense;
        if (damage < 0)
            health -= damage;

        Debug.Log(gameObject.name + " taking " + damage + " damage.");
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}

