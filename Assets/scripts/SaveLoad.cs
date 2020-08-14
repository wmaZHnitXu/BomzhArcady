using UnityEngine;
using System.Collections.Generic;
using System.Collections;
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
    [SerializeField]
    private List<Save> saves = new List<Save>();
    private Save currentSave;
    [System.Serializable]
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
    private string savePath;
    [SerializeField] private Text PlayText;
    void Start()
    {
        Debug.Log(Application.dataPath);
        savePath = Application.dataPath + "/save";
        if (File.Exists(savePath)) Read();
        me = this;
        if (saves != null && saves.Count != 0) {
            selectedSave = saves.Count-1;
            Load();
            PlayText.text = "Продолжить";
        }
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
        RemoveAllSavePanels();
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
        saves = new List<Save>();
        string stringSaves = File.ReadAllText(savePath);
        var splitedSaves = stringSaves.Split('#');
        foreach (string save in splitedSaves) {
            if (save != string.Empty) {
                string[] splitedSave = save.Split('@');
                saves.Add(DeserializeStats(splitedSave[0]));
                saves[saves.Count-1].inventories = DeserializeInventories(splitedSave[1]);
                saves[saves.Count-1].objects = DeserializeObjects(splitedSave[2]);

            }
        }
        PrintAllSaves();
    }
    public void Write () {
        string result = string.Empty;
        foreach (Save save in saves) {
            string save1 = SerializeStats(save);
            string save2 = SerializeInventories(save.inventories);
            string save3 = SerializeObjects(save.objects);
            result = string.Concat(result, save1, "@", save2, "@" , save3, "#");
        }
        File.WriteAllText(savePath, result);
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
        characterctrl.it.Init();
        int saveId = selectedSave; 
        var entities = FindObjectsOfType<hpBase>();
        foreach (hpBase hp in entities) {
            if (!hp.isStatic) Destroy(hp);
        }     
        Save loadSave = saves[saveId];
        timeCtrl.me.day = loadSave.Day;
        timeCtrl.me.SetTime(loadSave.TimeOfDay);
        characterctrl.me.position = loadSave.playerPos;
        characterctrl.it.SetStatsFromSave(loadSave.playerHunger, loadSave.playerWater, loadSave.playerHealth, loadSave.playerMoney);
        Debug.Log(loadSave.inventories != null); Debug.Log(loadSave.objects != null);
        inventoryCtrl.me.Set2DimArrayToInventory(loadSave.inventories[0]);
        foreach (ObjectData objectData in loadSave.objects) {
            if (objectData.id != 0) {
                GameObject prototype = objectData.isItem ? inventoryCtrl.me.prefabs[objectData.id] : npcManager.me.GetNpcWithId(objectData.id);
                Instantiate(prototype, objectData.pos, objectData.rot); //0 может быть у денежек.
            }
        }
    }
    public void StartGame () {
        timeCtrl.me.GoTime = true;
        startScreenCtrl.me.StartGame();
    }
    public void Delete () {
        saves.Remove(saves[selectedSave]);
        PrintAllSaves();
        SelectSave();
        Write();
    }
    public void SaveSave () {
        GenerateCurrentSave();
        currentSave.name = nameField.text;
        saves.Add(currentSave);
        PrintCurrentSave();
        Write();
    }
    public void SwitchSaveLoad (bool save) {
        SaveLoadButtons[0].SetActive(save);
        SaveLoadButtons[1].SetActive(save);
        SaveLoadButtons[2].SetActive(!save);
        SaveLoadButtons[3].SetActive(!save);
    }
    private void RemoveAllSavePanels () {
        for (int i = 0; i < ContentTransform.childCount; i++) {
            Destroy(ContentTransform.GetChild(i).gameObject);
        }
    }
    #region Serialization/Deserialization
    private string SerializeStats (Save sv) {
        string result = string.Empty;
        result = string.Concat(
            sv.name, ":",
            sv.date, ":",
            sv.playerPos.x.ToString(), ":",
            sv.playerPos.y.ToString(), ":",
            sv.playerHealth.ToString(), ":",
            sv.playerHunger.ToString(), ":",
            sv.playerWater.ToString(), ":",
            sv.playerMoney.ToString(), ":",
            sv.TimeOfDay.ToString(), ":",
            sv.Day.ToString()
        );
        return result;
    }
    private string SerializeInventories (List<int[,]> inventories) {
        string result = string.Empty;
        foreach (int[,] inventory in inventories) {
            string oneinvent = string.Empty;
            for (int i = 0; i < 16; i++) {
                oneinvent += inventory[i, 0].ToString() + "," + inventory[i, 1].ToString() + ",";
            }
            result += oneinvent + ";";
            result = result.Remove(result.Length-2);
        }
        return result;
    }
    private string SerializeObjects (List<ObjectData> objects) {
        string result = string.Empty;
        foreach (ObjectData obj in objects) {
            result = string.Concat(
                result,
                obj.id.ToString(), ":",
                obj.pos.x.ToString(), ":",
                obj.pos.y.ToString(), ":",
                obj.pos.z.ToString(), ":",
                obj.rot.eulerAngles.z.ToString(), ":",
                obj.isItem.ToString(), ";"
            );
        }
        result = result.Remove(result.Length-1);
        return result;
    }
    private Save DeserializeStats (string save1) {
        Save result = new Save();
        Debug.Log(save1);
        string[] splitedStats = save1.Split(':');
        result.name = splitedStats[0];
        result.date = splitedStats[1];
        result.playerPos = new Vector3(float.Parse(splitedStats[2]), float.Parse(splitedStats[3]), 0);
        result.playerHealth = int.Parse(splitedStats[4]);
        result.playerHunger = int.Parse(splitedStats[5]);
        result.playerWater = int.Parse(splitedStats[6]);
        result.playerMoney = int.Parse(splitedStats[7]);
        result.TimeOfDay = int.Parse(splitedStats[8]);
        result.Day = int.Parse(splitedStats[9]);
        return result;
    }
    private List<int[,]> DeserializeInventories (string save2) {
        List<int[,]> result = new List<int[,]>();
        string[] splitedInventories = save2.Split(';');
        foreach (string inventory in splitedInventories) {
            int[,] inv;
            result.Add(inv = new int[16,2]);
            string[] splitedInventory = inventory.Split(',');
            //foreach(string splt in splitedInventory) Debug.Log(splt);
            for (int i = 0; i < 31; i += 2) {
                Debug.Log(splitedInventory[i]);
                inv[i / 2, 0] = int.Parse(splitedInventory[i]);
                inv[i / 2, 1] = int.Parse(splitedInventory[i+1]);
            } 
        }
        return result;
    }
    private List<ObjectData> DeserializeObjects (string save3) {
        List<ObjectData> result = new List<ObjectData>();
        string[] splitedObjects = save3.Split(';');
        foreach (string obj in splitedObjects) {
            Debug.Log(obj);
            string[] splitedObj = obj.Split(':');
            result.Add(new ObjectData(
                int.Parse(splitedObj[0]),
                new Vector3(float.Parse(splitedObj[1]), float.Parse(splitedObj[2]), float.Parse(splitedObj[3])),
                Quaternion.Euler(0,0,float.Parse(splitedObj[4])),
                bool.Parse(splitedObj[5])
            ));
        }
        return result;
    }
    #endregion
}
