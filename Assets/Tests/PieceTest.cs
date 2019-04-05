using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

namespace Tests
{
    public class PieceTest {
        [Test]
        public void PieceGoTest1() {
            // mocking the game
            Board board = new Board(4, 40, 4);
            PlayerType[] playerTypes = new PlayerType[] {PlayerType.NOTHING, PlayerType.AI, PlayerType.NOTHING, PlayerType.NOTHING};
            Game game = new Game(board, playerTypes);
            Player player = game.players[0];
            Piece piece = player.pieces[0];
            piece.GetIn();

            // test case:
            KeyValuePair<int[], int> stepsData = piece.Go(BlockType.NOTHING, 4);
            int[] steps = stepsData.Key;
            int inGoalIndex = stepsData.Value;
            int sp = player.startPosition;

            int[] expectedSteps = new int[] {sp + 1, sp + 2, sp + 3, sp + 4};

            Assert.AreEqual(expectedSteps, steps);
            Assert.AreEqual(-1, inGoalIndex);
        }

        [Test]
        public void PieceGoTest2() {
            // mocking the game
            Board board = new Board(4, 40, 4);
            PlayerType[] playerTypes = new PlayerType[] {PlayerType.NOTHING, PlayerType.AI, PlayerType.NOTHING, PlayerType.NOTHING};
            Game game = new Game(board, playerTypes);
            Player player = game.players[0];
            Piece piece = player.pieces[0];
            piece.GetIn();

            // test case:
            piece.position = (player.startPosition + board.roadSize - 2) % board.roadSize;
            piece.pacesGone = board.roadSize - 1;
            KeyValuePair<int[], int> stepsData = piece.Go(BlockType.INGOAL, 3);
            int[] steps = stepsData.Key;
            int inGoalIndex = stepsData.Value;
            int sp = player.startPosition;

            int[] expectedSteps = new int[] {10, 0, 1};
            Assert.AreEqual(10, player.startPosition);
            Assert.AreEqual(8, piece.position);
            Assert.AreEqual(expectedSteps, steps);
            Assert.AreEqual(0, inGoalIndex);
        }


    }
}
