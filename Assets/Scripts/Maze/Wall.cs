using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeProject {
  public class Wall : MonoBehaviour {
    private Vector3 CellSize;

    // Start is called before the first frame update
    void Awake() {
      CellSize = GetComponent<Collider>().bounds.size;
    }


    public void ShowWall() {
      GetComponent<Renderer>().enabled = true;
    }

    public void HideWall() {
      GetComponent<Renderer>().enabled = false;
    }

    public float GetCellSize() {
      return CellSize.x;
    }
  }
}