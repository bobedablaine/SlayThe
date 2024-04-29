using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

//using System.Numerics;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LogicScript : MonoBehaviour
{
    public Camera main;

    [SerializeField]
    private PlayerScript player;
    [SerializeField]
    private EnemyScript enemy;
    [SerializeField]
    private GameObject[] cardArray = new GameObject[3];
    [SerializeField]
    public List<GameObject> cardList;
    [SerializeField]
    public List<GameObject> enemyList;
    public bool playersTurn = true;
    GameObject holderGameObject;
    HolderScript holderScript;
    private CardGameState gameState;
    private CardGameAI ai;
    public List<int> aiHand;
    public List<GameObject> aiList;

    public Text playerHealth;
    public Text playerTempHP;
    public Text enemyHealth;
    public Text enemyTempHP;
    private bool showingResult = false;
    public Text gameResults;

    // Start is called before the first frame update
    void Start()
    {
        main = Camera.main;
        player = FindObjectOfType<PlayerScript>();
        enemy = FindObjectOfType<EnemyScript>();
        cardArray = Resources.LoadAll<GameObject>("Prefabs/NumberedCards");
        cardList = cardArray.ToList<GameObject>();
        enemyList = cardArray.ToList<GameObject>();
        for (int i = 0; i < cardList.Count; ++i)
        {
            Instantiate(cardList[i], new Vector3(i*4 - 8, -3f, 0f), UnityEngine.Quaternion.identity);
        }
        for (int i = 0; i < enemyList.Count; ++i)
        {
            aiList.Add(Instantiate(enemyList[i], new Vector3(i*4 - 8, 3f, 0f), UnityEngine.Quaternion.identity));
        }
        for (int i = 0; i < enemyList.Count; ++i)
        {
            SpriteRenderer temp = aiList[i].GetComponent<SpriteRenderer>();
            temp.flipY = true;
        }
        holderGameObject = GameObject.FindGameObjectWithTag("Holder");
        holderScript = holderGameObject.GetComponent<HolderScript>();
        for (int i = 1; i <= 5; ++i)
        {
            aiHand.Add(i);
        }
        gameState = new CardGameState(10, 10, 0, 0, aiHand); // Initial player and enemy health, no temporary hit points
        ai = new CardGameAI();

        showingResult = false;
        gameResults.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        

        if (!gameState.IsGameOver())
        {
            if (!playersTurn)
            {
                enemy.tempHP = 0;

                playerHealth.text = "Health: " + player.health;
                enemyHealth.text = "Health: " + enemy.health;
                playerTempHP.text = "Temp HP: " + player.tempHP;
                enemyTempHP.text = "Temp HP: " + enemy.tempHP;

                gameState.enemyHealth = enemy.health;
                gameState.playerHealth = player.health;
                gameState.enemyTempHp = enemy.tempHP;
                gameState.playerTempHp = player.tempHP;
                gameState.aiHand = aiHand;
                Card bestMove = ai.GetBestMove(gameState, 0);
                gameState = ai.SimulateMove(gameState, bestMove);
                GameObject toDestroy = enemyList[bestMove.number-1];
                Debug.Log("Chosen: " + bestMove.number);
                player.health = gameState.playerHealth;
                player.tempHP = gameState.playerTempHp;
                enemy.health = gameState.enemyHealth;
                enemy.tempHP = gameState.enemyTempHp;

                playerHealth.text = "Health: " + player.health;
                enemyHealth.text = "Health: " + enemy.health;
                playerTempHP.text = "Temp HP: " + player.tempHP;
                enemyTempHP.text = "Temp HP: " + enemy.tempHP;
                
                aiHand.Remove(bestMove.number);
                foreach (GameObject temp in aiList)
                {
                    if (temp != null)
                    {
                        numOne tempScript = temp.GetComponent<numOne>();
                        if (tempScript.number == bestMove.number)
                        {
                            Destroy(temp);
                        }
                    }
                    
                }
                playersTurn = true;
                player.tempHP = 0;
            }
        }
        else
        {
            if (!showingResult)
            {
                if (player.health > enemy.health)
                {
                    gameResults.text = "Player Wins!";
                }
                else if (player.health < enemy.health)
                {
                    gameResults.text = "AI Wins!";
                } else
                {
                    gameResults.text = "It's a Draw!";
                }
                showingResult = true;
            }
        }   

    }


    public void PlayerAttackPos()
    {
        if(enemy.tempHP > 0)
            {
                holderScript.numberHeld -= enemy.tempHP;
                if (holderScript.numberHeld > 0)
                {
                    enemy.tempHP = 0;
                    enemy.health -= holderScript.numberHeld;
                } 
                else
                    enemy.tempHP = 0;
                Destroy(holderScript.objectHeld);
            }
            else
            {
                enemy.health -= holderScript.numberHeld;
                Destroy(holderScript.objectHeld);
            }

        gameState.enemyHealth = enemy.health;
        gameState.playerHealth = player.health;
        gameState.enemyTempHp = enemy.tempHP;
        gameState.playerTempHp = player.tempHP;
        playerHealth.text = "Health: " + player.health;
        enemyHealth.text = "Health: " + enemy.health;
        playerTempHP.text = "Temp HP: " + player.tempHP;
        enemyTempHP.text = "Temp HP: " + enemy.tempHP;

        playersTurn = false;
    }

    public void PlayerDefensePos()
    {
        player.tempHP += holderScript.numberHeld;
        Destroy(holderScript.objectHeld);
        gameState.enemyHealth = enemy.health;
        gameState.playerHealth = player.health;
        gameState.enemyTempHp = enemy.tempHP;
        gameState.playerTempHp = player.tempHP;
        playerHealth.text = "Health: " + player.health;
        enemyHealth.text = "Health: " + enemy.health;
        playerTempHP.text = "Temp HP: " + player.tempHP;
        enemyTempHP.text = "Temp HP: " + enemy.tempHP;
        playersTurn = false;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("Scene1");
    }
}
