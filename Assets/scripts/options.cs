using UnityEngine;
using UnityEngine.UI;

public class options : MonoBehaviour
{
    [SerializeField] private Slider mainSndSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private float music;
    [SerializeField] private float mainSound;
    public static options me;
    private bool resetting;
    void Start () {
        me = this;
        if (PlayerPrefs.HasKey("music")) {
            Read();
        }
        ResetSliders();
    }
    public void UpdateSettings () {
        if (resetting) return;
        music = musicSlider.value;
        mainSound = mainSndSlider.value;
    }
    public void Write () {
        PlayerPrefs.SetFloat("mainSnd", mainSound);
        PlayerPrefs.SetFloat("music", music);
        PlayerPrefs.Save();
    }
    public void Read () {
        music = PlayerPrefs.GetFloat("music");
        mainSound = PlayerPrefs.GetFloat("mainSnd");
    }
    public void ResetSliders () {
        resetting = true;
        mainSndSlider.value = mainSound;
        musicSlider.value = music;
        resetting = false;
    }
}
