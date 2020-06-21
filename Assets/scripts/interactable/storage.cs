using UnityEngine;

public class storage : MonoBehaviour
{
    public string storageName;
    public int capacity;
    public int[,] itemsInside;

    void Start () {
        itemsInside = new int[capacity, 2];
        AddItem(4);
        AddItem(5);
        AddItem(6);
        AddItem(7);
        AddItem(8);
        AddItem(9);
        inventoryCtrl.me.AddItem(12);
    }
    public bool AddItem (int itemId) {
        for (int i = 0; i < itemsInside.GetLength(0); i++) {
            if (itemsInside[i,0] == 0) {
                itemsInside[i,0] = itemId;  
            }
            if (itemsInside[i,0] == itemId) {
                itemsInside[i,1]++;
                return true;
            } 
        }
        return false;
    }
    public void RemoveItem (int itemId) {
        for (int i = 0; i < itemsInside.GetLength(0); i++) {
            if (itemsInside[i,0] == itemId) {
                itemsInside[i,1]--;
                if (itemsInside[i,1] == 0) itemsInside[i,0] = 0;
                return;
            }
        }
    }
    public int PullAnyItem () {
        for (int i = 0; i < capacity; i++) {
            if (itemsInside[i,0] != 0) return itemsInside[i,0];
        }
        return 0;
    }
    public void OpenInterface () {
        storageCanvas.me.store = this;
        storageCanvas.me.gameObject.SetActive(true);
    }
}
