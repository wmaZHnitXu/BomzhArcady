using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class telega : MonoBehaviour
{
    [SerializeField] private Vector3 correction;
    [SerializeField] private Vector3 defPos;
    [SerializeField] public List<tovarBolvanka> tovariInside = new List<tovarBolvanka>();
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int costInside;
    [SerializeField] private Text costInsideText;
    private bool enoughMoney;
    public void AddTovar (tovarBolvanka tovar) {
        tovariInside.Add(tovar);
        costInside += tovar.cost;
        UpdateCost();
    }
    public void RemoveTovar (tovarBolvanka tovar) {
        tovariInside.Remove(tovar);
        costInside -= tovar.cost;
        UpdateCost();
    }
    private void Awake () {
        defPos = transform.position;
    }
    private void OnEnable()
    {
        
    }

    private void Update()
    {
        rb.velocity = (characterctrl.me.position + correction - transform.position) * 20f;
    }
    private void OnDisable () {
        foreach (tovarBolvanka bolvan in FindObjectsOfType<tovarBolvanka>()) Destroy(bolvan.gameObject);
        transform.position = defPos;
        rb.velocity = Vector3.zero;
        tovariInside.Clear();
    }
    private void UpdateCost () {
        enoughMoney = costInside <= characterctrl.money;
        costInsideText.text = "Оплатить: " + (enoughMoney ? "<color=white>" : "<color=#ff3248>") + costInside.ToString() + "</color>";
    }
    public void Buy () {
        if (!enoughMoney) return;
        foreach (tovarBolvanka bolvanka in tovariInside) {
            Instantiate(inventoryCtrl.me.prefabs[bolvanka.Id], bolvanka.transform.position, bolvanka.transform.rotation)
            .GetComponent<itemCtrl>().Pull(true);
            Destroy(bolvanka.gameObject);
        }
        characterctrl.money -= costInside;
        tovariInside.Clear();
    }
}
