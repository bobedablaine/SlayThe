using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    [SerializeField]
    private LogicScript logic;

    [SerializeField]
    Collider2D cardCollider;
    // Start is called before the first frame update
    void Start()
    {
        logic = FindObjectOfType<LogicScript>();
        cardCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 mousePos = logic.main.ScreenToWorldPoint(Input.mousePosition);
        // if (Input.GetMouseButton(0))
        // {
        //     if (cardCollider.OverlapPoint(mousePos))
        //     {
        //         transform.position = logic.main.ScreenToWorldPoint(Input.mousePosition);
        //         //Debug.Log(logic.main.ScreenToWorldPoint(Input.mousePosition));
        //         transform.position += Vector3.forward;
        //     }
        // }
        // if (cardCollider.OverlapPoint(mousePos))
        // {
        //     if (Input.GetMouseButtonUp(0) && transform.position.y > 0)
        //     {
        //         Debug.Log("Card Hit");
        //         logic.PlayerHit(1);
        //         Destroy(gameObject);
        //     }
        // }
    }
}
