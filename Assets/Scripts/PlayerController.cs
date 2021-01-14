﻿using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour, IAttackable
{
    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float speed = 4;

    [SerializeField, Tooltip("Acceleration while grounded.")]
    float walkAcceleration = 75;

    [SerializeField, Tooltip("Acceleration while in the air.")]
    float airAcceleration = 20;

    [SerializeField, Tooltip("Deceleration while in the air.")]
    float airDeceleration = 30;

    [SerializeField, Tooltip("Deceleration applied when character is grounded and not attempting to move.")]
    float groundDeceleration = 70;

    [SerializeField, Tooltip("Max height the character will jump regardless of gravity")]
    float jumpHeight = 3;

    private BoxCollider2D boxCollider;

    private Vector2 velocity;

    private bool grounded;

    private float acceleration;
    private float deceleration;

  
    public PlayerState playerState;
    private void Awake()
    {
        playerState = new PlayerState();
        boxCollider = GetComponent<BoxCollider2D>();

        acceleration = grounded ? walkAcceleration : airAcceleration;
        deceleration = grounded ? groundDeceleration : airDeceleration;
    }

    private void Update()
    {
        MoveHorizontal();
        CollisionCheck();
        Jump();
        Attack();
        Gravity();
    }

    private void MoveHorizontal()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput != 0)
        {
            transform.localScale = new Vector3(1 * moveInput, 1, 1);
            velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput, acceleration * Time.deltaTime);
            playerState.isWalking = true;
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
            playerState.isWalking = false;
        }
        transform.Translate(velocity * Time.deltaTime);
    }
    private void CollisionCheck()
    {
        grounded = false;

        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

        foreach (Collider2D hit in hits)
        {
            if (hit.Equals(boxCollider))
                continue;

            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0)
                {
                    grounded = true;
                    playerState.isJumping = false;
                }
            }

        }
    }

    private void Jump()
    {
        if (grounded)
        {
            velocity.y = 0;
            if (Input.GetButton("Jump"))
            {
                velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
                playerState.isJumping = true;
            }
        }
    }

    public void Gravity()
    {
        velocity.y += Physics2D.gravity.y * Time.deltaTime;
    }

    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            playerState.isAttacking = true;
        }
    }
}


public class PlayerState
{
    private bool _isWalking = false;
    private bool _isIdling = true;
    private bool _isJumping = false;
    private bool _isAttacking = false;


    public bool isWalking
    {
        get => _isWalking;
        set 
        {
            _isWalking = value;
            _isIdling = !value;
        }
    }
    public bool isIdling
    {
        get => _isIdling;
        set
        {
            _isWalking = !value;
            _isIdling = value;
        }
    }
    public bool isJumping
    {
        get => _isJumping;
        set => _isJumping = value;
    }
    public bool isAttacking
    {
        get => _isAttacking;
        set => _isAttacking = value;
    }

  
}
