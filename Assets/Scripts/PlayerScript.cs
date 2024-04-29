using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
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
            KillPlayer();


    }

    void KillPlayer()
    {
        Destroy(gameObject);
        Debug.Log("He dead");
    }
}
