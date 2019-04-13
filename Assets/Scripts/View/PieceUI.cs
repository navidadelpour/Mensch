using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceUI : MonoBehaviour {
    
    private float speed = 4;
    public float moveDelay = .2f;
    Vector3 zIndex = Vector3.back * 2;

    IEnumerator Move(Vector3 destination, float multiplier) {
        destination += zIndex;
        SFX.instance.PlayPieceSound();
        while(transform.position != destination) {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed * multiplier);
            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator StepMove(Transform[] steps, float multiplier = 1) {
        yield return new WaitForSeconds(moveDelay);
        for(int i = 0; i < steps.Length; i++) {
            yield return StartCoroutine(Move(steps[i].position, multiplier));
            yield return new WaitForSeconds(moveDelay);
        }
        transform.position -= zIndex / 2;
    }

    public void OnClick(GameObject piece) {
        int pieceIndex = piece.transform.GetSiblingIndex();
        int playerIndex = piece.transform.parent.parent.GetSiblingIndex();
        Visualizer.instance.game.AttemptMovePieceFromUser(playerIndex, pieceIndex);
    }
}
