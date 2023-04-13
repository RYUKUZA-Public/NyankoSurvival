using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private Player player;
    public Player Player => player;

    private void Awake()
    {
        Instance = this;
    }
}
