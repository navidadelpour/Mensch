using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class Game {

    Thread thread;
    ManualResetEvent mre;

    bool end;
    public bool paused;
    public bool turnBased;
    public bool shouldDice = true;

    public Board board;
    public Player winnedPlayer;
    public Player activePlayer;
    public Player[] players;
    public int activePlayerIndex = -1;

    public int diceNumber;
    private Random randomGenerator = new Random();

    // TODO: safly invoking events...(a good choice is to update C# to v6 and use x?.invoke() notaion)
    #region declaring events 
    public delegate void RollDiceHandler(RollDiceEventArgs eventArgs);
    public event RollDiceHandler RolledDiceEvent;

    public delegate void SetNextTurnHandler(SetNextTurnEventArgs eventArgs);
    public event SetNextTurnHandler SetNextTurnEvent;

    public delegate void GetInPieceHandler(GetInPieceEventArgs eventArgs);
    public event GetInPieceHandler GetInPieceEvent;

    public delegate void GetOutPieceHandler(GetOutPieceEventArgs eventArgs);
    public event GetOutPieceHandler GetOutPieceEvent;
    
    public delegate void MovePieceHandler(MovePieceEventArgs eventArgs);
    public event MovePieceHandler MovePieceEvent;

    #endregion

    #region initialization
    public Game(Board board, PlayerType[] playerTypes, bool turnBased) {
        this.board = board;
        this.turnBased = turnBased;

        InitPlayers(playerTypes);
    }

    private void InitPlayers(PlayerType[] playerTypes) {
        players = new Player[playerTypes.Length];
        for(int i = 0; i < playerTypes.Length; i++) {
            switch (playerTypes[i]) {
                case PlayerType.HUMAN:
                    players[i] = new Player(this, i);
                    break;
                case PlayerType.AI:
                    players[i] = new AIPlayer(this, i);
                    break;
                default:
                    players[i] = null;
                    break;
            }
        }
    }

    #endregion

    // start the game
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

    public void TryMovePiece(Piece piece) {
        // checking right state
        if(shouldDice)
            return;
        shouldDice = true;

        // checking right player
        if(activePlayer != piece.player) {
            return;
        }

        KeyValuePair<Piece, BlockType> hitted = piece.GetBlock(diceNumber);
        Piece possibleHittedPiece = hitted.Key;
        BlockType blockType = hitted.Value;
        UnityEngine.Debug.Log(blockType.ToString()); 

        // checking right move
        if(blockType == BlockType.OUTGOAL || blockType == BlockType.ALLY) {
            return;
        }

        // start moving
        if(piece.isIn) {
            if(blockType == BlockType.INGOAL)
                piece.GoInGoal(diceNumber);
            else
                piece.GoForward(diceNumber);
            MovePieceEvent(new MovePieceEventArgs(piece, diceNumber));
        } else if(diceNumber == 6){
            piece.GetIn();
            GetInPieceEvent(new GetInPieceEventArgs(piece));
        }

        if(blockType == BlockType.ENEMY) {
            possibleHittedPiece.GetOut();
            GetOutPieceEvent(new GetOutPieceEventArgs(possibleHittedPiece));
        }

        if(!HasWinner())
            NextTurn();
    }

    public void TryThrowDice() {
        if(!shouldDice)
            return;
        shouldDice = false;

        ThrowDice();
        RolledDiceEvent(new RollDiceEventArgs(diceNumber, activePlayer));
        if(CanMove(activePlayer))
            activePlayer.DoMove(diceNumber);
        else
            NextTurn();
    }

    private void NextTurn() {
        if(end)
            return;
        Pauser();

        SetNextPlayer();
        SetNextTurnEvent(new SetNextTurnEventArgs(activePlayer));
        shouldDice = true;
        activePlayer.DoDice();
    }

    private void Pauser() {
        mre.WaitOne();
        if(turnBased)
            Pause();
    }

    private void ThrowDice() {
        diceNumber = randomGenerator.Next(1, 7);
    }


    private void SetNextPlayer() {
        if(diceNumber != 6){
            activePlayer = players[(++activePlayerIndex) % players.Length];
        }
    }

    private bool HasWinner() {
        bool win = true;
        for(int i = 0; i < activePlayer.pieces.Length; i++) {
            if(!activePlayer.pieces[i].inGoal) {
                win = false;
                break;
            }
        }

        if(win) {
            winnedPlayer = activePlayer;
            UnityEngine.Debug.Log(winnedPlayer.index + " win");
        }
        return win;
    }

    private bool CanMove(Player player) {
        for(int i = 0; i < player.pieces.Length; i++) {
            KeyValuePair<Piece, BlockType> hitted = player.pieces[i].GetBlock(diceNumber);
            if(hitted.Value != BlockType.OUTGOAL && hitted.Value != BlockType.ALLY) {
                return true;
            }
        }
        return false;
    }

}