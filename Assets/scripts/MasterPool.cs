using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class masterPool : MonoBehaviour
{
    public GameObject[] npcs;
    static private List<GameObject> _npcPool = new List<GameObject>();
    [SerializeField]
    private Transform poolPos;
    public static int howManyNpcWeHave; //Кастыыыыль кастылище. нужен дабы сдержать бесконечный инстантиейт. прост лень было нормально закодить.
    static private int _index;
    static private masterPool _me;
    void Start()
    {
        _me = this;
        FillMe();
    }
    static public void EjectNpc (Vector3 pos) {
        if (_npcPool.Count != 0) {
        _index = UnityEngine.Random.Range(0,_npcPool.Count-1);
        _npcPool[_index].transform.position = pos;
        _npcPool[_index].SetActive(true);
        _npcPool.RemoveAt(_index);
        if (_npcPool.Count == 0) _me.FillMe();
        }
    }
    static public void EjectNpc (Vector3 pos,int id) { //Не работает
        _index = UnityEngine.Random.Range(0,_npcPool.Count-1);
        _npcPool[_index].transform.position = pos;
        _npcPool[_index].SetActive(true);
        _npcPool.RemoveAt(_index);
        if (_npcPool.Count == 0) _me.FillMe();
    }
    static public void InsertNpc (GameObject npc) {
        npc.SetActive(false);
        _npcPool.Add(npc);
    }
    static public void InsertNpc (GameObject npc, float time) {
        _me.StartCoroutine(InsertCd(npc,time));
    }
    public void FillMe () { // (6)_(9)      upd: тупая шутка, но убирать я ее не буду
    if (howManyNpcWeHave <= 10) { //Вот он, кастыль целковый полушка четвертушка осьмушка пудовичок etc.
        foreach(GameObject g in npcs) {
            _npcPool.Add(Instantiate(g,poolPos.position,poolPos.rotation));
            howManyNpcWeHave++;
            _npcPool[_npcPool.Count-1].SetActive(false);
            }
        }
    }
    public static IEnumerator InsertCd (GameObject npc, float time) {
        yield return new WaitForSeconds(time);
        npc.SetActive(false);
        _npcPool.Add(npc);
    }
}
