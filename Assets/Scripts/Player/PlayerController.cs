using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private JoystickManager _joystickManager;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _speed;
    [SerializeField] private Rigidbody2D _rb;


    private float _directionX;
    private float _directionY;
    private Vector2 _playerDirection;

    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");

    private void Update()
    {
        _directionX = _joystickManager.InputHorizontal();
        _directionY = _joystickManager.InputVertical();
        _playerDirection = new Vector2(_directionX, _directionY).normalized;
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_playerDirection.x * _speed, _playerDirection.y * _speed);
    }

    void UpdateAnimation()
    {
        _animator.SetFloat(MoveX, _directionX);
        _animator.SetFloat(MoveY, _directionY);
    }

    IEnumerator IncreaseSpeed()
    {
        _speed *= 2;
        yield return new WaitForSeconds(2);
        _speed /= 2;
        yield return new WaitForSeconds(30);
    }
}