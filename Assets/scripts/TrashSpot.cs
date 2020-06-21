using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpot : MonoBehaviour
{
    [System.Serializable]
    public struct TrashWithprobability {
        public float probability;
        public GameObject TrashPrefab;
    }
    [SerializeField] private TrashWithprobability[] trashs;
    [SerializeField] private int trashRespawnCd;
    [SerializeField] private float spawnRangeX;
    [SerializeField] private int maxTrashCount = 3;
    [SerializeField] private int trashCount;
    [SerializeField] private bool Initialized;
    private void Start () {
        if (!Initialized)
            FirstInitalization();
    }
    public void TrashIsDead () {
        trashCount--;
        StartCoroutine(ReviveTrash());
    }
    public void FirstInitalization () {
        for (int i = 0; i < maxTrashCount; i++) {
            trashCount++;
            Instantiate(getRandomTrash(), transform.position + new Vector3(Random.Range(spawnRangeX,-spawnRangeX), 0, 0), Quaternion.identity).GetComponent<musorCtrl>()
            .deathEvent += TrashIsDead;;
        }
        Initialized = true;
    }    
    private GameObject getRandomTrash () {
        float maxTrash = 0;
        float probabilityRandomized;
        GameObject result = null;
        foreach (TrashWithprobability trash in trashs) {
            probabilityRandomized = Random.Range(0, trash.probability);
            if (probabilityRandomized > maxTrash) {
                maxTrash = trash.probability;
                result = trash.TrashPrefab;
            }
        }
        return result;
    }
    private IEnumerator ReviveTrash () {
        if (trashCount != maxTrashCount) {
            yield return new WaitForSeconds(trashRespawnCd);
            trashCount++;
            Instantiate(getRandomTrash(), transform.position + new Vector3(Random.Range(spawnRangeX,-spawnRangeX), 0, 0), Quaternion.identity).GetComponent<musorCtrl>()
            .deathEvent += TrashIsDead;
            yield return new WaitForSeconds(2);
        }
    }
}
