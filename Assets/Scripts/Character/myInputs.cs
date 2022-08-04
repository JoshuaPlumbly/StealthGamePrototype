using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myInputs : MonoBehaviour
{
    public const string Horizontal = "Horizontal";
    public const string Vertical = "Vertical";


    public float Axis (string input)
    {
        float output = Input.GetAxis(input);
        return output;
    }

    public float RawAxis (string input)
    {
        float output = Input.GetAxisRaw(input);
        return output;
    }

    public bool Button (string input)
    {
        bool output = Input.GetButton(input);
        return output;
    }

    public bool ButtonDown(string input)
    {
        bool output = Input.GetButtonDown(input);
        return output;
    }

    public bool ButtonUp(string input)
    {
        bool output = Input.GetButtonUp(input);
        return output;
    }


}
