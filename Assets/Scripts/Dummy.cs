using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : ObjectState
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "AttackHitBox")
        {
            Debug.Log("toi bi tan cong");
        }
    }
}
