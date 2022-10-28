using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowBarricadePlank : MonoBehaviour
{
    public bool isClaimed = false;
    public bool isDestroyed = false;

    private List<MeshRenderer> mRenderers;

    private void Start()
    {
        mRenderers = new List<MeshRenderer>();
        mRenderers.AddRange( gameObject.GetComponentsInChildren<MeshRenderer>());
    }

    public void Destroy()
    {
        foreach(MeshRenderer mRenderer in mRenderers)
        {
            mRenderer.enabled = false;
        }

        isDestroyed = true;
    }

    public void Create()
    {
        foreach (MeshRenderer mRenderer in mRenderers)
        {
            mRenderer.enabled = true;
        }

        isDestroyed = false;
    }

    public void Claim()
    {
        isClaimed = true;
    }

    public void UnClaim()
    {
        isClaimed = false;
    }
}
