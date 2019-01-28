using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeProject {
  public class FitCameraToGrid : MonoBehaviour {
    public void FitCamera(int NumCols, int NumRows, float CellSize) {
      gameObject.transform.position = new Vector3((float)NumCols / 2 - CellSize / 2, (float)NumRows / 2 - CellSize / 2, -1.0f);
      var camera = GetComponent<Camera>();
      camera.aspect = 1.0f;
      camera.orthographicSize = (float)NumCols / 2;
      Screen.SetResolution(600, 600, false);
    }
  }
}