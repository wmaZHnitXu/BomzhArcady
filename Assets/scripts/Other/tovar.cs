using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class tovar : MonoBehaviour
{
    [SerializeField] private Transform catalka;
    [SerializeField] private int id;
    [SerializeField] private Text uiInfo;
    [SerializeField] private Button uiButton;
    public int cost;
    [SerializeField] private Kopeika kopeika;
    [SerializeField] private Transform uiButtonTransform;
    private bool near;
    [SerializeField] private GameObject bolvankaPrefab;
    [SerializeField] private Transform telegaTransform;
    UnityAction a;
    private void Start () {
        a += Pick;
        uiInfo = kopeika.buyInfoText;
        uiButton = kopeika.buyButton;
        uiButtonTransform = kopeika.buyUiTransform;
        bolvankaPrefab = kopeika.bolvankaForTovarPrefab;
        telegaTransform = kopeika.telegaTransform;
    }
    void Update()
    {
        float dist = Mathf.Abs(characterctrl.me.position.x - transform.position.x);
        if (dist < 1.1f) {
            if (!near)
            SetNear(true);
        }
        else {
            if (near)
            SetNear(false);
        }
    }
    private void SetNear (bool n) {
        near = n;
        if (n) {
            uiInfo.text = cost.ToString();
            uiButtonTransform.position = transform.position;
            uiButton.onClick.RemoveAllListeners();
            uiButton.onClick.AddListener(a);
        }
        else {
            uiButtonTransform.position = new Vector2(-9999,-9999);
        }
    }
    public void Pick () {
        Instantiate(bolvankaPrefab,transform.position, transform.rotation).GetComponent<tovarBolvanka>()
        .Init(id, cost, telegaTransform);
    }
}
