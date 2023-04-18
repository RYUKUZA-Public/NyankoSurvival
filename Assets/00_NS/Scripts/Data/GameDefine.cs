using System;
using UnityEngine;

public class GameDefine : MonoBehaviour
{
    private const int FRAMERATE = 30;
    
    /// <summary>
    /// アニメパラメータ
    /// </summary>
    public enum EnemyAnimationType { Hit, Dead }
    public enum PlayerAnimationType { Speed, Dead }
    /// <summary>
    /// Pool タイプ
    /// </summary>
    public enum PoolType { Monster, Weapon }
    /// <summary>
    /// Playerの情報
    /// </summary>
    public enum PlayerInfoType { Exp, Level, Kill, Time, Hp }
    /// <summary>
    /// 業績リスト
    /// </summary>
    public enum Achive { UnlockNoToRi }
    
    /// <summary>
    /// タグ値
    /// </summary>
    public enum Tag { Tile, Area, Enemy, Bullet }
    
    /// <summary>
    /// PlayerPrefs保存値
    /// </summary>
    public enum PlayerPrefsKey { MyData }

    /// <summary>
    /// オーディオリスト
    /// </summary>
    public enum Bgm { Title, Battle }
    public enum Sfx { Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win }
    
    /// <summary>
    /// 業績ポップアップ表示時間
    /// </summary>
    public static readonly float MiniPopupWaitTime = 3f;
    /// <summary>
    /// ランダムで追点するアイテムの個数
    /// </summary>
    public static readonly int RANDOM_ITEM_COUNT = 3;

    
    public static readonly string HitKey = EnemyAnimationType.Hit.ToString();
    public static readonly string DeadKey = EnemyAnimationType.Dead.ToString();
    public static readonly string SpeedKey = PlayerAnimationType.Speed.ToString();
    
    /// <summary>
    /// Enemyノックバック時、パワー
    /// </summary>
    public const float EnemyKnockBackPower = 3f;
}