namespace MazeProject
{
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
    
    // sets the content of the cell[row][col]
    void SetCellType(int row, int col, CellType type);

    // returns the content of the cell[row][col]
    CellType GetCellType(int row, int col);

    // returns the number of rows the maze was initialized to
    int GetNumRows();

    // returns the number of columns the maze was initialized to
    int GetNumColumns();    

    // Associate a metadata attribute with a given maze cell
    void SetMetadata(int row, int col, string attribute, object data);

    // Retrieve the metadata attribute associated with a given cell,
    // or null if no such attribute was previously associated.
    object GetMetadata(int row, int col, string attribute);
  }
}
