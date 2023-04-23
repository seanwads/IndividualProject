using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    private PlayerController _player;
    [SerializeField] private RawImage hpBar;
    private Slider _hpSlider;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject endGameScreen;
    private SceneManager _sceneManager;

    [SerializeField] private GameObject[] tutorialWindows;
    private int _curTutorial;
    private bool _tutorialActive;

        void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _hpSlider = hpBar.GetComponent<Slider>();
        deathScreen.SetActive(false);
        _sceneManager = FindObjectOfType<SceneManager>();
    }
    
    void Update()
    {
        _hpSlider.value = _player.GetHealthPercent();

        if (!pauseMenu.activeSelf && !deathScreen.activeSelf && !endGameScreen.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_tutorialActive)
            {
                tutorialWindows[_curTutorial].SetActive(false);
                _curTutorial++;
                _tutorialActive = false;
            }
            else
            {
                if (pauseMenu.activeSelf)
                {
                    pauseMenu.SetActive(false);
                }
                else
                {
                    pauseMenu.SetActive(true);
                }
            }
        }
        
    }

    public void PlayerDeath()
    {
        deathScreen.SetActive(true);
    }

    public void RespawnButton()
    {
        _sceneManager.Respawn();
        deathScreen.SetActive(false);
    }

    public void MainMenuButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void QuitButton()
    {
        Application.Quit();
    }

    public void EndGame()
    {
        endGameScreen.SetActive(true);
    }

    public void Tutorial()
    {
        tutorialWindows[_curTutorial].SetActive(true);
        _tutorialActive = true;
    }
    
}