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

    [SerializeField]
    private AttackBarricadeState curState;

    public void Setup(ZombieManager _zManager)
    {
        zManager = _zManager;
    }

    public void EnteredBarricadeActionArea(WindowBarricade barricade)
    {
        barricade.PlankCreated.AddListener(ActiveBarricadePlankAdded);
        activeBarricade = barricade;
        curState = AttackBarricadeState.ARRIVED;
    }

    public void ExitedBarricadeActionArea(WindowBarricade barricade)
    {
        barricade.PlankCreated.RemoveListener(ActiveBarricadePlankAdded);
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
                        return DestroyActionState.ATTACKING_PLAYER;
                    }
                    else
                    {
                        // If there is an undestroyed, unclaimed plank, claim it and start to destroy it
                        if (activeBarricade.BarricadeHasUnDestroyedPlanks())
                        {
                            if (activeBarricade.BarricadeHasUnclaimedPlanks())
                            {
                                curState = AttackBarricadeState.STARTING_ATTACK;
                                activePlank = activeBarricade.ClaimUnDestroyedPlank();
                                curStartAttackCooldown = StartAttackTime;
                            }
                            else
                            {
                                curState = AttackBarricadeState.WAITING;
                                activeBarricade.PlankCreated.AddListener(CheckBarricadeDestroyedWhileWaiting);
                            }
                        }
                        else
                        {
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
