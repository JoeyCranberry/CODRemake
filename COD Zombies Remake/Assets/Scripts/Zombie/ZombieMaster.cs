using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMaster : MonoBehaviour
{
    public List<ZombieManager> zombieManagers;

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
}
