using UnityEngine;

public class LevelUpPop : MonoBehaviour
{
    private RectTransform _rect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    public void Show()
    {
        _rect.localScale = Vector3.one;
    }

    public void Hide()
    {
        _rect.localScale = Vector3.zero;
    }
}
