using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    private float delay = 2;

    void Start() {
        StartCoroutine(ChangeScene());
    }

    private IEnumerator ChangeScene() {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("GameScene");
    }
}
