using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockageManager : MonoBehaviour
{
    private EnviromentManager envManager;
    public void Setup(EnviromentManager _envManager)
    {
        envManager = _envManager;
    }
}
