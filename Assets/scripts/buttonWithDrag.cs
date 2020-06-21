using UnityEngine;
using UnityEngine.UI;

public class buttonWithDrag : MonoBehaviour
{
    [SerializeField]
    private Image mySprite;
    public bool clicked;
    [SerializeField]
    private Sprite[] sprites = new  Sprite[2];
    [SerializeField]
    private RectTransform myRectTransform;
    private Vector2 startPos;
    [SerializeField]
    private float radius = 60;
    private void Start() {
        startPos = ((RectTransform)transform).anchoredPosition;
        //Debug.Log(myRect.ToString());
    }

    private void Update() {
        var touches = Input.touches;
        if (clicked) {
            SwitchButtonState(false);
        }
        foreach(Touch t in touches) {
            Debug.Log(t.position.ToString());
            if ((startPos - t.position).SquareDistance() <= radius ) {
                if (!clicked) {
                    SwitchButtonState(true);
                }
            }
        }
    }
    private void SwitchButtonState(bool b)
    {
        mySprite.sprite = b ? sprites[1] : sprites[0];
        clicked = b;
    }
}
