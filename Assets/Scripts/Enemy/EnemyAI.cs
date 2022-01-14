using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    [field: SerializeField] public static bool PlayerInAreaToDamage { get; internal set; }
    public static bool PlayerInAreaToChase { get; private set; }
    public Transform Player { get; private set; }

    [SerializeField] private GameObject _player;
    [SerializeField] private float _speed;
    [SerializeField] private Collider2D _bounds;
    [SerializeField] private HealthBarComponent _healthBarComponentOfPlayer;
    private Vector3 _directionVector;
    private Transform _npcTransform;
    private Rigidbody2D _npcRigidbody2D;
    private Animator _animator;
    private bool _isMoving;


    private float _moveTimeInSeconds;
    private float _waitTimeInSeconds;

    [SerializeField] private float minMoveTime;
    [SerializeField] private float maxMoveTime;

    [SerializeField] private float minWaitTime;
    [SerializeField] private float maxWaitTime;

    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    private static readonly int IsAngry = Animator.StringToHash("IsAngry");


    [SerializeField] Seeker _seeker;
    [SerializeField] AIPath _aiPath;
    [SerializeField] AIDestinationSetter _aiDestSetter;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _minDistanceToDamage;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _npcRigidbody2D = GetComponent<Rigidbody2D>();
        _npcTransform = GetComponent<Transform>();
        _seeker.enabled = false;
        _aiPath.enabled = false;
        _aiDestSetter.enabled = false;
        _waitTimeInSeconds = Random.Range(minWaitTime, maxWaitTime);
        _moveTimeInSeconds = Random.Range(minMoveTime, maxMoveTime);

        ChangeDirection();
    }

    private void Update()
    {
        if ((_player.transform.position - gameObject.transform.position).magnitude <= _minDistance)
        {
            PlayerInAreaToChase = true;
            if (_aiPath.desiredVelocity.x != 0 || _aiPath.desiredVelocity.y != 0)
            {
                UpdateAnimation();
            }

            if ((_player.transform.position - gameObject.transform.position).magnitude <= _minDistanceToDamage)
            {
                PlayerInAreaToDamage = true;
            }
        }
        else
        {
            PlayerInAreaToDamage = false;
            PlayerInAreaToChase = false;
        }
    }

    private void FixedUpdate()
    {
        if (_isMoving && !PlayerInAreaToChase)
        {
            _moveTimeInSeconds -= Time.deltaTime;
            if (_moveTimeInSeconds <= 0)
            {
                _moveTimeInSeconds = Random.Range(minMoveTime, maxMoveTime);
                _isMoving = false;
            }

            Move();
        }
        else if (!_isMoving && !PlayerInAreaToChase)
        {
            _waitTimeInSeconds -= Time.deltaTime;
            if (_waitTimeInSeconds <= 0)
            {
                ChangeDirection();
                _isMoving = true;
                _waitTimeInSeconds = Random.Range(minWaitTime, maxWaitTime);
            }
        }
        else if (PlayerInAreaToDamage)
        {
            _healthBarComponentOfPlayer.Damage(0.25f);
        }
        else if (PlayerInAreaToChase)
        {
            _seeker.enabled = true;
            _aiPath.enabled = true;
            _aiDestSetter.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerInAreaToDamage = true;
        }
        else
        {
            ChangeDirection();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerInAreaToDamage = false;
        }
    }

    private void Move()
    {
        var template = _npcTransform.position + _directionVector * _speed * Time.fixedDeltaTime;
        if (_bounds.bounds.Contains(template))
        {
            _npcRigidbody2D.MovePosition(template);
        }
        else
        {
            ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        var direction = Random.Range(0, 4);

        switch (direction)
        {
            case 0:
                _directionVector = Vector3.right;
                break;
            case 1:
                _directionVector = Vector3.left;
                break;
            case 2:
                _directionVector = Vector3.up;
                break;
            case 3:
                _directionVector = Vector3.down;
                break;
        }

        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        if (PlayerInAreaToChase)
        {
            _animator.SetFloat(MoveX, _aiPath.desiredVelocity.x);
            _animator.SetFloat(MoveY, _aiPath.desiredVelocity.y);
            _animator.SetBool(IsAngry, true);
        }
        else
        {
            _animator.SetFloat(MoveX, _directionVector.x);
            _animator.SetFloat(MoveY, _directionVector.y);
            _animator.SetBool(IsAngry, false);
        }
    }
}