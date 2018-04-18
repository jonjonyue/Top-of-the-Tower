using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FloatingText: MonoBehaviour
{

    public float moveSpeed;
    public int damageNumber;
    public Text displayNumber;
    private int sanity = 0;

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{

        displayNumber.text = "" + damageNumber;
        if (damageNumber >= 0)
        {
            displayNumber.color = Color.green;
        }
        else
        {
            displayNumber.color = Color.red;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y + (moveSpeed * Time.deltaTime), transform.position.z);
	}
}
