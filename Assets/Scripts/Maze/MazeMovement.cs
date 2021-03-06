﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeProject {
  public class MazeMovement : MonoBehaviour {

    public float speed = 20f;
    public Transform player;

    private Maze maze;
    private bool move = false;
    private Vector3 endMarker;
    private Vector3 startMarker;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;



    // Start is called before the first frame update
    void Start() {
      maze = GetComponent<Maze>();
    }

    // Update is called once per frame
    void Update() {
      if (move) {
        // Distance moved = time * speed.
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed = current distance divided by total distance.
        float fracJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        player.position = Vector3.Lerp(startMarker, endMarker, fracJourney);
        if(fracJourney >= 1.0) {
          move = false;
          maze.MoveCommandComplete();
        }
      }
    }

    public void ActivatePlayer(int row, int col) {
      float size = GetComponent<Maze>().GetCellSize();
      player.gameObject.SetActive(true);
      player.position = new Vector3(size * col, size * row);
    }

    public void ExecuteMoveCommand(MoveCommand cmd) {
      move = true;
      float size = maze.GetCellSize();
      Vector3 velocity = Vector3.zero;
      switch (cmd) {
        case MoveCommand.MOVE_UP:
          velocity = new Vector3(0, 1, 0);
          break;
        case MoveCommand.MOVE_DOWN:
          velocity = new Vector3(0, -1, 0);
          break;
        case MoveCommand.MOVE_LEFT:
          velocity = new Vector3(-1, 0, 0);
          break;
        case MoveCommand.MOVE_RIGHT:
          velocity = new Vector3(1, 0, 0);
          break;
      }
      startMarker = player.position;
      endMarker = startMarker + velocity * size;
      journeyLength = Vector3.Distance(startMarker, endMarker);
      startTime = Time.time;
    }
  }
}