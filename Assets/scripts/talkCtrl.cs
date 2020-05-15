using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class talkCtrl : MonoBehaviour
{
    public static GameObject talkCanv;
    public Text said;
    [SerializeField]
    public static Text[] allAnswTexts;
    public answerStruct[] answers;
    public replicaStruct[] repliki; //0 - дефолтная реплика, если ответ, то просто не отображается.
    private List<string> inverse = new List<string>();
    private int answerCount;
    public int replId;
    private char[] charray;
    private static characterctrl _c;
    [SerializeField]
    private GameObject talkButton;
    public static GameObject[] textObjects = new GameObject[4];
    public void StartDialog () {
        replId = 0;
        if (talkCanv == null) {
            _c = characterctrl.it;
            talkCanv = _c.answerObj;
            allAnswTexts = _c.answerTexts;
        }
        _c.talker = this;
        SetActiveButton(false);
        for (int i = 0; i < allAnswTexts.Length; i++) {
            textObjects[i] = allAnswTexts[i].gameObject;
        }
        NextTalking();
    }
    
    void NextTalking () {
        StopAllCoroutines();
        talkCanv.SetActive(true);
        ClearTexts();
        StartCoroutine(LoadRepl());
    }
    void ClearTexts () {
        foreach (Text t in allAnswTexts) {
            t.text = string.Empty;
        }
    }
    public void Answer () {
        if (_c.answerNumber < answerCount) {
        //System.Array.Reverse(answers[repliki[replId].answerCode].nextReplCode);
        replId = answers[repliki[replId].answerCode].nextReplCode[_c.answerNumber];
        Debug.Log(replId.ToString());
        //System.Array.Reverse(answers[repliki[replId].answerCode].nextReplCode);
        Debug.Log(_c.answerNumber);
        NextTalking();
        }
    }
    IEnumerator LoadRepl () {
        foreach (GameObject g in textObjects) {
            g.SetActive(false);
        }
        //Вывод реплики.
        charray = repliki[replId].repl.ToCharArray();
        said.text = string.Empty;
        foreach (char c in charray) {
            said.text += c;
            yield return new WaitForSeconds((c == '!' || c == '.' || c == ',' || c == '?') ? 0.3f : 0.05f);
        }
        foreach (GameObject g in textObjects) {
            g.SetActive(true);
        }
        //Каллбэк
        if (repliki[replId].questId != 0) {
            scenarioCtrl.me.questContainer.DialogCallback(repliki[replId].questId);
            Debug.Log("Callback");
        }
        //Ответы.
        if (repliki[replId].answerCode != 0) {
            answerCount = answers[repliki[replId].answerCode].answer.Length;
            inverse.AddRange(answers[repliki[replId].answerCode].answer);
            inverse.Reverse();
            for (int i = answerCount-1; i+1 != 0; i--) {
                allAnswTexts[i].text = inverse[i];
                if (allAnswTexts[i].text == string.Empty)
                    textObjects[i].SetActive(false);
            }
        }
        else {
            if (!repliki[replId].cascade) {
                yield return new WaitForSeconds(1f);
                StopDialog();
            }
            else {
                replId = repliki[replId].nextReplCode; //Аккуратненько, а то будет стековерфлоу.
                StartCoroutine(LoadRepl());
            }
        }
    }
    IEnumerator LoadRepl (string text) { //Метод юзается только для единичных реплик (без диалога)
        charray = text.ToCharArray();
        said.text = string.Empty;
        foreach (char c in charray) {
        said.text += c;
        yield return new WaitForSeconds((c == '!' || c == '.' || c == ',' || c == '?') ? 0.3f : 0.05f);
        }
        yield return new WaitForSeconds(1.3f);
        StopDialog();
    }
    
    public void StopDialog () {
        SetActiveButton(true);
        if (talkCanv != null) //Если greetings, то он еще не инициализирован
            talkCanv.SetActive(false);
        said.text = string.Empty;
    }
    public void SayThis (string text) {
        StopAllCoroutines();
        StartCoroutine(LoadRepl(text));
    }
    public void SetActiveButton (bool active) {
        talkButton.SetActive(active);
    }
    public void Shutdown () {
        StopAllCoroutines();
        if (talkCanv != null)
            talkCanv.SetActive(false);
        SetActiveButton(false);
        said.text = string.Empty;
    }
}
