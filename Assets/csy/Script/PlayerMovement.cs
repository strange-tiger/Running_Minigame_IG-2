using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move Line")]
    [SerializeField] private float[] _moveXPositions = { -2.5f, 0, 2.5f };
    [SerializeField] private float _moveSpeed = 2.5f;
    private int _currentMovePosition = 1;
    private bool _isMoveable = true;

    [Header("Jump")]
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _jumpSpeed = 1f;
    private bool _isJumping = false;

    // 기본 필요 component
    private Rigidbody _rigidbody;
    private PlayerInput _input;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(_isMoveable)
        {
            Move();
        }

        if(!_isJumping)
        {
            Jump();
        }
    }

    private void Move()
    {
        int nextMovePosition = _currentMovePosition + _input.X;

        if(_input.X == 0 || nextMovePosition < 0 || nextMovePosition > 2)
        {
            return;
        }

        _currentMovePosition = nextMovePosition;
        StartCoroutine(MoveLine(nextMovePosition, _input.X));
    }

    // 라인 이동 코루틴
    private IEnumerator MoveLine(int nextPos, int moveDirection)
    {
        _isMoveable = false;

        float endXPosition = _moveXPositions[nextPos];
        Debug.Log(endXPosition);

        while(true)
        {
            float nextXPosition = moveDirection * _moveSpeed * Time.deltaTime;
            _rigidbody.MovePosition(_rigidbody.position + new Vector3(nextXPosition, 0f, 0f));

            if(Mathf.Pow((endXPosition - _rigidbody.position.x), 2) <= 0.001)
            {
                transform.position = new Vector3(endXPosition, transform.position.y, transform.position.z);
                break;
            }

            yield return null;
        }

        _isMoveable = true;
    }

    private void Jump()
    {
        if (!_input.Jump)
        {
            return;
        }

        StartCoroutine(Jumping(_jumpHeight));
    }

    private IEnumerator Jumping(float endYPosition)
    {
        _isJumping = true;

        while(true)
        {
            float currentYPosition = _jumpSpeed * Time.deltaTime;
            _rigidbody.MovePosition(_rigidbody.position + new Vector3(0f, currentYPosition, 0f));


            if (Mathf.Pow((endYPosition - _rigidbody.position.x), 2) <= 0.001)
            {
                transform.position = new Vector3(transform.position.x, endYPosition, transform.position.z);
                break;
            }

            yield return null;
        }

        while(true)
        {
            float currentYPosition = _jumpSpeed * Time.deltaTime;
            _rigidbody.MovePosition(_rigidbody.position + new Vector3(0f, currentYPosition, 0f));


            if (Mathf.Pow((0f - _rigidbody.position.x), 2) <= 0.001)
            {
                transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
                break;
            }

            yield return null;
        }


        _isJumping = false;
    }
}