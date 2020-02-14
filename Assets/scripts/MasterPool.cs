using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterPool : MonoBehaviour
{
    public GameObject[] npcs;
    static private List<GameObject> npcPool = new List<GameObject>();
    [SerializeField]
    private Transform poolPos;
    public static int howManyNpcWeHave; //Кастыыыыль кастылище. нужен дабы сдержать бесконечный инстантиейт. прост лень было нормально закодить.
    static private int index;
    static private MasterPool me;
    void Start()
    {
        me = this;
        FillMe();
    }
    static public void EjectNpc (Vector3 pos) {
        if (npcPool.Count != 0) {
        index = UnityEngine.Random.Range(0,npcPool.Count-1);
        npcPool[index].transform.position = pos;
        npcPool[index].SetActive(true);
        npcPool.RemoveAt(index);
        if (npcPool.Count == 0) me.FillMe();
        }
    }
    static public void EjectNpc (Vector3 pos,int id) { //Не работает
        index = UnityEngine.Random.Range(0,npcPool.Count-1);
        npcPool[index].transform.position = pos;
        npcPool[index].SetActive(true);
        npcPool.RemoveAt(index);
        if (npcPool.Count == 0) me.FillMe();
    }
    static public void InsertNpc (GameObject npc) {
        npc.SetActive(false);
        npcPool.Add(npc);
    }
    static public void InsertNpc (GameObject npc, float time) {
        me.StartCoroutine(insertCd(npc,time));
    }
    public void FillMe () { // (6)_(9)      upd: тупая шутка, но убирать я ее не буду
    if (howManyNpcWeHave <= 10) { //Вот он, кастыль целковый полушка четвертушка осьмушка пудовичок etc.
        foreach(GameObject g in npcs) {
            npcPool.Add(Instantiate(g,poolPos.position,poolPos.rotation));
            howManyNpcWeHave++;
            npcPool[npcPool.Count-1].SetActive(false);
            }
        }
    }
    public static IEnumerator insertCd (GameObject npc, float time) {
        yield return new WaitForSeconds(time);
        npc.SetActive(false);
        npcPool.Add(npc);
    }
}
