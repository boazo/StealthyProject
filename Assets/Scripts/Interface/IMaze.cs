namespace MazeProject {
  /*
   * CellType enum is used to indicate the content of the cell
   */
  public enum CellType {
    CELL_IS_EMPTY,
    CELL_IS_WALL,
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
  }
}
