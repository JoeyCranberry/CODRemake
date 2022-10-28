using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    private ZombieMaster zMaster;

    private ZombieHealthManager zHealthManager;
    private ZombieController zController;
    private ZombieBarricadeManager zBarricadeManager;

    [SerializeField]
    private ZombieState zombieState = ZombieState.SPAWNED;

    public void Setup(ZombieMaster _zMaster)
    {
        zMaster = _zMaster;
    }

    private void Start()
    {
        zHealthManager = gameObject.GetComponent<ZombieHealthManager>();
        zController = gameObject.GetComponent<ZombieController>();
        zBarricadeManager = gameObject.GetComponent<ZombieBarricadeManager>();

        SetupChildrenManagers();
    }

    private void Update()
    {
        switch(zombieState)
        {
            // If the zombie has just spawned, request the closest window, and pathfind to it
            case ZombieState.SPAWNED:
                Transform closestWindow = zMaster.GetClosestWindow(transform);
                zController.PathfindToPosition(closestWindow.position);
                zombieState = ZombieState.PATHFINDING_TO_WINDOW;
                break;
            // Will pathfind to window until it's reached the action area
            case ZombieState.PATHFINDING_TO_WINDOW:
                break;
            // Triggered once entered action area trigger
            case ZombieState.BREAKING_WINDOW_BARRICADE:
                if(zBarricadeManager.DestroyBarricade())
                {
                    zController.SetPlayerTransform(zMaster.GetPlayerTransform());
                    zController.PathfindAtPlayer();
                    zController.StartPathfinding();
                    // TODO: Add traverse window state
                    zombieState = ZombieState.PATHFINDING_TO_PLAYER;
                }
                break;
            case ZombieState.PATHFINDING_TO_PLAYER:
                // If within attack distance
                if(zController.PathfindAtPlayer())
                {
                    zombieState = ZombieState.ATTACKING_PLAYER;
                }
                break;
            case ZombieState.ATTACKING_PLAYER:
                break;
        }
    }

    private void SetupChildrenManagers()
    {
        zHealthManager.Setup(this);
        zController.Setup(this);
        zBarricadeManager.Setup(this);
    }

    public void EnteredBarricadeActionArea(WindowBarricade barricade)
    {
        zController.StopPathfinding();

        zombieState = ZombieState.BREAKING_WINDOW_BARRICADE;

        zBarricadeManager.EnteredBarricadeActionArea(barricade);
    }

    public void ExitedBarricadeActionArea(WindowBarricade barricade)
    {
        zBarricadeManager.ExitedBarricadeActionArea(barricade);
    }

    private enum ZombieState
    { 
        SPAWNED,
        PATHFINDING_TO_WINDOW,
        BREAKING_WINDOW_BARRICADE,
        TRAVERSING_WINDOW,
        ATTACKING_PLAYER,
        PATHFINDING_TO_PLAYER
    }

}
