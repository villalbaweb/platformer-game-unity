using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [Header("Damage Control")]
    [SerializeField] int damage = 100;

    [Header("Movement")]
    [SerializeField] [Range(1f, 20f)] float speed = 10.0f;
    [SerializeField] float destroyAfter = 0.5f;

    [Header("Sound Effects")]
    [SerializeField] AudioClip destroyAudioSFX;


    // Cache
    Player _player;
    Animator _animator;
    float projectileDirection;
    bool isMoving = true;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        SetProjectileDirection();
        StartCoroutine(DestroyProjectileAfter());
    }

    private void SetProjectileDirection()
    {
        projectileDirection = CalculateProjectileDirection();
        transform.localScale = new Vector2(-(Mathf.Sign(projectileDirection)), 1f);
    }

    private float CalculateProjectileDirection()
    {
        _player = FindObjectOfType<Player>();
        float playerXPos = _player.gameObject.transform.position.x;
        float gunXPos = _player.gameObject.transform.GetChild(1).gameObject.transform.position.x;
        return gunXPos - playerXPos;
    }

    IEnumerator DestroyProjectileAfter()
    {
        yield return new WaitForSecondsRealtime(destroyAfter);
        StartDestroyAnimation();    // TODO check if after hit enemy and destroy this will execute ????
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if(!isMoving) { return; }
        transform.Translate((projectileDirection > Mathf.Epsilon ?  Vector2.right : Vector2.left) * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartDestroyAnimation();

        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        if(enemyHealth)
        {
            enemyHealth.TakeDamage(damage);
        }
    }

    private void StartDestroyAnimation()
    {
        isMoving = false;
        PlayDestroySFX();
        _animator.SetTrigger("Splatt");
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    private void PlayDestroySFX()
    {
        if (!destroyAudioSFX) { return; }

        AudioSource.PlayClipAtPoint(destroyAudioSFX, Camera.main.transform.position);
    }
}
