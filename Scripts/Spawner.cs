using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public enum Type
    {
        Enemy,
        Ally,
    }
    
    private Type type;
    [SerializeField] private GameObject hero;
    void Start()
    {
        if (gameObject.CompareTag("Ally")) type = Type.Ally;
        else if (gameObject.CompareTag("Enemy")) type = Type.Enemy;
    }
    
    public void SpawnHero()
    {
        GameObject newObject = Instantiate(hero, transform.position, transform.rotation);

        if (type == Type.Ally) newObject.tag = "Ally";
        else if (type == Type.Enemy) newObject.tag = "Enemy";
    }
}
