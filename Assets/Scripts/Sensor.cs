using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    bool isActive;

    private void OnEnable()
    {
        isActive = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool State()
    {
        return isActive;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isActive = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isActive = false;
    }
}
