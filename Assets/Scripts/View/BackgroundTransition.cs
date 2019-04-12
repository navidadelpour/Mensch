using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundTransition : MonoBehaviour {

    public Sprite[] backgrounds;
    private Image backImage;
    private Image frontImage;
    private int currentIndex;
    public float speed;
    private float alpha;

    void Awake() {
        backImage = transform.GetChild(0).GetComponent<Image>();
        frontImage = transform.GetChild(1).GetComponent<Image>();
        
        currentIndex = Random.Range(0, backgrounds.Length);
        alpha = Random.Range(0f, 1f);
        backImage.sprite = backgrounds[currentIndex];
        frontImage.sprite = NextSprite();
        frontImage.color = White(alpha);
    }

    void Update() {
        alpha = frontImage.color.a;
        if(alpha < 1) {
            alpha += Time.deltaTime * speed;
        } else {
            backImage.sprite = frontImage.sprite;
            frontImage.sprite = NextSprite();
            alpha = 0;
        }
        frontImage.color = White(alpha);
    }

    private Color White(float alpha) {
        return new Color (1, 1, 1, alpha);
    }

    private Sprite NextSprite() {
        return backgrounds[(++currentIndex) % backgrounds.Length];
    }

}
