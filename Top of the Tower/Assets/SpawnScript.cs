using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

    GameObject item;
    List<string> itemString;
    List<string> itemController;
    int itemIndex;
    int transparency;
    bool decreasing = true;

    void Start () {
        decreasing = true;
        transparency = 100;
        itemString = new List<string>();
        itemController = new List<string>();
        itemString.Add("Banana/unnamed.png");
        itemString.Add("Pill/pill.png");
        itemString.Add("Potion1/HealthPotion1spritemap.png");
        itemString.Add("Ring/ringOfStrengthspritemap.png");
        itemString.Add("Sword/sword.png");
        itemString.Add("ThrowingStar/tstarpng.png");

        itemController.Add("Banana/unnamed_0.controller");
        itemController.Add("Pill/pill_0.controller");
        itemController.Add("Potion1/HealthPotion1spritemap_0.controller");
        itemController.Add("Ring/ringOfStrengthspritemap_0.controller");
        itemController.Add("Sword/sword_0.controller");
        itemController.Add("ThrowingStar/tstarpng_0.controller");

        item = new GameObject();
        item.name = "RandomSpawnItem";
        item.transform.position = new Vector3(15f, 0f, 2f);
        item.AddComponent<SpriteRenderer>();
        item.AddComponent<Animator>();
        updateItem();
        
    }
	
	// Update is called once per frame
	void Update () {
        
        Color spt = item.GetComponent<SpriteRenderer>().color;
        
        spt.a = (float)(transparency / 100.0);
        item.GetComponent<SpriteRenderer>().color = spt;
        Debug.Log(item.GetComponent<SpriteRenderer>().color.a);

        if (transparency == 0)
        {
            updateItem();
        }
        
        if (decreasing)
        {
            transparency = transparency - 1;
            if (transparency < 0)
            {
                decreasing = false;
            }
        }
        else
        {
            transparency = transparency + 1;
            if (transparency > 100)
            {
                decreasing = true;
            }
        }

    }

    void updateItem()
    {
        SpriteRenderer spt = item.GetComponent<SpriteRenderer>();
        itemIndex = Random.Range(0, 6);
        //itemIndex = 5;
        spt.sprite = AssetDatabase.LoadAssetAtPath("Assets/" + itemString[itemIndex], typeof(Sprite)) as Sprite;

        Animator anim = item.GetComponent<Animator>();
        anim.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath("Assets/" + itemController[itemIndex], typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        //item.transform.localScale = new Vector3(10f,10f,10f);
    }
}
