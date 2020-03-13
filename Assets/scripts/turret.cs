using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;

public class turret : MonoBehaviour
{

    [SerializeField] private List<int> ammo = new List<int>();
    [SerializeField] private bool right;
    [SerializeField] private float shootingSpeed;
    [SerializeField] private Transform[] bolvanki;
    [SerializeField] private SpriteRenderer[] bolvankiSprites;
    [SerializeField] private Transform headTransform;
    [SerializeField] private Transform target;
    [SerializeField] private float range;
    [SerializeField] private float updatePeriod;
    private Quaternion gunRotation;
    [SerializeField] private float rotationSpeed;
    private bool active; 
    private bool Active
    {
        get => active;

        set
        {
            active = value;
            if (!Active) return;
            StopAllCoroutines();
            StartCoroutine(LifeTime());
        }
    }


    private void OnEnable()
    {
        
    }

    private IEnumerator LifeTime()
    {
        while (Active)
        {
            if (target == null)
            {
                yield return new WaitForSeconds(updatePeriod);
                
            }
        }
    }
}
