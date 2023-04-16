using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static float Speed
    {
        get { return GameManager.Instance.PlayerId == 1 ? 1.1f : 1f; }
    }
    
    public static float WqaponSpeed
    {
        get { return GameManager.Instance.PlayerId == 0 ? 1.1f : 1f; }
    }
    
    public static float WqaponRate
    {
        get { return GameManager.Instance.PlayerId == 0 ? 0.9f : 1f; }
    }
}
