using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private bool isDead = false;

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
        if(!isDead)
        {
            _animator.SetTrigger(AnimationID.Die);
            isDead = true;
        }
    }

}
