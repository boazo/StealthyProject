using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace MazeProject {

  public class Maze : MonoBehaviour, IMaze {
    [Tooltip("A wall in the maze")]
    public GameObject WallPrefab;

    public GameObject StartMarker;
    public GameObject EndMarker;

    private int StartRow;
    private int StartCol;
    private int EndRow;
    private int EndCol;

    private int NumRows;
    private int NumCols;

    private CellType[,] Grid;

    private float CellSize;

    private List<MoveCommand> commands;
    private int curCmdIndex = 0;
    private bool solveMaze = false;
    private bool executeNextCmd = false;

    class ColRow {
      public ColRow(int r, int c) {
        row = r;
        col = c;
      }
      public int row;
      public int col;
    }

    class ColRowEqualityComparer : IEqualityComparer<ColRow> {
      public bool Equals(ColRow cr1, ColRow cr2) {
        if (cr2 == null && cr1 == null)
          return true;
        else if (cr1 == null || cr2 == null)
          return false;
        else if (cr1.row == cr2.row && cr1.col == cr2.col)
          return true;
        else
          return false;
      }

      public int GetHashCode(ColRow cr) {
        int hCode = cr.row ^ cr.col;
        return hCode.GetHashCode();
      }
    }

    Dictionary<ColRow, GameObject> ColRowToGameObjects = new Dictionary<ColRow, GameObject>(new ColRowEqualityComparer());
    Dictionary<ColRow, Dictionary<string, object>> ColRowToMetadata = new Dictionary<ColRow, Dictionary<string, object>>(new ColRowEqualityComparer());

    public void InitMazeFromFile(string filename) {
      string dataPath = Path.Combine(Application.persistentDataPath, filename);
      StreamReader reader = new StreamReader(dataPath);

      List<string> gridAsString = new List<string>();
      int cols = 0;
      int rows = 0;
      StartRow = int.Parse(reader.ReadLine());
      StartCol = int.Parse(reader.ReadLine());
      EndRow = int.Parse(reader.ReadLine());
      EndCol = int.Parse(reader.ReadLine());

      while (!reader.EndOfStream) {
        gridAsString.Add(reader.ReadLine());
      }
      cols = gridAsString[0].Length;
      rows = gridAsString.Count;
      InitMaze(rows, cols);

      for (int i = 0; i < NumRows; i++) {
        for (int j = 0; j < NumCols; j++) {
          int cell = gridAsString[i][j] - '0';
          if (cell == 0) {
            SetCellType(i, j, CellType.CELL_IS_EMPTY);
          } else {
            SetCellType(i, j, CellType.CELL_IS_WALL);
          }
        }
      }
      SetStartEndPoints(StartRow, StartCol, EndRow, EndCol);
    }

    public void InitMaze(int numRows, int numCols) {
      NumRows = numRows;
      NumCols = numCols;

      Grid = new CellType[NumRows, NumCols];
      for (int i = 0; i < NumRows; i++) {
        GameObject row = new GameObject("row_" + i);
        row.transform.SetParent(gameObject.transform);

        for (int j = 0; j < NumCols; j++) {
          GameObject newWall = Instantiate(WallPrefab);
          newWall.transform.SetParent(row.transform);
          newWall.name = i.ToString() + "_" + j.ToString();
          Wall wall = newWall.GetComponent<Wall>();
          newWall.transform.position = new Vector3(wall.GetCellSize() * j, wall.GetCellSize() * i, 0);
          CellSize = wall.GetCellSize();
          ColRowToGameObjects.Add(new ColRow(i, j), newWall);
          SetCellType(i, j, CellType.CELL_IS_EMPTY);
        }
      }

      Camera.main.GetComponent<FitCameraToGrid>().FitCamera(NumRows, NumCols, GetCellSize());
    }

    public void SetCellType(int row, int col, CellType type) {
      Grid[row, col] = type;
      switch (type) {
        case CellType.CELL_IS_EMPTY:
          Grid[row, col] = CellType.CELL_IS_EMPTY;
          ColRowToGameObjects[new ColRow(row, col)].GetComponent<Wall>().HideWall();
          break;
        case CellType.CELL_IS_WALL:
          Grid[row, col] = CellType.CELL_IS_WALL;
          ColRowToGameObjects[new ColRow(row, col)].GetComponent<Wall>().ShowWall();
          break;
        default:
          break;
      }
    }

    public CellType GetCellType(int row, int col) {
      return Grid[row, col];
    }

    // Associate a metadata attribute with a given maze cell
    public void SetMetadata(int row, int col, string attribute, object data) {
      ColRow key = new ColRow(row, col);
      Dictionary<string, object> metadata = null; 
      if (!ColRowToMetadata.ContainsKey(key)) {
        metadata = new Dictionary<string, object>();
        ColRowToMetadata[key] = metadata;
      } 
      metadata = ColRowToMetadata[key];      

      if (metadata.ContainsKey(attribute)) {
        metadata[attribute] = data;
      } else {
        metadata.Add(attribute, data);
      }
    }

    // Retrieve the metadata attribute associated with a given cell,
    // or null if no such attribute was previously associated.
    public object GetMetadata(int row, int col, string attribute) {
      object ret = null;
      ColRow key = new ColRow(row, col);
      if (ColRowToMetadata.ContainsKey(key)) {
        var metadata = ColRowToMetadata[key];
        if (metadata.ContainsKey(attribute)) {
          ret = metadata[attribute];
        }
      }
      return ret;
    }

    public float GetCellSize() {
      return CellSize;
    }

    // returns the number of rows the maze was initialized to
    public int GetNumRows() {
      return NumRows;
    }

    // returns the number of columns the maze was initialized to
    public int GetNumColumns() {
      return NumCols;
    }

    // saves the maze to the disk
    public void SaveMaze(string name) {
      string dataPath = Path.Combine(Application.persistentDataPath, name);
      StreamWriter writer = new StreamWriter(dataPath);
      writer.WriteLine(StartRow);
      writer.WriteLine(StartCol);
      writer.WriteLine(EndRow);
      writer.WriteLine(EndCol);

      for (int i = 0; i < NumRows; i++) {
        for (int j = 0; j < NumCols; j++) {
          string value = "1";
          if (Grid[i, j] == CellType.CELL_IS_EMPTY) {
            value = "0";
          }
          writer.Write(value);
        }
        writer.Write(writer.NewLine);
      }
      writer.Close();
    }

    // sets the start and end points
    public void SetStartEndPoints(int srow, int scol, int erow, int ecol) {
      StartRow = srow;
      StartCol = scol;
      EndRow = erow;
      EndCol = ecol;
      float size = GetCellSize();
      StartMarker.SetActive(true);
      EndMarker.SetActive(true);
      StartMarker.transform.position = new Vector3(scol * size, srow * size, 0f);
      EndMarker.transform.position = new Vector3(ecol * size, erow * size, 0f);
    }

    // returns the coordinates of the start and end points
    public void GetStartEndPoints(ref int srow, ref int scol, ref int erow, ref int ecol) {
      srow = StartRow;
      scol = StartCol;
      erow = EndRow;
      ecol = EndCol;
    }

    private bool CheckSolution() {
      int curRow = StartRow;
      int curCol = StartCol;
      for (int i = 0; i < commands.Count; i++) {
        switch (commands[i]) {
          case MoveCommand.MOVE_UP:
            curRow = Mathf.Min(NumRows - 1, curRow + 1);
            break;
          case MoveCommand.MOVE_DOWN:
            curRow = Mathf.Max(0, curRow - 1); 
            break;
          case MoveCommand.MOVE_LEFT:
            curCol = Mathf.Max(0, curCol - 1);
            break;
          case MoveCommand.MOVE_RIGHT:
            curCol = Mathf.Min(NumCols - 1, curCol + 1);
            break;
        }
        if (Grid[curRow, curCol] == CellType.CELL_IS_WALL) {
          Debug.Log("The solution goes inside walls!");
          return false;
        }
      }
      if (curRow == EndRow && curCol == EndCol) {
        return true;
      }
      return false;
    }

    public bool SolveMaze(List<MoveCommand> commands) {
      this.commands = commands;
      if (CheckSolution()) {
        GetComponent<MazeMovement>().ActivatePlayer(StartRow, StartCol);
        curCmdIndex = 0;
        solveMaze = true;
        executeNextCmd = true;
        return true;
      } else {
        Debug.Log("Solution is no good!");
        return false;
      }
    }

    public void MoveCommandComplete() {
      executeNextCmd = true;
    }

    void Update() {
      if (solveMaze) {
        if (executeNextCmd) {
          if (commands.Count > curCmdIndex) {
            GetComponent<MazeMovement>().ExecuteMoveCommand(commands[curCmdIndex]);
            curCmdIndex++;
            executeNextCmd = false;
          } else {
            Debug.Log("Destination Reached!!!");
            solveMaze = false;
          }
        }
      }
    }
  }
}
