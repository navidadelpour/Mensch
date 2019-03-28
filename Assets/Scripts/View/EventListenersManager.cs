using System;
using UnityEngine;

public class EventListenersManager : MonoBehaviour {
    Game game;
    TaskManager taskManager;

    public void Awake() {
        this.game = Visualizer.instance.game;
        this.taskManager = Visualizer.instance.taskManager;
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
        taskManager.Add(() => {
            StartCoroutine(Visualizer.instance.OnRolledDice(obj, e));
        });
    }

    public void OnSetNextPlayer(object obj, SetNextTurnEventArgs e) {
        taskManager.Add(() => {
            StartCoroutine(Visualizer.instance.OnSetNextPlayer(obj, e));
        });
    }

    public void OnGetInPiece(object obj, GetInPieceEventArgs e) {
        taskManager.Add(() => {
            StartCoroutine(Visualizer.instance.OnGetInPiece(obj, e));
        });
    }

    public void OnGetOutPiece(object obj, GetOutPieceEventArgs e) {
        taskManager.Add(() => {
            StartCoroutine(Visualizer.instance.OnGetOutPiece(obj, e));
        });
    }

    public void OnMovePiece(object obj, MovePieceEventArgs e) {
        taskManager.Add(() => {
            StartCoroutine(Visualizer.instance.OnMovePiece(obj, e));
        });
    }

}