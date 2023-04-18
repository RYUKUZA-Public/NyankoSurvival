using System;
using System.Collections;
using UnityEngine;

public class AchiveManager : NewMonoBehaviour
{
    /// <summary>
    /// ロックされたキャラクター
    /// </summary>
    [SerializeField]
    private GameObject[] lockedCharacters;
    /// <summary>
    /// ロック解除されたキャラクター
    /// </summary>
    [SerializeField]
    private GameObject[] unlockedCharacters;
    /// <summary>
    /// 業績達成、お知らせミニポップアップ
    /// </summary>
    [SerializeField]
    private GameObject uiNotice;
    /// <summary>
    /// 業績リスト
    /// </summary>
    private GameDefine.Achive[] _achives;
    /// <summary>
    /// ポップアップ終了までの待ち時間
    /// </summary>
    private WaitForSecondsRealtime _wait;

    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        _achives = (GameDefine.Achive[])Enum.GetValues(typeof(GameDefine.Achive));
        _wait = new WaitForSecondsRealtime(GameDefine.MiniPopupWaitTime);

        // データがない場合、新規Playerの初期化
        if (!PlayerPrefs.HasKey(GameDefine.PlayerPrefsKey.MyData.ToString()))
            Init();
    }

    /// <summary>
    /// 開始時、ロック解除キャラクターチェック
    /// </summary>
    private void Start() => UnlockCharacter();
    
    /// <summary>
    /// Update Manager
    /// </summary>
    public override void NewLateUpdate()
    {
        // 業績達成の確認
        foreach (GameDefine.Achive achive in _achives)
            CheckAchive(achive);
    }

    /// <summary>
    /// 新規Player 初期化
    /// </summary>
    private void Init()
    {
        PlayerPrefs.SetInt(GameDefine.PlayerPrefsKey.MyData.ToString(), 1);

        foreach (GameDefine.Achive achive in _achives)
            PlayerPrefs.SetInt(achive.ToString(), 0);
    }

    /// <summary>
    /// キャラクターロック解除業績達成
    /// </summary>
    private void UnlockCharacter()
    {
        for (int i = 0; i < lockedCharacters.Length; i++)
        {
            // 現在の業績名
            string achiveName = _achives[i].ToString();
            // 業績が解除されたか確認
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            // ロックされたキャラクター、オブジェクトを有効または無効にする
            lockedCharacters[i].SetActive(!isUnlock);
            // ロック解除されたキャラクター、オブジェクトを有効または無効にする
            unlockedCharacters[i].SetActive(isUnlock);
        }
    }
    
    /// <summary>
    /// 業績達成の確認
    /// </summary>
    private void CheckAchive(GameDefine.Achive achive)
    {
        bool isAchive = false;

        switch (achive)
        {
            case GameDefine.Achive.UnlockNoToRi: // 「Unlock NoToRi」業績の場合
                isAchive = GameManager.Instance.Kill >= 10; // 10人以上処置したか確認
                break;
        }

        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            // 該当業績を達成したことを示すPlayerPrefs値を1に設定
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for (int i = 0; i < uiNotice.transform.childCount; i++)
            {
                bool isActive = i == (int)achive;
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }
            
            // 業績達成ポップアップ表示
            StartCoroutine(NoticeRoutine());
        }
    }

    /// <summary>
    /// 業績達成ポップアップ表示
    /// </summary>
    private IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        AudioManager.Instance.PlaySfx(GameDefine.Sfx.LevelUp);
        
        yield return _wait;
        
        uiNotice.SetActive(false);
    }
}
