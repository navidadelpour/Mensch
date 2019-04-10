using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceUI : MonoBehaviour {
    
    private float speed = 4;
    public float moveDelay = .2f;

    IEnumerator Move(Vector3 destination) {
        while(transform.position != destination) {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator StepMove(Transform[] steps) {
        for(int i = 0; i < steps.Length; i++) {
            yield return StartCoroutine(Move(steps[i].position));
            yield return new WaitForSeconds(moveDelay);
        }
    }

    public void OnClick(GameObject piece) {
        int pieceIndex = piece.transform.GetSiblingIndex();
        int playerIndex = piece.transform.parent.parent.GetSiblingIndex();
        Visualizer.instance.game.AttemptMovePieceFromUser(playerIndex, pieceIndex);
    }
}
