using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using Random = System.Random;
[Serializable]
public struct npcSpawnStructure
{
    public float frequency;
    public GameObject[] npcPrefab;
    public float[] probability;
    public GameObject boss;
}
public class timeCtrl : MonoBehaviour
{
    [SerializeField] private Vector3 bossApPos;
    public int time;
    private WaitForSeconds ses;
    public float timestep;
    public int minutes;
    public int hours;
    public Text clock;
    public Color[] timeColors = new Color[5];
    public int[] times = new int[] {360,720,1080,1440,0}; //0,360,720,1080
    public float[] multiplers = new float[5];
    public SpriteRenderer skySprite;
    public UnityEngine.Experimental.Rendering.Universal.Light2D sunLight;
    public static bool night;
    [SerializeField]
    private GameObject maniac;
    [SerializeField]
    private maniacCtrl maniacCtrl;
    [SerializeField]
    private Text alarmText;
    [SerializeField]
    private RectTransform alarmTransform;
    private bool bossEjected;
    [SerializeField] private npcSpawnStructure[] nights;
    [SerializeField] public int day;
    [SerializeField] private Animator nightRaidText;
    [SerializeField] private UnityAction twinerAct;
    [SerializeField] private ParticleSystem nightSys;
    public int enemyCount;
    [SerializeField] private int maxEnemyCount;
    public static timeCtrl me;
    private bool sunIsDown;
    private Coroutine timeRoutine;
    private bossBase boss;
    [SerializeField] private Image[] bossBar = new Image[2];
    public bool GoTime {
        get => (timeRoutine != null);
        set {
            if (value != GoTime) {
                if (value) timeRoutine = StartCoroutine(TimeRecount());
                else StopCoroutine(timeRoutine);
            }
        }
    }
    [SerializeField] private GameObject bossBounds;
    public UnityAction remindAboutNight;
    public UnityAction remindAboutMorning;
    void Awake()
    {
        me = this;
        ses = new WaitForSeconds(timestep);
    }

    private IEnumerator TimeRecount () {
        while (true) {
            yield return ses;
            time++;
            if (time == 1400) remindAboutNight.Invoke();
            if (time == 1440) {
                time = 0;
                day++;
                night = true;
                StartCoroutine(NightRaid());
            }
            SetTime(time);
        }
    }

    private IEnumerator NightRaid() //Todo Спавн нпс и босса
    {
        Debug.Log("Raid Started");
        while (night)
        {
            if (enemyCount != maxEnemyCount) {
                Instantiate(NpcRandomized(), characterctrl.it.GetNpcSpawnPosition(), Quaternion.identity);
                enemyCount++;
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(nights[day-1].frequency,nights[day-1].frequency * 2));
        }
        if (nights[day-1].boss != null) StartBossRaid();
        NightAlert(true);
    }

    private GameObject NpcRandomized()
    {
        var resultId = 0;
        float maxWeight = 0;
        for (var i = 0; i < nights[day-1].probability.Length; i++)
        {
            var newWeight = UnityEngine.Random.Range(0,nights[day-1].probability[i]);
            if (!(maxWeight < newWeight)) continue;
            maxWeight = newWeight;
            resultId = i;
        }
        return nights[day-1].npcPrefab[resultId];
    }
    public void NightAlert (bool end) {
        twinerAct = new UnityAction(NightAlertReturn);
        retardedTwiner.me.CallAnimation(alarmTransform,new Vector2(0,-160), 0.1f, twinerAct, 4f);
        if (end) {
            alarmText.text = "Наступает утро.";
        }
        else {
            nightSys.Play();
            alarmText.text = "Ночь " + (day+1).ToString(); //если сообщение будет появляться в полночь, то убрать
        }
    }
    public void NightAlertReturn () {
        retardedTwiner.me.CallAnimation(alarmTransform,new Vector2(0,25), 0.1f, null, 2f);
    }
    public void SetTime (int времяБлять) {
        time = времяБлять;
        hours = времяБлять / 60;
        minutes = времяБлять % 60;

        if (minutes < 10)
            clock.text = hours.ToString() + ":0" + minutes.ToString();
        else
            clock.text = hours.ToString() + ":" + minutes.ToString();
        for (var i = 0; i < 5; i++) {
            multiplers[i] = (360 - (float)(0 < Math.Abs(times[i] - времяБлять) ? Math.Abs(times[i] - времяБлять) : 1)) / 360;
            if (multiplers[i] < 0) multiplers[i] = 0;
        }
        skySprite.color = timeColors[0] * multiplers[0] + timeColors[1] * multiplers[1] + timeColors[2] * multiplers[2] + timeColors[3] * multiplers[3] + timeColors[4] * multiplers[4];
        night = времяБлять < 300;
        sunIsDown = времяБлять > 1350 || времяБлять < 300;
        if (sunIsDown & sunLight.intensity > 0.66f) {
            sunLight.intensity = Mathf.Lerp(sunLight.intensity,0.65f,0.02f);
        }

        if (!sunIsDown & sunLight.intensity < 1.59f)
        {
            sunLight.intensity = Mathf.Lerp(sunLight.intensity, 1.6f, 0.01f);
        }
        switch (времяБлять) {
            case 1400:
                Debug.Log("NightAlert");
                NightAlert(false);
            break;
        }

        if (sunLight.intensity < 0.7f && !bossEjected && characterctrl.NearNpcs.Count == 0) {
            bossEjected = true;
            maniac.transform.position = characterctrl.me.position + (UnityEngine.Random.Range(0,100) > 50 ? new Vector3(10,0,0) : new Vector3 (-10,0,0));
            maniac.SetActive(true);
        }
        if (sunLight.intensity > 0.7 & bossEjected) {
            bossEjected = false;
            maniac.SetActive(false);
        }
    }
    public void StartBossRaid () {
        if (!nights[day-1].boss) return;
        StartCoroutine(BossAppears());
    }
    IEnumerator BossAppears () {
        Debug.Log("startBossRaid");
        bossBounds.SetActive(true);
        GoTime = false;
        clock.text = "#####";
        yield return new WaitForSeconds(1f);
        boss = Instantiate(nights[day-1].boss, bossApPos, Quaternion.identity).GetComponent<bossBase>();
        clock.text = boss.nameOfNpc;
        bossBar[0].fillAmount = 0;
        retardedTwiner.me.CallAnimation(bossBar[0].rectTransform, new Vector2(0, -80), 0.01f);
    }
    private Coroutine fillroutine;
    public void UpdateBossBar () {
        if (fillroutine != null) StopCoroutine(fillroutine);
        fillroutine = StartCoroutine(UpdateBossBarIEnum(boss.hp));
    }
    private IEnumerator UpdateBossBarIEnum (float fill) {
        while (Mathf.Abs(fill - bossBar[0].fillAmount) > 0.01f) {
            bossBar[0].fillAmount = Mathf.Lerp(bossBar[0].fillAmount, fill, 0.1f);
            yield return new WaitForFixedUpdate();
        }
        bossBar[0].fillAmount = fill;
    }
    public void StopBossRaid () {
        retardedTwiner.me.CallAnimation(bossBar[0].rectTransform, new Vector2(0, 30), 0.01f);
        GoTime = true;
        remindAboutMorning.Invoke();
    }
}
