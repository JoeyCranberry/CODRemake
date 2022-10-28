using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentManager : MonoBehaviour
{
    public PlayerManager playerManager;

    private ZombieDropsManager zombieDrops;
    private WallWeaponManager wallWeaponManager;
    private PerkMachineManager perkMachineManager;
    private ObstacleManager obstacleManager;
    private MysteryBoxManager mysteryBoxManager;

    private ZombieMaster zombieMaster;

    private void Start()
    {
        zombieDrops = gameObject.GetComponent<ZombieDropsManager>();
        wallWeaponManager = gameObject.GetComponent<WallWeaponManager>();
        perkMachineManager = gameObject.GetComponent<PerkMachineManager>();
        obstacleManager = gameObject.GetComponent<ObstacleManager>();
        mysteryBoxManager = gameObject.GetComponent<MysteryBoxManager>();

        zombieMaster = gameObject.GetComponentInChildren<ZombieMaster>();

        SetupManagers();
    }

    private void SetupManagers()
    {
        zombieDrops.Setup(this);
        wallWeaponManager.Setup(this);
        perkMachineManager.Setup(this);
        obstacleManager.Setup(this);
        mysteryBoxManager.Setup(this);

        zombieMaster.Setup(this);
    }

    public Transform GetPlayerTransform()
    {
        return playerManager.gameObject.transform;
    }

    public Transform GetClosestWindowBarricade(Transform requestor)
    {
        return obstacleManager.GetClosestWindowBarricade(requestor);
    }
}
