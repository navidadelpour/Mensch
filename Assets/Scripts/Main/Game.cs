using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class Game {

    private Thread thread;
    private ManualResetEvent mre;

    public bool end;
    public bool paused;
    private bool turnBased;
    private bool shouldDice = true;

    public Board board;
    private Player activePlayer;
    public Player[] players;

    private int diceNumber;
    private Random randomGenerator = new Random();

    public event EventHandler<RollDiceEventArgs> RolledDiceEvent;
    public event EventHandler<SetNextTurnEventArgs> SetNextTurnEvent;
    public event EventHandler<GetInPieceEventArgs> GetInPieceEvent;
    public event EventHandler<GetOutPieceEventArgs> GetOutPieceEvent;
    public event EventHandler<MovePieceEventArgs> MovePieceEvent;

    public Game(Board board, PlayerType[] playerTypes, bool turnBased) {
        this.board = board;
        this.turnBased = turnBased;

        InitPlayers(playerTypes);
    }

    private void InitPlayers(PlayerType[] playerTypes) {
        players = new Player[playerTypes.Length];
        for(int i = 0; i < playerTypes.Length; i++) {
            players[i] = PlayerFactory.Create(playerTypes[i], this, i);
        }
    }

    public Thread Start() {
        thread = new Thread(new ThreadStart(NextTurn));
        mre = new ManualResetEvent(true);
        thread.Start();
        return thread;
    }

    public void Pause() {
        paused = true;
        mre.Reset();
    }

    public void Resume() {
        paused = false;
        mre.Set();
    }

    public void End() {
        end = true;
    }

    public void TryMovePiece(int playerIndex, int pieceIndex) {
        TryMovePiece(players[playerIndex].pieces[pieceIndex]);
    }

    public void TryMovePiece(Piece piece) {
        // checking right state
        if(shouldDice)
            return;

        // checking right player
        if(!piece.Belongs(activePlayer)) {
            return;
        }

        KeyValuePair<Piece, BlockType> hitted = piece.GetBlock(diceNumber);
        Piece possibleHittedPiece = hitted.Key;
        BlockType blockType = hitted.Value;
        UnityEngine.Debug.Log(blockType.ToString() + ": " + possibleHittedPiece); 

        // checking right move
        if(!piece.CanMove(blockType)) {
            return;
        }

        // start moving
        if(piece.isIn) {
            if(blockType == BlockType.INGOAL)
                piece.GoInGoal(diceNumber);
            else
                piece.GoForward(diceNumber);
            MovePieceEvent(this, new MovePieceEventArgs(piece, diceNumber));
        } else if(diceNumber == 6){
            piece.GetIn();
            GetInPieceEvent(this, new GetInPieceEventArgs(piece));
        }

        if(blockType == BlockType.ENEMY) {
            possibleHittedPiece.GetOut();
            GetOutPieceEvent(this, new GetOutPieceEventArgs(possibleHittedPiece));
        }

        shouldDice = true;
        if(!HasWinner()) {
            if(turnBased)
                Pause();
            mre.WaitOne();

            NextTurn();
        } else {
            UnityEngine.Debug.Log(activePlayer.index + " win");
        }
    }

    public void TryThrowDice() {
        if(!shouldDice)
            return;
        shouldDice = false;

        ThrowDice();
        RolledDiceEvent(this, new RollDiceEventArgs(diceNumber, activePlayer));
        if(turnBased)
            Pause();
        mre.WaitOne();

        if(activePlayer.CanMove(diceNumber))
            activePlayer.DoMove(diceNumber);
        else
            NextTurn();
    }

    private void NextTurn() {
        if(end)
            return;

        if(diceNumber != 6){
            SetNextPlayer();
            SetNextTurnEvent(this, new SetNextTurnEventArgs(activePlayer));
        }

        shouldDice = true;
        activePlayer.DoDice();
    }

    private void ThrowDice() {
        diceNumber = randomGenerator.Next(1, 7);
    }


    private void SetNextPlayer() {
        activePlayer = players[(Array.IndexOf(players, activePlayer) + 1) % players.Length];
    }

    private bool HasWinner() {
        bool win = true;
        for(int i = 0; i < activePlayer.pieces.Length; i++) {
            if(!activePlayer.pieces[i].inGoal) {
                win = false;
                break;
            }
        }
        return win;
    }

}