using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    // Config Params
    [Header("Configuration")]
    [SerializeField] GameObject projectile = null;
    [SerializeField] GameObject gun = null;    // This will provide a position where to instantiate the projectile prefab from

    // State
    GameObject _projectileParent;

    const string PROJECTILE_PARENT_NAME = "Player Projectiles";

    // Start is called before the first frame update
    void Start()
    {
        CreateProjectileParent();
    }

    private void CreateProjectileParent()
    {
        _projectileParent = GameObject.Find(PROJECTILE_PARENT_NAME);

        if (!_projectileParent)
        {
            _projectileParent = new GameObject(PROJECTILE_PARENT_NAME);
        }
    }

    private void Shoot()
    {
        GameObject projectileInstance = Instantiate(projectile, gun.transform.position, Quaternion.identity);
        projectileInstance.transform.parent = _projectileParent.transform;
    }
}
