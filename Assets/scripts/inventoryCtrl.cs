using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public struct weaponStruct {
    public float shootDelay;
    public float reloadSpeed;
    public GameObject bullet, mag;
    public Sprite[] sprite;
    public int[] ammo;
    public bool melee; //В ammo[0] кладется дамаг
}
public class inventoryCtrl : MonoBehaviour
{
    public static inventoryCtrl me;
    public GameObject clickPanel;
    public byte[,,] items = new byte[4,4,2];
    // Start is called before the first frame update
    public Image[] images = new Image[16];
    public Sprite[] sprites;
    public Text[] texts = new Text[16];
    public byte itemClickedId;
    public byte invClickedId;
    public SpriteRenderer inHandImg;
    public bool isEnabled;
    public GameObject inventory;
    public itemStruct[] structs;
    public GameObject[] prefabs;
    public weaponStruct[] weapons;
    public bool withWeapon;
    public characterctrl ctrl;
    public SpriteRenderer inHandWeapon;
    public static byte wantedLevel;
    [SerializeField]
    private Text descText;
    [SerializeField]
    private Image descImage;
    [SerializeField]
    private Button useButton;
    [SerializeField]
    private Button trashButton;
    private int inHandItem;
    void OnEnable()
    {
        me = this;
        ctrl = characterctrl.it;
    }

