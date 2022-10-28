using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowBarricadeActionArea : MonoBehaviour
{
    public ActionAreaType AreaType;

    private WindowBarricade barricade;

    public void Setup(WindowBarricade _barricade)
    {
        barricade = _barricade;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (AreaType == ActionAreaType.ZOMBIE)
        {
            ZombieManager enteringZombie = other.GetComponentInParent<ZombieManager>();
            if(enteringZombie != null)
            {
                enteringZombie.EnteredBarricadeActionArea(barricade);
            }
        }
        else
        {
            PlayerManager playerManager = other.GetComponentInParent<PlayerManager>();

            if(playerManager != null)
            {
                playerManager.EnteredBarricadeActionArea(barricade);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (AreaType == ActionAreaType.ZOMBIE)
        {
            ZombieManager exitingZombie = other.GetComponentInParent<ZombieManager>();
            if (exitingZombie != null)
            {
                exitingZombie.ExitedBarricadeActionArea(barricade);
            }
        }
        else
        {
            PlayerManager playerManager = other.GetComponentInParent<PlayerManager>();
            if (playerManager != null)
            {
                playerManager.ExitedBarricadeActionArea(barricade);
            }
        }
    }

    public enum ActionAreaType
    {
        PLAYER,
        ZOMBIE
    }
}
