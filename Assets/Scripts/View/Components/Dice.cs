using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour {

    private SpriteRenderer image;

    public Transform rotationPoint;
    public Sprite[] sprites;
    public float rotationSpeed;
    public float moveSpeed;
    public float delay;
    public float minTime;
    public float maxTime;
    private bool condition;

    void Awake() {
        image = GetComponent<SpriteRenderer>();
    }

    public void OnDiceClick() {
        Visualizer.instance.game.AttemptThrowDiceFromUser();
    }

    public IEnumerator Throw(int diceNumber) {
        float time = Random.Range(minTime, maxTime);
        condition = true;
        StartCoroutine(Change());
        StartCoroutine(Rotate());
        SFX.instance.PlayDiceSound();
        yield return new WaitForSeconds(time);
        image.sprite = sprites[diceNumber - 1];
        condition = false;
    }


    public IEnumerator Change() {
        while(condition) {
            image.sprite = sprites[Random.Range(0, sprites.Length)];
            yield return new WaitForSeconds(delay);
        }
    }

    public IEnumerator Rotate() {
        float _rotationSpeed = Random.Range(rotationSpeed * .8f, rotationSpeed * 1.2f);
        float _moveSpeed = Random.Range(moveSpeed * .8f, moveSpeed * 1.2f);
        while(condition) {
            transform.RotateAround(transform.position, Vector3.forward, Time.deltaTime * _rotationSpeed);
            transform.RotateAround(rotationPoint.position, Vector3.forward, Time.deltaTime * _moveSpeed);
            yield return null;
        }
    }

}
