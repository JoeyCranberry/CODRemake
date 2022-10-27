using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealthManager : MonoBehaviour
{
    private ZombieManager zManager;

    public void Setup(ZombieManager _zManager)
    {
        zManager = _zManager;
    }
}
