using UnityEngine;
using UnityEngine.UI;

public class buttonWithDrag : MonoBehaviour
{
    [SerializeField]
    private Image mySprite;
    public bool clicked;
    [SerializeField]
    private Sprite[] sprites = new  Sprite[2];

    private void OnMouseEnter()
    {
        SwitchButtonState(true);
    }

    private void OnMouseDown()
    {
        SwitchButtonState(true);
    }


    private void OnMouseExit()
    {
        SwitchButtonState(false);
    }

    private void OnMouseUp()
    {
        SwitchButtonState(false);
    }

    private void SwitchButtonState(bool b)
    {
        mySprite.sprite = b ? sprites[1] : sprites[0];
        clicked = !b;
    }
}
