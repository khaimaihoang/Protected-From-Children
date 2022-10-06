using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    private PlayerManager _playerManager;
    private AnimatorManager _animatorManager;
    private Vector3 _forwardVec;
    private Rigidbody _rb;
    private float _verticalInput;
    private float _horizontalInput;
    private float _movementSpeed;

    public float offsetDistance;

    [Header("Speed")]
    public float walkingSpeed;

    [Header("Rotate Sharpness")]
    public float rotateSpeed;

    void Awake()
    {
        _playerManager = GetComponent<PlayerManager>();
        _animatorManager = GetComponent<AnimatorManager>();
        _forwardVec = transform.forward;
        _rb = GetComponent<Rigidbody>();
    }

    // public void HandleAllMovement(){
    //     HandleInput();
    //     HandleMovement();
    //     HandleRotation();
    // }

    private Vector3 HandleInput(){
        _verticalInput = InputManager.Instance.verticalInput;
        _horizontalInput = InputManager.Instance.horizontalInput;
        Vector3 _moveDirection = Vector3.forward * _verticalInput;
        _moveDirection = _moveDirection + Vector3.right * _horizontalInput;
        return _moveDirection;
    }

    public void HandleMovementFromInput(){
        Vector3 _moveDirection = HandleInput();
        HandleMovement(_moveDirection);
        HandleRotation(_moveDirection);
    }

    public void HandleMovementToPosition(Vector3 targetPosition)
    {
        Vector3 _moveDirection = targetPosition - transform.position;
        float dis = _moveDirection.magnitude;
        transform.DOMove(targetPosition, 0.2f);
        _moveDirection.Normalize();

        float moveAmount = Mathf.Clamp(dis, 0, offsetDistance) / offsetDistance;
        _animatorManager.PlayerMovement(moveAmount);
        HandleRotation(_moveDirection);
    }

    //public void HandleMovementToPosition(Vector3 targetPosition){
    //    Vector3 _moveDirection = targetPosition - transform.position;
    //    float dis = _moveDirection.magnitude;
    //    Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * walkingSpeed);
    //    transform.position = newPosition;
    //    _moveDirection.Normalize();

    //    float moveAmount = Mathf.Clamp(dis, 0, offsetDistance) / offsetDistance;
    //    _animatorManager.PlayerMovement(moveAmount);

    //    // HandleMovement(_moveDirection);
    //    HandleRotation(_moveDirection);
    //}

    private void HandleMovement(Vector3 _moveDirection){
        float moveAmount = Mathf.Clamp01(Mathf.Abs(_moveDirection.x) + Mathf.Abs(_moveDirection.z));
        //Debug.Log(_moveDirection);
        _moveDirection.Normalize();
        _moveDirection.y = 0;

        _movementSpeed = walkingSpeed / 3;

        _moveDirection = _moveDirection * _movementSpeed  * moveAmount;
        // Debug.Log(_moveDirection);

        _rb.velocity = _moveDirection;
        _animatorManager.PlayerMovement(moveAmount);
    }

    private void HandleRotation(Vector3 _moveDirection){
        if (_moveDirection.magnitude == 0) return;
        float angle = Vector3.SignedAngle(Vector3.forward, _moveDirection, Vector3.up) - Vector3.SignedAngle(_forwardVec, transform.forward, transform.up);
        transform.RotateAround(transform.position, transform.up, angle);
    }
}
