using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    private SceneManager _sceneManager;
    private GameMenuManager _gameMenuManager;
    private PlayerController _player;
    private bool _tutorialPlayed;

    private enum States {Checkpoint, EndGame, KillPlane, Tutorial}

    [SerializeField] private States state;

    void Start()
    {
        _sceneManager = FindObjectOfType<SceneManager>();
        _gameMenuManager = FindObjectOfType<GameMenuManager>();
        _player = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (state)
            {
                case States.Checkpoint:
                    if (_sceneManager.curCheckpoint != transform)
                    {
                        _sceneManager.curCheckpoint = transform;
                        _sceneManager.checkpointNum++;
                    }
                    break;
                case States.EndGame:
                    _gameMenuManager.EndGame();
                    break;
                case States.KillPlane:
                    _player.TakeDamage(100f);
                    break;
                case States.Tutorial:
                    if (!_tutorialPlayed)
                    {
                        _gameMenuManager.Tutorial();
                        _tutorialPlayed = true;
                    }
                    break;
            }
        }
    }
}
