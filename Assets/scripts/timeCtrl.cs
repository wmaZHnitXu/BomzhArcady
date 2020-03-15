﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Experimental.Rendering.LWRP;
using Random = System.Random;

public struct npcSpawnStructure
{
    public GameObject[] npcPrefab;
    public float[] probability;
}
public class timeCtrl : MonoBehaviour
{
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
    private bool bossEjected;
    [SerializeField] private npcSpawnStructure[] nights;
    [SerializeField] private float[] nightFrequency;
    [SerializeField] private int day;
    void Start()
    {
        ses = new WaitForSeconds(timestep);
        StartCoroutine(TimeRecount());
    }

    private IEnumerator TimeRecount () {
        while (true) {
            yield return ses;
            time++;
            if (time == 1440) {
                time = 0;
            }
            hours = time / 60;
            minutes = time % 60;

            if (minutes < 10)
                clock.text = hours.ToString() + ":0" + minutes.ToString();
            else
                clock.text = hours.ToString() + ":" + minutes.ToString();
            for (var i = 0; i < 5; i++) {
                multiplers[i] = (360 - (float)(0 < Math.Abs(times[i] - time) ? Math.Abs(times[i] - time) : 1)) / 360;
                if (multiplers[i] < 0) multiplers[i] = 0;
            }
            skySprite.color = timeColors[0] * multiplers[0] + timeColors[1] * multiplers[1] + timeColors[2] * multiplers[2] + timeColors[3] * multiplers[3] + timeColors[4] * multiplers[4];
            night = time < 250 | time > 1180;
            if (night & sunLight.intensity > 0.66f) {
                sunLight.intensity = Mathf.Lerp(sunLight.intensity,0.65f,0.01f);
            }
            if (!night & sunLight.intensity < 1.59f )
                sunLight.intensity = Mathf.Lerp(sunLight.intensity,1.6f,0.01f);
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
    }

    private IEnumerator NightRaid() //Todo Спавн нпс и босса
    {
        while (night)
        {
            Instantiate(NpcRandomized(), characterctrl.it.GetNpcSpawnPosition(), Quaternion.identity);
            yield return new WaitForSeconds(UnityEngine.Random.Range(nightFrequency[day],nightFrequency[day] * 2));
        }
    }

    private GameObject NpcRandomized()
    {
        var resultId = 0;
        float maxWeight = 0;
        for (var i = 0; i < nights[day].probability.Length; i++)
        {
            var newWeight = UnityEngine.Random.Range(0,nights[day].probability[i]);
            if (!(maxWeight < newWeight)) continue;
            maxWeight = newWeight;
            resultId = i;
        }
        return nights[day].npcPrefab[resultId];
    }
}
