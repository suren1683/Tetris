using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] tetrimino = new GameObject[7];
    public GameObject[] tetrimino1 = new GameObject[7];
    public Transform spawnpoint;
    public Transform NextBlock;

    public GameObject nextBlock;

    public GameObject canvas;
    public TMP_Text scoreText;

    public TMP_Text curScoreText;

    public int currentIndex;
    public int nextIndex;

    public static Transform[,] grid = new Transform[10,24];
    public static int score = 0;

    private void Start() {
        nextIndex = Random.Range(0, 7);
        currentIndex = nextIndex;
        CreateNextBlock();
        canvas.SetActive(false);
    }

    public void CreateNextBlock() {
        Destroy(nextBlock);

        Instantiate(tetrimino[currentIndex], spawnpoint.position, Quaternion.identity);

        nextIndex = Random.Range(0, 7);
        nextBlock = Instantiate(tetrimino1[nextIndex], NextBlock.position + new Vector3(0, 0, -1), Quaternion.identity);

        currentIndex = nextIndex;
    }

    public void GameOver() {
        Time.timeScale = 0;
        canvas.SetActive(true);
        scoreText.text = score.ToString();
    }

    public void Restart() {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
        score = 0;
    }

    public void Quit() {
        Application.Quit();
    }

    public void RenewScore() {
        curScoreText.text = score.ToString();
    }
}
