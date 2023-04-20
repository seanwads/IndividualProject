using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _credits;

    public void StartButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void CreditsButton()
    {
        _mainMenu.SetActive(false);
        _credits.SetActive(true);
    }

    public void ReturnButton()
    {
        _credits.SetActive(false);
        _mainMenu.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
