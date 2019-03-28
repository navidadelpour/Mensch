using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick(GameObject piece) {
        int pieceIndex = piece.transform.GetSiblingIndex();
        int playerIndex = piece.transform.parent.parent.GetSiblingIndex();
        Visualizer.instance.game.TryMovePiece(playerIndex, pieceIndex);
    }
}
