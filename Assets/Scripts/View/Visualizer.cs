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
    [HideInInspector] public Transform blocksParent;
    public GameObject blockPrefab;
    public GameObject piecePrefab;
    public Text diceLabel;
    public Text playerLabel;

    public PlayerData[] playersData;
    private PlayerData player;
    public Player[] players;
    Board board;
    public Game game;
    public Thread gameThread;
    public Thread mainThread;
    public TaskManager taskManager;

    void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    void Start() {
        if(DataForGameScene.isValid)
            playersData = DataForGameScene.playersData;
        if(log)
            gameObject.AddComponent(typeof(Logger));

        if(ui)
            new GameGUIMaker(this);

        mainThread = Thread.CurrentThread;
        // mainThread.Name = "MainThread";
        
        SetupGameLogic();
        taskManager = new TaskManager();
        gameObject.AddComponent(typeof(EventListenersManager));
        gameThread = game.Start();
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Space))
            if(!game.paused)
                game.Pause();
            else
                game.Resume();


        if(taskManager.HasTask()) {
            // game.Pause();
            Debug.Log("tasks: " + taskManager.TasksCount());
            taskManager.Do();
            // game.Resume();
        }
    }

    private void OnApplicationQuit() {
        Debug.Log("Quiting with " + taskManager.TasksCount() + " task");
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
        PlayerType[] playerTypes = new PlayerType[playersData.Length];
        for(int i = 0; i < playersData.Length; i++)
            playerTypes[i] = playersData[i].type;

        board = new Board(4, 40, 4);
        game = new Game(board, playerTypes, turnBased);
        players = game.players;
        player = playersData[0];

    }

    public IEnumerator OnRolledDice(object obj, RollDiceEventArgs e) {
        taskManager.taskRunning = true;
        game.Pause();

        diceLabel.text = "";
        for(int i = 0; i < 6; i++) {
            yield return new WaitForSeconds(.1f);
            diceLabel.text += ".";
        }
        diceLabel.text = e.diceNumber.ToString();

        game.Resume();
        taskManager.taskRunning = false;
    }

    public IEnumerator OnSetNextPlayer(object obj, SetNextTurnEventArgs e) {
        taskManager.taskRunning = true;
        game.Pause();

        diceLabel.text = "O";
        playerLabel.text = "setting...";
        yield return new WaitForSeconds(.3f);
        player = playersData[e.player.index];
        playerLabel.text = "Player " + e.player.index;
        playerLabel.color = player.color;
        yield return new WaitForSeconds(.3f);

        game.Resume();
        taskManager.taskRunning = false;
    }

    public IEnumerator OnGetInPiece(object obj, GetInPieceEventArgs e) {
        taskManager.taskRunning = true;
        game.Pause();

        // yield return new WaitForSeconds(1f);
        // SetToPosition(player.piecesParent, blocksParent, e.piece.index, e.piece.position);
        PieceUI pieceUi = player.piecesParent.GetChild(e.piece.index).GetComponent<PieceUI>();
        Transform[] transforms = new Transform[] {blocksParent.GetChild(e.piece.position)};
        yield return StartCoroutine(pieceUi.StepMove(transforms));


        game.Resume();
        taskManager.taskRunning = false;
    }

    public IEnumerator OnGetOutPiece(object obj, GetOutPieceEventArgs e) {
        taskManager.taskRunning = true;
        game.Pause();

        PieceUI pieceUi = playersData[e.piece.player.index].piecesParent.GetChild(e.piece.index).GetComponent<PieceUI>();
        Transform[] transforms = new Transform[] {playersData[e.piece.player.index].outsParent.GetChild(e.piece.position)};
        yield return StartCoroutine(pieceUi.StepMove(transforms));

        game.Resume();
        taskManager.taskRunning = false;
    }

    public IEnumerator OnMovePiece(object obj, MovePieceEventArgs e) {
        taskManager.taskRunning = true;
        game.Pause();

        PieceUI pieceUi = player.piecesParent.GetChild(e.piece.index).GetComponent<PieceUI>();
        Transform[] transforms = new Transform[e.steps.Length];
        Transform transform = blocksParent;
        for(int i = 0; i < e.steps.Length; i++) {
            if(i == e.inGoalIndex)
                transform = player.goalsParent;
            transforms[i] = transform.GetChild(e.steps[i]);
        }
        yield return StartCoroutine(pieceUi.StepMove(transforms));

        game.Resume();
        taskManager.taskRunning = false;
    }

    public void SetToPosition(Transform fromParent, Transform toParent, int fromIndex, int toIndex) {
        try{
            Transform from = fromParent.GetChild(fromIndex);
            Transform to = toParent.GetChild(toIndex);
            from.position = to.position + Vector3.back;
        } catch (Exception e) {
            Debug.LogError(e.Message);
            Debug.Log(fromIndex);
            Debug.Log(toIndex);
        }
    }

    public void OnDiceClick() {
        game.AttemptThrowDiceFromUser();
    }

}