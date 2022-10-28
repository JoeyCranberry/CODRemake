using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBarricadeBuilder : MonoBehaviour
{
    private PlayerManager playerManager;

    public PlayerBarrierBuilderPrompt buildPrompt;

    private WindowBarricade activeBarricade;

    public float BuildTimePerPlank = 5f;
    private bool isBuilding = false;
    private float curBuildTime;

    private void Start()
    {
        buildPrompt.SetProgressBuildMax(BuildTimePerPlank);
    }

    public void Setup(PlayerManager _playerManager)
    {
        playerManager = _playerManager;
    }

    private void Update()
    {
        if(isBuilding)
        {
            curBuildTime -= Time.deltaTime;

            buildPrompt.UpdateBuildProgress(BuildTimePerPlank - curBuildTime);

            if (curBuildTime <= 0d)
            {
                CreatePlank();
            }
        }
    }

    public void EnteredBarricadeActionArea(WindowBarricade barricade)
    {
        barricade.PlankCreated.AddListener(ActiveBarricadePlankRemoved);
        activeBarricade = barricade;
        CheckDisplayBuildPrompt(true, barricade);
    }

    public void ExitedBarricadeActionArea(WindowBarricade barricade)
    {
        barricade.PlankCreated.RemoveListener(ActiveBarricadePlankRemoved);
        activeBarricade = null;
        CheckDisplayBuildPrompt(false, barricade);

        isBuilding = false;
        buildPrompt.HideBuildProgress();
    }

    public void CheckDisplayBuildPrompt(bool isWithinArea, WindowBarricade barricade)
    {
        if(isWithinArea && barricade.BarricadeHasDestroyedPlanks())
        {
            buildPrompt.ShowPrompt();
        }
        else
        {
            buildPrompt.HidePrompt();
            buildPrompt.HideBuildProgress();
        }
    }    

    private void ActiveBarricadePlankRemoved()
    {
        if(activeBarricade != null && activeBarricade.BarricadeHasDestroyedPlanks())
        {
            buildPrompt.ShowPrompt();
        }
    }

    public void StartBuilding()
    {
        isBuilding = true;
        curBuildTime = BuildTimePerPlank;

        buildPrompt.ShowBuildProgress();
    }

    private void CreatePlank()
    {
        curBuildTime = BuildTimePerPlank;

        if(activeBarricade != null && activeBarricade.BarricadeHasDestroyedPlanks())
        {
            activeBarricade.CreatePlank();

            CheckDisplayBuildPrompt(true, activeBarricade);
        }
    }

    public void StopBuilding()
    {
        isBuilding = false;

        buildPrompt.HideBuildProgress();
    }
}
