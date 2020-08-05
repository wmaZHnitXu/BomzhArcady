using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class savePanel : MonoBehaviour, IDeselectHandler, ISelectHandler
{
    [SerializeField] private bool Selected;
    [SerializeField] private Animator animator;
    private int HashedPulled;
    [SerializeField] private Text nameOfSave;
    [SerializeField] private Text dateOfSave;
    [SerializeField] private Text saveTimeInGame;
    [SerializeField] private Button myButt; //Я ЕБЛАН Я ЕБЛАН Я ЕБЛАН Я ЕБЛАН Я ЕБЛАН Я ЕБЛАН Я ЕБЛАН Я ЕБЛАН Я ЕБЛАН
    public int idOfSave;
    private static GameObject[] goodObjectsToClick;
    
    private void Start () {
        if (goodObjectsToClick == null) {
            SaveLoad.me.SelectSave(this);
            goodObjectsToClick = new GameObject[2];
            goodObjectsToClick[0] = GameObject.Find("SaveContext/Load");
            goodObjectsToClick[1] = GameObject.Find("SaveContext/Delete");
            SaveLoad.me.PullSavesPanel(false);
        }
        if (!animator) animator = GetComponent<Animator>();
        HashedPulled = Animator.StringToHash("pulled");
    }
    public void Initialize (int id, string name, string date, string timeInGame) {
        idOfSave = id;
        if (name != string.Empty)
            nameOfSave.text = name;
        else
            nameOfSave.text = "Сохранение";
        dateOfSave.text = date;
        saveTimeInGame.text = timeInGame;
    }
    public void Select () {
        Selected = true;
        ResetAnimation();
        SaveLoad.me.SelectSave(this);
    }
    public void Deselect () {
        Selected = false;
        ResetAnimation();
        SaveLoad.me.SelectSave();
    }
    private IEnumerator NestorShilo (BaseEventData data) {
        yield return new WaitForSeconds(0.1f);
        Debug.Log(goodObjectsToClick[0].name);
        if (data.selectedObject != null) {
            Debug.Log(data.selectedObject.name);
            if (data.selectedObject == goodObjectsToClick[0] || data.selectedObject == goodObjectsToClick[1]) {
                myButt.Select();
                yield break;
            }
        }
        Deselect();
    }
    private IEnumerator NestorShilo2 () {
        yield return new WaitForSeconds(0.1f);
        Select();
    }

    public void ResetAnimation () {
        animator.SetBool(HashedPulled, Selected);
    }
    public void OnDeselect (BaseEventData data) {
        if (!Selected) return; 
        StartCoroutine(NestorShilo(data));
    }
    public void OnSelect (BaseEventData data) {
        if (Selected) return;
        StartCoroutine(NestorShilo2());
    }
}
