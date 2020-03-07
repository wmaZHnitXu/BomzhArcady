﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quest0 : quest
{
    private Transform playerTransform;
    private bool talked;
    [SerializeField]
    private Transform dudearm;
    [SerializeField]
    private SpriteRenderer phone;
    [SerializeField]
    private Transform durkaTransform;
    [SerializeField]
    private carNpcCtrl durkaCtrl;
    [SerializeField]
    private Animator durkaDoorAnim;
    [SerializeField]
    private talkCtrl talker;
    [SerializeField]
    private Transform[] saniTARS = new Transform[2];
    [SerializeField]
    private npcGoCtrl[] saniTARSCtrls = new npcGoCtrl[2];
    [SerializeField]
    private GameObject durkaDead;
    [SerializeField]
    private Transform LookAt;
    void Start () {
        playerTransform = characterctrl.me;
        StartCoroutine(questBody()); 
    }
    IEnumerator questBody () { //Пока экспериментирую. Нужно, чтобы квест можно было загрузить из сохранения.
        while (!characterctrl.it.gameStarted) {
            yield return new WaitForEndOfFrame(); //Да, да, да, когда станешь умнее, то сделаешь тут событие
        }
        if (stage == 0) { //Тело разбито на ифы со стейджами для моего личного удобства, так проще представить весь квест. + сохранения реализовать можно.
            whatToDo = "Добраться до Новых Выбитей.";
            while (playerTransform.position.x < 122f) {
                yield return new WaitForEndOfFrame();
            } 
            stage++;
        }
        if (stage == 1) {
            whatToDo = "Поговорить с военными.";
            while (!talked) {
                yield return new WaitForEndOfFrame();
            }
            stage++;
        }
        if (stage == 2) {
            whatToDo = "Найти способ перебраться через мост.";
            stage++;
        }
        if (stage == 3) {
            whatToDo = "Задание выполнено!";
            stage++;
        }
        if (stage == 10) {
            whatToDo = "Задание провалено?";
            //Руку крутим
            characterctrl.it.StartCutScene(true);
            Quaternion q = Quaternion.Euler(0,0,170);
            phone.enabled = true;
            while (dudearm.localRotation != q) {
                yield return new WaitForFixedUpdate();
                dudearm.localRotation = Quaternion.RotateTowards(dudearm.localRotation,q,3f);
            }
            yield return new WaitForSeconds(0.5f);
            //Говорим
            talker.SayThis("Алло, дурка?");
            yield return new WaitForSeconds(2f);
            talker.SayThis("Майор Доигралес вас тревожит.");
            yield return new WaitForSeconds(3f);
            talker.SayThis("Да, да, опять буйный индивид на мосту.");
            yield return new WaitForSeconds(4f);
            talker.SayThis("Хорошо, ожидаем.");
            yield return new WaitForSeconds(2f);
            talker.SayThis("Далеко не уходи, за тобой сейчас заедут");
            while (dudearm.localRotation != Quaternion.identity) {
                yield return new WaitForFixedUpdate();
                dudearm.localRotation = Quaternion.RotateTowards(dudearm.localRotation,Quaternion.identity,3f);
            }
            phone.enabled = false;
            durkaCtrl.enabled = true;
            while (durkaCtrl.dist > 8f) {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(4f);
            characterctrl.it.SetCamTarget(saniTARS[0]);
            durkaDoorAnim.SetBool("open",true);
            yield return new WaitForSeconds(2f);
            saniTARSCtrls[0].targetVector3 = new Vector3 (saniTARS[0].position.x,-2.2f,-0.2f);
            saniTARSCtrls[1].targetVector3 = new Vector3 (saniTARS[1].position.x,-2.2f,-0.1f);
            saniTARSCtrls[0].StartStopGoing(true);
            saniTARSCtrls[1].StartStopGoing(true);
            yield return new WaitForSeconds(0.5f);
            saniTARSCtrls[0].targetVector3 = new Vector3 (playerTransform.position.x - 1,-2.2f,-0.2f);
            saniTARSCtrls[1].targetVector3 = new Vector3 (playerTransform.position.x + 1,-2.2f,-0.1f);
            yield return new WaitForSeconds(1f);
            saniTARSCtrls[0].ReturnMe();
            saniTARSCtrls[1].ReturnMe();
            yield return new WaitForFixedUpdate();
            characterctrl.rb.isKinematic = true;
            playerTransform.SetParent(saniTARS[0]);
            playerTransform.localPosition = new Vector3(playerTransform.localPosition.x,playerTransform.localPosition.y,0);
            while (saniTARSCtrls[0].targetVector3 != saniTARS[0].position) {
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(0.5f);
            durkaDoorAnim.SetBool("open",false);
            characterctrl.it.SetCamTarget(LookAt);
            characterctrl.it.camspeed = 0.01f;
            yield return new WaitForSeconds(1f);
            characterctrl.it.camspeed = 0.1f;
            durkaCtrl.destination.x = 260;
            while (durkaTransform.position.x < 220)
                yield return new WaitForFixedUpdate();
            StartCoroutine(Avaria());
        }
    }
    public override void dialogCallback(int Id) {
        switch (Id) {
            case 1:
            talked = true;
            stage = 9;
            break;
        }
    }
    IEnumerator Avaria () {
        Vector3 newPos;
        playerTransform.SetParent(null);
        durkaDead.SetActive(true);
        durkaDead.transform.position = durkaTransform.position;
        durkaTransform.gameObject.SetActive(false);
        characterctrl.it.StartCutScene(false);
        playerTransform.position = new Vector3(characterctrl.me.position.x,characterctrl.me.position.y,0);
        newPos = characterctrl.me.position;
        newPos.x += 10;
        while ((playerTransform.position - newPos).magnitude > 0.5f) {
            playerTransform.position = Vector3.Lerp(playerTransform.position,newPos,0.1f);
            yield return new WaitForFixedUpdate();
        }
    }
}
