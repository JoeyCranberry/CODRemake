using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    private PlayerManager playerManager;

    public Slider HealthSlider;
    public GameObject DeathDisplay;

    public float StartHealth = 10f;
    public float DelayToHeal = 5f;
    public float HealthAmountPerTick = .1f;

    [SerializeField]
    private float curHealth;
    private float curDelayToHeal;

    public void Setup(PlayerManager _playerManager)
    {
        playerManager = _playerManager;
    }

    private void Start()
    {
        curHealth = StartHealth;
        HealthSlider.maxValue = StartHealth;
        HealthSlider.value = StartHealth;
    }

    public bool TakeDamage(float amount)
    {
        curHealth -= amount;
        HealthSlider.value = curHealth;

        curDelayToHeal = DelayToHeal;

        if (curHealth <= 0f)
        {
            PlayerDead();
            return true;
        }

        return false;
    }

    private void PlayerDead()
    {
        DeathDisplay.SetActive(true);
    }

    public bool HealOverTime()
    {
        if(curDelayToHeal <= 0f)
        {
            curHealth += HealthAmountPerTick * Time.deltaTime;

            if(curHealth >= StartHealth)
            {
                curHealth = StartHealth;
                HealthSlider.value = curHealth;
                return true;
            }
        }
        else
        {
            curDelayToHeal -= Time.deltaTime;
        }

        return false;
    }
}
