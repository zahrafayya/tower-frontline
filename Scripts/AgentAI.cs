using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AgentAI : Agent
{
    [SerializeField] private Scoring enemyScore;
    [SerializeField] private Scoring playerScore;
    [SerializeField] private HealthBar enemyHealthObject;
    [SerializeField] private HealthBar playerHealthObject;

    private Spawner spawner;
    private int receivedAction = 0;
    private int currentAction = 0;
    private int enemyScoreValue;
    private int playerScoreValue;
    private int coin;
    private int enemyHealthValue;
    private int playerHealthValue;

    private void Start()
    {
        spawner = gameObject.GetComponent<Spawner>();
        coin = spawner.coinValue;
    }

    private void Update()
    {
        enemyScoreValue = enemyScore.score;
        playerScoreValue = playerScore.score;

        enemyHealthValue = enemyHealthObject.currentHealth;
        playerHealthValue = playerHealthObject.currentHealth;
    }


    public override void OnEpisodeBegin()
    {
        playerHealthObject.currentHealth = 100;
        enemyHealthObject.currentHealth = 100;

        spawner.coinValue = 10;
        spawner.maxCoinValue = 10;

        enemyScore.score = 0;
        playerScore.score = 0;
        
        Pawn[] pawnComponents = FindObjectsOfType<Pawn>();
        
        foreach (Pawn pawnComponent in pawnComponents)
        {
            if (pawnComponent.objectType == Pawn.ObjectType.Pawn)
            {
                Destroy(pawnComponent.gameObject);
            }
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(enemyScoreValue);
        sensor.AddObservation(playerScoreValue);
        sensor.AddObservation(coin);
        sensor.AddObservation(playerHealthValue);
        sensor.AddObservation(enemyHealthValue);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        receivedAction = actions.DiscreteActions[0];
        
        if (currentAction != receivedAction)
        {
            if (receivedAction == 1)
            {
                spawner.SpawnWarrior();
            }

            else if (receivedAction == 2)
            {
                spawner.SpawnKnight();
            }
        
            else if (receivedAction == 3)
            {
                spawner.SpawnKing();
            }

            currentAction = receivedAction;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
    
        if (Input.GetKeyDown(KeyCode.Q))
        {
            discreteActions[0] = 1;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            discreteActions[0] = 2;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            discreteActions[0] = 3;
        }
        else
        {
            discreteActions[0] = 0; // Set to default value if no key is pressed
        }
    }

    public void KillReward()
    {
        SetReward(+1f);
    }

    public void DeathPenalty()
    {
        SetReward(-1f);
    }

    public void LosePenalty()
    {
        SetReward(-25f);
        // EndEpisode();
    }

    public void WinReward()
    {
        SetReward(+25f);
        // EndEpisode();
    }
}
