using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoneyManager : MonoBehaviour
{
    private PlayerManager playerManager;

    public void Setup(PlayerManager _playerManager)
    {
        playerManager = _playerManager;
    }

    private int GetPerkPointCost(PerkMachineManager.PERK_MACHINE_TYPE type)
    {
        switch(type)
        {
            case PerkMachineManager.PERK_MACHINE_TYPE.JUGGERNOG:
                return 10;
            case PerkMachineManager.PERK_MACHINE_TYPE.SPEED_COLA:
                return 10;
            case PerkMachineManager.PERK_MACHINE_TYPE.DOUBLE_TAP:
                return 10;
            case PerkMachineManager.PERK_MACHINE_TYPE.QUICK_REVIVE:
                return 10;
            case PerkMachineManager.PERK_MACHINE_TYPE.PHD_FLOPPER:
                return 10;
            case PerkMachineManager.PERK_MACHINE_TYPE.STAMIN_UP:
                return 10;
            case PerkMachineManager.PERK_MACHINE_TYPE.DEADSHOT:
                return 10;
            case PerkMachineManager.PERK_MACHINE_TYPE.MULE_KICK:
                return 10;
            case PerkMachineManager.PERK_MACHINE_TYPE.TOMBSTONE:
                return 10;
            case PerkMachineManager.PERK_MACHINE_TYPE.WHOS_WHO:
                return 10;
            case PerkMachineManager.PERK_MACHINE_TYPE.ELECTRIC_CHERRY:
                return 10;
            case PerkMachineManager.PERK_MACHINE_TYPE.VULTURE_AID:
                return 10;
            case PerkMachineManager.PERK_MACHINE_TYPE.WIDOWS_WINE:
                return 10;
            default:
                return 10;
        }
    }

    private int GetPointAmount(POINT_TYPE type)
    {
        switch(type)
        {
            case POINT_TYPE.NON_LETHAL_HIT:
                return 10;
            case POINT_TYPE.LETHAL_TORSO_HIT:
                return 60;
            case POINT_TYPE.LETHAL_LIMB_HIT:
            case POINT_TYPE.LETHAL_EXPLOSIVE_HIT:
                return 50;
            case POINT_TYPE.LETHAL_NECK_HIT:
                return 70;
            case POINT_TYPE.LETHAL_HEADSHOT:
                return 100;
            case POINT_TYPE.LETHAL_MELEE:
                return 130;
            case POINT_TYPE.PLANK_REPAIRED:
                return 10;
            case POINT_TYPE.COLLECT_CARPENTER:
                return 200;
            case POINT_TYPE.COLLECT_NUKE:
                return 400;
            default:
                return 10;
        }
    }

    public enum POINT_TYPE
    {
        NON_LETHAL_HIT,
        LETHAL_TORSO_HIT,
        LETHAL_LIMB_HIT,
        LETHAL_EXPLOSIVE_HIT,
        LETHAL_NECK_HIT,
        LETHAL_HEADSHOT,
        LETHAL_MELEE,
        PLANK_REPAIRED,
        COLLECT_CARPENTER,
        COLLECT_NUKE
    }
}
