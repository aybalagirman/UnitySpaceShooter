using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField]
    private bool _gameover;
    [SerializeField]
    private GameObject _pauseMenu;

    void Update() {
        if (Input.GetKeyDown(KeyCode.R) && _gameover) {
            SceneManager.LoadScene(1); //current game scene
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            _pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void HidePauseMenu() {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GameIsOver() {
        _gameover = true;
    }
}
