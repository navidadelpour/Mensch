using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;
using System.Collections.Generic;

public class Visualizer : MonoBehaviour {

    public bool turnBased;
    public bool log;
    public PlayerData[] playersData;
    public Player[] players;
    Board board;
    public Game game;
    Thread gameThread;

    public GameObject blockPrefab;
    public Text dice;

    void Start() {
        if(log)
            gameObject.AddComponent(typeof(Logger));

        SetupGameVisual();
        SetupGameLogic();
        gameThread = game.Start();
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.B))
            if(!game.paused)
                game.Pause();
            else
                game.Resume();
    }

    private void OnApplicationQuit() {
        game.Pause();
    }

    void OnApplicationPause(bool pauseStatus) {
        if(gameThread == null)
            return;
        if(!pauseStatus)
            game.Pause();
        else
            game.Resume();
    }

    public void SetupGameLogic() {
        PlayerType[] playerTypes = new PlayerType[playersData.Length];
        for(int i = 0; i < playersData.Length; i++)
            playerTypes[i] = playersData[i].type;

        board = new Board(4, 40, 4);
        game = new Game(board, playerTypes, turnBased);
        players = game.players;

        Subscribtion();
    }

    public void SetupGameVisual() {
        // setup board normal blocks
        Transform blocksParent = (new GameObject("Blocks")).transform;
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
                GameObject block = Instantiate(blockPrefab, position, Quaternion.identity, blocksParent);
                block.name = "block " + index;
                index ++;
            }
        }
        blocksParent.GetChild(blocksParent.childCount - 1).SetSiblingIndex(0);
        blocksParent.GetChild(0).name = "block " + 0;



        // setup players area
        Transform playersParent = (new GameObject("Players")).transform;
        directions = new Vector2[] {
            Vector2.down, Vector2.left, Vector2.up, Vector2.right
        };
        for(int i = 0; i < 4; i++) {
            GameObject player = new GameObject("player " + i);
            player.transform.parent = playersParent;
            playersData[i].transform = player.transform;

            // setup dice position
            position = (directions[i] + directions[(i + 1) % 4]) * 3;
            GameObject dice = new GameObject("Dice Position");
            dice.transform.position = position;
            dice.transform.parent = player.transform;
            playersData[i].dice = dice.transform;


            // setup goal positions;
            Transform goalsParent = (new GameObject("Goals")).transform;
            goalsParent.parent = player.transform;
            playersData[i].goalsParent = goalsParent;
            position = Vector2.zero;
            for(int j = 0; j < 4; j++) {
                position += directions[i];
                GameObject block = Instantiate(blockPrefab, position, Quaternion.identity, goalsParent);
                block.name = "goal block " + j;
                block.transform.localScale *= .5f;
            }

            // setup out positions;
            Transform outsParent = (new GameObject("Outs")).transform;
            outsParent.parent = player.transform;
            playersData[i].outsParent = outsParent;

            Transform piecesParent = (new GameObject("Pieces")).transform;
            piecesParent.parent = player.transform;
            playersData[i].piecesParent = piecesParent;

            position = (directions[i] + directions[(i + 1) % 4]) * 4;
            for(int j = 0; j < 4; j++) {
                position += directions[(i + j) % 4];
                GameObject block = Instantiate(blockPrefab, position, Quaternion.identity, outsParent);
                block.name = "out block " + j;
                GameObject piece = Instantiate(blockPrefab, position, Quaternion.identity, piecesParent);
                piece.name = "piece " + j;
                piece.GetComponent<SpriteRenderer>().color = playersData[i].color;
            }

        }
    }

    public void OnPieceClick() {
        // TODO: fetch piece data
        game.MovePiece(null);
    }

    public void OnDiceClick() {
        game.RollDice();
    }

    public void Subscribtion() {
        game.RolledDiceEvent += new Game.RollDiceHandler(OnRolledDice);
        game.SetNextTurnEvent += new Game.SetNextTurnHandler(OnSetNextPlayer);
        game.GetInPieceEvent += new Game.GetInPieceHandler(OnGetInPiece);
        game.GetOutPieceEvent += new Game.GetOutPieceHandler(OnGetOutPiece);
        game.MovePieceEvent += new Game.MovePieceHandler(OnMovePiece);
    }
    public void OnRolledDice(RollDiceEventArgs e) {

    }

    public void OnSetNextPlayer(SetNextTurnEventArgs e) {

    }

    public void OnGetInPiece(GetInPieceEventArgs e) {

    }

    public void OnGetOutPiece(GetOutPieceEventArgs e) {

    }

    public void OnMovePiece(MovePieceEventArgs e) {

    }

}