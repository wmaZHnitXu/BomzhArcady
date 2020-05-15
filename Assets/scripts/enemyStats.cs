using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class enemyStats : MonoBehaviour
{
    [SerializeField] private Transform statsTransform;
    private Animator stat;
    private hpBase targetHp;
    private Transform targetTransform;
    public static enemyStats me;
    [SerializeField] private Image bar;
    [SerializeField] private Text nameAndHpCount;
    private string nameOfNpc;

    private void Start()
    {
        me = this;
    }

    public void ShowNpcStats(hpBase hp)
    {
        if (targetHp != null)
                targetHp.addHitEvent.RemoveListener(RefreshStats);
        targetHp = hp;
        targetHp.addHitEvent.AddListener(RefreshStats);
        targetTransform = hp.transform;
        nameOfNpc = hp.nameOfNpc;
        RefreshStats();
    }

    private void Update()
    {
        if (targetTransform != null)
            statsTransform.position = targetTransform.position;
    }

    private IEnumerator FollowNpc()
    {
        yield break;
    }
    private void RefreshStats()
    {
        nameAndHpCount.text = nameOfNpc + " " + targetHp.hp.ToString() + "/" + targetHp.maxHp.ToString();
        bar.fillAmount = (float)targetHp.hp / (float)targetHp.maxHp;
    }
}
