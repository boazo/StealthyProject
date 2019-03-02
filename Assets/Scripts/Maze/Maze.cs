using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace MazeProject {

  public class Maze : MonoBehaviour, IMaze {
    [Tooltip("A wall in the maze")]
    public GameObject WallPrefab;

    private int NumRows;
    private int NumCols;

    private CellType[,] Grid;
    
    private float CellSize;

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
    Dictionary<ColRow, Dictionary<string, object>> ColRowToMetadata = new Dictionary<ColRow, Dictionary<string, object>>();

    public void InitMazeFromFile(string filename) {
      string dataPath = Path.Combine(Application.persistentDataPath, filename);
      StreamReader reader = new StreamReader(dataPath);

      List<string> gridAsString = new List<string>();
      int cols = 0;
      int rows = 0;
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
          newWall.transform.position = new Vector3(wall.GetCellSize() * i, wall.GetCellSize() * j, 0);
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
      var metadata = ColRowToMetadata[new ColRow(row, col)];
      if (metadata == null) {
        metadata = new Dictionary<string, object>();
        ColRowToMetadata[new ColRow(row, col)] = metadata;
      }
      if(metadata.ContainsKey(attribute)) {
        metadata[attribute] = data;
      }
    }

    // Retrieve the metadata attribute associated with a given cell,
    // or null if no such attribute was previously associated.
    public object GetMetadata(int row, int col, string attribute) {
      object ret = null;
      var metadata = ColRowToMetadata[new ColRow(row, col)];
      if(metadata != null) {
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
  }
}
