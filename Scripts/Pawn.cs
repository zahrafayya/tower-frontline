using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int idleDuration;
    [SerializeField] private float attackRange; // New variable for attack range
    
    private GameObject childObjectAnimation;
    private Animator animator;

    private float time = 0.0f;
    public float interpolationPeriod = 0.5f;
    
    private String currentAction = "";
    private String promptedAction = "Run";

    private float targetAttackInterval = 0.5f * 60;
    private float attackInterval = 0;

    private HealthBar enemyHealthBar;

    private bool canDecreaseHealth = false; // Flag to control the delay

    public enum Action
    {
        Run,
        Attack,
        Idle
    }

    public void GetAction()
    {
        
    }

    // Animation Controller
    private void SetAnimationRun()
    {
        animator.SetBool("isAttack", false);
        animator.SetBool("isRun", true);
    }
    
    private void SetAnimationAttack()
    {
        animator.SetBool("isAttack", true);
        animator.SetBool("isRun", false);
    }
    
    // Action Controller
    private void Run()
    {
        if (!currentAction.Equals("Run"))
        {
            SetAnimationRun();
            currentAction = "Run";
        }
        

        float moveAmount = speed * Time.deltaTime;
        transform.Translate(Vector3.right * moveAmount);

    }

    private void Attack()
    {
        SetAnimationAttack();
        currentAction = "Attack";
    }

    void Start()
    {
        childObjectAnimation = transform.Find("Animation").gameObject;
        animator = childObjectAnimation.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float targetDeltaTime = 1f / 60f; // Desired time per frame for 60 FPS
        float deltaTime = Time.deltaTime;

        time += Time.deltaTime;

        if (deltaTime < targetDeltaTime)
        {
            float remainingTime = targetDeltaTime - deltaTime;
            System.Threading.Thread.Sleep((int)(remainingTime * 1000));
            deltaTime = Time.deltaTime;
        }

        if(promptedAction.Equals("Run"))
        {
            Run();

            // // Check if the distance to the enemy is less than or equal to the attack range
            // if (Vector3.Distance(transform.position, enemy.transform.position) <= attackRange)
            // {
            //     promptedAction = "Attack";
            // }
        }
        else if (promptedAction.Equals("Attack") && !currentAction.Equals("Attack"))
        {
            Attack();
        }
        else if(currentAction.Equals("Attack"))
        {

            if (time >= interpolationPeriod) {
                time = time - interpolationPeriod;
        
                if(!canDecreaseHealth)
                    canDecreaseHealth = true;
            }
            
        }
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if(!promptedAction.Equals("Attack"))
            {
                promptedAction = "Attack";
                enemyHealthBar = collision.gameObject.GetComponent<HealthBar>();
            }
        }

        if (currentAction.Equals("Attack"))
        {
            DecreaseEnemyHealthBar();
        }
    }

    public void DecreaseEnemyHealthBar()
    {
        if (canDecreaseHealth)
        {
            canDecreaseHealth = false;
            enemyHealthBar.DecreaseHealth();
        }
    }

    private IEnumerator ResetDecreaseHealthFlag()
    {
        yield return new WaitForSeconds(0.5f); // Wait for half a second
        canDecreaseHealth = true; // Set the flag to true
    }
}