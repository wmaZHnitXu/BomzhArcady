using UnityEngine;
public struct Inventory
{
    public int[,] itemsInStorage;
    public Inventory (int[,] array) {
        itemsInStorage = array;
    }
}
