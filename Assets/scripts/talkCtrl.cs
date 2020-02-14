using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class talkCtrl : MonoBehaviour
{
    public static GameObject talkCanv;
    [SerializeField]
    private Text said;
    [SerializeField]
    public static Text[] allAnswTexts;
    public answerStruct[] answers;
    public replicaStruct[] repliki; //0 - дефолтная реплика, если ответ, то просто не отображается.
    private List<string> inverse = new List<string>();
    private int answerCount;
    public int replId;
    private char[] charray;
    private static characterctrl c;
    private bool touched;
    void OnMouseDown () {
        touched = true;
    }
    void OnMouseUp () {
        if (touched) {
            replId = 0;
            if (talkCanv == null) {
                c = characterctrl.it;
                talkCanv = c.answerObj;
                allAnswTexts = c.answerTexts;
            }
            c.talker = this;
            NextTalking();
        }
        touched = false;
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
        if (c.answerNumber < answerCount) {
        System.Array.Reverse(answers[repliki[replId].answerCode].nextReplCode);
        replId = answers[repliki[replId].answerCode].nextReplCode[c.answerNumber];
        System.Array.Reverse(answers[repliki[replId].answerCode].nextReplCode);
        Debug.Log(c.answerNumber);
        NextTalking();
        }
    }
    public IEnumerator LoadRepl () {
        charray = repliki[replId].repl.ToCharArray();
        said.text = string.Empty;
        foreach (char c in charray) {
        said.text += c;
        yield return new WaitForSeconds(0.05f);
        }
        if (repliki[replId].answerCode != 0) {
            answerCount = answers[repliki[replId].answerCode].answer.Length;
            inverse.AddRange(answers[repliki[replId].answerCode].answer);
            inverse.Reverse();
            for (int i = answerCount-1; i+1 != 0; i--) {
            allAnswTexts[i].text = inverse[i];
            }
        }
        else {
            yield return new WaitForSeconds(1f);
            StopDialog();
        }
    }
    public void StopDialog () {
        talkCanv.SetActive(false);
        said.text = string.Empty;
    }
}
