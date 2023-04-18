using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : NewMonoBehaviour
{
    public static GameManager Instance;
    
    [Header("[ Game Control ]")]
    [SerializeField]
    private float _maxGameTime = 1 * 10f; // 最大ゲーム時間、終了時の勝利
    public float MaxGameTime => _maxGameTime;
    
    public float GameTime { get; private set; }
    public bool IsLive { get; private set; }

    [Header("[ Player Info ]")]
    [SerializeField]
    private float maxHp = 100; // Playerの最大体力
    public float MaxHp => maxHp;
    
    /// <summary>
    /// 近距離武器の基本攻撃スピード
    /// </summary>
    [SerializeField] 
    private float meleeWeaponBaseSpeed = 150f;
    public float MeleeWeaponBaseSpeed => meleeWeaponBaseSpeed;
    
    /// <summary>
    /// 遠距離武器の基本攻撃スピード
    /// </summary>
    [SerializeField] 
    private float rangeWeaponBaseSpeed = 0.5f;
    public float RangeWeaponBaseSpeed => rangeWeaponBaseSpeed;
    
    /// <summary>
    /// Playerの基本移動速度
    /// </summary>
    [SerializeField] 
    private float playerBaseMoveSpeed = 3f;
    public float PlayerBaseMoveSpeed => playerBaseMoveSpeed;
    
    /// <summary>
    /// 次のレベルに上がるための経験値必要量 (Player Level)
    /// </summary>
    [SerializeField]
    private int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };
    public int[] NextExp => nextExp;
    
    /// <summary>
    /// 現在のPlayer情報
    /// </summary>
    public int PlayerId { get; private set; }
    public int Level { get; private set; }
    public int Exp { get; private set; }
    public float Hp { get; set; }
    public int Kill { get; set; }
    
    [Header("[ Game Object ]")]
    [SerializeField]
    private PlayerController playerController;
    public PlayerController PlayerController => playerController;
    
    [SerializeField]
    private PoolManager pool;
    public PoolManager Pool => pool;
    
    [SerializeField]
    private LevelUpPop levelUpPop;
    [SerializeField]
    private Result uiResult;
    /// <summary>
    /// 全体的を倒す機能
    /// </summary>
    [SerializeField]
    private GameObject enemyCleaner;

    [Header("[ Test ]")]
    [SerializeField] private Text fpsText;

    /// <summary>
    /// TODO. TEST
    /// </summary>
    private void FpsTest()
    {
        float fps = 1 / Time.unscaledDeltaTime;
        fpsText.text = fps.ToString("F0");
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        // vSyncCount 解除
        QualitySettings.vSyncCount = 0;
        // FPS 30に固定
        Application.targetFrameRate = 30;
        Instance = this;
        IsLive = false;
    }

    /// <summary>
    /// タイトルBGM再生
    /// </summary>
    private void Start() => AudioManager.Instance.PlayBgm(GameDefine.Bgm.Title);

    /// <summary>
    /// Player 選択時のキャラクター初期化
    /// </summary>
    public void GameStart(int id)
    {
        PlayerId = id;
        Hp = maxHp;
        
        playerController.gameObject.SetActive(true);
        levelUpPop.Select(PlayerId % 2);
        TimeResume();
        
        AudioManager.Instance.PlayBgm(GameDefine.Bgm.Battle);
        AudioManager.Instance.PlaySfx(GameDefine.Sfx.Select);
    }
    
    /// <summary>
    /// ゲーム再起動
    /// </summary>
    public void GameRetry() => SceneManager.LoadScene(0);

    /// <summary>
    /// ゲーム勝利時の演出
    /// </summary>
    public void GameVictory() => StartCoroutine(GameVictoryRoutine());
    private IEnumerator GameVictoryRoutine()
    {
        IsLive = false;
        enemyCleaner.SetActive(true); // フィールド全的死亡
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        TimeStop();
        
        AudioManager.Instance.PlaySfx(GameDefine.Sfx.Win);
        AudioManager.Instance.StopBgm();
    }

    /// <summary>
    /// ゲームオーバー
    /// </summary>
    public void GameOver() => StartCoroutine(GameOverRoutine());
    /// <summary>
    /// ゲームオーバー演出
    /// </summary>
    private IEnumerator GameOverRoutine()
    {
        IsLive = false;
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        TimeStop();
        
        AudioManager.Instance.PlaySfx(GameDefine.Sfx.Lose);
        AudioManager.Instance.StopBgm();
    }

    /// <summary>
    /// Update Manager
    /// ゲーム時間を測定し、勝利するかどうかを判断
    /// </summary>
    public override void NewUpdate()
    {
        // TODO. Test
        FpsTest();
        
        // ゲームが進行中でない場合は無視
        if (!IsLive)
            return;
        
        // Play経過時間測定
        GameTime += Time.deltaTime;
        // 指定された時間に、到達した場合、ゲーム勝利
        if (GameTime > _maxGameTime)
        {
            GameTime = _maxGameTime;
            GameVictory();
        }
    }

    /// <summary>
    /// 敵を処置するたびに、経験値を獲得
    /// Playerレベルアップ
    /// </summary>
    public void GetExp()
    {
        // ゲームが進行中でない場合は無視
        if (!IsLive)
            return;
        
        // 経験値はモンスターごとに1ずつ増加
        Exp++;
        // 現在の経験値が、次のレベルで必要な経験値と同じになったら、レベルアップ
        if (Exp == nextExp[Mathf.Min(Level, nextExp.Length - 1)])
        {
            Level++;
            // レベルアップ後は必須で経験値初期化
            Exp = 0;
            // アイテム選択ポップアップ表示
            levelUpPop.Show();
        }
    }

    /// <summary>
    /// ゲーム時間を一時停止
    /// </summary>
    public void TimeStop()
    {
        IsLive = false;
        playerController.InitTimeStopPlayer();
        Time.timeScale = 0f;
    }
    
    /// <summary>
    /// ゲームの時間をもう一度進行
    /// </summary>
    public void TimeResume()
    {
        IsLive = true;
        Time.timeScale = 1f;
    }
}
