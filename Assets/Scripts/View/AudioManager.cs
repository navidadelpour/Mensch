using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {

    public Button musicButton;
    public Sprite onSprite;
    public Sprite offSprite;
    private Image icon;
    private AudioSource music;

    void Awake() {
        icon = musicButton.transform.GetChild(0).GetComponent<Image>();
        musicButton.onClick.AddListener(OnMusicButtonClick);
        music = GetComponent<AudioSource>();
        music.mute = PlayerPrefs.GetInt("music") == 1;
        icon.sprite = music.mute ? offSprite : onSprite;
    }

    public void OnMusicButtonClick() {
        music.mute = !music.mute;
        PlayerPrefs.SetInt("music", music.mute ? 1 : 0);
        icon.sprite = music.mute ? offSprite : onSprite;
    }
}
