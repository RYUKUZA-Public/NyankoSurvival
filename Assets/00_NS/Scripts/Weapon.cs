using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private int id;

    public int Id => id;
    [SerializeField]
    private int prefabId;
    [SerializeField]
    private float damage;
    [SerializeField]
    private int count;
    [SerializeField]
    private float speed;

    public float Speed { get { return speed; } set { speed = value; } }

    private float _timer;
    private Player _player;

    private void Awake()
    {
        _player = GameManager.Instance.Player;
    }
    
    public void Init(ItemData data)
    {
        // Basic
        name = $"Weapon {data.itemId}";
        transform.parent = _player.transform;
        transform.localPosition = Vector3.zero;
        
        // property
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for (int i = 0; i < GameManager.Instance.Pool.WeaponPrefabs.Length; i++)
        {
            if (data.projectile == GameManager.Instance.Pool.WeaponPrefabs[i])
            {
                prefabId = i;
                break;
            }
        }
        
        
        switch (id)
        {
            case 0:
                speed = 150;
                Place();
                break;
            default:
                speed = 0.4f;
                break;
        }
        
        _player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    private void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                
                break;
            default:
                _timer += Time.deltaTime;
                if (_timer > speed)
                {
                    _timer = 0;
                    Fire();
                }
                break;
        }
        
        // TODO. TEST
        if (Input.GetKeyDown(KeyCode.Space))
        {
            WeaponLevelUp(20, 5);
        }
    }

    public void WeaponLevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
            Place();
        
        _player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    

    private void Place()
    {
        for (int i = 0; i < count; i++)
        {
            Transform bullet;
            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet = GameManager.Instance.Pool.Get(PoolType.Weapon, prefabId).transform;
                bullet.transform.parent = transform;
            }
            
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            
            // 近接は、無限に貫通すりので、-1 (Per)
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
        }   
    }

    private void Fire()
    {
        if (!_player.Scan.NearestTarget)
            return;

        Vector3 targetPos = _player.Scan.NearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;
        
        
        
        Transform bullet = GameManager.Instance.Pool.Get(PoolType.Weapon, prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
