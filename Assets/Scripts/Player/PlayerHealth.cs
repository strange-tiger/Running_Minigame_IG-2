using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public bool IsDead { get; private set; }

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        GameManager.Instance.PlayerHealth = this;
    }

    public void Die()
    {
        if(!IsDead)
        {
            _animator.SetTrigger(AnimationID.Die);
            IsDead = true;
        }
    }

}
