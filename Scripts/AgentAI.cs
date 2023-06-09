using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AgentAI : Agent
{
    [SerializeField] private GameObject enemyScoreObject;
    [SerializeField] private GameObject playerScoreObject;
    [SerializeField] private GameObject enemyHealthObject;
    [SerializeField] private GameObject playerHealthObject;

    private Spawner spawner;
    private int receivedAction = 0;
    private int currentAction = 0;
    private int enemyScore;
    private int playerScore;
    private int coin;
    private int enemyHealth;
    private int playerHealth;

    private void Start()
    {
        enemyScore = enemyScoreObject.GetComponent<Scoring>().score;
        playerScore = playerScoreObject.GetComponent<Scoring>().score;
        spawner = gameObject.GetComponent<Spawner>();
        coin = spawner.coinValue;
        
        if (playerHealthObject)
            playerHealth = playerHealthObject.GetComponent<HealthBar>().currentHealth;
        
        if (enemyHealthObject)
            enemyHealth = enemyHealthObject.GetComponent<HealthBar>().currentHealth;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(enemyScore);
        sensor.AddObservation(playerScore);
        sensor.AddObservation(coin);
        sensor.AddObservation(playerHealth);
        sensor.AddObservation(enemyHealth);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Debug.Log(actions.DiscreteActions[0]);
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
        EndEpisode();
    }

    public void WinReward()
    {
        SetReward(+25f);
        EndEpisode();
    }
}
