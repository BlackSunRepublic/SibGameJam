using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    private PlayerMovment move;
    [HideInInspector] public bool isInteract;
    private void Awake()
    {
        move = GetComponent<PlayerMovment>();
    }
    void Update()
    {
        if (move.isActive)
        {
            move.Move(Input.GetAxis("Horizontal"));
            if (Input.GetButtonDown("Jump"))
            {
                move.Jump();
            }
            if (isInteract && Input.GetKeyDown(KeyCode.E))
            {
                move.Interaction();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                move.Ability();
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            move.isActive = !move.isActive;
        }
    }
}
