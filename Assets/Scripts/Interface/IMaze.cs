using System.Collections.Generic;

namespace MazeProject {
  /*
   * CellType enum is used to indicate the content of the cell
   */
  public enum CellType {
    CELL_IS_EMPTY,
    CELL_IS_WALL,
  }

  public enum MoveCommand {
    MOVE_UP,
    MOVE_DOWN,
    MOVE_LEFT,
    MOVE_RIGHT
  }

  /*
   * CellType enum is used to indicate whether the content of the cell
   */
  public interface IMaze {
    // initializes the maze, with the number of rows and columuns
    void InitMaze(int numRows, int numCols);

    // initializes the maze from a file (just the file name, without the path)
    void InitMazeFromFile(string filename);

    // saves the maze to the disk (just the file name, without the path)
    /*
    On Windows the file path is :

    C: \Users\< username >\AppData\LocalLow\< company name >\< app name >\ <filename>

    On OSX the file path is:

    Users /< username >/ Library / Application Support /< company name >/< app name > / <filename>
    */
    void SaveMaze(string filename);

    // sets the content of the cell[row][col]
    void SetCellType(int row, int col, CellType type);

    // sets the start and end points
    void SetStartEndPoints(int srow, int scol, int erow, int ecol);

    // returns the coordinates of the start and end points
    void GetStartEndPoints(ref int srow, ref int scol, ref int erow, ref int ecol);

    // returns the content of the cell[row][col]
    CellType GetCellType(int row, int col);

    // Associate a metadata attribute with a given maze cell
    void SetMetadata(int row, int col, string attribute, object data);

    // Retrieve the metadata attribute associated with a given cell,
    // or null if no such attribute was previously associated.
    object GetMetadata(int row, int col, string attribute);

    // returns the number of rows the maze was initialized to
    int GetNumRows();

    // returns the number of columns the maze was initialized to
    int GetNumColumns();

    // try to solve the maze with the list of commands
    bool SolveMaze(List<MoveCommand> commands);
  }
}
