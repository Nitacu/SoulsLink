using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FindingDevices : MonoBehaviour
{
    [SerializeField] bool useKeyBoard = true;
    [SerializeField] PlayerInput playerinput;


    // Start is called before the first frame update
    void Start()
    {
        var all = InputDevice.all;
        foreach (var item in all)
        {
            Debug.Log("All: " + item);
        }

        InputDevice dev;
        dev = InputSystem.GetDevice("Keyboard");
        //playerinput.SwitchCurrentControlScheme(dev);
    }

    public InputDevice selectDevice()
    {
        InputDevice device = new InputDevice();

        var gamepads = InputSystem.FindControls("<Gamepads>");
        var keyboards = InputSystem.FindControls("<Keyboard>");

        Debug.Log("Keyboards count: " + keyboards.Count);

        if (gamepads.Count > 0)//hay gamepads
        {
            Debug.Log("gamepad find");
        }
        else if (useKeyBoard && keyboards.Count > 0)
        {
            Debug.Log("return keyboard");
            return keyboards[0].device;
        }

        Debug.Log("return null");
        return null;
    }

    public void OnJoinGame()
    {

    }


    public void OnPlayerJoined(PlayerInput input)
    {
        InputDevice[] devices = new InputDevice[1];
        devices[0] = selectDevice();

        if (devices[0] != null)
        {
            input.SwitchCurrentControlScheme(devices);
        }

    }


    public void switchDevice(PlayerInput input)
    {
        InputDevice[] devices = new InputDevice[1];
        devices[0] = selectDevice();

        input.SwitchCurrentControlScheme(devices);
    }
}
