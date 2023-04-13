using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    /// <summary>
    /// Pool対象 (Monster)
    /// </summary>
    [SerializeField]
    private GameObject[] prefabs;
    /// <summary>
    /// Poolリスト
    /// </summary>
    private List<GameObject>[] _pools;

    private void Awake()
    {
        // Pool 初期化
        _pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < _pools.Length; i++)
            _pools[i] = new List<GameObject>();
    }

    /// <summary>
    /// 当該Index(Monster Id)をPoolで、取得または、生成
    /// </summary>
    public GameObject Get(int index)
    {
        // Poolで、非活性オブジェクトの中で、最初に見つけたものをリターン
        foreach (GameObject go in _pools[index])
        {
            if (!go.activeSelf)
            {
                go.SetActive(true);
                return go;
            }
        }

        //  Poolで、非活性オブジェクトがない場合、新しく作成してリターン
        GameObject newObj = Instantiate(prefabs[index], transform);
        _pools[index].Add(newObj);
        
        return newObj;
    }
}
