using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMaster : MonoBehaviour
{
    public WaveManager WaveManager;

    public GameObject ZombiePrefab;

    private EnviromentManager envManager;

    private List<ZombieManager> zManagers;

    // For now, simple just grab all the zombie manangers from children - will later be handled by wave mananger
    private void Awake()
    {
        zManagers = new List<ZombieManager>();
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

    public PlayerManager GetPlayerManager()
    {
        return envManager.GetPlayerManager();
    }

    public void SpawnZombieAtPosition(Vector3 position, float additionalHealth)
    {
        GameObject newZombie = Instantiate(ZombiePrefab, position, Quaternion.identity, gameObject.transform);

        ZombieManager zManager = newZombie.GetComponent<ZombieManager>();

        if (zManager != null)
        {
            zManager.Setup(this, additionalHealth);
            zManagers.Add(zManager);
        }
    }

    public void ZombieKilled(ZombieManager manager)
    {
        zManagers.Remove(manager);

        if(zManagers.Count <= 0)
        {
            WaveManager.EndRound();
        }
    }
}
