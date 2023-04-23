using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private enum States
    {
        Idle,
        Chase,
        Attack
    }

    private States _curState;
    private PlayerController _player;
    private NavMeshAgent _nav;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float chasingRadius;
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackRecoveryTime;
    private float _attackTimer;
    [SerializeField] private float attackDamage;
    private Animator _animator;
    private bool _isRunning;
        

    void Start()
    {
        _curState = States.Idle;
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _nav = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        switch (_curState)
        {
            case States.Idle:
            {
                _isRunning = false;
                Idle();
                break;
            }
            case States.Chase:
            {
                _isRunning = true;
                Chase();
                break;
            }
            case States.Attack:
            {
                Attack();
                break;
            }
        }
        _animator.SetBool("isRunning", _isRunning);
        Debug.Log(_animator.GetBool("isRunning"));
    }

    private void Idle()
    {
        if (Physics.CheckSphere(transform.position, chasingRadius, playerMask))
        {
            _curState = States.Chase;
        }
    }

    private void Chase()
    {
        if (Physics.CheckSphere(transform.position, attackRadius, playerMask))
        {
            _curState = States.Attack;
        }
        else if (!Physics.CheckSphere(transform.position, chasingRadius, playerMask))
        {
            _curState = States.Idle;
        }
        else
        {
            _nav.SetDestination(_player.transform.position);
        }

    }

    private void Attack()
    {
        if (!Physics.CheckSphere(transform.position, chasingRadius, playerMask))
        {
            _curState = States.Idle;
        }
        else if (!Physics.CheckSphere(transform.position, attackRadius, playerMask))
        {
            _curState = States.Chase;
        }
        else
        {
            if (_attackTimer > 0)
            {
                _attackTimer -= Time.deltaTime;
            }
            else
            {
                _animator.Play("Attack");
                _player.TakeDamage(attackDamage);
                _attackTimer = attackRecoveryTime;
            }
        }
    }
    

}
