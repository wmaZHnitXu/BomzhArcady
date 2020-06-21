using UnityEngine;
using UnityEngine.UI;

public class storageCanvas : MonoBehaviour
{
    public storage store;
    [SerializeField] private Image[] inventoryImgs;
    [SerializeField] private Text[] inventoryCounts;
    [SerializeField] private Image[] storageImgs;
    [SerializeField] private Image[] storageBacks; //Чтобы не отрисовывать ненужные ячейки хранилища
    [SerializeField] private Text[] storageCounts;
    private int selectedItem;
    [SerializeField] private Transform selector;
    [SerializeField] private Image selectorImg;
    [SerializeField] private Text storageName;
    public static storageCanvas me;
    
    public storageCanvas() {
        me = this;
    }
    void OnEnable () {
        RenderInventory();
        RenderStorage();
        storageName.text = store.storageName; 
    }
    public void RenderInventory () {
        for (int i = 0; i < 16; i++) {
            int x = i % 4;
            int y = i / 4;
            inventoryImgs[i].sprite = inventoryCtrl.me.sprites[inventoryCtrl.me.items[x, y, 0]];
            int count = inventoryCtrl.me.items[x, y, 1];
            inventoryCounts[i].text = count > 0 ? count.ToString() : string.Empty;
        }
    }
    public void RenderStorage () {
        for (int i = 0; i < 16; i++) {
            if (i < store.itemsInside.GetLength(0)) {
                storageImgs[i].sprite = inventoryCtrl.me.sprites[store.itemsInside[i, 0]];
                int count = store.itemsInside[i, 1];
                storageImgs[i].enabled = true;
                storageCounts[i].text = count > 0 ? count.ToString() : string.Empty;
                storageBacks[i].enabled = true;
            }
            else {
                storageBacks[i].enabled = false;
                storageCounts[i].text = string.Empty;
                storageImgs[i].enabled = false;
            }
        } 
    }
    public void StorageButtonCallBack (int buttonId) {
        selector.position = storageBacks[buttonId].transform.position;
        int i = store.itemsInside[buttonId,0];
        if (i != 0) {
            if (inventoryCtrl.me.AddItem((byte)i)) {
                store.RemoveItem(i);
                RenderStorage();
                RenderInventory();
                selectorImg.color = Color.white;
            }
            else {
                selectorImg.color = Color.red;
            }
        }
    }
    public void InventoryButtonCallBack (int buttonId) {
        selector.position = inventoryImgs[buttonId].transform.position;
        int i = inventoryCtrl.me.items[buttonId % 4, buttonId / 4, 0];
        if (i != 0) {
            if (store.AddItem(i)) {
                inventoryCtrl.me.RemoveItem((byte)i);
                RenderStorage();
                RenderInventory();
                selectorImg.color = Color.white;
            }
            else {
                selectorImg.color = Color.red;
            }
        }
    }
}
