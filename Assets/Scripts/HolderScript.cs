using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderScript : MonoBehaviour
{
    [SerializeField]
    private LogicScript logic;

    [SerializeField]
    Collider2D cardCollider;
    Collider2D holderCollider;
    bool cardHeld = false;
    public int numberHeld;
    public GameObject objectHeld;
    // Start is called before the first frame update
    void Start()
    {
        logic = FindObjectOfType<LogicScript>();
        holderCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
