using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxObject : MonoBehaviour
{
    public int tile = 0; //use to inform is it the 1 or 2 tile 
    public float depth = 1;
    private float length; //Store length of the bbox

    private GameController gameController; //Store our game controller
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d            = GetComponent<Rigidbody2D>();
        gameController  = GameController.instance;
        length          = GetComponent<SpriteRenderer>().bounds.size.x;

        transform.position = new Vector3(tile * length, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        float realVelocity = gameController.GetGameSpeed() / depth;
        rb2d.velocity = new Vector2(-realVelocity, 0);

        //if the object x position more than half its length we can return it to the beginning
        if(transform.position.x < -length / 2.0f)
        {
            RepositionObject();
        }
    }

    private void RepositionObject()
    {
        //go back 2 times it length
        Vector3 offset = new Vector3(length * 2.0f, 0, 0);
        transform.position = transform.position + offset;
    }
}
