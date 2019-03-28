using System;
using UnityEngine;

[Serializable]
public class GameGUIMaker {

    private Visualizer visualizer;
    private Transform playersParent;
    private GameObject player;
    private Vector2[] directions;

    public GameGUIMaker(Visualizer visualizer) {
        this.visualizer = visualizer;
        SetupRoadBlocks();
        SetupPlayerArea();
    }

    private void SetupRoadBlocks() {
        visualizer.blocksParent = (new GameObject("Blocks")).transform;
        Vector2 position = new Vector2(-1, -5);
        Vector2[] directions = {
            Vector2.up, Vector2.left, Vector2.up,
            Vector2.right, Vector2.up, Vector2.right,
            Vector2.down, Vector2.right, Vector2.down,
            Vector2.left, Vector2.down, Vector2.left,
        };
        int repeat;
        int index = 1;
        for (int i = 0; i < directions.Length; i++) {
            repeat = (i + 1) % 3 == 0 ? 2 : 4;
            for(int j = 0; j < repeat; j++) {
                position += directions[i];
                GameObject block = MonoBehaviour.Instantiate(visualizer.blockPrefab, position, Quaternion.identity, visualizer.blocksParent);
                block.name = "block " + index;
                index ++;
            }
        }
        visualizer.blocksParent.GetChild(visualizer.blocksParent.childCount - 1).SetSiblingIndex(0);
        visualizer.blocksParent.GetChild(0).name = "block " + 0;
    }

    private void SetupPlayerArea() {
        playersParent = (new GameObject("Players")).transform;
        directions = new Vector2[] {
            Vector2.down, Vector2.left, Vector2.up, Vector2.right
        };
        for(int i = 0; i < visualizer.playersData.Length; i++) {
            SetupPlayer(i);
            SetupDicePosition(i);
            SetupGoalBlocks(i);
            SetupOutAndPiecesBlocks(i);
        }
    }

    private void SetupPlayer(int i) {
        player = new GameObject("player " + i);
        player.transform.parent = playersParent;
        visualizer.playersData[i].transform = player.transform;
    }

    private void SetupDicePosition(int i) {
        Vector2 position = (directions[i] + directions[(i + 1) % 4]) * 3;
        GameObject dice = new GameObject("Dice Position");
        dice.transform.position = position;
        dice.transform.parent = player.transform;
        visualizer.playersData[i].dice = dice.transform;
    }

    private void SetupGoalBlocks(int i) {
        Transform goalsParent = (new GameObject("Goals")).transform;
        goalsParent.parent = player.transform;
        visualizer.playersData[i].goalsParent = goalsParent;
        Vector2 position = directions[i] * 5;
        for(int j = 0; j < 4; j++) {
            position -= directions[i];
            GameObject block = MonoBehaviour.Instantiate(visualizer.blockPrefab, position, Quaternion.identity, goalsParent);
            block.name = "goal block " + j;
            block.transform.localScale *= .5f;
        }

    }

    private void SetupOutAndPiecesBlocks(int i) {
        Transform outsParent = (new GameObject("Outs")).transform;
        outsParent.parent = player.transform;
        visualizer.playersData[i].outsParent = outsParent;

        Transform piecesParent = (new GameObject("Pieces")).transform;
        piecesParent.parent = player.transform;
        visualizer.playersData[i].piecesParent = piecesParent;

        Vector2 position = (directions[i] + directions[(i + 1) % 4]) * 4;
        for(int j = 0; j < 4; j++) {
            position += directions[(i + j) % 4];
            GameObject block = MonoBehaviour.Instantiate(visualizer.blockPrefab, position, Quaternion.identity, outsParent);
            block.name = "out block " + j;
            GameObject piece = MonoBehaviour.Instantiate(visualizer.piecePrefab, position, Quaternion.identity, piecesParent);
            piece.name = "piece " + j;
            piece.GetComponent<SpriteRenderer>().color = visualizer.playersData[i].color;
            piece.transform.position += Vector3.back;
        }
    }

}