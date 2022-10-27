using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    private PlayerManager playerManager;
    public void Setup(PlayerManager _playerManager)
    {
        playerManager = _playerManager;
    }
}
