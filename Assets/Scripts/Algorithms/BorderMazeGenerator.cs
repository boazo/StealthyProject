﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MazeProject;
namespace BorderMazeGenerator {
  public class SimpleMazeGenerator : MonoBehaviour {
    public int NumRows;
    public int NumCols;
    public GameObject theMaze;

    void Start() {
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
    }
  }
}