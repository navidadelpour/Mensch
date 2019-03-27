using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

public class Visualizer : MonoBehaviour {

    public bool turnBased;
    public bool log;
    public bool ui;
    public bool onePlayer;
    [HideInInspector] public Transform blocksParent;
    public GameObject blockPrefab;
    public Text diceLabel;
    public Text playerLabel;

    public PlayerData[] playersData;
    [HideInInspector] public PlayerData player;
    public Player[] players;
    Board board;
    public Game game;
    Thread gameThread;
    Thread mainThread;

    public delegate void Task();
    Queue<Task> tasksQueue = new Queue<Task>();

    void Start() {
        if(log)
            gameObject.AddComponent(typeof(Logger));

        if(ui)
            SetupGameVisual();

        Thread mainThread = Thread.CurrentThread;

        SetupGameLogic();
        gameThread = game.Start();
    }

    void Update() {
        // Debug.Log(tasksQueue.Count);
        if(Input.GetKeyDown(KeyCode.Space))
            if(!game.paused)
                game.Pause();
            else
                game.Resume();
        while(tasksQueue.Count > 0) {
            tasksQueue.Dequeue()();
        }
    }

    private void OnApplicationQuit() {
        Debug.Log("Quiting with " + tasksQueue.Count + " task");
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

    public void SetupGameLogic() {
        PlayerType[] playerTypes = new PlayerType[onePlayer ? 1 : playersData.Length];
        for(int i = 0; i < (onePlayer ? 1 : playersData.Length); i++)
            playerTypes[i] = playersData[i].type;

        board = new Board(4, 40, 4);
        game = new Game(board, playerTypes, turnBased);
        players = game.players;
        player = playersData[0];

        Subscribtion();
    }

    private void SetupGameVisual() {
        GameGUI gameGui = new GameGUI(this);
    }
    
    public void Subscribtion() {
        game.RolledDiceEvent += new Game.RollDiceHandler(OnRolledDice);
        game.SetNextTurnEvent += new Game.SetNextTurnHandler(OnSetNextPlayer);
        game.GetInPieceEvent += new Game.GetInPieceHandler(OnGetInPiece);
        game.GetOutPieceEvent += new Game.GetOutPieceHandler(OnGetOutPiece);
        game.MovePieceEvent += new Game.MovePieceHandler(OnMovePiece);
    }
    
    // FIXME: thread problems with unity API
    #region event listeners
    
    public void OnRolledDice(RollDiceEventArgs e) {
        tasksQueue.Enqueue(new Task(() => {
            diceLabel.text = e.diceNumber.ToString();
        }));
    }

    public void OnSetNextPlayer(SetNextTurnEventArgs e) {
        tasksQueue.Enqueue(new Task(() => {
            player = playersData[e.player.index];
            playerLabel.text = "Player " + e.player.index;
            playerLabel.color = player.color;
        }));
    }

    public void OnGetInPiece(GetInPieceEventArgs e) {
        tasksQueue.Enqueue(new Task(() => {
            SetToPosition(player.piecesParent, blocksParent, e.piece.index, e.piece.position);
        }));
    }

    public void OnGetOutPiece(GetOutPieceEventArgs e) {
        tasksQueue.Enqueue(new Task(() => {
            SetToPosition(playersData[e.piece.player.index].piecesParent, playersData[e.piece.player.index].outsParent, e.piece.player.index, e.piece.position);
        }));
    }

    public void OnMovePiece(MovePieceEventArgs e) {
        Transform t = blocksParent;
        if(e.piece.inGoal)
            t = player.goalsParent;
        tasksQueue.Enqueue(new Task(() => {
            SetToPosition(player.piecesParent, t, e.piece.index, e.piece.position);
        }));
    }

    public void SetToPosition(Transform fromParent, Transform toParent, int fromIndex, int toIndex) {
        Transform from = fromParent.GetChild(fromIndex);
        Transform to = toParent.GetChild(toIndex);
        from.position = to.position + Vector3.back;;
    }

    #endregion

    public void OnPieceClick() {
        // TODO: fetch piece data
        // game.MovePiece(null);
    }

    public void OnDiceClick() {
        // game.RollDice();
    }

}