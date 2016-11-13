using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class PlayerManager : LivingEntity
{
    public float moveSpeed = 1f;

    PlayerController _playerController;
    GunController _gunController;

    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _gunController = GetComponent<GunController>();
    }

    protected override void Start()
    {
        base.Start();
    }

    public void Move(Vector3 velocity)
    {
        _playerController.Move(velocity * moveSpeed);
    }

    public void Look(Vector2 direction)
    {
        _playerController.Look(direction);
    }

    public void Fire()
    {
        _gunController.Fire();
    }
}
