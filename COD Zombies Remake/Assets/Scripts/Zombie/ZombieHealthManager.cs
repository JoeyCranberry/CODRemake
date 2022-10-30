using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealthManager : MonoBehaviour
{
    private ZombieManager zManager;

    public float StartHealth;

    [SerializeField]
    private float curHealth;

    public void Setup(ZombieManager _zManager)
    {
        zManager = _zManager;
    }

    private void Start()
    {
        curHealth = StartHealth;
    }

    public bool TakeDamage(float amount)
    {
        curHealth -= amount;

        if(curHealth <= 0f)
        {
            return true;
        }

        return false;
    }
}
