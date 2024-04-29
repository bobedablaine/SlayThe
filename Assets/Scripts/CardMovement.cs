using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMovement : MonoBehaviour
{
    [SerializeField]
    private LogicScript logic;

    [SerializeField]
    Collider2D cardCollider;
    [SerializeField]
    GameObject holderGameObject;
    Collider2D holderCollider;
    HolderScript holderScript;
    bool cardHeld = false;
    numOne thisCard;
    // Start is called before the first frame update
    void Start()
    {
        logic = FindObjectOfType<LogicScript>();
        cardCollider = gameObject.GetComponent<BoxCollider2D>();
        holderGameObject = GameObject.FindGameObjectWithTag("Holder");
        holderCollider = holderGameObject.GetComponent<BoxCollider2D>();
        holderScript = holderGameObject.GetComponent<HolderScript>();
        thisCard = gameObject.GetComponent<numOne>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = logic.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButton(0))
        {
            if (cardCollider.OverlapPoint(mousePos))
            {
                cardHeld = true;
                transform.position = logic.main.ScreenToWorldPoint(Input.mousePosition);
                //Debug.Log(logic.main.ScreenToWorldPoint(Input.mousePosition));
                transform.position += Vector3.forward;
            }
            else
            {
                cardHeld = false;
            }
        }

        if (holderCollider.OverlapPoint(mousePos) && Input.GetMouseButtonUp(0) && cardHeld)
        {
            transform.position = new Vector3(0, 0, 0);
            holderScript.objectHeld = thisCard.gameObject;
            holderScript.numberHeld = thisCard.number;
        }
    }
}
