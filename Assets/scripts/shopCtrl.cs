using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public struct item {
    public GameObject obj;
    public int cost;
    public byte id;
    public byte needLvl;
    public GameObject panObj;
    public Image objImag;
    public Text costText;
    public GameObject locker;
}

public class shopCtrl : MonoBehaviour
{
    public Color[] textColors = new Color[2];
    public item[] items;
    public Button buyB;
    public int selectedItem;
    public Text description;
    public Text itemName;
    public Image itemImg;
    public Transform lootSpawnPoint;
    // Start is called before the first frame update
    public void OnEnable()
    {
        foreach(item i in items) {
            i.costText.text = (i.cost.ToString() + " руб.");
            i.objImag.sprite = inventoryCtrl.me.sprites[i.id];
            if (characterctrl.level < i.needLvl)
                i.panObj.SetActive(false);
            else 
                i.panObj.SetActive(true);
            if (i.cost > characterctrl.money) {
                i.costText.color = textColors[1];
            }
            else {
                i.costText.color = textColors[0];
            }
        }
    }
    public void Select (int myId) {
        selectedItem = myId;
        description.text = inventoryCtrl.me.structs[items[selectedItem].id].descr;
        itemName.text = inventoryCtrl.me.structs[items[selectedItem].id].name;
        itemImg.sprite = inventoryCtrl.me.sprites[items[selectedItem].id];
        if (items[selectedItem].cost > characterctrl.money) {
            buyB.interactable = false;
        }
        else {
            buyB.interactable = true;
        }
    }
    public void Buy () {
        Instantiate(inventoryCtrl.me.prefabs[items[selectedItem].id],lootSpawnPoint.position,lootSpawnPoint.rotation);
        characterctrl.money -= items[selectedItem].cost;
    }
}
