using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    private EnviromentManager envManager;

    private WindowBarricadeManager windowBarricadeManager;
    private BlockageManager blockageManager;

    private void Start()
    {
        windowBarricadeManager = gameObject.GetComponent<WindowBarricadeManager>();
        blockageManager = gameObject.GetComponent<BlockageManager>();
    }

    public void Setup(EnviromentManager _envManager)
    {
        envManager = _envManager;
        windowBarricadeManager.Setup(envManager);
        blockageManager.Setup(envManager);
    }

    public Transform GetClosestWindowBarricade(Transform requestor)
    {
        return windowBarricadeManager.GetClosestWindowBarricade(requestor);
    }
}
