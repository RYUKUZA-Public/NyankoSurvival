using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : NewMonoBehaviour
{
    [SerializeField]
    private GameDefine.PlayerInfoType type;

    private Text _myText;
    private Slider _mySlider;
    private GameManager _gameManager;
    
    private Dictionary<GameDefine.PlayerInfoType, System.Action> _updateMethods = 
        new Dictionary<GameDefine.PlayerInfoType, System.Action>();
    
    private void Awake()
    {
        _myText = GetComponent<Text>();
        _mySlider = GetComponent<Slider>();
        _gameManager = GameManager.Instance;
        
        // 各タイプに合わせた、アップデート関数をディクショナリーに登録
        _updateMethods[GameDefine.PlayerInfoType.Exp] = UpdateExp;
        _updateMethods[GameDefine.PlayerInfoType.Level] = UpdateLevel;
        _updateMethods[GameDefine.PlayerInfoType.Kill] = UpdateKill;
        _updateMethods[GameDefine.PlayerInfoType.Time] = UpdateTime;
        _updateMethods[GameDefine.PlayerInfoType.Hp] = UpdateHp;
    }

    /// <summary>
    /// ディクショナリーから、タイプに合わせたアップデート関数呼び出し
    /// </summary>
    public override void NewLateUpdate() => _updateMethods[type]?.Invoke();

    private void UpdateExp()
    {
        float currentExp = _gameManager.Exp;
        float maxExp = _gameManager.NextExp[Mathf.Min(_gameManager.Level, _gameManager.NextExp.Length - 1)];
        _mySlider.value = currentExp / maxExp;
    }
    
    private void UpdateLevel() => _myText.text = $"Lv.{_gameManager.Level:F0}";

    private void UpdateKill() => _myText.text = $"{_gameManager.Kill:F0}";

    private void UpdateTime()
    {
        float remainTime = _gameManager.MaxGameTime - _gameManager.GameTime;
        int min = Mathf.FloorToInt(remainTime / 60); 
        int sec = Mathf.FloorToInt(remainTime % 60);
        _myText.text = $"{min:D2}:{sec:D2}";
    }
    
    private void UpdateHp()
    {
        float currentHp = _gameManager.Hp;
        float maxHp = _gameManager.MaxHp;
        _mySlider.value = currentHp / maxHp;
    }
}
