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
    public bool waitingForUserToDice;
    public bool waitingForUserToChoose;

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
    public event EventHandler ShouldDiceEvent;
    public event EventHandler OutTurnEvent;
    public event EventHandler CantMoveEvent;
    public event EventHandler ShouldMoveEvent;

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

    public void Pause() {
        externalDelay.Reset();
    }

    public void Resume() {
        externalDelay.Set();
    }

    public void End() {
        end = true;
    }

    public void Delay() {
        internalDelay.WaitOne(delayTime);
        externalDelay.WaitOne();
    }

    // called from main thread
    public void Start() {
        externalDelay = new ManualResetEvent(true);
        internalDelay = new ManualResetEvent(false);

        thread = new Thread(new ThreadStart(Run));
        thread.Start();
    }

    // called from main thread
    public void AttemptChoosePieceFromUser(int playerIndex, int pieceIndex) {
        Player player = players[playerIndex];
        playerSelectedPiece = player.pieces[pieceIndex];

        if(waitingForUserToChoose) {
            Block hitted = playerSelectedPiece.GetBlock(diceNumber);
            
            if(!playerSelectedPiece.Belongs(activePlayer) || !playerSelectedPiece.CanMove(hitted.type)) {
                if (!playerSelectedPiece.Belongs(activePlayer))
                    OutTurnEvent(this, null);
                else if(!playerSelectedPiece.CanMove(hitted.type))
                    CantMoveEvent(this, null);
            } else
                waitingForUserToChoose = false;
        }
    }

    // called from main thread
    public void AttemptThrowDiceFromUser() {
        if(waitingForUserToDice) {
            waitingForUserToDice = false;
        }
    }

    public void Run() {
        while(!end) {
            if(diceNumber != 6)
                SetNextPlayer();

            if(activePlayer.Dice())
                ThrowDice();

            if(!activePlayer.CanMove(diceNumber))
                continue;

            Move(activePlayer.Choose(diceNumber));

            if(!HasWinner())
                continue;
            
            Win();
        }
    }
    private void Move(Piece piece) {
        Block block = piece.GetBlock(diceNumber);
        if(piece.isIn) {
            Steps stepsData = piece.Go(block.type, diceNumber);
            MovePieceEvent(this, new MovePieceEventArgs(piece, diceNumber, stepsData));
        } else if(diceNumber == 6){
            piece.GetIn();
            GetInPieceEvent(this, new GetInPieceEventArgs(piece));
        }

        // hit enemy
        if(block.type == BlockType.ENEMY) {
            block.piece.GetOut();
            GetOutPieceEvent(this, new GetOutPieceEventArgs(block.piece));
        }
        Delay();
    }

    private void ThrowDice() {
        diceNumber = randomGenerator.Next(1, 7);
        RolledDiceEvent(this, new RollDiceEventArgs(diceNumber, activePlayer));
        Delay();
    }

    private void SetNextPlayer() {
        int currentPlayerIndex = activePlayer == null ? -1 : Array.IndexOf(players, activePlayer);
        do {
            activePlayer = players[(++currentPlayerIndex) % players.Length];
        } while(activePlayer == null);
        SetNextTurnEvent(this, new SetNextTurnEventArgs(activePlayer));
        Delay();
    } 

    private void Win() {
        WinEvent(this, new WinEventArgs(activePlayer));
        end = true;
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