    public bool AddItem(byte itemId) {
        bool added = false;
        for(byte i = 0; i < 4 & added == false; i++) {
            for(byte i2 = 0; i2 < 4 & added == false; i2++) {
                if (items[i2,i,0] == itemId | items[i2,i,0] == 0) {
                    Debug.Log(items[i2,i,0].ToString() + itemId.ToString() + (items[i2,i,0] == 0).ToString());
                    items[i2,i,0] = itemId;
                    items[i2,i,1]++;
                    added = true;//Debug.Log(i2.ToString()+i.ToString());
                }
            }
        }
        RenderInventory();
        return added;
    }
    public bool AddItem(byte itemId, byte count) {
        bool added = false;
        if (structs[itemId].type != 1) {
            for(byte i = 0; i < 4 & added == false; i++) {
                for(byte i2 = 0; i2 < 4 & added == false; i2++) {
                    if (items[i2,i,0] == itemId | items[i2,i,0] == 0) {
                        Debug.Log(items[i2,i,0].ToString() + itemId.ToString() + (items[i2,i,0] == 0).ToString());
                        items[i2,i,0] = itemId;
                        items[i2,i,1]+=count;
                        added = true;//Debug.Log(i2.ToString()+i.ToString());
                    }
                }
            }
        }
        RenderInventory();
        return added;
    }
    public void RenderInventory () {
         FormatInventory();
        for(byte i = 0; i < 4; i++) {
            for(byte i2 = 0; i2 < 4; i2++) {
                images[i2+(i*4)].sprite = sprites[items[i2,i,0]];
                texts[i2+(i*4)].text = items[i2,i,1].ToString();
                if (items[i2,i,1] == 0)
                    texts[i2 + (i*4)].enabled = false;
                else
                    texts[i2 + (i*4)].enabled = true;
                }
            }
        }
    public void ItemCallBack (int id) {
        itemClickedId = items[id % 4,id / 4,0];
        invClickedId = System.Convert.ToByte(id);
            if (itemClickedId != 0) { 
                Debug.Log(itemClickedId);
                clickPanel.SetActive(true);
                descImage.sprite = sprites[itemClickedId];
                descText.text = structs[itemClickedId].descr;
                useButton.interactable = true;
                trashButton.interactable = true;
                clickPanel.transform.position = images[id].transform.position;
            }
            else {
                clickPanel.SetActive(false);
                descText.text = string.Empty;
                descImage.sprite = sprites[0];
                useButton.interactable = false;
                trashButton.interactable = false;
            }
        }
    public void AddInHand () {
        if (structs[itemClickedId].type != 3) {
            inHandImg.sprite = sprites[itemClickedId];
            ToggleInventory();
            if (withWeapon) {
                DeactivateWeapon();
            }
        }
        else {
            WeaponInHand();
        }
        if (structs[itemClickedId].flip) {
            inHandImg.transform.localRotation = Quaternion.Euler(0,0,-90);
        }
        else
            inHandImg.transform.localRotation = Quaternion.Euler(0,0,0);
        inHandItem = itemClickedId;
    }
    public void ToggleInventory () {
        if (isEnabled) {
            isEnabled = false;
            inventory.SetActive(false);
            characterctrl.it.busy = false;
        }
        else {
            isEnabled = true;
            inventory.SetActive(true);
            RenderInventory();
            clickPanel.SetActive(false);
            descText.text = string.Empty;
            descImage.sprite = sprites[0];
            useButton.interactable = false;
            trashButton.interactable = false;
            itemClickedId = 0;
            characterctrl.it.busy = true;
        }
    }
    void OnMouseUp() {
        if (itemClickedId != 0 && isEnabled == false) {
            switch (structs[itemClickedId].type) {
                case 0:
                    characterctrl.Health = System.Convert.ToByte(structs[itemClickedId].health < 0 ? characterctrl.Health - System.Convert.ToByte(Mathf.Abs(structs[itemClickedId].health)) :  characterctrl.Health + System.Convert.ToByte(structs[itemClickedId].health));
                    characterctrl.Health = characterctrl.Health > 100 ? 100 : characterctrl.Health;
                    characterctrl.eat += structs[itemClickedId].eat;
                    characterctrl.eat = characterctrl.eat > 100 ? (byte)100 : characterctrl.eat;
                    characterctrl.water += structs[itemClickedId].water;
                    characterctrl.water = characterctrl.water > 100 ? (byte)100 : characterctrl.water;
                    characterctrl.exp += structs[itemClickedId].exp;
                break;
                case 4:
                break;
            }
            items[invClickedId % 4,invClickedId / 4,1] -= 1;
            if (items[invClickedId % 4,invClickedId / 4,1] <= 0) {
                items[invClickedId % 4,invClickedId / 4,0] = 0;
                itemClickedId = 0;
            }
            inHandImg.sprite = sprites[itemClickedId];
            RenderInventory();
        }
    }
    public void RemoveItem (byte id) {
        bool removed = false;
        for(byte i = 0; i < 4 & removed == false; i++) {
            for(byte i2 = 0; i2 < 4 & removed == false; i2++) {
                if (items[i2,i,0] == id) {
                    //Debug.Log(items[i2,i,0].ToString() + id.ToString() + (items[i2,i,0] == 0).ToString());
                    if (items[i2,i,1] == 1 ) {
                        items[i2,i,1] = 0;
                        items[i2,i,0] = 0;
                        useButton.interactable = false;
                        trashButton.interactable = false;
                        if (inHandItem == itemClickedId) {
                            if (withWeapon) {
                                DeactivateWeapon();
                            }
                            else {
                                inHandImg.sprite = sprites[0];
                            }
                            inHandItem = 0;
                        }
                    }
                    else
                        items[i2,i,1]--;
                    removed = true;//Debug.Log(i2.ToString()+i.ToString());
                }
            }
        }
        RenderInventory();
    }
    public void FormatInventory() {
        for (int id = 1; id < 16; id++) {
            if  (items[id % 4,id / 4,0] != 0 && items[(id-1) % 4,(id-1) / 4,0] == 0) {
                items[(id-1) % 4,(id-1) / 4,0] = items[id % 4,id / 4,0];
                //Debug.Log(id.ToString() + (id-1).ToString() + "Id:" + items[id % 4,id / 4,0].ToString());
                items[(id-1) % 4,(id-1) / 4,1] = items[id % 4,id / 4,1];
                items[id % 4,id / 4,0] = 0;
                items[id % 4,id / 4,1] = 0;
            }
        }
    }
    public void TrashItem() {
        Instantiate(prefabs[itemClickedId],transform.position,transform.rotation);
        RemoveItem(itemClickedId);
        clickPanel.transform.position = new Vector3(100f,100f,100f);
    }
    public void WeaponInHand () {
        DeactivateWeapon();
        inHandImg.sprite = null;
        withWeapon = true;
        ctrl.wsprites = weapons[structs[itemClickedId].weapid].sprite;
        inHandWeapon.sprite = weapons[structs[itemClickedId].weapid].sprite[0];
        if (!weapons[structs[itemClickedId].weapid].melee) {
            ctrl.arm.enabled = false;
            ctrl.weapon = structs[itemClickedId].weapid;
            ctrl.withCross = true;
            ctrl.magObj = weapons[structs[itemClickedId].weapid].mag;
            ctrl.bullet = weapons[structs[itemClickedId].weapid].bullet;
            ctrl.SetWeapon(true);
        }
        else {
            ctrl.meleeDamage = weapons[structs[itemClickedId].weapid].ammo[0];
        }
        ctrl.attackCd[0] =  weapons[structs[itemClickedId].weapid].reloadSpeed;
        ctrl.attackCd[1] =  weapons[structs[itemClickedId].weapid].shootDelay;
        ctrl.magazine = weapons[structs[itemClickedId].weapid].ammo;
        ToggleInventory();
    }
    void DeactivateWeapon () {
        ctrl.weapon = 0;
        ctrl.arm.enabled = true;
        inHandWeapon.sprite = null;
        withWeapon = false;
        ctrl.SetWeapon(false);
        ctrl.meleeDamage = weapons[0].ammo[0];
    }
}
