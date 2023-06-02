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
    [SerializeField] private int attackDamage;
    [SerializeField] private float idleDuration;

    private float time = 0.0f;
    private Action currentAction = Action.Null;
    private Action promptedAction = Action.Run;
    
    private GameObject childObjectAnimation;
    private Animator animator;
    private HealthBar enemyHealthBar;
    
    private bool isDoneIdling = false;

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
            if(promptedAction != Action.Idle)
            {
                promptedAction = Action.Idle;
            }
        }
    }
    
    // Utils
    public void OnAnimationEvent(string eventData)
    {
        if (eventData.Equals("Hit")) enemyHealthBar.DecreaseHealth(attackDamage);
        else if (eventData.Equals("IdleStart")) promptedAction = Action.Idle;
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