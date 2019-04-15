using System;
using System.Collections;
using UnityEngine;

public class EventListenersManager : MonoBehaviour {
    private bool started;
    private Game game;
    private TaskManager taskManager;
    private Visualizer visualizer;

    public void Awake() {
        this.visualizer = Visualizer.instance;
        this.game = visualizer.game;
        this.taskManager = new TaskManager();
        started = true;
        Subscribtion();
    }

    void Update() {
        if(started && taskManager.HasTask()) {
            Debug.Log("tasks: " + taskManager.TasksCount());
            taskManager.Do();
        }
    }


    public void Subscribtion() {
        game.RolledDiceEvent += new EventHandler<RollDiceEventArgs>(OnRolledDice);
        game.SetNextTurnEvent += new EventHandler<SetNextTurnEventArgs>(OnSetNextPlayer);
        game.GetInPieceEvent += new EventHandler<GetInPieceEventArgs>(OnGetInPiece);
        game.GetOutPieceEvent += new EventHandler<GetOutPieceEventArgs>(OnGetOutPiece);
        game.MovePieceEvent += new EventHandler<MovePieceEventArgs>(OnMovePiece);
        game.WinEvent += new EventHandler<WinEventArgs>(OnWin);
        game.ShouldDiceEvent += new EventHandler(OnShouldDiceEvent);
        game.OutTurnEvent += new EventHandler(OnOutTurnEvent);
        game.CantMoveEvent += new EventHandler(OnCantMoveEvent);
        game.ShouldMoveEvent += new EventHandler(OnShouldMoveEvent);
    }

    public void OnRolledDice(object obj, RollDiceEventArgs e) {
        taskManager.Add(() => {
            StartCoroutine(SafeRun(visualizer.OnRolledDice(obj, e)));
        });
    }

    public void OnSetNextPlayer(object obj, SetNextTurnEventArgs e) {
        taskManager.Add(() => {
            StartCoroutine(SafeRun(visualizer.OnSetNextPlayer(obj, e)));
        });
    }

    public void OnGetInPiece(object obj, GetInPieceEventArgs e) {
        taskManager.Add(() => {
            StartCoroutine(SafeRun(visualizer.OnGetInPiece(obj, e)));
        });
    }

    public void OnGetOutPiece(object obj, GetOutPieceEventArgs e) {
        taskManager.Add(() => {
            StartCoroutine(SafeRun(visualizer.OnGetOutPiece(obj, e)));
        });
    }

    public void OnMovePiece(object obj, MovePieceEventArgs e) {
        taskManager.Add(() => {
            StartCoroutine(SafeRun(visualizer.OnMovePiece(obj, e)));
        });
    }

    public void OnWin(object obj, WinEventArgs e) {
        taskManager.Add(() => {
            StartCoroutine(SafeRun(visualizer.OnWin(obj, e)));
        });
    }

    public void OnShouldDiceEvent(object obj, EventArgs e) {
        taskManager.Add(() => {
            StartCoroutine(SafeRun(visualizer.OnShouldDiceEvent(obj, e)));
        });
    }

    public void OnOutTurnEvent(object obj, EventArgs e) {
        taskManager.Add(() => {
            StartCoroutine(SafeRun(visualizer.OnOutTurnEvent(obj, e)));
        });
    }

    public void OnCantMoveEvent(object obj, EventArgs e) {
        taskManager.Add(() => {
            StartCoroutine(SafeRun(visualizer.OnCantMoveEvent(obj, e)));
        });
    }

    public void OnShouldMoveEvent(object obj, EventArgs e) {
        taskManager.Add(() => {
            StartCoroutine(SafeRun(visualizer.OnShouldMoveEvent(obj, e)));
        });
    }

    public IEnumerator SafeRun(IEnumerator runnable) {
        taskManager.taskRunning = true;
        game.Pause();
        yield return StartCoroutine(runnable);
        game.Resume();
        taskManager.taskRunning = false;
    }

}