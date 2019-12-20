using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlider : MonoBehaviour
{
    // Config Params
    [Header("Slider Control")]
    [SerializeField] float slideSpeed = 15.0f;
    [SerializeField] float slideTime = 0.5f;

    // Cached Components
    Joystick _joystick;
    Animator _animator;
    Rigidbody2D _rigidbody2D;
    CapsuleCollider2D _bodyCapsuleCollider2D;
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
        if (!_bodyCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladders")) && _joystick.Vertical <= -0.80 && !isSliding)
        {
            StartCoroutine(SlideControl());
        }
    }

    private void SetIsSliding(bool _isSliding)
    {
        isSliding = _isSliding;
        _player.SetIsMoveEnabled(!_isSliding);
    }

    IEnumerator SlideControl()
    {
        isSliding = true;
        _player.SetIsMoveEnabled(false);
        SlidingSfxPlay();

        _rigidbody2D.velocity = new Vector2(Mathf.Sign(_rigidbody2D.velocity.x) * slideSpeed, _rigidbody2D.velocity.y);
        yield return new WaitForSeconds(slideTime);
        isSliding = false;
        _player.SetIsMoveEnabled(true);
    }

    private void SlidingSfxPlay()
    {
        _animator.SetTrigger("Slide");
        _dustParticleSystem.Play();
    }

}
