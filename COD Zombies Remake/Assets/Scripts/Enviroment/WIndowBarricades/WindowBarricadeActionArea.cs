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
                barricade.ZombieEnteredActionArea();
            }
        }
        else
        {
            PlayerManager playerManager = other.GetComponentInParent<PlayerManager>();

            if(playerManager != null)
            {
                playerManager.EnteredBarricadeActionArea(barricade);
                barricade.PlayerEnteredActionArea();
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
                barricade.ZombieExitedActionArea();
            }
        }
        else
        {
            PlayerManager playerManager = other.GetComponentInParent<PlayerManager>();
            if (playerManager != null)
            {
                playerManager.ExitedBarricadeActionArea(barricade);
                barricade.PlayerExitedActionArea();
            }
        }
    }

    public enum ActionAreaType
    {
        PLAYER,
        ZOMBIE
    }
}
