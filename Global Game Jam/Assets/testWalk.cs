using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class testWalk : MonoBehaviour
{
    public HingeJoint2D hinge;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Keyboard.current.spaceKey.isPressed)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            hinge.connectedAnchor = mousePos;
        }
    }
}
