using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerWeapon : MonoBehaviour
{
    private PlayerWeaponManager wManager;

    private PlayerWeaponStats weaponStats;

    private Transform shootFromLocation;
    private LayerMask raycastLayerMask;

    private TMP_Text ammoDisplayText;

    private AudioSource fireAudioSource;
    private AudioSource reloadAudioSource;
    private PlayerWeaponReloadAudio reloadAudio;

    private int curBulletsInMag = 0;
    private int curBulletsInReserve = 0;

    private float curTimeLeftToReload;
    private float curTimeLeftToFire;

    public WeaponFiringState curFireState = WeaponFiringState.ABLE_TO_FIRE;

    public void Setup(PlayerWeaponManager _wManager, PlayerWeaponStats _weaponStats, TMP_Text _ammoDisplayText, Transform _shootFromLocation, LayerMask _raycastLayerMask)
    {
        wManager = _wManager;
        weaponStats = _weaponStats;
        ammoDisplayText = _ammoDisplayText;

        curBulletsInMag = weaponStats.BulletsInMag;
        curBulletsInReserve = weaponStats.BulletsInReserve;

        shootFromLocation = _shootFromLocation;
        raycastLayerMask = _raycastLayerMask;
    }

    private void Start()
    {
        fireAudioSource = gameObject.AddComponent<AudioSource>();
        reloadAudioSource = gameObject.AddComponent<AudioSource>();
        reloadAudio = gameObject.AddComponent<PlayerWeaponReloadAudio>();

        reloadAudio.Setup(reloadAudioSource, weaponStats.ReloadClips);
    }

    private void Update()
    {
        switch(curFireState)
        {
            case WeaponFiringState.WAITING_BETWEEN_SHOTS:
                WaitOnFire();
                break;
            case WeaponFiringState.RELOADING:
                WaitOnReload();
                break;
        }
    }

    #region Firing
    public void FireWeapon()
    {
        switch (curFireState)
        {
            case WeaponFiringState.ABLE_TO_FIRE:
                curBulletsInMag--;
                FireHitscan();
                CheckReloadRequired();
                UpdateAmmoDisplay();
                break;
            case WeaponFiringState.UNABLE_TO_FIRE:
                fireAudioSource.Play();
                break;
        }
    }

    private void CheckReloadRequired()
    {
        if (curBulletsInMag <= 0)
        {
            if (curBulletsInReserve > 0)
            {
                TriggerReload();
            }
            else
            {
                curFireState = WeaponFiringState.UNABLE_TO_FIRE;
                fireAudioSource.clip = weaponStats.NoBulletsSound;
            }
        }
        else
        {
            fireAudioSource.clip = weaponStats.FireSound;
            fireAudioSource.Play();

            curFireState = WeaponFiringState.WAITING_BETWEEN_SHOTS;
            curTimeLeftToFire = 60f / weaponStats.FireRatePerMinute;
        }
    }

    private void FireHitscan()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(shootFromLocation.position, Camera.main.transform.forward, out hit, Mathf.Infinity, raycastLayerMask))
        {
            Debug.DrawRay(shootFromLocation.position, Camera.main.transform.forward * hit.distance, Color.red);
            ZombieDamageReciever zombie = hit.collider.GetComponent<ZombieDamageReciever>();
            if (zombie != null)
            {
                zombie.RecieveDamage(weaponStats.Damage, weaponStats.HeadshotMultplier, weaponStats.AbdomenMultiplier, weaponStats.ChestMultiplier);
            }
        }
    }

    private void WaitOnFire()
    {
        curTimeLeftToFire -= Time.deltaTime;
        if (curTimeLeftToFire <= 0f)
        {
            curFireState = WeaponFiringState.ABLE_TO_FIRE;
        }
    }
    #endregion

    #region Reloading
    public bool TriggerReload()
    {
        // If the reload process hasn't already been started and there are bullets remaining
        if (curFireState == WeaponFiringState.ABLE_TO_FIRE)
        {
            float reloadTime = curBulletsInMag > 0 ? weaponStats.ReloadTimeLoaded : weaponStats.ReloadTimeUnloaded;

            curTimeLeftToReload = reloadTime;
            reloadAudio.Play(reloadTime);

            curFireState = WeaponFiringState.RELOADING;
            return true;
        }

        return false;
    }

    private void WaitOnReload()
    {
        curTimeLeftToReload -= Time.deltaTime;
        if (curTimeLeftToReload <= 0f)
        {
            Reload();
        }
    }

    private void Reload()
    {
        // Add bullets in mag to reserve
        curBulletsInReserve += curBulletsInMag;
        curBulletsInMag = 0;

        // Add bullets to mag until no bullets remain or mag is full
        while (curBulletsInReserve > 0 && curBulletsInMag < weaponStats.BulletsInMag)
        {
            curBulletsInReserve--;
            curBulletsInMag++;
        }

        curFireState = WeaponFiringState.ABLE_TO_FIRE;
        UpdateAmmoDisplay();
    }
    #endregion

    public void UpdateAmmoDisplay()
    {
        ammoDisplayText.text = curBulletsInMag + "/" + curBulletsInReserve;
    }

    public enum WeaponFiringState
    {
        ABLE_TO_FIRE,
        WAITING_BETWEEN_SHOTS,
        RELOADING,
        UNABLE_TO_FIRE
    }
}
