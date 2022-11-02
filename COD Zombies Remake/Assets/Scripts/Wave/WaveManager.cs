using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public ZombieMaster zombieMaster;
    public PlayerManager playerManager;

    public float AdditionalHealthPerRound = 100f;

    public float TimeBetweenRounds = 10f;
    public int MaximumZombies = 24;
    private float curTimeTillNextRound;
    

    public GameObject SpawnPositionParent;
    private List<SpawnPosition> zombieSpawnPositions;

    private WaveState curState;

    private int curRound = 1;

    private static System.Random random;

    private void Start()
    {
        random = new System.Random();

        zombieSpawnPositions = new List<SpawnPosition>();
        zombieSpawnPositions.AddRange(SpawnPositionParent.GetComponentsInChildren<SpawnPosition>());

        StartRound();
    }

    private void Update()
    {
        switch(curState)
        {
            case WaveState.WAVE_STARTING:
                break;
            case WaveState.WAVE_RUNNING:
                break;
            case WaveState.WAVE_ENDED:
                curTimeTillNextRound -= Time.deltaTime;

                if(curTimeTillNextRound <= 0f)
                {
                    StartRound();
                }
                break;
        }
    }

    public void EndRound()
    {
        curTimeTillNextRound = TimeBetweenRounds;
        
        curState = WaveState.WAVE_ENDED;
        curRound++;
    }

    private void StartRound()
    {
        curState = WaveState.WAVE_STARTING;
        SpawnZombies();

        curState = WaveState.WAVE_RUNNING;
    }

    private void SpawnZombies()
    {
        int zombiesToSpawn = (int)Mathf.Min((0.000058f * Mathf.Pow(curRound, 3) + 0.074032f * Mathf.Pow(curRound, 2) + 0.718119f * curRound + 14.738699f), MaximumZombies);
        List<SpawnPosition> shuffledSpawns = RandomPermutation<SpawnPosition>(zombieSpawnPositions).ToList();

        for (int i = 0; i < Mathf.Min(zombiesToSpawn, shuffledSpawns.Count); i++)
        {
            zombieMaster.SpawnZombieAtPosition(shuffledSpawns[i].transform.position, AdditionalHealthPerRound * curRound);
        }
    }

    public static IEnumerable<T> RandomPermutation<T>(IEnumerable<T> sequence)
    {
        T[] retArray = sequence.ToArray();

        for (int i = 0; i < retArray.Length - 1; i += 1)
        {
            int swapIndex = random.Next(i, retArray.Length);
            if (swapIndex != i)
            {
                T temp = retArray[i];
                retArray[i] = retArray[swapIndex];
                retArray[swapIndex] = temp;
            }
        }

        return retArray;
    }

    public enum WaveState
    { 
        WAVE_STARTING,
        WAVE_RUNNING,
        WAVE_ENDED
    }

}
