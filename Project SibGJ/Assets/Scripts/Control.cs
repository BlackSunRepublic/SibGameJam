using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    private PlayerMovment move;
    [HideInInspector] public bool isInteract;
    public PlayerMovment dasy;
    public PlayerMovment shadow;
    private bool isSwith = false;
    private void Awake()
    {
        move = dasy;
    }
    void Update()
    {
        move.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
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
 
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchPerson();
        }
    }
    public void SwitchPerson()
    {
        if (dasy.gameObject.activeSelf && shadow.gameObject.activeSelf)
        {
            if (!isSwith)
            {
                move.virtualCamera.SetActive(false);
                move.isActive = false;
                move = shadow;
                move.virtualCamera.SetActive(true);
                move.isActive = true;
                isSwith = true;
            }
            else if (!move.isActiveAbility)
            {
                move.virtualCamera.SetActive(false);
                move.isActive = false;
                move = dasy;
                move.virtualCamera.SetActive(true);
                move.isActive = true;
                isSwith = false;
            }
        }
    }
}
