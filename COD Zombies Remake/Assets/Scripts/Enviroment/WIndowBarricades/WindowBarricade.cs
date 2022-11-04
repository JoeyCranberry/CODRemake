using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class WindowBarricade : MonoBehaviour
{
    private ObstacleManager obstacleManager;

    public GameObject PlankParent;
    [SerializeField]
    private List<WindowBarricadePlank> plankBarricades;

    public WindowBarricadeActionArea PlayerActionArea;
    public WindowBarricadeActionArea ZombieActionArea;

    // Events
    public UnityEvent PlankMadeAvailable;
    public UnityEvent PlankDestroyed;

    // Plank Count
    private int startPlankCount;
    private int curPlankCount;

    public bool playerInActionArea = false;
    private int zombiesInActionArea = 0;

    private void Start()
    {
        plankBarricades = new List<WindowBarricadePlank>();
        plankBarricades.AddRange(PlankParent.GetComponentsInChildren<WindowBarricadePlank>());
        startPlankCount = plankBarricades.Count;
        curPlankCount = startPlankCount;

        SetupActionAreas();
    }

    public void Setup(ObstacleManager _obstacleManager)
    {
        obstacleManager = _obstacleManager;
    }

    private void SetupActionAreas()
    {
        PlayerActionArea.Setup(this);
        ZombieActionArea.Setup(this);
    }

    public void RemovePlank()
    {
        if (PlankDestroyed != null)
        {
            PlankDestroyed.Invoke();
        }
    }

    public void DestroyPlank(WindowBarricadePlank targetPlank)
    {
        targetPlank.Destroy();
        curPlankCount--;

        RemovePlank();
    }

    public void CreatePlank()
    {
        WindowBarricadePlank destroyedPlank = plankBarricades.Where(p => p.isDestroyed == true).FirstOrDefault();
        destroyedPlank.Create();

        AddPlank();
    }

    public void AddPlank()
    {
        if (PlankMadeAvailable != null)
        {
            PlankMadeAvailable.Invoke();
        }
    }

    public WindowBarricadePlank ClaimUnDestroyedPlank()
    {
        int indexOf = plankBarricades.FindIndex(p => p.isClaimed == false && p.isDestroyed == false);
        if(indexOf < 0 )
        {
            return null;
        }
        else
        {
            WindowBarricadePlank unclaimedPlank = plankBarricades[indexOf];
            unclaimedPlank.Claim();

            return unclaimedPlank;
        }
    }

    public bool BarricadeHasUnDestroyedPlanks()
    {
        return plankBarricades.FindIndex(p => p.isDestroyed == false) != -1;
    }

    public bool BarricadeHasUnclaimedUnDestroyedPlanks()
    {
        return plankBarricades.FindIndex(p => p.isClaimed == false && p.isDestroyed == false) != -1;
    }

    public bool BarricadeHasDestroyedPlanks()
    {
        return plankBarricades.FindIndex(p => p.isDestroyed == true) != -1;
    }

    public void PlayerEnteredActionArea()
    {
        playerInActionArea = true;
    }

    public void PlayerExitedActionArea()
    {
        playerInActionArea = false;
    }


    // TODO: Doesn't yet handle when a zombie in the action area dies
    public void ZombieEnteredActionArea()
    {
        zombiesInActionArea++;
    }

    public void ZombieExitedActionArea()
    {
        zombiesInActionArea--;
    }

    public void UnclaimPlank(WindowBarricadePlank plank)
    {
        plank.UnClaim();
    }


    private void PrintPlanks()
    {
        foreach(WindowBarricadePlank plank in plankBarricades)
        {
            Debug.Log("Plank " + plank.gameObject.name + " Destroyed: " + plank.isDestroyed + " Claimed: " + plank.isClaimed);
        }
    }
}
