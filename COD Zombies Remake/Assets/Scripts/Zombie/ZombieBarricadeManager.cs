using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBarricadeManager : MonoBehaviour
{
    private ZombieManager zManager;

    public double StartAttackTime = 2d;
    public double AttackCooldown = 5d;

    private double curStartAttackCooldown;
    private double curAttackCooldown;

    private WindowBarricade activeBarricade;
    private WindowBarricadePlank activePlank;

    private bool inBarricadeActionArea = false;

    [SerializeField]
    private AttackBarricadeState curState;

    public bool PrintToDebug = false;

    public void Setup(ZombieManager _zManager)
    {
        zManager = _zManager;
    }

    public void EnteredBarricadeActionArea(WindowBarricade barricade)
    {
        // Fixes issue where zombies can trigger the action area multiple times resetting their state
        if(!inBarricadeActionArea)
        {
            inBarricadeActionArea = true;
            barricade.PlankCreated.AddListener(ActiveBarricadePlankAdded);
            activeBarricade = barricade;
            curState = AttackBarricadeState.ARRIVED;

            if (PrintToDebug)
                Debug.Log("Reached barricade for " + barricade.gameObject.name);
        }
    }

    public void ExitedBarricadeActionArea(WindowBarricade barricade)
    {
        barricade.PlankCreated.RemoveListener(ActiveBarricadePlankAdded);
        inBarricadeActionArea = false;
    }

    /*
     * Returns true when the barricade is fully destroyed
     */
    public DestroyActionState DestroyBarricade()
    {
        switch(curState)
        {
            case AttackBarricadeState.ARRIVED:
                if (activeBarricade != null)
                {
                    if (activeBarricade.playerInActionArea)
                    {
                        if (PrintToDebug)
                            Debug.Log("Player is in action area, attacking ");
                        return DestroyActionState.ATTACKING_PLAYER;
                    }
                    else
                    {
                        // If there is an undestroyed, unclaimed plank, claim it and start to destroy it
                        if (activeBarricade.BarricadeHasUnDestroyedPlanks())
                        {
                            if (activeBarricade.BarricadeHasUnclaimedUnDestroyedPlanks())
                            {
                                if (PrintToDebug)
                                    Debug.Log("Barricade has undestroyed and undestroyed planks, claiming one.");
                                curState = AttackBarricadeState.STARTING_ATTACK;
                                activePlank = activeBarricade.ClaimUnDestroyedPlank();
                                curStartAttackCooldown = StartAttackTime;
                            }
                            else
                            {
                                if (PrintToDebug)
                                    Debug.Log("Barricade has undestroyed planks, but they are all claimed.");
                                curState = AttackBarricadeState.WAITING;
                                activeBarricade.PlankCreated.AddListener(CheckBarricadeDestroyedWhileWaiting);
                            }
                        }
                        else
                        {
                            if (PrintToDebug)
                                Debug.Log("Barricade does not have any undestroyed planks - all done.");
                            curState = AttackBarricadeState.BARRICADE_DESTROYED;
                            return DestroyActionState.BARICADE_DESTROYED;
                        }
                    }
                }
                break;
            case AttackBarricadeState.STARTING_ATTACK:
                if (curStartAttackCooldown > 0d)
                {
                    curStartAttackCooldown -= Time.deltaTime;
                }
                else
                {
                    if (PrintToDebug)
                        Debug.Log("Start time elapsed, destroying plank.");
                    curState = AttackBarricadeState.DESTROY_BARRICADE_PLANK;
                }
                break;
            case AttackBarricadeState.WAITING:
                return DestroyActionState.WAITING;
            case AttackBarricadeState.DESTROY_BARRICADE_PLANK:
                activeBarricade.DestroyPlank(activePlank);
                curState = AttackBarricadeState.ATTACK_COOLDOWN;
                curAttackCooldown = AttackCooldown;
                break;
            case AttackBarricadeState.ATTACK_COOLDOWN:
                if (curAttackCooldown > 0d)
                {
                    curAttackCooldown -= Time.deltaTime;
                }
                else
                {
                    if (PrintToDebug)
                        Debug.Log("Finished post-destroy wait, setting state to arrived.");
                    curState = AttackBarricadeState.ARRIVED;
                }
                break;
            case AttackBarricadeState.BARRICADE_DESTROYED:
                return DestroyActionState.BARICADE_DESTROYED;
        }

        return DestroyActionState.ACTIVELY_DESTROYING;
    }

    private void ActiveBarricadePlankAdded()
    {
        if(curState == AttackBarricadeState.WAITING)
        {
            curState = AttackBarricadeState.ARRIVED;
        }
    }

    // Called when waiting and a plank has been destroyed
    private void CheckBarricadeDestroyedWhileWaiting()
    {
        curState = AttackBarricadeState.ARRIVED;
    }

    private enum AttackBarricadeState
    { 
        ARRIVED,
        STARTING_ATTACK,
        DESTROY_BARRICADE_PLANK,
        ATTACK_COOLDOWN,
        WAITING,
        BARRICADE_DESTROYED
    }

    public enum DestroyActionState
    {
        ACTIVELY_DESTROYING,
        ATTACKING_PLAYER,
        WAITING,
        BARICADE_DESTROYED
    }
}
