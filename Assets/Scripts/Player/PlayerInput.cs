using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public int X { get; private set; }

    public bool Jump { get; private set; }

    private void Awake()
    {
        X = 0;
        Jump = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            X = 1;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            X = -1;
        }
        else
        {
            X = 0;
        }
        
        Jump = (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow));
    }
}
