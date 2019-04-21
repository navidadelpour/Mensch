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
    public Dice dice;
    public DialogBox dialogBox;
    public WinPanel winPanel;

    public PlayerData[] playersData;
    public PlayerData activePlayer;
    private Board board;
    public Game game;

    private PlayerIconsHandler playerIconsHandler;

    void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    private void OnApplicationQuit() {
        EndGame();
    }

    void OnApplicationFocus(bool pauseStatus) {
        if(game == null)
            return;
        if(!pauseStatus) {
            game.paused = true;
            game.Pause();
        } else {
            game.paused = false;
            game.Resume();
        }
    }

    public void EndGame() {
        if(game == null)
            return;
        game.End();
    }

    public void Setup() {
        PlayerType[] playerTypes = new PlayerType[playersData.Length];
        for(int i = 0; i < playersData.Length; i++)
            playerTypes[i] = playersData[i].type;

        board = new Board(4, 40, 4);
        game = new Game(board, playerTypes);
        activePlayer = playersData[0];

        gameObject.AddComponent(typeof(EventListenersManager));
        playerIconsHandler = GetComponent<PlayerIconsHandler>();
        dice.gameObject.SetActive(true);

        game.Start();
    }

    public IEnumerator OnRolledDice(object obj, RollDiceEventArgs e) {
        yield return StartCoroutine(dice.Throw(e.diceNumber));
    }

    public IEnumerator OnSetNextPlayer(object obj, SetNextTurnEventArgs e) {
        yield return new WaitForSeconds(.5f);
        activePlayer = playersData[e.player.index];
        playerIconsHandler.NextPlayer(e.player.index);
        yield return new WaitForSeconds(.5f);        
    }

    public IEnumerator OnGetInPiece(object obj, GetInPieceEventArgs e) {
        PieceUI pieceUi = activePlayer.piecesParent.GetChild(e.piece.index).GetComponent<PieceUI>();
        Transform[] transforms = new Transform[] {blocksParent.GetChild(e.piece.position)};
        yield return StartCoroutine(pieceUi.StepMove(transforms));
    }

    public IEnumerator OnGetOutPiece(object obj, GetOutPieceEventArgs e) {
        PieceUI pieceUi = playersData[e.piece.player.index].piecesParent.GetChild(e.piece.index).GetComponent<PieceUI>();
        Transform[] transforms = new Transform[] {playersData[e.piece.player.index].outsParent.GetChild(e.piece.position)};
        SFX.instance.PlayPieceHitSound();
        yield return StartCoroutine(pieceUi.StepMove(transforms, 2));
    }

    public IEnumerator OnMovePiece(object obj, MovePieceEventArgs e) {
        PieceUI pieceUi = activePlayer.piecesParent.GetChild(e.piece.index).GetComponent<PieceUI>();
        Transform[] transforms = GetStepsTransform(e.steps.positions, e.steps.inGoalIndex);
        yield return StartCoroutine(pieceUi.StepMove(transforms));
    }

    private Transform[] GetStepsTransform(int[] steps, int inGoalIndex) {
        Transform[] transforms = new Transform[steps.Length];
        Transform transform = blocksParent;
        for(int i = 0; i < steps.Length; i++) {
            if(i == inGoalIndex)
                transform = activePlayer.goalsParent;
            transforms[i] = transform.GetChild(steps[i]);
        }
        return transforms;
    }

    public IEnumerator OnCantMoveEvent(object obj, EventArgs e) {
        // this piece cant move
        dialogBox.Popup("!ﻪﻨﮐ ﺖﮐﺮﺣ ﻪﻧﻮﺗ ﯽﻤﻧ ﻩﺮﻬﻣ ﻦﯾﺍ");
        yield return null;
    }

    public IEnumerator OnOutTurnEvent(object obj, EventArgs e) {
        // it's not your turn
        dialogBox.Popup("!ﺲﯿﻧ ﺖﺘﺑﻮﻧ");
        yield return null;
    }

    public IEnumerator OnShouldDiceEvent(object obj, EventArgs e) {
        // you should throw dice
        dialogBox.Popup("!ﯼﺯﺍﺪﻨﺑ ﺱﺎﺗ ﺪﯾﺎﺑ");
        yield return null;
    }

    public IEnumerator OnShouldMoveEvent(object obj, EventArgs e) {
        // you should move your piece
        dialogBox.Popup("!ﻩﺪﺑ ﻥﻮﮑﺗ ﻭﺭ ﺕﺍ ﻩﺮﻬﻣ");
        yield return null;
    }


    public IEnumerator OnWin(object obj, WinEventArgs e) {
        winPanel.Popup(playerIconsHandler.playerIcons[e.player.index].gameObject);
        yield return null;
    }

}