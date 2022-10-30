using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerController controller;
    private PlayerHealthManager healthManager;
    private PlayerMoneyManager moneyManager;
    private PlayerBarricadeBuilder barricadeBuilder;

    private bool inBarricadeActionArea = false;

    private bool healing = false;

    private Transform playerBody;

    private void Start()
    {
        // Grab other manangers from game object
        controller = gameObject.GetComponent<PlayerController>();
        healthManager = gameObject.GetComponent<PlayerHealthManager>();
        moneyManager = gameObject.GetComponent<PlayerMoneyManager>();
        barricadeBuilder = gameObject.GetComponent<PlayerBarricadeBuilder>();

        SetupChildrenManagers();

        playerBody = gameObject.transform;
    }

    private void SetupChildrenManagers()
    {
        controller.Setup(this);
        healthManager.Setup(this);
        moneyManager.Setup(this);
        barricadeBuilder.Setup(this);
    }


    private void Update()
    {
        if(inBarricadeActionArea)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                barricadeBuilder.StartBuilding();
            }

            if(Input.GetKeyUp(KeyCode.E))
            {
                barricadeBuilder.StopBuilding();
            }
        }

        if(healing)
        {
            healing = !healthManager.HealOverTime();
        }
    }
    
    public void EnteredBarricadeActionArea(WindowBarricade barricade)
    {
        barricadeBuilder.EnteredBarricadeActionArea(barricade);
        inBarricadeActionArea = true;
    }

    public void ExitedBarricadeActionArea(WindowBarricade barricade)
    {
        barricadeBuilder.ExitedBarricadeActionArea(barricade);
        inBarricadeActionArea = false;
    }

    public void TakeDamage(float amount)
    {
        healthManager.TakeDamage(amount);
        healing = true;
    }
}
