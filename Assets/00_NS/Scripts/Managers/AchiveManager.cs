using System;
using System.Collections;
using UnityEngine;

public class AchiveManager : NewMonoBehaviour
{
    [SerializeField] private GameObject[] lockCharacter;
    [SerializeField] private GameObject[] unlockCharacter;

    [SerializeField] private GameObject uiNotice;
    
    enum Achive { UnlockNoToRi }

    private Achive[] _achives;

    private WaitForSecondsRealtime _wait;

    private void Awake()
    {
        _achives = (Achive[])Enum.GetValues(typeof(Achive));
        _wait = new WaitForSecondsRealtime(3);

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

    public override void NewLateUpdate()
    {
        foreach (Achive achive in _achives)
        {
            CheckAchive(achive);
        }
    }

    private void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch (achive)
        {
            case Achive.UnlockNoToRi:
                isAchive = GameManager.Instance.Kill >= 10;
                break;
        }

        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for (int i = 0; i < uiNotice.transform.childCount; i++)
            {
                bool isActive = i == (int)achive;
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }
            
            StartCoroutine(NoticeRoutine());
        }
    }

    private IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.LevelUp);
        
        yield return _wait;
        
        uiNotice.SetActive(false);
    }
}
