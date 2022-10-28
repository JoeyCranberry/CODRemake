using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowBarricadeManager : MonoBehaviour
{
    private EnviromentManager envManager;

    public GameObject windowParent;

    private List<WindowBarricade> barricades;

    private void Start()
    {
        barricades = new List<WindowBarricade>();
        barricades.AddRange(windowParent.GetComponentsInChildren<WindowBarricade>());
    }

    public void Setup(EnviromentManager _envManager)
    {
        envManager = _envManager;
    }

    public Transform GetClosestWindowBarricade(Transform requestor)
    {
        float shortestDistance = float.MaxValue;
        // By default if there are no barricades the closest one will be this manager's gameObject's position
        Transform closestWindowBarricade = transform;

        foreach(WindowBarricade barricade in barricades)
        {
            float distance = Vector3.Distance(barricade.transform.position, requestor.position);
            if(distance < shortestDistance)
            {
                closestWindowBarricade = barricade.transform;
                shortestDistance = distance;
            }
        }

        return closestWindowBarricade;
    }
}
