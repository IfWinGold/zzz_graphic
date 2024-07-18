using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtility
{
    public static Vector3 GetInputDirection()
    {
        float mx = Input.GetAxis("Horizontal");
        float my = Input.GetAxis("Vertical");
        return new Vector3(mx, 0f, my);
    }    
}
