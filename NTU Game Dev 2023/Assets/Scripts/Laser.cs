using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2 (player.transform.position.x, player.transform.position.y); //+ (GetComponent<BoxCollider2D>().bounds.size.x) / 2.0f
        transform.rotation = player.transform.rotation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            collision.gameObject.GetComponent<Enemy>().SetTarget(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            collision.gameObject.GetComponent<Enemy>().SetTarget(false);
        }
    }
}
