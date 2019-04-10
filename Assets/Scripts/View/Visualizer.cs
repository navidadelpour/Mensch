using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

public class Visualizer : MonoBehaviour {

    public static Visualizer instance;

    [HideInInspector] public Transform blocksParent;
    public GameObject blockPrefab;
    public GameObject piecePrefab;
    public Text diceLabel;

    public PlayerData[] playersData;
    private PlayerData player;
    public Player[] players;
    Board board;
    public Game game;
    public Thread gameThread;
    public TaskManager taskManager;

    private bool started;
    private PlayerIconsHandler playerIconsHandler;

    void Awake() {
        if(instance == null) {
            instance = this;
        }
        playersData = new PlayerData[4];
        diceLabel.gameObject.SetActive(false);
    }

    void Update() {
        if(started && taskManager.HasTask()) {
            Debug.Log("tasks: " + taskManager.TasksCount());
            taskManager.Do();
        }
    }

    private void OnApplicationQuit() {
        if(game != null)
            game.End();
    }

    void OnApplicationPause(bool pauseStatus) {
        if(gameThread == null)
            return;
        if(!game.paused)
            game.Pause();
        else
            game.Resume();
    }


    public void Setup() {
        PlayerType[] playerTypes = new PlayerType[playersData.Length];
        for(int i = 0; i < playersData.Length; i++)
            playerTypes[i] = playersData[i].type;

        board = new Board(4, 40, 4);
        game = new Game(board, playerTypes);
        players = game.players;
        player = playersData[0];

        new GameGUIMaker(this);
        taskManager = new TaskManager();
        gameObject.AddComponent(typeof(EventListenersManager));
        playerIconsHandler = GetComponent<PlayerIconsHandler>();

        gameThread = game.Start();
        diceLabel.gameObject.SetActive(true);
        started = true;
    }

    public IEnumerator OnRolledDice(object obj, RollDiceEventArgs e) {
        diceLabel.text = "";
        yield return new WaitForSeconds(.2f);
        for(int i = 0; i < 6; i++) {
            yield return new WaitForSeconds(.1f);
            diceLabel.text += ".";
        }
        diceLabel.text = e.diceNumber.ToString();
        yield return new WaitForSeconds(.2f);
    }

    public IEnumerator OnSetNextPlayer(object obj, SetNextTurnEventArgs e) {
        diceLabel.text = "O";
        yield return new WaitForSeconds(.5f);
        player = playersData[e.player.index];
        playerIconsHandler.NextPlayer(e.player.index);
    }

    public IEnumerator OnGetInPiece(object obj, GetInPieceEventArgs e) {
        PieceUI pieceUi = player.piecesParent.GetChild(e.piece.index).GetComponent<PieceUI>();
        Transform[] transforms = new Transform[] {blocksParent.GetChild(e.piece.position)};
        yield return StartCoroutine(pieceUi.StepMove(transforms));
    }

    public IEnumerator OnGetOutPiece(object obj, GetOutPieceEventArgs e) {
        PieceUI pieceUi = playersData[e.piece.player.index].piecesParent.GetChild(e.piece.index).GetComponent<PieceUI>();
        Transform[] transforms = new Transform[] {playersData[e.piece.player.index].outsParent.GetChild(e.piece.position)};
        yield return StartCoroutine(pieceUi.StepMove(transforms));
    }

    public IEnumerator OnMovePiece(object obj, MovePieceEventArgs e) {
        PieceUI pieceUi = player.piecesParent.GetChild(e.piece.index).GetComponent<PieceUI>();
        Transform[] transforms = new Transform[e.steps.Length];
        Transform transform = blocksParent;
        for(int i = 0; i < e.steps.Length; i++) {
            if(i == e.inGoalIndex)
                transform = player.goalsParent;
            transforms[i] = transform.GetChild(e.steps[i]);
        }
        yield return StartCoroutine(pieceUi.StepMove(transforms));
    }

    public IEnumerator OnWin(object obj, WinEventArgs e) {
        yield return null;
    }

}