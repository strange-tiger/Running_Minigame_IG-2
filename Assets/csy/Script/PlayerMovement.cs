using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move Line")]
    [SerializeField] private float[] _moveXPositions = { -2.5f, 0, 2.5f };
    [SerializeField] private float _moveSpeed = 2.5f;
    private int _currentMovePosition = 1;
    private bool _isMoving = false;

    [Header("Jump")]
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _jumpSpeed = 2f;
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
        if(!_isMoving)
        {
            Move();
        }

        if(!_isJumping)
        {
            Jump();
        }
    }

    // 라인 이동 관련
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

    private IEnumerator MoveLine(int nextPos, int moveDirection)
    {
        _isMoving = true;

        float endXPosition = _moveXPositions[nextPos];
        Debug.Log(endXPosition);

        while(true)
        {
            float nextXPosition = moveDirection * _moveSpeed * Time.deltaTime;
            transform.Translate(nextXPosition, 0f, 0f);

            if(Mathf.Pow((endXPosition - transform.position.x), 2) <= 0.001)
            {
                transform.position = new Vector3(endXPosition, transform.position.y, transform.position.z);
                break;
            }

            yield return null;
        }

        if (!_isJumping)
        {
            _isMoving = false;
        }
    }

    // 점프 관련
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
        endYPosition += 1f;

        while(true)
        {
            float currentYPosition = _jumpSpeed * Time.deltaTime;
            transform.Translate(0f, currentYPosition, 0f);
            Debug.Log(transform.position.y);


            if (Mathf.Pow((endYPosition - transform.position.y), 2) <= 0.001)
            {
                transform.position = new Vector3(transform.position.x, endYPosition, transform.position.z);
                break;
            }

            yield return null;
        }

        while(true)
        {
            float currentYPosition = -_jumpSpeed * Time.deltaTime;
            transform.Translate(0f, currentYPosition, 0f);

            if (Mathf.Pow((1f - transform.position.y), 2) <= 0.001)
            {
                transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
                break;
            }

            yield return null;
        }

        _isMoving = false;
        _isJumping = false;
    }
}