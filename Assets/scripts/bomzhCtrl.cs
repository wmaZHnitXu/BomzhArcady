using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomzhCtrl : npcCtrl
{
    public bool sit;
    public bool friend;
    [SerializeField]
    private GameObject[] heads;
    [SerializeField]
    private GameObject[] bodyParts;
    public Transform constructor;
    public int head;
    public int body;
    public static List<Transform> itemTransforms = new List<Transform>();
    public Color[] colors;
    [SerializeField]
    private SpriteRenderer inHandItemRenderer;
    private int inHandId;
    [SerializeField]
    private SpriteRenderer armRender;

    
    protected override void OnEnable() {
        AllInit();
        BomzhConstruct(true);
    }
    protected void BomzhConstruct (bool i) {
        if (i) {
            head = Random.Range(0,heads.Length);
            body = Random.Range(0,bodys.Length);
        }
        Instantiate(heads[head],constructor.position,constructor.rotation,constructor);
        Instantiate(bodyParts[body],constructor.position,constructor.rotation,constructor);
        armRender.color = colors[body];
    }
    protected override void IdemSwitch(bool b) {
        if (idet != b)
            foreach(Animator a in forIdem) {
                a.enabled = b;
            }
            idet = b;
    }
    protected override void UpdatePlus() {
        if (inHandId == 0)
            foreach (Transform t in itemTransforms) {
                if (Mathf.Abs(t.position.x - transform.position.x) < 1) {
                    inHandId = t.gameObject.GetComponent<itemCtrl>().id;
                    inHandItemRenderer.sprite = inventoryCtrl.me.sprites[inHandId];
                    inHandItemRenderer.transform.localRotation = Quaternion.Euler(0,0,inventoryCtrl.me.structs[inHandId].flip ? -90 : 0);
                    lootSet = new GameObject[1] {inventoryCtrl.me.prefabs[inHandId]};
                    Destroy(t.gameObject);
                }
        }
    }
}
