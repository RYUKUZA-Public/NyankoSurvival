using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    [Header("[ 出現間隔 ]")]
    public float SpawnTime;
    [Header("[ 体力 ]")]
    public int Hp;
    [Header("[ 移動速度 ]")]
    public float Speed;
}

public class EnemySpawner : NewMonoBehaviour
{
    /// <summary>
    /// 出現する敵の情報を格納する配列
    /// </summary>
    [SerializeField]
    private EnemySpawnData[] spawnData;
    /// <summary>
    /// 出現位置の配列
    /// </summary>
    private Transform[] _spawnPoint;
    /// <summary>
    /// // 現在のステージレベル
    /// </summary>
    private int _level;
    /// <summary>
    /// ステージレベル毎の制限時間
    /// </summary>
    private float _levelTime;
    /// <summary>
    /// 出現までの時間を計測するタイマー
    /// </summary>
    private float _timer;

    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        _spawnPoint = GetComponentsInChildren<Transform>();
        // ステージレベル毎の制限時間を計算
        _levelTime = GameManager.Instance.MaxGameTime / spawnData.Length;
    }

    /// <summary>
    /// Update Manager
    /// </summary>
    public override void NewUpdate()
    {
        // Play中でなければ無視
        if (!GameManager.Instance.IsLive)
            return;
        
        // タイマーを更新
        _timer += Time.deltaTime;
        // 現在のステージレベルを計算
        _level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.GameTime / _levelTime), spawnData.Length - 1 );

        // 出現間隔に達したら
        if (_timer > spawnData[_level].SpawnTime)
        {
            _timer = 0; // タイマーをリセット
            SpawnEnemy(); // 敵を出現させる
        }
    }
    
    /// <summary>
    /// 敵の生成
    /// </summary>
    private void SpawnEnemy()
    {
        // プールから敵を取得
        GameObject enemy = GameManager.Instance.Pool.Get(GameDefine.PoolType.Monster, _level);
        // ランダムな位置に出現させる
        enemy.transform.position = _spawnPoint[Random.Range(1, _spawnPoint.Length)].position;
        // 敵の情報を初期化
        enemy.GetComponent<EnemyController>().Init(spawnData[_level]);
    }
}