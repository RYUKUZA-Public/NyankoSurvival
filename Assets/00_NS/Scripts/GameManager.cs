using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private Player player;
    public Player Player => player;
    
    [SerializeField] private PoolManager pool;
    public PoolManager Pool => pool;

    private void Awake()
    {
        Instance = this;
    }
}
