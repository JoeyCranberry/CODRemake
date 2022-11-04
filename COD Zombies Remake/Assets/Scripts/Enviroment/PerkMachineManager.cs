using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkMachineManager : MonoBehaviour
{
    private EnviromentManager envManager;
    public void Setup(EnviromentManager _envManager)
    {
        envManager = _envManager;
    }

    public enum PERK_MACHINE_TYPE
    {
        JUGGERNOG,
        SPEED_COLA,
        DOUBLE_TAP,
        QUICK_REVIVE,
        PHD_FLOPPER,
        STAMIN_UP,
        DEADSHOT,
        MULE_KICK,
        TOMBSTONE,
        WHOS_WHO,
        ELECTRIC_CHERRY,
        VULTURE_AID,
        WIDOWS_WINE
    }
}
