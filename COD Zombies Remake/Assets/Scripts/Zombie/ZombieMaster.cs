using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMaster : MonoBehaviour
{
    public List<ZombieManager> zombieManagers;

    private EnviromentManager envManager;

    // For now, simple just grab all the zombie manangers from children - will later be handled by wave mananger
    private void Start()
    {
        zombieManagers.AddRange(gameObject.GetComponentsInChildren<ZombieManager>());

        // Give all zombie managers a reference to the master
        foreach(var manager in zombieManagers)
        {
            manager.Setup(this);
        }
    }
   
    public void Setup(EnviromentManager _envManager)
    {
        envManager = _envManager;
    }

    public Transform GetClosestWindow(Transform zombieRequestor)
    {
        return envManager.GetClosestWindowBarricade(zombieRequestor);
    }

    public Transform GetPlayerTransform()
    {
        return envManager.GetPlayerTransform();
    }
}
