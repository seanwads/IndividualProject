using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    private PlayerController _player;
    [SerializeField] private RawImage _hpBar;
    private Slider _hpSlider;
    [SerializeField] private GameObject _deathScreen;
    [SerializeField] private GameObject _pauseMenu;
    private SceneManager _sceneManager;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _hpSlider = _hpBar.GetComponent<Slider>();
        _deathScreen.SetActive(false);
        _sceneManager = FindObjectOfType<SceneManager>();
    }
    
    void Update()
    {
        _hpSlider.value = _player.GetHealthPercent();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pauseMenu.activeSelf)
            {
                _pauseMenu.SetActive(false);
            }
            else
            {
                _pauseMenu.SetActive(true);
            }
        }
    }

    public void PlayerDeath()
    {
        _deathScreen.SetActive(true);
    }

    public void RespawnButton()
    {
        _sceneManager.Respawn();
        _deathScreen.SetActive(false);
    }

    public void MainMenuButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}