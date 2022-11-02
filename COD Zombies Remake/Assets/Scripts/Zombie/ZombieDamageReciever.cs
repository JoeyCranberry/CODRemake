using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDamageReciever : MonoBehaviour
{
    private ZombieHealthManager healthManager;

    public ZombieBodyPart BodyPart;

    public void Setup(ZombieHealthManager _healthManager)
    {
        healthManager = _healthManager;
    }

    public void RecieveDamage(float baseAmount, float headshotMultiplier, float abdomenMultiplier, float chestMultiplier)
    {
        float totalDamage = baseAmount;
        switch(BodyPart)
        {
            case ZombieBodyPart.HEAD:
                totalDamage *= headshotMultiplier;
                break;
            case ZombieBodyPart.ABDOMEN:
                totalDamage *= abdomenMultiplier;
                break;
            case ZombieBodyPart.CHEST:
                totalDamage *= chestMultiplier;
                break;
        }

        healthManager.TakeDamage(totalDamage);
    }

    public enum ZombieBodyPart
    {
        HEAD,
        ABDOMEN,
        CHEST,
        EVERYTHING_ELSE
    }
}
