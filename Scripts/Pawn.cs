using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField] private float speed;
    private bool isRunning = true;
    private void Run()
    {
        float moveAmount = speed * Time.deltaTime;

        transform.Translate(Vector3.right * moveAmount);

    }

    void Start()
    {
          
    }

    // Update is called once per frame
    void Update()
    {
        if(isRunning) Run(); 
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // if (collision.gameObject.CompareTag("Target"))
        // {
        isRunning = false;
        // }
    }
}
