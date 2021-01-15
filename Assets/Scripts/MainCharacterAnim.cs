﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterAnim : MonoBehaviour
{
    private PlayerState _playerState;

    private Animator _animator;
    [SerializeField]
    private bool _attacking;
    [SerializeField]
    private bool _damaged;
    [SerializeField]
    private bool _died;
    
    private void Awake()
    {
        _playerState = GetComponent<PlayerState>();
        _animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("isWalking", _playerState.isWalking);
        _animator.SetBool("isIdling", _playerState.isIdling);        
        _animator.SetBool("isJumping", _playerState.isJumping);
        
        if (_playerState.isAttacking && !_attacking)
            Attack();

        if (_playerState.isDamaged && !_damaged)
            Damaged();

        if (_playerState.isDied && !_died)
        {
            Died();
        }
    }

    public void Died()
    {
        _died = true;
        _animator.SetTrigger("Died");
    }

    public void DiedFinish()
    {
        //TODO 죽었을때 처리
    }


    public void Damaged()
    {
        _damaged = true;
        _attacking = false;
        _playerState.isAttacking = false;
        _playerState.isIdling = true;
        _playerState.isJumping = false;
        _animator.SetTrigger("Damaged");
    }

    public void DamagedStart()
    {
        float direction = transform.localScale.x;
        Debug.Log("Start");
        transform.Translate(new Vector2((int)direction * - 1, 0));
    }

    public void DamagedFinish()
    {
        _damaged = false;
        _playerState.isDamaged = false;
    }

    public void Attack()
    {
        _attacking = true;
        _animator.SetTrigger("Attack");
    }

    public void AttackFinish()
    {
        _attacking = false;
        _playerState.isAttacking = false;
    }
}
