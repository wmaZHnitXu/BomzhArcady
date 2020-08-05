using UnityEngine;
using UnityEngine.UI;
public class Kopeika : MonoBehaviour
{
    private bool near;
    [SerializeField] private Animator doorsAnim;
    [SerializeField] private GameObject buttons;
    private int HashedOpen;
    [SerializeField] private Vector3 wonderLand;
    [SerializeField] private GameObject wonderLandObject;
    public GameObject bolvankaForTovarPrefab;
    public Transform buyUiTransform;
    public Text buyInfoText;
    public Button buyButton;
    public Transform telegaTransform;
    void Start()
    {
        wonderLandObject.SetActive(false);
        HashedOpen = Animator.StringToHash("open");
    }

    void Update()
    {
        float dist = Mathf.Abs(characterctrl.me.position.x - transform.position.x);
        if (dist > 1.5f && near) {
            SetNear(false);
        }
        else if (dist < 1.5f && !near) {
            SetNear(true);
        }
    }
    public void SetNear (bool op) {
        near = op;
        buttons.SetActive(op);
        doorsAnim.SetBool(HashedOpen, op);
    }
    public void TransportToWonderland () {
        characterctrl.it.SetCamZoom(4f);
        wonderLandObject.SetActive(true);
        Vector3 playerRelativePos = characterctrl.me.position - transform.position;
        characterctrl.it.InstantPosChange(wonderLand + playerRelativePos);
    }
    public void TransportFromWonderland () {
        wonderLandObject.SetActive(false);
        Vector3 playerRelativePos = characterctrl.me.position - wonderLand;
        characterctrl.it.InstantPosChange(transform.position + playerRelativePos);
        characterctrl.it.SetCamZoom(5f);
        tovarBolvanka.DestroyAll();
    }
}
