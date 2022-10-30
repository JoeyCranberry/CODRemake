using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackManager : MonoBehaviour
{
    private ZombieManager zManager;

    public float AttackDamage = 1f;
    public float AttackCooldown = 3f;

    private float curCooldown;

    public void Setup(ZombieManager _zManager)
    {
        zManager = _zManager;
    }

    public void AttackPlayer(PlayerManager player)
    {
        // TODO: add a visiblity and distance check when not at a window, otherwise, this will attack behind walls
        player.TakeDamage(AttackDamage);
        curCooldown = AttackCooldown;
    }

    public bool WaitNextAttack()
    {
        curCooldown -= Time.deltaTime;

        if(curCooldown <= 0f)
        {
            return true;
        }

        return false;
    }
}
