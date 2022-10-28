using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBarrierBuilderPrompt : MonoBehaviour
{
    public GameObject BuildPrompt;
    public Slider buildProgress;

    public void ShowPrompt()
    { 
        BuildPrompt.SetActive(true);
    }

    public void HidePrompt()
    {
        BuildPrompt.SetActive(false);
    }

    public void ShowBuildProgress()
    {
        buildProgress.gameObject.SetActive(true);
    }

    public void HideBuildProgress()
    {
        buildProgress.gameObject.SetActive(false);
    }

    public void SetProgressBuildMax(float pMaxValue)
    {
        buildProgress.maxValue = pMaxValue;
    }

    public void UpdateBuildProgress(float pValue)
    {
        buildProgress.value = pValue;
    }
}
