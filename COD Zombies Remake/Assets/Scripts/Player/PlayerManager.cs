using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerController controller;
    private PlayerHealthManager healthManager;
    private PlayerMoneyManager moneyManager;

    private Transform playerBody;

    private void Start()
    {
        // Grab other manangers from game object
        controller = gameObject.GetComponent<PlayerController>();
        healthManager = gameObject.GetComponent<PlayerHealthManager>();
        moneyManager = gameObject.GetComponent<PlayerMoneyManager>();

        SetupChildrenManagers();

        playerBody = gameObject.transform;
    }

    private void SetupChildrenManagers()
    {
        controller.Setup(this);
        healthManager.Setup(this);
        moneyManager.Setup(this);
    }
}
