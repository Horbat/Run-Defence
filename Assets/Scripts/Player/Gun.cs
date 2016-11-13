using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public float fireRate = 1f;

    public GameObject bulletObject;

    Transform _muzzleTrans;

    float _nextTimeShoot;

    const string _pathMuzzle = "Muzzle"; 

    void Awake()
    {
        _muzzleTrans = transform.Find(_pathMuzzle);
    }

    void Start()
    {
        _nextTimeShoot = Time.time + fireRate;
    }

    public void Shoot()
    {
        if (_nextTimeShoot < Time.time)
        {
            _nextTimeShoot = Time.time + fireRate;

            Instantiate(bulletObject, _muzzleTrans.position, _muzzleTrans.rotation);
        }
    }
}