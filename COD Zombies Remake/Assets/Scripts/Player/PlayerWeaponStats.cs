using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWeaponStats", menuName = "ScriptableObjects/PlayerWeaponStats", order = 1)]
public class PlayerWeaponStats : ScriptableObject
{
    public int BulletsInMag = 8;
    public int BulletsInReserve = 32;

    public float Damage = 20f;
    public float ReloadTimeUnloaded = 1.8f;
    public float ReloadTimeLoaded = 1.63f;
    public float FireRatePerMinute = 625f;

    public float HeadshotMultplier = 3.5f;
    public float ChestMultiplier = 1.25f;
    public float AbdomenMultiplier = 1.1f;

    public AudioClip FireSound;
    public AudioClip NoBulletsSound;
    public List<AudioClip> ReloadClips;

    public FireMode WeaponFireMode;

    public enum FireMode
    {
        SEMI_AUTOMATIC,
        BURST,
        AUTOMATIC
    }
}

