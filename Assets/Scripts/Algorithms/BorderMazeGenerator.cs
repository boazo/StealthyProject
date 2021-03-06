﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MazeProject;
namespace BorderMazeGenerator {
  public class BorderMazeGenerator : MonoBehaviour {
    public int NumRows;
    public int NumCols;
    public GameObject theMaze;
    public bool generate = true;

    void GenerateBorder() {
      var maze = theMaze.GetComponent<IMaze>();
      maze.InitMaze(NumRows, NumCols);

      for (int i = 0; i < NumRows; i++) {
        for (int j = 0; j < NumCols; j++) {
          if (j > 0 && j < (NumCols - 1) &&
              i > 0 && i < (NumRows - 1)) {
            maze.SetCellType(i, j, CellType.CELL_IS_EMPTY);
          } else {
            maze.SetCellType(i, j, CellType.CELL_IS_WALL);
          }
        }
      }
      maze.SetStartEndPoints(1, 1, NumRows - 2, NumCols - 2);
      maze.SaveMaze("border_maze_generator.txt");

    }

    void LoadBorderFromFile() {
      var maze = theMaze.GetComponent<IMaze>();
      maze.InitMazeFromFile("border_maze_generator.txt");

    }

    void TestCommandExecution() {
      List<MoveCommand> commands = new List<MoveCommand>();
      for (int i = 0; i < 17; i++) {
        commands.Add(MoveCommand.MOVE_UP);
      }
      for (int i = 0; i < 17; i++) {
        commands.Add(MoveCommand.MOVE_RIGHT);
      }
      var maze = theMaze.GetComponent<IMaze>();
      maze.SolveMaze(commands);
    }
    void Start() {
      if (generate) {
        GenerateBorder();
      } else {
        LoadBorderFromFile();
        TestCommandExecution();
      }
    }
  }
}