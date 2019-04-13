using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour {

    private Text text;
    private Animator animator;

    void Awake() {
        text = transform.GetChild(0).GetComponent<Text>();
        animator = GetComponent<Animator>();
    }

    void Start() {

    }

    void Update() {
        
    }

    public void Popup(string message) {
        text.text = message;
        animator.SetTrigger("popup");
    }
}
