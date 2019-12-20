using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Config params
    [Header("Health")]
    [SerializeField] int health = 300;

    [Header("Audio Effects")]
    [SerializeField] AudioClip dieAudioSFX = null;

    // Cache
    Animator _animator;
    CapsuleCollider2D _mainBodyCollider2D;
    BoxCollider2D _feetCollider2D;
    bool isDead;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _mainBodyCollider2D = GetComponent<CapsuleCollider2D>();
        _feetCollider2D = GetComponent<BoxCollider2D>();
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckLife();   
    }

    private void CheckLife()
    {
        if(!isDead && health <= 0)
        {
            StartCoroutine(DieVFX());
        }
    }

    IEnumerator DieVFX()
    {
        isDead = true;
        if(_mainBodyCollider2D)
        {
            _mainBodyCollider2D.enabled = false;
        }

        if(_feetCollider2D)
        {
            _feetCollider2D.enabled = false;
        }
        _animator.SetTrigger("Die");
        PlayDieSFX();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private void PlayDieSFX()
    {
        if (!dieAudioSFX) { return; }

        AudioSource.PlayClipAtPoint(dieAudioSFX, Camera.main.transform.position, 0.3f);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
