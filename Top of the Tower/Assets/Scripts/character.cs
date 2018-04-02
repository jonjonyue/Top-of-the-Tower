using UnityEngine;
using System.Collections;

public class character : MonoBehaviour
{
	// Enums
	public enum charType {player, enemy};

	// Informational
	public string charName;

	// General Stats
	public int health;
	public int defense;
	public int speed;

	// Code variables
	public bool isAlive = true;


    // Game status related
    protected static int killed = 0;
    
    protected bool counted = false;

    protected static object locker = new object();

    public int curHP()
    {
        return health;
    }
}

