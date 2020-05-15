using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickSwitchBack : clickCallback
{
    [SerializeField]
    private int i;
    public int iMax;
    public Sprite[] sprites;
    public byte[] ids;
    [SerializeField]
    private SpriteRenderer r;
    private metalCtrl mtl;

    void Start () {
        mtl = kiosk as metalCtrl;
        i = iMax - 1;
        //OnMouseUp();
    }

    public override void OnMouseUp() {
        if (down) {
            if (i != iMax) {
                r.sprite = sprites[i];
                mtl.needId = ids[i];
                //Debug.Log(mtl.needId);
                i++;
            }
            if (i == iMax) {
                i = 0;
            }
        }
    }
}
