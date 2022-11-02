using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerWeaponManager : MonoBehaviour
{
    private PlayerManager playerManager;

    public PlayerWeaponStats StarterWeapon;

    public TMP_Text ammoDisplayText;

    private PlayerWeapon primaryWeapon;
    private PlayerWeapon secondaryWeapon;

    public Transform ShootFromLocation;
    public LayerMask WhatCanBeShot;

    private PlayerWeapon tempWeapon;

    public void Setup(PlayerManager _playerManager)
    {
        playerManager = _playerManager;
    }

    private void Start()
    {
        // Setup starter weapon
        primaryWeapon = gameObject.AddComponent<PlayerWeapon>();
        primaryWeapon.Setup(this, StarterWeapon, ammoDisplayText, ShootFromLocation, WhatCanBeShot);
        primaryWeapon.UpdateAmmoDisplay();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            primaryWeapon.FireWeapon();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            primaryWeapon.TriggerReload();
        }
    }
}
