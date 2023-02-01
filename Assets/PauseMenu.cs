using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool PauseState = false;
    public GameObject PauseMenuUI;
    public GameObject SettingsMenuUI;
    public GameObject crosshair;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (PauseState) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void SwitchFixedBrickOrientation() {
        main.orientFixedBricks = !main.orientFixedBricks;
    }

    public void Resume() {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        PauseState = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        crosshair.SetActive(true);
    }

    public void LoadMenu() {
        PauseState = false;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    
    public void QuitGame() {
        Application.Quit();
    }

    public void Settings() {
        PauseMenuUI.SetActive(false);
        SettingsMenuUI.SetActive(true);
    }

    public void Back() {
        PauseMenuUI.SetActive(true);
        SettingsMenuUI.SetActive(false);
    }

    public void Pause() {
        PauseMenuUI.SetActive(true);
        crosshair.SetActive(false);
        Time.timeScale = 0f;
        PauseState = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
