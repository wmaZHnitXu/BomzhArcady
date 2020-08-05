using UnityEngine;
using System.Collections.Generic;

public class tovarBolvanka : MonoBehaviour
{
    public int cost;
    private bool insideTelegi;
    [SerializeField] private float telegaDist;
    [SerializeField] private Transform telegaTransform;
    private telega teleg;
    [SerializeField] private SpriteRenderer sr;
    private static List<GameObject> bolvano4ki = new List<GameObject>();
    public int Id;
    public void Init (int id, int cost, Transform telegaTransform) {
        bolvano4ki.Add(gameObject);
        this.cost = cost;
        Id = id;
        sr.sprite = inventoryCtrl.me.sprites[id];
        this.telegaTransform = telegaTransform;
        teleg = telegaTransform.GetComponent<telega>();
    }

    void Update()
    {
        float distance = (telegaTransform.position - transform.position).sqrMagnitude;
        if (distance <= telegaDist) {
            if (!insideTelegi) SwitchInside(true);
        }
        else {
            if (insideTelegi) SwitchInside(false);
        }
    }
    private void SwitchInside (bool inside) {
        insideTelegi = inside;
        if (inside) {
            teleg.AddTovar(this);
        }
        else {
            teleg.RemoveTovar(this);
        }
    }
    public static void DestroyAll () {
        foreach(GameObject g in bolvano4ki) {
            Destroy(g);
        }
        bolvano4ki.Clear();
    }
}
