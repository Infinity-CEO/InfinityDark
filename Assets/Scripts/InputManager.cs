using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class InputManager : MonoBehaviour
{
    private Controller crl;

    [HideInInspector]
    public float vertical;
    [HideInInspector]
    public float horizontal;
    [HideInInspector]
    public float xValue, yValue;
    private bool pause = false;

    private void Start()
    {
        crl = GetComponent<Controller>();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private void FixedUpdate() 
    {
        if(GameManager.gm.gState != GameManager.GameState.Start)
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.Escape) && crl != null)pauseGame();
        {

        }
        if(pause)
        {
            vertical = 0;
            horizontal = 0;
            xValue = 0;
            yValue = 0;
        }
        else
        {
            vertical = SimpleInput.GetAxisRaw("Vertical");
            horizontal = SimpleInput.GetAxisRaw("Horizontal");
            //horizontal = joystick.Horizontal;
            //vertical = joystick.Vertical;
            xValue = CrossPlatformInputManager.GetAxisRaw("Mouse Y");
            yValue = CrossPlatformInputManager.GetAxisRaw("Mouse X");
            //xValue = SimpleInput.GetAxis("Mouse Y");
            //yValue = SimpleInput.GetAxis("Mouse X");
        }


        if(Input.GetKeyDown(KeyCode.Space))
        {
            crl.Jump();
        }
    }
    private void pauseGame()
    {
        if(pause)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            pause = false;
            return;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pause = true;
        }
    }
}
