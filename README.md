# Mensch

Mensch is a digital take on the classic board game, made in [Unity](https://unity3d.com/).

## Description

Enjoy the nostalgic game of Mensch (a "Ludo"/"Sorry!"-style pachisi game) in a polished 3D environment with friends, or against the built-in AI.

- Local multiplayer and AI opponents
- Roll the dice, move pieces around the board, and get all your pieces home first
- Animated pieces, dice, and board interactions

## Tech

The game logic (`Scripts/Main`) is completely decoupled from Unity — `Board`, `Piece`, and `Game` are plain C# classes with no `MonoBehaviour` dependency, running on their own background thread and using `ManualResetEvent` to pause/resume between turns. They talk to the Unity-side view layer (`Scripts/View`) purely through C# events (`RolledDiceEvent`, `MovePieceEvent`, `WinEvent`, ...), so the rules engine is testable in isolation from rendering.

Players are pluggable via a `PlayerFactory`: a `HumanPlayer` waits on UI input, while `AIPlayer` scores every legal move with a heuristic (blocking, escaping, capturing, reaching goal, etc.) and picks the best one — no external libraries involved.

## Requirements

- Unity 2018.3.5f1 (or newer)

## Screenshots

<p align="center">
  <img width="200" src="https://raw.githubusercontent.com/navidadelpour/Mensch/master/Assets/Screenshots/com.navidadelpour.Mensch-536307540819.webp" />
  <img width="200" src="https://raw.githubusercontent.com/navidadelpour/Mensch/master/Assets/Screenshots/com.navidadelpour.Mensch-602662842826.webp" />
  <img width="200" src="https://raw.githubusercontent.com/navidadelpour/Mensch/master/Assets/Screenshots/com.navidadelpour.Mensch-710139940748.webp" />
  <img width="200" src="https://raw.githubusercontent.com/navidadelpour/Mensch/master/Assets/Screenshots/com.navidadelpour.Mensch-905449381616.webp" />
</p>

## Download

This game is presented on [Cafe Bazaar](https://cafebazaar.ir) for free!

[Download link](https://cafebazaar.ir/app/com.navidadelpour.Mensch)

## Credits

- Graphics: [photoshop](https://www.adobe.com/products/photoshop.html) and [illustrator](https://www.adobe.com/products/illustrator.html)
- Game Engine: [Unity](https://unity3d.com/)


Made with :heart: by [navidadelpour](https://navidadelpour.ir)
