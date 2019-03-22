using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class Game {

    Thread thread;
    ManualResetEvent mre;
    public bool paused;
    public bool turnBased;

    // TODO: use a bool instead of Enum
    public bool shouldDice;
    public Board board;
    public Player winnedPlayer;
    public Player[] players;
    public int currentPlayerIndex;

    public int diceNumber;
    private Random randomGenerator;

    #region events 

    // TODO: safly invoking events...(a good choice is to update C# to v6 and use x?.invoke() notaion)
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

    public Game(Board board, PlayerType[] playerTypes, bool turnBased) {
        this.board = board;
        this.turnBased = turnBased;

        // instantiate players
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

        // default values for game
        shouldDice = true;
        currentPlayerIndex = -1;
        randomGenerator = new Random();

    }

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

    public void MovePiece(Piece piece) {
        if(shouldDice) {
            return;
        }
        if(piece == null) {
            NextTurn();
            return;
        }

        if(players[currentPlayerIndex] == piece.player) {
            KeyValuePair<Piece, BlockType> tuple = piece.GetBlock(diceNumber);
            Piece possibleHittedPiece = tuple.Key;
            BlockType blockType = tuple.Value;
            UnityEngine.Debug.Log(blockType.ToString()); 
            
            if(blockType != BlockType.OUTGOAL && blockType != BlockType.ALLY) {
                if(blockType == BlockType.ENEMY) {
                    possibleHittedPiece.GetOut();
                    GetOutPieceEvent(new GetOutPieceEventArgs(piece, possibleHittedPiece));
                }

                if(piece.isIn) {
                    piece.GoForward(diceNumber, blockType == BlockType.INGOAL);
                    MovePieceEvent(new MovePieceEventArgs(piece, diceNumber));
                } else if(diceNumber == 6){
                    piece.GetIn();
                    GetInPieceEvent(new GetInPieceEventArgs(piece));
                }

                if(winnedPlayer != null)
                    UnityEngine.Debug.Log(winnedPlayer.index + " win");
                else
                    NextTurn();
            }
        }
    }
    public void RollDice() {
        if(!shouldDice) {
            return;
        }

        diceNumber = randomGenerator.Next(1, 7);
        shouldDice = false;

        Player player = players[currentPlayerIndex];
        bool canMove = player.CanMove(diceNumber);

        RolledDiceEvent(new RollDiceEventArgs(diceNumber, player, canMove));

        if(canMove) {
            player.DoMove(diceNumber);
        } else {
            NextTurn();
        }
    }

    // if next == false => same player rolls the dice again
    public void NextTurn() {
        mre.WaitOne();
        if(turnBased)
            Pause();

        UnityEngine.Debug.Log("----------------------------------------------------------------");
        if(diceNumber != 6){
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
            SetNextTurnEvent(new SetNextTurnEventArgs(players[currentPlayerIndex]));
        }
        
        shouldDice = true;
        players[currentPlayerIndex].DoDice();
    }

}