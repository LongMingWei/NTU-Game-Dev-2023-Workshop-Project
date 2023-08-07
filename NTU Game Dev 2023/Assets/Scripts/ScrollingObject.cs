using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = new Vector2(-GameController.instance.GetGameSpeed(), 0);

        //if the game is over, stop moving
        if(GameController.instance.GetGameOver())
        {
            rb2d.velocity = Vector2.zero;
        }
    }
}
