using UnityEngine;
using System.Linq;

public class LevelUpPop : MonoBehaviour
{
    private RectTransform _rect;
    /// <summary>
    /// LevelUpポップアップに、入るアイテム
    /// </summary>
    private LevelUpPopupItem[] _items;

    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _items = GetComponentsInChildren<LevelUpPopupItem>(true);
    }

    /// <summary>
    /// ポップアップ表示
    /// </summary>
    public void Show()
    {
        // ランダムにアイテムを選択
        RandomlySelectItems();
        _rect.localScale = Vector3.one;
        // ゲーム時間停止
        GameManager.Instance.TimeStop();
        AudioManager.Instance.PlaySfx(GameDefine.Sfx.LevelUp);
    }

    /// <summary>
    /// ポップアップ非表示
    /// </summary>
    public void Hide()
    {
        _rect.localScale = Vector3.zero;
        // ゲーム時間の再開
        GameManager.Instance.TimeResume();
        AudioManager.Instance.PlaySfx(GameDefine.Sfx.Select);
    }

    /// <summary>
    /// 選択されたアイテムクリックイベントの実行
    /// </summary>
    public void Select(int index) => _items[index].OnClick();

    /// <summary>
    /// ランダムにアイテムを選択
    /// </summary>
    private void RandomlySelectItems()
    {
        // すべてのアイテムを非活性
        foreach (LevelUpPopupItem item in _items)
            item.gameObject.SetActive(false);
        
        // ランダムアイテム3つ抽選
        int[] randomIndices = GetRandomIndices(GameDefine.RANDOM_ITEM_COUNT);
        
        for (int i = 0; i < randomIndices.Length; i++)
        {
            LevelUpPopupItem selectedItem = _items[randomIndices[i]];

            // Maxレベルの場合、消費アイテムを表示する。
            if (selectedItem.ItemLevel == selectedItem.ItemData.damages.Length)
                _items[4].gameObject.SetActive(true);
            else
                selectedItem.gameObject.SetActive(true);
        }
    }
    
    /// <summary>
    /// count個数分のランダムインデックス配列を返却
    /// (重複アイテム検査)
    /// </summary>
    private int[] GetRandomIndices(int count)
    {
        // ランダムIndex配列の初期化
        int[] randomIndices = new int[count];
        for (int i = 0; i < count; i++)
            randomIndices[i] = -1;

        // 重複する値が選択されないようにチェック
        for (int i = 0; i < count; i++)
        {
            bool isDuplicated = true;
            while (isDuplicated)
            {
                // _items 配列内のインデックスランダム選択
                int randomIndex = Random.Range(0, _items.Length);
                if (!randomIndices.Contains(randomIndex))
                {
                    // 選択済みの値でない場合は、配列に保存
                    randomIndices[i] = randomIndex;
                    isDuplicated = false;
                }
            }
        }

        return randomIndices;
    }
}
