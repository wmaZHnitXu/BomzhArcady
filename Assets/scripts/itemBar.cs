using UnityEngine;
using UnityEngine.UI;

public class itemBar : MonoBehaviour
{
    [SerializeField] private Image[] images = new Image[5];
    [SerializeField] private Text[] texts = new Text[5];
    [SerializeField] private int[] items = new int[5];
    public static itemBar me;
    private void Start () {
        me = this;
        inventoryCtrl.me.OnInventoryModified += InventoryModified;
    }
    
    public void SetItem (int itemId, int slot) {
        images[slot].sprite = inventoryCtrl.me.sprites[itemId];
        texts[slot].text = inventoryCtrl.me.GetCountOfItems(itemId).ToString();
        items[slot] = itemId;
    }
    public void OnSlotClicked (int slot) {
        if (!inventoryCtrl.me.isEnabled)
            inventoryCtrl.me.UseItemFromOutside(items[slot]);
        else {
            if (inventoryCtrl.me.itemClickedId != 0 && inventoryCtrl.me.itemClickedId != items[slot]) SetItem(inventoryCtrl.me.itemClickedId, slot);
            else RemoveItemFromSlot(slot);
        }
    }
    public void InventoryModified () {
        for (int i = 0; i < 5; i++) {
            if (items[i] != 0 ) {
                int count = inventoryCtrl.me.GetCountOfItems(items[i]);
                if (count <= 0) RemoveItemFromSlot(i);
                else texts[i].text = count.ToString();
            }
        }
    }
    public void RemoveItemFromSlot (int slot) {
        texts[slot].text = string.Empty;
        images[slot].sprite = inventoryCtrl.me.emptySprite;
        items[slot] = 0;
    }
}
