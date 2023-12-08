using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetrimino : MonoBehaviour {
    public float fallTime = 1f;
    private float previousTime = 0f;

    private int currentIndex = 0;
    private int previousIndex;

    private int width = 10;
    private int height = 24;
    private int deadLine = 20;

    private GameManager game;

    void Start() {
        previousIndex = currentIndex;
        game = FindObjectOfType<GameManager>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            transform.position += Vector3.right;

            if (!canMove()) {
                transform.position += Vector3.left;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            transform.position += Vector3.left;

            if (!canMove()) {
                transform.position += Vector3.right;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - previousTime > fallTime) {
            transform.position += Vector3.down;

            if (!canMove()) {
                transform.position += Vector3.up;

                AddtoGrid();

                CheckLine();

                if (!IsGameOver()) {
                    game.CreateNextBlock();
                }
                else {
                    game.GameOver();
                }

                this.enabled = false;
            }

            previousTime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.Z)) {
            currentIndex++;

            if (currentIndex >= transform.childCount) {
                currentIndex = 0;
            }

            if (canMove()) {
                transform.GetChild(currentIndex).gameObject.SetActive(true);
                transform.GetChild(previousIndex).gameObject.SetActive(false);
            }
            else {
                currentIndex--;
            }

            previousIndex = currentIndex;
        }
        else if (Input.GetKeyDown(KeyCode.X)) {
            currentIndex--;

            if (currentIndex < 0) {
                currentIndex = transform.childCount;
            }

            if (canMove()) {
                transform.GetChild(currentIndex).gameObject.SetActive(true);
                transform.GetChild(previousIndex).gameObject.SetActive(false);
            }
            else {
                currentIndex++;
            }

            previousIndex = currentIndex;
        }
        else if (Input.GetKeyDown(KeyCode.Space)) {
            while (canMove()) {
                transform.position += Vector3.down;
            }
            transform.position += Vector3.up;

            AddtoGrid();

            CheckLine();

            if (!IsGameOver()) {
                game.CreateNextBlock();
            }
            else {
                game.GameOver();
            }
            this.enabled = false;
        }
    }

    bool canMove() {
        foreach (Transform children in transform.GetChild(currentIndex)) {
            int FlooredX = Mathf.FloorToInt(children.transform.position.x);
            int FlooredY = Mathf.FloorToInt(children.transform.position.y);

            if (FlooredX < 0 || FlooredX >= width || FlooredY < 0) {
                return false;
            }

            if (GameManager.grid[FlooredX, FlooredY] != null) {
                return false;
            }
        }

        return true;
    }

    void AddtoGrid() {
        foreach (Transform children in transform.GetChild(currentIndex)) {
            int FlooredX = Mathf.FloorToInt(children.transform.position.x);
            int FlooredY = Mathf.FloorToInt(children.transform.position.y);

            GameManager.grid[FlooredX, FlooredY] = children;
        }
    }

    void CheckLine() {
        foreach (Transform children in transform.GetChild(currentIndex)) {
            int FlooredY = Mathf.FloorToInt(children.transform.position.y);

            while (HasLines(FlooredY)) {
                GameManager.score += 100;
                DeleteLine(FlooredY);
                DownLines(FlooredY);
            }
        }
    }

    bool HasLines(int y) {
        for (int i = 0; i < width; i++) {
            if (GameManager.grid[i, y] == null) {
                return false;
            }
        }
        return true;
    }

    void DeleteLine(int y) {
        for (int i = 0; i < width; i++) {
            Destroy(GameManager.grid[i, y].gameObject);
            GameManager.grid[i, y] = null;
        }
    }

    void DownLines(int y) {
        for (int i = y + 1; i < height; i++) {
            for (int j = 0; j < width; j++) {
                if (GameManager.grid[j, i] != null) {
                    GameManager.grid[j, i - 1] = GameManager.grid[j, i];
                    GameManager.grid[j, i] = null;
                    GameManager.grid[j, i - 1].transform.position += Vector3.down;
                }
            }
        }
    }

    bool IsGameOver() {
        foreach (Transform children in transform.GetChild(currentIndex)) {
            int FlooredY = Mathf.FloorToInt(children.transform.position.y);

            if (FlooredY >= deadLine) {
                return true;
            }
        }

        return false;
    }
}
