using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    const string PICKUPS_PARENT_NAME = "Pickups";

    // Config Params
    [Header("Configurations")]
    [SerializeField] float timeToDestroyStarAfterTaken = 0.5f;
    [SerializeField] AudioClip _takenSFX;

    // Cache
    GameObject _pickupParent;
    Animator _animator;
    GameSession _gameSession;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _gameSession = FindObjectOfType<GameSession>();

        CreateStarParent();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(TakeStar());
    }

    IEnumerator TakeStar()
    {
        PlayTakenSFX();
        _animator.SetTrigger("TakeStar");
        yield return new WaitForSecondsRealtime(timeToDestroyStarAfterTaken);
        Destroy(gameObject);
        _gameSession.CollectStar(1);
    }

    private void PlayTakenSFX()
    {
        if (!_takenSFX) { return; }

        AudioSource.PlayClipAtPoint(_takenSFX, Camera.main.transform.position, 0.25f);
    }

    private void CreateStarParent()
    {
        _pickupParent = GameObject.Find(PICKUPS_PARENT_NAME);

        if (!_pickupParent)
        {
            _pickupParent = new GameObject(PICKUPS_PARENT_NAME);
        }
    }
}
