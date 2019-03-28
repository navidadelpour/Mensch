using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

public class Visualizer : MonoBehaviour {

    public static Visualizer instance;

    public bool turnBased;
    public bool log;
    public bool ui;
    public bool onePlayer;
    [HideInInspector] public Transform blocksParent;
    public GameObject blockPrefab;
    public GameObject piecePrefab;
    public Text diceLabel;
    public Text playerLabel;

    public PlayerData[] playersData;
    [HideInInspector] public PlayerData player;
    public Player[] players;
    Board board;
    public Game game;
    Thread gameThread;

    public delegate void Task();
    Queue<Task> tasksQueue = new Queue<Task>();

    void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    void Start() {
        if(log)
            gameObject.AddComponent(typeof(Logger));

        if(ui)
            new GameGUIMaker(this);


        SetupGameLogic();
        gameThread = game.Start();
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Space))
            if(!game.paused)
                game.Pause();
            else
                game.Resume();
        if(tasksQueue.Count > 0) {
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

    public void Subscribtion() {
        game.RolledDiceEvent += new EventHandler<RollDiceEventArgs>(OnRolledDice);
        game.SetNextTurnEvent += new EventHandler<SetNextTurnEventArgs>(OnSetNextPlayer);
        game.GetInPieceEvent += new EventHandler<GetInPieceEventArgs>(OnGetInPiece);
        game.GetOutPieceEvent += new EventHandler<GetOutPieceEventArgs>(OnGetOutPiece);
        game.MovePieceEvent += new EventHandler<MovePieceEventArgs>(OnMovePiece);
    }
    
    public void OnRolledDice(object obj, RollDiceEventArgs e) {
        tasksQueue.Enqueue(new Task(() => {
            diceLabel.text = e.diceNumber.ToString();
        }));
    }

    public void OnSetNextPlayer(object obj, SetNextTurnEventArgs e) {
        tasksQueue.Enqueue(new Task(() => {
            player = playersData[e.player.index];
            playerLabel.text = "Player " + e.player.index;
            playerLabel.color = player.color;
        }));
    }

    public void OnGetInPiece(object obj, GetInPieceEventArgs e) {
        tasksQueue.Enqueue(new Task(() => {
            SetToPosition(player.piecesParent, blocksParent, e.piece.index, e.piece.position);
        }));
    }

    public void OnGetOutPiece(object obj, GetOutPieceEventArgs e) {
        tasksQueue.Enqueue(new Task(() => {
            SetToPosition(playersData[e.piece.player.index].piecesParent, playersData[e.piece.player.index].outsParent, e.piece.index, e.piece.position);
        }));
    }

    public void OnMovePiece(object obj, MovePieceEventArgs e) {
        tasksQueue.Enqueue(new Task(() => {
            Transform t = blocksParent;
            if(e.piece.inGoal)
                t = player.goalsParent;
            SetToPosition(player.piecesParent, t, e.piece.index, e.piece.position);
        }));
    }

    public void SetToPosition(Transform fromParent, Transform toParent, int fromIndex, int toIndex) {
        Transform from = fromParent.GetChild(fromIndex);
        Transform to = toParent.GetChild(toIndex);
        from.position = to.position + Vector3.back;;
    }

    public void OnDiceClick() {
        Debug.Log("HUMAN DICE");
        game.TryThrowDice();
    }

}