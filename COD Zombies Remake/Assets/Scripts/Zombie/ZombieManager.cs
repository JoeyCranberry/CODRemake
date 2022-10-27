using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    private ZombieMaster zMaster;

    private ZombieHealthManager zHealthManager;
    private ZombieController zController;

    public void Setup(ZombieMaster _zMaster)
    {
        zMaster = _zMaster;
    }

    private void Start()
    {
        zHealthManager = gameObject.GetComponent<ZombieHealthManager>();
        zController = gameObject.GetComponent<ZombieController>();

        SetupChildrenManagers();
    }

    private void SetupChildrenManagers()
    {
        zHealthManager.Setup(this);
        zController.Setup(this);
    }
}
