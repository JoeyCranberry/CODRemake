using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealthManager : MonoBehaviour
{
    private ZombieManager zManager;
    private List<ZombieDamageReciever> damageRecievers;

    public float StartHealth = 100;

    [SerializeField]
    private float curHealth;

    public void Setup(ZombieManager _zManager, float AdditionalRoundHealth)
    {
        zManager = _zManager;
        curHealth = StartHealth + AdditionalRoundHealth;
    }

    private void Start()
    {
        damageRecievers = new List<ZombieDamageReciever>();
        damageRecievers.AddRange(gameObject.GetComponentsInChildren<ZombieDamageReciever>());
        SetupDamageRecievers();
    }

    private void SetupDamageRecievers()
    {
        foreach(ZombieDamageReciever reciever in damageRecievers)
        {
            reciever.Setup(this);
        }
    }

    public bool TakeDamage(float amount)
    {
        curHealth -= amount;

        if(curHealth <= 0f)
        {
            zManager.ZombieKilled();
            return true;
        }

        return false;
    }
}
