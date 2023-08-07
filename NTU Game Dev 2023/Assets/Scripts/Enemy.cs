using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float bounty = 100;
    private bool targetLocked = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetLocked)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Destroy(gameObject);
                GameController.instance.SetScore(bounty);
                GameController.instance.PlaySnipe();

            }
        }
    }

    public void SetTarget(bool value)
    {
        targetLocked = value;
    }

}
