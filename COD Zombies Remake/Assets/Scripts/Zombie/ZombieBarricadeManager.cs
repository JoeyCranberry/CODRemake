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

    private GameObject leftArm;
    private GameObject rightArm;

    private bool inBarricadeActionArea = false;

    [SerializeField]
    private AttackBarricadeState curState = AttackBarricadeState.NOT_IN_ACTION_ZONE;

    public bool PrintToDebug = false;
    [SerializeField]
    private int planksDestroyed = 0;

    private void Start()
    {
        leftArm = transform.Find("LeftArm").gameObject;
        rightArm = transform.Find("RightArm").gameObject;
    }

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
            barricade.PlankMadeAvailable.AddListener(ActiveBarricadePlankMadeAvailable);
            activeBarricade = barricade;
            curState = AttackBarricadeState.ARRIVED;

            if (PrintToDebug)
                Debug.Log("Reached barricade for " + barricade.gameObject.name);
        }
    }

    public void ExitedBarricadeActionArea(WindowBarricade barricade)
    {
        barricade.PlankMadeAvailable.RemoveListener(ActiveBarricadePlankMadeAvailable);
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

                                activePlank = activeBarricade.ClaimUnDestroyedPlank();
                                if(activePlank != null)
                                {
                                    curStartAttackCooldown = StartAttackTime;
                                    RotateArms();
                                    curState = AttackBarricadeState.STARTING_ATTACK;
                                }
                            }
                            else
                            {
                                if (PrintToDebug)
                                    Debug.Log("Barricade has undestroyed planks, but they are all claimed.");

                                activeBarricade.PlankMadeAvailable.AddListener(CheckBarricadeDestroyedWhileWaiting);
                                curState = AttackBarricadeState.WAITING;
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

                    planksDestroyed++;
                    UnRotateArms();
                    curState = AttackBarricadeState.ARRIVED;
                }
                break;
            case AttackBarricadeState.BARRICADE_DESTROYED:
                return DestroyActionState.BARICADE_DESTROYED;
        }

        return DestroyActionState.ACTIVELY_DESTROYING;
    }

    private void ActiveBarricadePlankMadeAvailable()
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

    public void UnclaimActivePlank()
    {
        if(!activePlank.isDestroyed)
        {
            activePlank.UnClaim();
        }
    }

    private void RotateArms()
    {
        leftArm.transform.rotation = Quaternion.Euler(new Vector3(-32f, leftArm.transform.rotation.eulerAngles.y, leftArm.transform.rotation.eulerAngles.z));
        rightArm.transform.rotation = Quaternion.Euler(new Vector3(-32f, rightArm.transform.rotation.eulerAngles.y, rightArm.transform.rotation.eulerAngles.z));
    }

    private void UnRotateArms()
    {
        leftArm.transform.rotation = Quaternion.Euler(new Vector3(0f, leftArm.transform.rotation.eulerAngles.y, leftArm.transform.rotation.eulerAngles.z));
        rightArm.transform.rotation = Quaternion.Euler(new Vector3(0f, rightArm.transform.rotation.eulerAngles.y, rightArm.transform.rotation.eulerAngles.z));
    }

    private enum AttackBarricadeState
    { 
        NOT_IN_ACTION_ZONE,
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
