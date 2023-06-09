using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public enum Action
    {
        Null,
        Run,
        Attack,
        Idle,
        Dead
    }
    
    public enum ObjectType
    {
        PlayerTower,
        EnemyTower,
        Pawn
    }

    public enum Faction
    {
        Ally,
        Enemy,
    }
    
    [SerializeField] private float speed;
    [SerializeField] private int attackDamage;
    [SerializeField] private float idleDuration;
    [SerializeField] public int cost;
    [SerializeField] private GameObject endGameScreen;
    [SerializeField] public GameObject scoreEndScreen;
    [SerializeField] public AgentAI playerAI;
    [SerializeField] public AgentAI enemyAI;

    private Action currentAction = Action.Null;
    private Action promptedAction = Action.Run;

    private GameObject childObjectAnimation;
    private Animator animator;
    private HealthBar enemyHealthBar;
    private ObjectType objectType;
    private Faction faction;
    private Scoring playerScore;
    private Scoring enemyScore;
    private bool isDoneIdling = false;
    private TextMeshProUGUI scoreText;

    // Animation Controller
    private void SetAnimationDead()
    {
        animator.SetBool("isAttack", false);
        animator.SetBool("isRun", false);
        animator.SetBool("isIdle", false);
        animator.SetBool("isDead", true);
    }
    
    private void SetAnimationIdle()
    {
        animator.SetBool("isAttack", false);
        animator.SetBool("isRun", false);
        animator.SetBool("isIdle", true);
    }
    private void SetAnimationRun()
    {
        animator.SetBool("isAttack", false);
        animator.SetBool("isRun", true);
        animator.SetBool("isIdle", false);
    }
    
    private void SetAnimationAttack()
    {
        animator.SetBool("isAttack", true);
        animator.SetBool("isRun", false);
        animator.SetBool("isIdle", false);
    }
    
    // Action Controller
    private void Dead()
    {
        promptedAction = Action.Dead;
        currentAction = Action.Dead;

        if (objectType == ObjectType.EnemyTower || objectType == ObjectType.PlayerTower)
        {
            if (playerAI) playerAI.WinReward();
            if (enemyAI) enemyAI.LosePenalty();
            EndGame();
        }
        else
        {
            if (faction == Faction.Enemy) playerScore.IncreaseScore();
            else if (faction == Faction.Ally) enemyScore.IncreaseScore();
            
            SetAnimationDead();
        }
    }
    private void Run()
    {
        if (currentAction != Action.Run)
        {
            SetAnimationRun();
            currentAction = Action.Run;
        }
        
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(("Run")))
        {
            float moveAmount = speed * Time.deltaTime;
            
            if (faction == Faction.Ally)
                transform.Translate(Vector3.right * moveAmount);
            else if (faction == Faction.Enemy)
                transform.Translate(Vector3.left * moveAmount);
        }
    }

    private void Attack()
    {
        if (currentAction != Action.Attack)
        {
            StartCoroutine(AttackWait());
        }
        else if (!enemyHealthBar) // musuh sudah mati
        {
            promptedAction = Action.Run;
        }
    }

    private void Idle()
    {
        SetAnimationIdle();
        
        if (currentAction != Action.Idle)
        {
            currentAction = Action.Idle;
            StartCoroutine(IdleWait());
        }

        if (isDoneIdling)
        {
            promptedAction = Action.Attack;
            isDoneIdling = false;
        }
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        if (gameObject.name.Equals("Enemy Tower")) objectType = ObjectType.EnemyTower;
        else if (gameObject.name.Equals("Player Tower")) objectType = ObjectType.PlayerTower;
        else objectType = ObjectType.Pawn;

        if (gameObject.tag.Equals("Ally")) faction = Faction.Ally;
        else if (gameObject.tag.Equals("Enemy")) 
        {
            faction = Faction.Enemy;
            
            Vector3 reverseScale = gameObject.transform.localScale;
            reverseScale.x = reverseScale.x * -1;
            gameObject.transform.localScale = reverseScale;
        }
        
        Scoring[] scoringComponents = FindObjectsOfType<Scoring>();

        foreach (Scoring scoringComponent in scoringComponents)
        {
            if (scoringComponent.gameObject.CompareTag("Ally"))
            {
                playerScore = scoringComponent;
            }
            else enemyScore = scoringComponent;
        }
        
        if (scoreEndScreen)
            scoreText = scoreEndScreen.GetComponent<TextMeshProUGUI>();
    }
    
    void Update()
    {
        if(promptedAction == Action.Run)
        {
            Run();
        }
        else if (promptedAction == Action.Attack)
        {
            Attack();
        }
        else if (promptedAction == Action.Idle)
        {
            if (enemyHealthBar) Idle();
            else promptedAction = Action.Run;
        }
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        string friend;
        string opponent;

        if (faction == Faction.Ally)
        {
            friend = "Ally";
            opponent = "Enemy";
        }
        else
        {
            friend = "Enemy";
            opponent = "Ally";
        }
        
        if (collision.gameObject.CompareTag(opponent))
        {
            enemyHealthBar = collision.gameObject.GetComponent<HealthBar>();
            if (promptedAction != Action.Idle && enemyHealthBar)
            {
                promptedAction = Action.Idle;
            }
        }
        else if (collision.gameObject.CompareTag(friend))
        {
            Collider2D thisCollider = GetComponent<Collider2D>();
            Collider2D otherCollider = collision.gameObject.GetComponent<Collider2D>();
            
            Physics2D.IgnoreCollision(thisCollider, otherCollider);
        }
    }
    
    // Utils
    public void EndGame()
    {
        Time.timeScale = 0f;
        
        scoreText.text = $"Total Score: {playerScore.score}";
        
        Debug.Log(scoreText.text);

        endGameScreen.SetActive(true);
    }
    public void OnAnimationEvent(string eventData)
    {
        if (eventData.Equals("Hit")) enemyHealthBar.DecreaseHealth(attackDamage);
        else if (eventData.Equals("IdleStart")) promptedAction = Action.Idle;
        else if (eventData.Equals("Disappear"))
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
    
    public void OnDeadEvent()
    {
        Dead();
    }
    private IEnumerator IdleWait()
    {
        yield return new WaitForSeconds(idleDuration);
        isDoneIdling = true;
    }

    private IEnumerator AttackWait()
    {
        yield return new WaitForSeconds(0.4f);
        currentAction = Action.Attack;
        SetAnimationAttack();
    }
}