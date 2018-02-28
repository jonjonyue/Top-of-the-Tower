using UnityEngine;
using System.Collections;

public class character : MonoBehaviour
{
	// Enums
	public enum charType {player, enemy};

	// Informational
	public string name;

	// General Stats
	public int health;
	public int defense;
	public int speed;

	// Code variables
	public bool isAlive = true;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

