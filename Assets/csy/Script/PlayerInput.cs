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
        if (Input.GetAxis("Horizontal") > 0.001)
        {
            X = 1;
        }
        else if (Input.GetAxis("Horizontal") < -0.001)
        {
            X = -1;
        }
        else
        {
            X = 0;
        }
        
        Jump = (Input.GetAxis("Vertical") > 0);
    }
}
