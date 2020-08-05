using UnityEngine;
using UnityEngine.UI;

public abstract class bossBase : hpBase {
    public Image hpBar;
    [SerializeField] protected bool _right = true;
    public bool Right {
        get => _right;
        set {
            if (_right != value) {
                _right = value;
                transform.localScale = new Vector3(value ? startScale.x : -startScale.x, startScale.y, startScale.z);
            }
        }
    }
    private Vector3 startScale;
    protected new void Start () {
        base.Start();
        startScale = transform.localScale;
    }
    public override void AddHit(int hit) {
        base.AddHit(hit);
        hpBar.fillAmount = maxHp / hp;
    }
    public override void Death() {
        timeCtrl.me.StopBossRaid();
    }
}
