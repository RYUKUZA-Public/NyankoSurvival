using UnityEngine;

public class LevelUpPop : MonoBehaviour
{
    private RectTransform _rect;
    private LevelUpPopupItem[] _items;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _items = GetComponentsInChildren<LevelUpPopupItem>(true);
    }

    public void Show()
    {
        GetRandomitems();
        _rect.localScale = Vector3.one;
        GameManager.Instance.TimeStop();
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.LevelUp);
    }

    public void Hide()
    {
        _rect.localScale = Vector3.zero;
        GameManager.Instance.TimeResume();
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void Select(int index)
    {
        _items[index].OnClick();
    }
    
    private void GetRandomitems()
    {
        // すべてのアイテムを非活性
        foreach (LevelUpPopupItem item in _items)
            item.gameObject.SetActive(false);
        
        // ランダムアイテム3つ抽選
        int[] ran = new int[3];
        while (true)
        {
            ran[0] = Random.Range(0, _items.Length);
            ran[1] = Random.Range(0, _items.Length);
            ran[2] = Random.Range(0, _items.Length);
            
            if(ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;
        }

        for (int i = 0; i < ran.Length; i++)
        {
            LevelUpPopupItem ranLevelUpPopupItem = _items[ran[i]];

            // Maxレベルの場合は消費アイテムで対処
            if (ranLevelUpPopupItem.level == ranLevelUpPopupItem.data.damages.Length)
                _items[4].gameObject.SetActive(true);
            else
                ranLevelUpPopupItem.gameObject.SetActive(true);
        }

    }
}
