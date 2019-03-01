﻿using System.Collections;
using System.Collections.Generic;

namespace MazeProject {
  /*
   * CellType enum is used to indicate the content of the cell
   */
  public enum CellType {
    CELL_IS_EMPTY,
    CELL_IS_WALL,
    CELL_TRAVERSED  // used for marking cell for path-finding algorithms
  }

  /*
   * CellType enum is used to indicate whether the content of the cell
   */
  public interface IMaze {
    // initializes the maze, with the number of rows and columuns
    void InitMaze(int numRows, int numCols);
    
    // sets the content of the cell[row][col]
    void SetCellType(int row, int col, CellType type);

    // returns the content of the cell[row][col]
    CellType GetCellType(int row, int col);
    
    // returns the number of rows the maze was initialized to
    int GetNumRows();
    
    // returns the number of columns the maze was initialized to
    int GetNumColumns();
    
    // set the weight of a cell (used for path-finding algorithms)
    void SetWeight(int row, int col, int weight);

    // retrieve weight of specified cell
    int GetWeight(int row, int col);
  }
}
