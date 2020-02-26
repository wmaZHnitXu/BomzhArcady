using System.Collections;
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
    private talkCtrl talker;
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
}
