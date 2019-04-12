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
    public event EventHandler<WinEventArgs> WinEvent;

    public Game(Board board, PlayerType[] playerTypes) {
        this.board = board;
        delayTime = 200;

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
        InternalWait();
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

    public void InternalWait() {
        internalDelay.WaitOne(delayTime);
    }

    // called from main thread
    public void AttemptMovePieceFromUser(int playerIndex, int pieceIndex) {
        if(waitingForUserToMove) {
            Player player = players[playerIndex];
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


        if(shouldDice || !piece.Belongs(activePlayer) || !piece.CanMove(blockType)) {
            waitingForUserToMove = true;
            activePlayer.DoMove(diceNumber);
            return;
        }

        // start moving
        if(piece.isIn) {
            KeyValuePair<int[], int> stepsData = piece.Go(blockType, diceNumber);
            MovePieceEvent(this, new MovePieceEventArgs(piece, diceNumber, stepsData.Key, stepsData.Value));
            InternalWait();
        } else if(diceNumber == 6){
            piece.GetIn();
            GetInPieceEvent(this, new GetInPieceEventArgs(piece));
            InternalWait();
        }

        // hit enemy
        if(blockType == BlockType.ENEMY) {
            possibleHittedPiece.GetOut();
            GetOutPieceEvent(this, new GetOutPieceEventArgs(possibleHittedPiece));
            InternalWait();
        }

        shouldDice = true;
        if(!HasWinner()) {
            NextTurn();
        } else {
            WinEvent(this, new WinEventArgs(activePlayer));
        }
    }

    public void TryThrowDice() {
        externalDelay.WaitOne();
        if(!shouldDice)
            return;
        shouldDice = false;

        ThrowDice();

        RolledDiceEvent(this, new RollDiceEventArgs(diceNumber, activePlayer));
        InternalWait();

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
            InternalWait();
        }

        shouldDice = true;
        activePlayer.DoDice();
    }

    private void ThrowDice() {
        diceNumber = randomGenerator.Next(1, 7);
    }


    private void SetNextPlayer() {
        int currentPlayerIndex = activePlayer == null ? -1 : Array.IndexOf(players, activePlayer);
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