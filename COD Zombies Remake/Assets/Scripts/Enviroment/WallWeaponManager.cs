using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWeaponManager : MonoBehaviour
{
    private EnviromentManager envManager;
    public void Setup(EnviromentManager _envManager)
    {
        envManager = _envManager;
    }
}
