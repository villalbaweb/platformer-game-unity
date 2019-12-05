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
    [SerializeField] AudioClip dieAudioSFX;

    // Cache
    Animator _animator;
    bool isDead;

    private void Start()
    {
        _animator = GetComponent<Animator>();
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
        _animator.SetTrigger("Die");
        PlayDieSFX();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private void PlayDieSFX()
    {
        if (!dieAudioSFX) { return; }

        AudioSource.PlayClipAtPoint(dieAudioSFX, Camera.main.transform.position);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
