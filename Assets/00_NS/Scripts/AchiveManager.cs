using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    [SerializeField] private GameObject[] lockCharacter;
    [SerializeField] private GameObject[] unlockCharacter;

    enum Achive { UnlockNoToRi }

    private Achive[] _achives;

    private void Awake()
    {
        _achives = (Achive[])Enum.GetValues(typeof(Achive));

        if (!PlayerPrefs.HasKey("MyData"))
            Init();
    }

    private void Start()
    {
        UnlockCharacter();
    }

    private void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);

        foreach (Achive achive in _achives)
            PlayerPrefs.SetInt(achive.ToString(), 0);
    }

    private void UnlockCharacter()
    {
        for (int i = 0; i < lockCharacter.Length; i++)
        {
            string achiveName = _achives[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            lockCharacter[i].SetActive(!isUnlock);
            unlockCharacter[i].SetActive(isUnlock);
        }
    }
}