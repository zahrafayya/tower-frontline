using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public enum Type
    {
        Enemy,
        Ally,
    }
    
    [SerializeField] public GameObject coinUI;
    [SerializeField] public GameObject maxCoinUI;
    [SerializeField] private GameObject knight;
    [SerializeField] private GameObject warrior;
    [SerializeField] private GameObject king;
    
    private TextMeshProUGUI coinText;
    private TextMeshProUGUI maxCoinText;
    private Type type;
    public int coinValue = 10;
    private int maxCoinValue = 10;
    private float timer = 0f;
    private float incrementInterval = 1.5f;
    void Start()
    {
        coinText = coinUI.GetComponent<TextMeshProUGUI>();
        maxCoinText = maxCoinUI.GetComponent<TextMeshProUGUI>();
        
        if (gameObject.CompareTag("Ally")) type = Type.Ally;
        else if (gameObject.CompareTag("Enemy")) type = Type.Enemy;
    }

    void Update()
    {
        
        timer += Time.deltaTime;

        if (timer >= incrementInterval)
        {
            if (coinValue < maxCoinValue)
            {
                coinValue++;
                timer = 0f;
            }
        }
        
        if (coinText)
        {
            coinText.text = coinValue.ToString();   
        }
    }

    public void IncreaseMaxCoin(int maxCoin)
    {
        maxCoinValue = maxCoin;
        
        if (maxCoinText)
        {
            maxCoinText.text = maxCoinValue.ToString();   
        }
    }
    
    public void SpawnKnight()
    {
        if (coinValue >= 7)
        {   
            GameObject newObject = Instantiate(knight, transform.position, transform.rotation);

            if (type == Type.Ally) newObject.tag = "Ally";
            else if (type == Type.Enemy) newObject.tag = "Enemy";

            coinValue -= 7;
        }
    }
    
    public void SpawnWarrior()
    {
        if (coinValue >= 5)
        {   
            GameObject newObject = Instantiate(warrior, transform.position, transform.rotation);

            if (type == Type.Ally) newObject.tag = "Ally";
            else if (type == Type.Enemy) newObject.tag = "Enemy";

            coinValue -= 5;
        }
    }
    
    public void SpawnKing()
    {
        if (coinValue >= 9)
        {   
            GameObject newObject = Instantiate(king, transform.position, transform.rotation);

            if (type == Type.Ally) newObject.tag = "Ally";
            else if (type == Type.Enemy) newObject.tag = "Enemy";

            coinValue -= 9;
        }
    }
}
