using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    private PlayerController _player;
    [SerializeField] private RawImage _hpBar;
    private Slider _hpSlider;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _hpSlider = _hpBar.GetComponent<Slider>();

    }
    
    void Update()
    {
        _hpSlider.value = _player.GetHealthPercent();
    }
}