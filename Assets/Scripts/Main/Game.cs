using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class Game {

    private Thread thread;
    public ManualResetEvent internalDelay;
    private ManualResetEvent externalDelay;

    private int delayTime;

    public bool end;
    public bool paused;
    public bool waitingForUserToDice = true;
    public bool waitingForUserToMove = true;
    private bool shouldDice = true;

    public Board board;
    private Player activePlayer;
    public Player[] players;
    public Piece playerSelectedPiece;

    private int diceNumber;
    private Random randomGenerator = new Random();

    public event EventHandler<RollDiceEventArgs> RolledDiceEvent;
    public event EventHandler<SetNextTurnEventArgs> SetNextTurnEvent;
    public event EventHandler<GetInPieceEventArgs> GetInPieceEvent;
    public event EventHandler<GetOutPieceEventArgs> GetOutPieceEvent;
    public event EventHandler<MovePieceEventArgs> MovePieceEvent;

    public Game(Board board, PlayerType[] playerTypes) {
        this.board = board;
        delayTime = 500;

        InitPlayers(playerTypes);
    }

    private void InitPlayers(PlayerType[] playerTypes) {
        players = new Player[4];
        for(int i = 0; i < playerTypes.Length; i++) {
            players[i] = PlayerFactory.Create(playerTypes[i], this, i);
        }
    }

    public Thread Start() {
        thread = new Thread(new ThreadStart(NextTurn));
        thread.Name = "GameThread";
        externalDelay = new ManualResetEvent(true);
        internalDelay = new ManualResetEvent(false);
        internalDelay.WaitOne(delayTime);
        thread.Start();
        return thread;
    }

    public void Pause() {
        paused = true;
        externalDelay.Reset();
    }

    public void Resume() {
        paused = false;
        externalDelay.Set();
    }

    public void End() {
        end = true;
    }

    // called from main thread
    public void AttemptMovePieceFromUser(int playerIndex, int pieceIndex) {
        if(waitingForUserToMove) {
            Player player = players[playerIndex];
            if(player == null)
                return;
            playerSelectedPiece = player.pieces[pieceIndex];
            waitingForUserToMove = false;
        }
    }

    // called from main thread
    public void AttemptThrowDiceFromUser() {
        if(waitingForUserToDice) {
            waitingForUserToDice = false;
        }
    }

    public void TryMovePiece(Piece piece) {
        externalDelay.WaitOne();

        KeyValuePair<Piece, BlockType> hitted = piece.GetBlock(diceNumber);
        Piece possibleHittedPiece = hitted.Key;
        BlockType blockType = hitted.Value;
        // UnityEngine.Debug.Log(blockType.ToString() + ": " + possibleHittedPiece); 


        if(shouldDice || !piece.Belongs(activePlayer) || !piece.CanMove(blockType)) {
            waitingForUserToMove = true;
            activePlayer.DoMove(diceNumber);
            return;
        }

        // start moving
        if(piece.isIn) {
            KeyValuePair<int[], int> stepsData = piece.Go(blockType, diceNumber);
            MovePieceEvent(this, new MovePieceEventArgs(piece, diceNumber, stepsData.Key, stepsData.Value));
            internalDelay.WaitOne(delayTime);
        } else if(diceNumber == 6){
            piece.GetIn();
            GetInPieceEvent(this, new GetInPieceEventArgs(piece));
            internalDelay.WaitOne(delayTime);
        }

        // hit enemy
        if(blockType == BlockType.ENEMY) {
            possibleHittedPiece.GetOut();
            GetOutPieceEvent(this, new GetOutPieceEventArgs(possibleHittedPiece));
            internalDelay.WaitOne(delayTime);
        }

        shouldDice = true;
        if(!HasWinner()) {
            NextTurn();
        } else {
            UnityEngine.Debug.Log(activePlayer.index + " win");
        }
    }

    public void TryThrowDice() {
        externalDelay.WaitOne();
        if(!shouldDice)
            return;
        shouldDice = false;

        ThrowDice();
        RolledDiceEvent(this, new RollDiceEventArgs(diceNumber, activePlayer));
        internalDelay.WaitOne(delayTime);

        if(activePlayer.CanMove(diceNumber))
            activePlayer.DoMove(diceNumber);
        else
            NextTurn();
    }

    private void NextTurn() {
        externalDelay.WaitOne();
        if(end)
            return;

        if(diceNumber != 6){
            SetNextPlayer();
            SetNextTurnEvent(this, new SetNextTurnEventArgs(activePlayer));
            internalDelay.WaitOne(delayTime);
        }

        shouldDice = true;
        activePlayer.DoDice();
    }

    private void ThrowDice() {
        diceNumber = randomGenerator.Next(1, 7);
    }


    private void SetNextPlayer() {
        int currentPlayerIndex = Array.IndexOf(players, activePlayer);
        do {
            activePlayer = players[(++currentPlayerIndex) % players.Length];
        } while(activePlayer == null);
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