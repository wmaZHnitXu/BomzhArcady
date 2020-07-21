using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
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
    private List<Save> saves = new List<Save>();
    private Save currentSave;
    class Save
    {
        public string name;
        public List<ObjectData> objects;
        public Vector3 playerPos;
        public int playerHealth;
        public int playerHunger;
        public int playerWater;
        public int playerMoney;
        public List<int[,]> inventories;
        public List<buildingAndInvId> buildings;
        public int Day;
        public int TimeOfDay;
        public string date;
    }
    [SerializeField] private GameObject SavePanelPrefab;
    [SerializeField] private float StepUnderSavesInList;
    [SerializeField] private Transform ContentTransform;
    private string CurrentSaveInJson;
    public static SaveLoad me;
    [SerializeField] private Animator panelAnim;
    [SerializeField] private int selectedSave;
    [SerializeField] private GameObject[] SaveLoadButtons = new GameObject[4];
    [SerializeField] private InputField nameField;
    void Start()
    {
        me = this;
        PrintAllSaves();
        CurrentSaveInJson = JsonUtility.ToJson(currentSave);
    }
    public void GenerateCurrentSave (string saveName = "<color=#f0b129>Автосохранение</color>") {
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
        currentSave.playerPos = characterctrl.me.position;
        currentSave.playerHealth = characterctrl.it.hp;
        currentSave.playerHunger = characterctrl.eat;
        currentSave.playerWater = characterctrl.water;
        currentSave.playerMoney = characterctrl.money;
        currentSave.Day = timeCtrl.me.day;
        currentSave.TimeOfDay = timeCtrl.me.time;
        currentSave.inventories = new List<int[,]>();
        currentSave.inventories.Add(inventoryCtrl.me.Get2DimInventory());
        currentSave.date = System.DateTime.Now.Day.ToString() + "." + System.DateTime.Now.Month.ToString() + "." + System.DateTime.Now.Year.ToString();
    }
    private void PrintAllSaves () {
        for (int i = 0; i < saves.Count; i++) {
            Instantiate(SavePanelPrefab, ContentTransform.position + new Vector3(0,-30 - StepUnderSavesInList * i, 0), Quaternion.identity, ContentTransform)
            .transform.GetChild(0).GetComponent<savePanel>()
            .Initialize(i, saves[i].name, saves[i].date, "День " + saves[i].Day.ToString() + ", " + (saves[i].TimeOfDay/60).ToString() + ":" +  (saves[i].TimeOfDay%60).ToString());
        }
    }
    private void PrintCurrentSave () {
        int i = saves.Count-1;
        Instantiate(SavePanelPrefab, ContentTransform.position + new Vector3(0,-30 - StepUnderSavesInList * i, 0), Quaternion.identity, ContentTransform)
        .transform.GetChild(0)  .GetComponent<savePanel>()
        .Initialize(i, saves[i].name, saves[i].date, "День " + saves[i].Day.ToString() + ", " + (saves[i].TimeOfDay/60).ToString() + ":" +  (saves[i].TimeOfDay%60).ToString());
    }
    public void Read () {

    }
    public void Write () {

    }
    public void PullSavesPanel () {
        bool b = !panelAnim.GetBool("pull");
        PullSavesPanel(b);
    }
    public void PullSavesPanel (bool pull) {
        panelAnim.SetBool("pull", pull);
        if (!pull) foreach(GameObject g in SaveLoadButtons) g.SetActive(false);
        else SwitchSaveLoad(true);
    }
    public void SelectSave (savePanel savePan = null) {
        bool deselected = !savePan;
        if (deselected) {
            SwitchSaveLoad(true);
            selectedSave = 999999;
        }
        else {
            SwitchSaveLoad(false);
            selectedSave = savePan.idOfSave;
        }
    }
    public void Load () {
        int saveId = selectedSave;
        startScreenCtrl.me.StartGame();
        Save loadSave = saves[saveId];
        timeCtrl.me.day = loadSave.Day;
        timeCtrl.me.time = loadSave.TimeOfDay;
        characterctrl.me.position = loadSave.playerPos;
        characterctrl.it.SetStatsFromSave(loadSave.playerHunger, loadSave.playerWater, loadSave.playerHealth, loadSave.playerMoney);
        inventoryCtrl.me.Set2DimArrayToInventory(loadSave.inventories[0]);
        foreach (ObjectData objectData in loadSave.objects) {
            GameObject prototype = objectData.isItem ? inventoryCtrl.me.prefabs[objectData.id] : npcManager.me.GetNpcWithId(objectData.id);
            Instantiate(prototype, objectData.pos, objectData.rot);
        }
    }
    public void Delete () {
        
    }
    public void SaveSave () {
        GenerateCurrentSave();
        currentSave.name = nameField.text;
        saves.Add(currentSave);
        PrintAllSaves();
        Write();
    }
    public void SwitchSaveLoad (bool save) {
        SaveLoadButtons[0].SetActive(save);
        SaveLoadButtons[1].SetActive(save);
        SaveLoadButtons[2].SetActive(!save);
        SaveLoadButtons[3].SetActive(!save);
    }
}
