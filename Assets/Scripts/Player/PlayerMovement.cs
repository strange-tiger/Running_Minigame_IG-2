using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move Line")]
    [SerializeField] private float[] _xPositions = { -2.5f, 0, 2.5f };
    private int _currentXPosition = 1;
    [SerializeField] private float _moveSpeed = 4f;
    private bool _isMoving = false;

    [Header("Jump")]
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _jumpSpeed = 3f;
    private bool _isJumping = false;
    [SerializeField] private AudioClip _jumpSoundClip;

    // 기본 필요 component
    private PlayerInput _input;
    private PlayerHealth _health;

    private Rigidbody _rigidbody;
    
    private Animator _animator;
    private AudioSource _audioSource;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _health = GetComponentInChildren<PlayerHealth>();
        
        _rigidbody = GetComponent<Rigidbody>();
        
        _animator = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(_health.IsDead)
        {
            return;
        }

        if(!_isMoving)
        {
            Move();
        }

        if(!_isJumping)
        {
            Jump();
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }


    // 라인 이동 관련
    private void Move()
    {
        int nextMovePosition = _currentXPosition + _input.MoveX;

        if(_input.MoveX == 0 || nextMovePosition < 0 || nextMovePosition > 2)
        {
            return;
        }

        _isMoving = true;
        _currentXPosition = nextMovePosition;

        StartCoroutine(MoveLine(nextMovePosition, _input.MoveX));
    }

    private IEnumerator MoveLine(int nextPos, int moveDirection)
    {
        float endXPosition = _xPositions[nextPos];

        while(true)
        {
            float deltaxPosition = moveDirection * _moveSpeed * Time.deltaTime;
            _rigidbody.MovePosition(_rigidbody.position + new Vector3(deltaxPosition, 0f, 0f));

            if (Mathf.Pow((endXPosition - _rigidbody.position.x), 2) <= 0.01)
            {
                transform.position = new Vector3(endXPosition, transform.position.y, transform.position.z);

                if (!_isJumping)
                {
                    _isMoving = false;
                }

                break;
            }

            yield return null;
        }
    }

    // 점프 관련
    private void Jump()
    {
        if (!_input.Jump)
        {
            return;
        }

        _isJumping = true;


        StartCoroutine(Jumping(_jumpHeight));
    }

    private IEnumerator Jumping(float endYPosition)
    {
        // 이펙트
        _audioSource.PlayOneShot(_jumpSoundClip);
        _animator.SetTrigger(AnimationID.Jump);

        // 기존의 y값인 1 더해주기
        endYPosition += 1f;

        while(true)
        {
            float deltaYPosition = _jumpSpeed * Time.deltaTime;
            _rigidbody.MovePosition(_rigidbody.position + new Vector3(0f, deltaYPosition, 0f));

            if (Mathf.Pow((endYPosition - transform.position.y), 2) <= 0.01)
            {
                transform.position = new Vector3(transform.position.x, endYPosition, transform.position.z);
                
                break;
            }

            yield return null;
        }

        while(true)
        {
            float deltaYPosition = -_jumpSpeed * Time.deltaTime;
            _rigidbody.MovePosition(_rigidbody.position + new Vector3(0f, deltaYPosition, 0f));

            if (Mathf.Pow((1f - transform.position.y), 2) <= 0.01)
            {
                transform.position = new Vector3(transform.position.x, 1f, transform.position.z);

                _isMoving = false;
                _isJumping = false;
                
                break;
            }

            yield return null;
        }

    }
}