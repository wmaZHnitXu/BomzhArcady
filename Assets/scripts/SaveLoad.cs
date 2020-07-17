using UnityEngine;
using System.Collections.Generic;
using System.IO;
public class SaveLoad : MonoBehaviour
{
    struct buildingAndInvId
    {
        public int buildingId;
        public int invId;
    }
    struct ObjectData
    {
        public int id;
        public Vector3 pos;
        public Quaternion rot;
        public bool isItem; //or npc
        public ObjectData(int id_, Vector3 pos_, Quaternion rot_, bool isItem_)
        {
            id = id_;
            pos = pos_;
            rot = rot_;
            isItem = isItem_;
        }
    }
    private List<Save> saves;
    private Save currentSave;
    class Save
    {
        public string name;
        public List<ObjectData> objects;
        public int playerHealth;
        public int playerHunger;
        public int playerWater;
        public int playerMoney;
        public List<Inventory> inventories;
        public List<buildingAndInvId> buildings;
        public int Day;
        public int TimeOfDay;
    }
    private string inJsonData;
    void Start()
    {
        inJsonData = JsonUtility.ToJson(currentSave);
    }
    public void GenerateCurrentSave (string saveName = "Autosave") {
        currentSave = new Save();
        currentSave.name = saveName;
        currentSave.objects = new List<ObjectData>();
        foreach (itemCtrl item in FindObjectsOfType<itemCtrl>()) {
            currentSave.objects.Add(new ObjectData(item.id, item.transform.position, item.transform.rotation, true));
        }
        foreach (hpBase entity in FindObjectsOfType<hpBase>()) {
            if (!entity.isStatic)
                currentSave.objects.Add(new ObjectData(entity.Id, entity.transform.position, entity.transform.rotation, false));
        }
        currentSave.playerHealth = characterctrl.it.hp;
        currentSave.playerHunger = characterctrl.eat;
        currentSave.playerWater = characterctrl.water;
        currentSave.playerMoney = characterctrl.money;
        currentSave.Day = timeCtrl.me.day;
        currentSave.TimeOfDay = timeCtrl.me.time;
        currentSave.inventories = new List<Inventory>();
        //currentSave.inventories.Add(new Inventory(inventoryCtrl.me.items[,]))

    }

    public void PrintAllSaves () {

    }
    public void Read () {

    }
    public void Write () {

    }
}
