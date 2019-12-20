using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config params
    [Header("Movement")]
    [SerializeField] float runSpeed = 1.0f;
    [SerializeField] float climLadderSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] Vector2 deathHit = new Vector2(0f, 15f);

    [Header("Player Death Handler")]
    [Tooltip("The damage caused to the enemies when sliding")]
    [SerializeField] int timeToWaitWhendDie = 2;

    [Header("Audio Effects")]
    [SerializeField] AudioClip jumpAudioSFX = null;
    [SerializeField] AudioClip dieAudioSFX = null;
    [SerializeField] AudioClip lifeupAudioSFX = null;
    [SerializeField] AudioClip shootAudioSFX = null;

    // Cached components refs
    Joystick _joystick;
    Animator _animator;
    Rigidbody2D _rigidbody2D;
    CapsuleCollider2D _bodyCapsuleCollider2D;
    BoxCollider2D _feetBoxCollider2D;
    GameSession _gameSession;

    // State
    float originalGravityScale;
    bool isAlive;
    bool isMoveEnabled;
    bool isDieEnabled;
    List<string> lethalLayers;

    // Start is called before the first frame update
    void Start()
    {
        _joystick = FindObjectOfType<Joystick>();
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _bodyCapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _feetBoxCollider2D = GetComponent<BoxCollider2D>();
        _gameSession = FindObjectOfType<GameSession>();

        originalGravityScale = _rigidbody2D.gravityScale;
        isAlive = true;
        isMoveEnabled = true;
        isDieEnabled = true;
        lethalLayers = new List<string> { "Ghost Enemy", "Troll Enemy", "Obstacles" };
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }

        Run();
        Climb();
        FlipSprite();
        Die();
    }

    private void Climb()
    {
        if (!_bodyCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladders")))
        {
            LadderClimbAnimationHandler(false);
            return;
        }
        var joystickVerticalSpeed = _joystick.Vertical * climLadderSpeed;
        var keyboardVerticalSpeed = Input.GetAxis("Vertical") * climLadderSpeed;

        var verticalSpeed = Mathf.Abs(joystickVerticalSpeed) > Mathf.Epsilon ? joystickVerticalSpeed : keyboardVerticalSpeed;

        Vector2 playerClimbVelocity = new Vector2(_rigidbody2D.velocity.x, verticalSpeed);
        _rigidbody2D.velocity = playerClimbVelocity;

        LadderClimbAnimationHandler(true);
    }

    private void LadderClimbAnimationHandler(bool isClimbing)
    {
        _rigidbody2D.gravityScale = isClimbing ? 0f : originalGravityScale; // set the gravity of the rigid body accordingly to the climbing state
        _animator.SetBool("IsClimbing", isClimbing);
    }

    private void Run()
    {
        if (!isMoveEnabled) { return; }

        // we dont use Time.deltaTime because we are applying this value
        // to rigidBody2D and this is handled by the physics engine which calculates delta time by itself.
        // when using transform based movement, then we need Time.deltaTime

        // read entry from the joystick
        var joysTickSpeed = _joystick.Horizontal * runSpeed;
        // read entry from the input system
        var keyboardSpeed = Input.GetAxis("Horizontal") * runSpeed;
        // check wich entry is being used on priority with the joystick
        var horizontalSpeed = Mathf.Abs(joysTickSpeed) > Mathf.Epsilon ? joysTickSpeed : keyboardSpeed;

        Vector2 playerVelocity = new Vector2(horizontalSpeed, _rigidbody2D.velocity.y);
        _rigidbody2D.velocity = playerVelocity;

        RunWalkAnimationHandler(horizontalSpeed);
    }

    private void RunWalkAnimationHandler(float horizontalSpeed)
    {
        float absoluteSpeed = Mathf.Abs(horizontalSpeed);

        _animator.SetFloat("Speed", absoluteSpeed / runSpeed);
    }

    private void FlipSprite()
    {
        bool isPlayerMoving = Mathf.Abs(_rigidbody2D.velocity.x) > Mathf.Epsilon;
        if (isPlayerMoving)
        {
            transform.localScale = new Vector2(Mathf.Sign(_rigidbody2D.velocity.x), 1);
        }
    }

    private void Die()
    {
        if(!isDieEnabled) { return; }

        if (_bodyCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask(lethalLayers.ToArray())) || _feetBoxCollider2D.IsTouchingLayers(LayerMask.GetMask(lethalLayers.ToArray())))
        {
            StartCoroutine(DieHandler());
        }
    }

    IEnumerator DieHandler()
    {
        isAlive = false;
        PlayDieSFX();
        _animator.SetTrigger("Die");
        _rigidbody2D.velocity = deathHit;
        yield return new WaitForSeconds(timeToWaitWhendDie);
        _gameSession.ProcessPlayerDeath();
    }

    private void PlayDieSFX()
    {
        if (!dieAudioSFX) { return; }

        AudioSource.PlayClipAtPoint(dieAudioSFX, Camera.main.transform.position);
    }

    private void PlayJumpSFX()
    {
        if (!jumpAudioSFX) { return; }

        AudioSource.PlayClipAtPoint(jumpAudioSFX, Camera.main.transform.position);
    }

    private void PlayLifeUpSFX()
    {
        if (!lifeupAudioSFX) { return; }

        AudioSource.PlayClipAtPoint(lifeupAudioSFX, Camera.main.transform.position);
    }

    private void PlayShootSFX()
    {
        if (!shootAudioSFX) { return; }

        AudioSource.PlayClipAtPoint(shootAudioSFX, Camera.main.transform.position);
    }

    #region Public Methods

    public void JumpButtonHit()
    {
        // Findout if the collider is actually touching specific layer
        if(!_feetBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")) || !isAlive)
        {
            return;
        }

        PlayJumpSFX();
        Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
        _rigidbody2D.velocity += jumpVelocityToAdd;

    }

    public void ShootButtonHit()
    {
        if(!isAlive) { return; }
        _animator.SetTrigger("Shoot");
        PlayShootSFX();
    }

    /// <summary>
    /// This will make the player to start dissapearing
    /// </summary>
    public void DissapearVFX()
    {
        isAlive = false;
        _rigidbody2D.velocity = new Vector2(0f, 0f);
        _animator.SetTrigger("Dissapear");
    }

    public void LifeUp()
    {
        PlayLifeUpSFX();
    }

    public void SetIsMoveEnabled(bool _isMoveEnabled)
    {
        isMoveEnabled = _isMoveEnabled;
    }

    public void SetIsDieEnabled(bool _isDieEnabled)
    {
        isDieEnabled = _isDieEnabled;
    }

    #endregion
}
