using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    private float screenRight;
    private float groundRight;
    private float groundHeight;

    private bool hasGeneratedGround = false;

    // Start is called before the first frame update
    void Start()
    {
        hasGeneratedGround = false;
        boxCollider2D = GetComponent<BoxCollider2D>();
        screenRight = Camera.main.orthographicSize * Screen.width / Screen.height * 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        groundRight = transform.position.x + (boxCollider2D.bounds.size.x) / 2.0f;
        groundHeight = transform.position.y + (boxCollider2D.bounds.size.y) / 2.0f;

        if(groundRight < -screenRight / 2.0f)
        {
            Destroy(gameObject);
            return;
        }

        if (!hasGeneratedGround)
        {
            if(groundRight < screenRight)
            {
                hasGeneratedGround = true;
                GameController.instance.GenerateGround(this.gameObject);
            }
        }
    }

    public void SetHasGeneratedGround(bool value)
    {
        hasGeneratedGround = value;
    }

    public float GetGroundRight()
    {
        return groundRight;
    }

    public float GetGroundHeight()
    {
        return groundHeight;
    }

}
