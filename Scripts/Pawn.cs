using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int idleDuration;
    
    private GameObject childObjectAnimation;
    private Animator animator;

    private GameObject childObjectHealth;
    
    private String currentAction = "";
    private String promptedAction = "Run";
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
        if (!currentAction.Equals("Attack"))
        {
            SetAnimationAttack();
            currentAction = "Attack";
        }
    }

    void Start()
    {
        childObjectAnimation = transform.Find("Animation").gameObject;
        animator = childObjectAnimation.GetComponent<Animator>();
        
        childObjectHealth = transform.Find("HealthBar").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(promptedAction.Equals("Run")) Run(); 
        else if (promptedAction.Equals("Attack"))
        {
            Attack();
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            promptedAction = "Attack";
        }
    }
}
