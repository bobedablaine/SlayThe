using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

// public struct AIMove
// {
//     public int card;
//     public char moveType;
// }

// public struct GameState
// {
//     public List<int> aiHand;
//     public List<int> playerHand;
//     public int aiHealth;
//     public int playerHealth;
//     public int aiTempHP;
//     public int playerTempHP;
// }

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    public int health = 10;
    [SerializeField]
    private LogicScript logic;
    public int tempHP = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        logic = FindObjectOfType<LogicScript>();   //For auto finding
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (health <= 0)
            KillSelf();
        
        

    }

    void KillSelf()
    {
        Destroy(gameObject);
        Debug.Log("Enemy dead");
    }

}

public class Card
{
    public int number;

    public Card(int number)
    {
        this.number = number;
    }
}

public class CardGameState
{
    public int playerHealth;
    public int enemyHealth;
    public int playerTempHp;
    public int enemyTempHp;
    public List<int> aiHand;

    public CardGameState(int playerHealth, int enemyHealth, int playerTempHp, int enemyTempHp, List<int> aiHand)
    {
        this.playerHealth = playerHealth;
        this.enemyHealth = enemyHealth;
        this.playerTempHp = playerTempHp;
        this.enemyTempHp = enemyTempHp;
        this.aiHand = aiHand;
    }

    public bool IsGameOver()
    {
        return playerHealth <= 0 || enemyHealth <= 0 || aiHand.Count == 0;
    }

    public void EndTurn()
    {
        playerTempHp = 0; // Remove temporary hit points at the end of the player's turn
        enemyTempHp = 0; // Remove temporary hit points at the end of the enemy's turn
    }
}

public class CardGameAI
{
    private const int MAX_DEPTH = 3;
    private const int POSITIVE_INFINITY = 10000;
    private const int NEGATIVE_INFINITY = -10000;

    public Card GetBestMove(CardGameState gameState, int depth)
    {
        int bestScore = NEGATIVE_INFINITY;
        Card bestMove = null;
        List<Card> possibleMoves = GeneratePossibleMoves(gameState);

        foreach (Card move in possibleMoves)
        {
            CardGameState newState = SimulateMove(gameState, move);
            int score = Minimax(newState, depth + 1, NEGATIVE_INFINITY, POSITIVE_INFINITY, false);

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
        }

        return bestMove;
    }

    private int Minimax(CardGameState gameState, int depth, int alpha, int beta, bool isMaximizingPlayer)
    {
        if (depth == MAX_DEPTH || gameState.IsGameOver())
        {
            return EvaluateGameState(gameState);
        }

        if (isMaximizingPlayer)
        {
            int bestScore = NEGATIVE_INFINITY;
            List<Card> possibleMoves = GeneratePossibleMoves(gameState);

            foreach (Card move in possibleMoves)
            {
                //Debug.Log("calculating max");
                //Debug.Log("Move Number: " + move.number);
                CardGameState newState = SimulateMove(gameState, move);
                int score = Minimax(newState, depth + 1, alpha, beta, false);
                int evalScore = EvaluateGameState(newState);
                //Debug.Log("evalScore: " + evalScore + " Move: " + move.number);
                
                bestScore = Mathf.Max(bestScore, score);
                //Debug.Log("bestScore: " + bestScore);
                alpha = Mathf.Max(alpha, score);
                if (beta <= alpha)
                {
                    break;
                }
            }

            return bestScore;
        }
        else
        {
            int bestScore = POSITIVE_INFINITY;
            List<Card> possibleMoves = GeneratePossibleMoves(gameState);

            foreach (Card move in possibleMoves)
            {
                //Debug.Log("calculating mini" + " move number: " + move.number);
                CardGameState newState = SimulateMove(gameState, move);
                int score = Minimax(newState, depth + 1, alpha, beta, true);
                
                bestScore = Mathf.Min(bestScore, score);
                beta = Mathf.Min(beta, score);
                if (beta <= alpha)
                {
                    break;
                }
            }

            return bestScore;
        }
    }

    private List<Card> GeneratePossibleMoves(CardGameState gameState)
    {
        List<Card> possibleMoves = new List<Card>();
        for (int i = 1; i <= gameState.aiHand.Count; i++)
        {
            if (gameState.aiHand[i-1] != 0)
                possibleMoves.Add(new Card(gameState.aiHand[i-1]));
            else
                possibleMoves.Add(new Card(0));
            
        }
        for (int i = 1; i <= gameState.aiHand.Count; ++i)
        {
            Debug.Log("Possible Moves: " + possibleMoves[i-1].number);
        }
        //Debug.Log(possibleMoves[0].number + " " + possibleMoves[1].number + " " + possibleMoves[2].number + " " + possibleMoves[3].number + " " + possibleMoves[4].number);
        return possibleMoves;
    }

    public CardGameState SimulateMove(CardGameState gameState, Card move)
    {
        int value = move.number;
        CardGameState newState = new CardGameState(gameState.playerHealth, gameState.enemyHealth, gameState.playerTempHp, gameState.enemyTempHp, gameState.aiHand);

       
        
        if (gameState.playerTempHp > 2)
        {
            newState.enemyTempHp += value;
        }

        else if (gameState.enemyHealth < 5 && value % 2 == 0)
        {
            newState.enemyTempHp += value;
        }
        else
        {
            value -= newState.playerTempHp;
            if (value > 0)
            {
                newState.playerTempHp = 0;
                newState.playerHealth -= value;
            } 
            else
                newState.playerTempHp = 0;
        }
        
        
        return newState;
    }

    private int EvaluateGameState(CardGameState gameState)
    {
        int result = 0;
        if (gameState.enemyHealth - gameState.playerHealth > 0)
        {
            result += (gameState.enemyHealth - gameState.playerHealth) * 2;
        }
        else
        {
            result -= (gameState.enemyHealth - gameState.playerHealth) * 2;
        }
        if ((gameState.enemyTempHp+gameState.enemyHealth) - (gameState.playerHealth+gameState.playerTempHp) > 0)
            result += (gameState.enemyTempHp+gameState.enemyHealth) - (gameState.playerHealth+gameState.playerTempHp);
        else
            result -= (gameState.enemyTempHp+gameState.enemyHealth) - (gameState.playerHealth+gameState.playerTempHp);
        //result = (gameState.enemyTempHp + gameState.enemyHealth) - (gameState.playerHealth + gameState.playerTempHp);
        return result;
        //return gameState.enemyHealth - gameState.playerHealth;
    }
}


