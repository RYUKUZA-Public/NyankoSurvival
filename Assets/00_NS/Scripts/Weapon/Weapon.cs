using UnityEngine;

public class Weapon : NewMonoBehaviour
{
    private int _prefabId;
    private float _damage;
    private int _count;
    private float _timer;
    private PlayerController _playerController;
    
    public int Id { get; private set; }
    public float Speed { get; set; }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake() => _playerController = GameManager.Instance.PlayerController;

    public override void NewUpdate()
    {
        // Play中でなければ無視
        if (!GameManager.Instance.IsLive)
            return;
        
        // 0 近接, 1 遠距離
        switch (Id)
        {
            case 0:
                // 武器を回転
                transform.Rotate(Vector3.back * Speed * Time.deltaTime);
                break;
            default:
                // タイマーを増加させ、発射する時間になったら弾丸を発射
                _timer += Time.deltaTime;
                if (_timer > Speed)
                {
                    _timer = 0;
                    Fire();
                }
                break;
        }
    }
    
    /// <summary>
    /// 武器データの初期化
    /// </summary>
    public void Init(ItemData data)
    {
        // Basic
        name = $"Weapon {data.itemId}";
        transform.parent = _playerController.transform;
        transform.localPosition = Vector3.zero;
        
        // property
        Id = data.itemId;
        _damage = data.baseDamage;
        _count = data.baseCount;

        // ロードした武器プレファブの中から、武器データに該当するプレファブIDを探す
        for (int i = 0; i < GameManager.Instance.Pool.WeaponPrefabs.Length; i++)
        {
            if (data.projectile == GameManager.Instance.Pool.WeaponPrefabs[i])
            {
                _prefabId = i;
                break;
            }
        }
        
        // 武器の種類によって武器の速度設定
        switch (Id)
        {
            case 0:
                Speed = GameManager.Instance.MeleeWeaponBaseSpeed * CharacterSpecialAbility.WqaponSpeed;
                Place();
                break;
            default:
                Speed = GameManager.Instance.RangeWeaponBaseSpeed * CharacterSpecialAbility.WqaponRate;
                break;
        }
        
        // 武器の種類によってPlayerの手の画像を変更
        WeaponFlipper weaponFlipper = _playerController.Hands[(int)data.itemType];

        weaponFlipper.sprite.sprite = data.hand;
        weaponFlipper.gameObject.SetActive(true);
        
        _playerController.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }
    
    /// <summary>
    /// 武器のアップグレード
    /// </summary>
    public void WeaponLevelUp(float damage, int count)
    {
        this._damage = damage;
        this._count += count;

        // 近接武器の場合、武器の位置を更新
        if (Id == 0)
            Place();
        
        // Player Controllerに現在の装備の効果を適用するようメッセージを送信
        _playerController.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// 
    /// </summary>
    private void Place()
    {
        for (int i = 0; i < _count; i++)
        {
            // pool
            Transform bullet;
            if (i < transform.childCount)
                bullet = transform.GetChild(i);
            else
            {
                bullet = GameManager.Instance.Pool.Get(GameDefine.PoolType.Weapon, _prefabId).transform;
                bullet.transform.parent = transform;
            }
            
            // 位置と回転初期化
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // 回転値設定
            Vector3 rotVec = Vector3.forward * 360 * i / _count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            
            // 近接は、無限に貫通すりので、-100 (Per)
            bullet.GetComponent<BulletController>().Init(_damage, -100, Vector3.zero);
        }   
    }

    /// <summary>
    /// 周りに敵がいると弾丸を発射
    /// </summary>
    private void Fire()
    {
        // 近い敵がいなければ発射しない
        if (!_playerController.Scan.NearestTarget)
            return;

        // 弾丸が発射される位置と方向計算
        Vector3 targetPos = _playerController.Scan.NearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;
        
        // オブジェクトPoolから、弾丸オブジェクトを取得
        Transform bullet = GameManager.Instance.Pool.Get(GameDefine.PoolType.Weapon, _prefabId).transform;
        // 弾丸位置と回転値設定
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        // 弾丸に関する情報設定後、発射
        bullet.GetComponent<BulletController>().Init(_damage, _count, dir);
        
        AudioManager.Instance.PlaySfx(GameDefine.Sfx.Range);
    }
}
