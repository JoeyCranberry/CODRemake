using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    private ZombieManager zManager;

    private NavMeshAgent navAgent;

    private Transform player;

    public double AttackDistance = 1d;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void Setup(ZombieManager _zManager)
    {
        zManager = _zManager;
    }

    public void PathfindToPosition(Vector3 pathfindTo)
    {
        navAgent.SetDestination(pathfindTo);
    }

    public void SetPlayerTransform(Transform _player)
    {
        player = _player;
    }

    public bool PathfindAtPlayer()
    {
        navAgent.SetDestination(player.position);

        if(Vector3.Distance(player.position, gameObject.transform.position) <= AttackDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
   
    public void StopPathfinding()
    {
        navAgent.isStopped = true;
    }

    public void StartPathfinding()
    {
        navAgent.isStopped = false;
    }
}
