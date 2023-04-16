using UnityEngine;

public class LevelUpPop : MonoBehaviour
{
    private RectTransform _rect;
    private Item[] _items;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        _rect.localScale = Vector3.one;
    }

    public void Hide()
    {
        _rect.localScale = Vector3.zero;
    }

    public void Select(int index)
    {
        _items[index].OnClick();
    }
}
