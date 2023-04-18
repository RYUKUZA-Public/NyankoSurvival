using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    /// <summary>
    /// Pool対象 (Monster)
    /// </summary>
    [SerializeField]
    private GameObject[] monsterPrefabs;
    /// <summary>
    /// Pool対象 (Weapon)
    /// </summary>
    [SerializeField]
    private GameObject[] weaponPrefabs;
    public GameObject[] WeaponPrefabs => weaponPrefabs;
    
    /// <summary>
    /// Poolリスト
    /// </summary>
    private List<GameObject>[] _monsterPools;
    private List<GameObject>[] _weaponPools;

    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        // Pool 初期化
        _monsterPools = new List<GameObject>[monsterPrefabs.Length];
        _weaponPools = new List<GameObject>[weaponPrefabs.Length];

        for (int i = 0; i < _monsterPools.Length; i++)
            _monsterPools[i] = new List<GameObject>();

        for (int i = 0; i < _weaponPools.Length; i++)
            _weaponPools[i] = new List<GameObject>();
    }

    /// <summary>
    /// 当該Index(Monster Id)をPoolで、取得または、生成
    /// </summary>
    public GameObject Get(GameDefine.PoolType type, int index)
    {
        List<GameObject> objectPool = 
            type == GameDefine.PoolType.Monster ? _monsterPools[index] : _weaponPools[index];

        // 非活性オブジェクトを見つけてリターン
        foreach (GameObject go in objectPool)
        {
            if (!go.activeSelf)
            {
                go.SetActive(true);
                return go;
            }
        }
        
        // 非活性オブジェクトがない場合、新たに生成してリターン
        GameObject newObject = Instantiate(
            type == GameDefine.PoolType.Monster ? monsterPrefabs[index] : weaponPrefabs[index], transform);
        objectPool.Add(newObject);
    
        return newObject;
    }
}
