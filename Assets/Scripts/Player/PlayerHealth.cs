using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public bool IsDead { get; private set; }

    [SerializeField] private AudioClip _getCoinSoundClip;
    public int Score { get; private set; }
    public UnityEvent<int> OnGetCoin = new UnityEvent<int>();

    [SerializeField] private AudioClip _dieSoundClip;
    public UnityEvent<int, int> OnGameOver = new UnityEvent<int, int>();

    private Animator _animator;
    private AudioSource _audioSource;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _audioSource = GetComponentInParent<AudioSource>();
    }

    private void Start()
    {
        GameManager.Instance.PlayerHealth = this;
    }

    public void GetCoin()
    {
        _audioSource.PlayOneShot(_getCoinSoundClip);
        OnGetCoin.Invoke(++Score);
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
    
    public void GameOver()
    {
        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }
        int highScore = PlayerPrefs.GetInt("HighScore");

        OnGameOver.Invoke(Score, highScore);
    }
}
