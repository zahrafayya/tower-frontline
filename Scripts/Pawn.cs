using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public enum Action
    {
        Null,
        Run,
        Attack,
        Idle
    }
    
    [SerializeField] private float speed;
    [SerializeField] private float attackTime;
    [SerializeField] private int attackDamage;
    [SerializeField] private float idleDuration;

    private float time = 0.0f;
    private Action currentAction = Action.Null;
    private Action promptedAction = Action.Run;
    
    private GameObject childObjectAnimation;
    private Animator animator;
    private HealthBar enemyHealthBar;

    private bool canDecreaseHealth = false; // Flag to control the delay

    // Animation Controller
    
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
            transform.Translate(Vector3.right * moveAmount);
        }
    }

    private void Attack()
    {
        SetAnimationAttack();

        if (currentAction != Action.Attack)
        {
            currentAction = Action.Attack;
            StartCoroutine(ResetDecreaseHealthFlag());
        }
        else if (!enemyHealthBar) // musuh sudah mati
        {
            promptedAction = Action.Run;
        }
        else
        {
            if (canDecreaseHealth)
            {

                canDecreaseHealth = false;
                StartCoroutine(ResetDecreaseHealthFlag());
            }

            // promptedAction = Action.Idle;
        }
    }

    private void Idle()
    {
        SetAnimationIdle();
        if (currentAction != Action.Idle)
        {
            currentAction = Action.Idle;
        }

        StartCoroutine(IdleWait());
    }

    void Start()
    {
        childObjectAnimation = transform.Find("Animation").gameObject;
        animator = childObjectAnimation.GetComponent<Animator>();
    }
    
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
            Idle();
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemyHealthBar = collision.gameObject.GetComponent<HealthBar>();
            if(promptedAction != Action.Attack)
            {
                promptedAction = Action.Attack;
            }
        }
    }
    
    // Utils
    public void DecreaseEnemyHealthBar()
    {
        if (canDecreaseHealth)
        {
            enemyHealthBar.DecreaseHealth(attackDamage);
        }
    }

    private IEnumerator ResetDecreaseHealthFlag()
    {
        yield return new WaitForSeconds(attackTime); // Wait for half a second
        canDecreaseHealth = true;
        DecreaseEnemyHealthBar();// Set the flag to true
    }
    
    private IEnumerator IdleWait()
    {
        yield return new WaitForSeconds(idleDuration); // Wait for half a second
        promptedAction = Action.Attack;
    }
}