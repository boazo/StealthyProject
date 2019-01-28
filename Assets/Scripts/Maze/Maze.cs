using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float GetCellSize() {
      return CellSize;
    }
  }
}