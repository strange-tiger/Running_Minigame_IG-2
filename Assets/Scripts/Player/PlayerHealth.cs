using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public bool IsDead { get; private set; }

    [SerializeField] private AudioClip _getCoinSoundClip;
    [SerializeField] private AudioClip _dieSoundClip;

    private Animator _animator;
    private AudioSource _audioSource;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        GameManager.Instance.PlayerHealth = this;
    }

    public void GetCoin()
    {
        _audioSource.PlayOneShot(_getCoinSoundClip);
    }

    public void Die()
    {
        if(!IsDead)
        {
            _audioSource.PlayOneShot(_dieSoundClip);
            _animator.SetTrigger(AnimationID.Die);
            IsDead = true;
        }
    }

}
