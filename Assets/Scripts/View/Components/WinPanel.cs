using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour {
    public Button button;
    public GameObject icon;
    private Animator animator;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    public void Popup(GameObject obj) {
        gameObject.SetActive(true);
        Color color = obj.GetComponent<Image>().color;
        Sprite sprite = obj.transform.GetChild(0).GetComponent<Image>().sprite;
        icon.GetComponent<Image>().color = color;
        icon.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
        animator.SetTrigger("popup");
    }

}
