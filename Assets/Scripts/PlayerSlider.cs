using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlider : MonoBehaviour
{
    // Config Params
    [Header("Slider Control")]
    [SerializeField] float slideSpeed = 15.0f;
    [SerializeField] float slideTime = 0.5f;

    [Header("Slide Damage")]
    [SerializeField] int damage = 1000;

    // Cached Components
    Joystick _joystick;
    Animator _animator;
    Rigidbody2D _rigidbody2D;
    CapsuleCollider2D _bodyCapsuleCollider2D;
    BoxCollider2D _feetBoxCollider2D;
    ParticleSystem _dustParticleSystem;
    Player _player;

    // State
    bool isSliding;

    // Start is called before the first frame update
    void Start()
    {
        _joystick = FindObjectOfType<Joystick>();
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _bodyCapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _feetBoxCollider2D = GetComponent<BoxCollider2D>();
        _dustParticleSystem = transform.GetChild(2).gameObject.GetComponent<ParticleSystem>();
        _player = GetComponent<Player>();

        isSliding = false;
    }

    // Update is called once per frame
    void Update()
    {
        Slide();
    }

    private void Slide()
    {
        bool isSlidingEnabled = _feetBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")) && _joystick.Vertical <= -0.80 && !isSliding;

        if (isSlidingEnabled)
        {
            StartCoroutine(SlideControl());
        }
    }

    IEnumerator SlideControl()
    {
        isSliding = true;
        PlayerInteraction();
        SlidingSfxPlay();

        _rigidbody2D.velocity = new Vector2(Mathf.Sign(_rigidbody2D.velocity.x) * slideSpeed, _rigidbody2D.velocity.y);
        yield return new WaitForSeconds(slideTime);
        isSliding = false;
        PlayerInteraction();
    }

    private void PlayerInteraction()
    {
        _player.SetIsMoveEnabled(!isSliding);
        _player.SetIsDieEnabled(!isSliding);
    }

    private void SlidingSfxPlay()
    {
        _animator.SetTrigger("Slide");
        _dustParticleSystem.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!isSliding) { return; }

        Debug.Log($"OnCollisionEnter.. {collision.gameObject.name}");
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemyHealth)
        {
            enemyHealth.TakeDamage(damage);
        }
    }

}
