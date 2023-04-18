using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [Header("[ BGM ]")]
    [SerializeField]
    private AudioClip[] bgmClips;
    [SerializeField]
    private float bgmVolume;
    private AudioSource _bgmPlayer;
    
    [Header("[ SFX ]")]
    [SerializeField]
    private AudioClip[] sfxClips;
    [SerializeField]
    private float sfxCVolume;
    /// <summary>
    /// 同時に再生できるSFXチャンネル数
    /// </summary>
    [SerializeField]
    private int channels;
    private AudioSource[] _sfxPlayers;
    /// <summary>
    /// 現在SFXチャンネルIndex
    /// </summary>
    private int _channelIndex;
    
    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        Instance = this;
        Init();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Init()
    {
        // BGM
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        _bgmPlayer = bgmObject.AddComponent<AudioSource>();
        _bgmPlayer.playOnAwake = false;
        _bgmPlayer.loop = true;
        _bgmPlayer.volume = bgmVolume;
        
        // SFX
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        _sfxPlayers = new AudioSource[channels];

        for (int i = 0; i < _sfxPlayers.Length; i++)
        {
            _sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            _sfxPlayers[i].playOnAwake = false;
            _sfxPlayers[i].volume = sfxCVolume;
        }
    }

    /// <summary>
    /// BGM 再生
    /// </summary>
    public void PlayBgm(GameDefine.Bgm bgm)
    {
        var clip = bgmClips[(int)bgm];
        _bgmPlayer.clip = clip;
        _bgmPlayer.Play();
    }

    /// <summary>
    /// BGM 停止
    /// </summary>
    public void StopBgm() => _bgmPlayer.Stop();

    /// <summary>
    /// SFX 再生
    /// </summary>
    public void PlaySfx(GameDefine.Sfx sfx)
    {
        for (int i = 0; i < _sfxPlayers.Length; i++)
        {
            // チャネルIndex計算
            int loopIndex = (i + _channelIndex) % _sfxPlayers.Length;
            // 該当チャンネルが再生中
            if (_sfxPlayers[loopIndex].isPlaying)
                continue;

            int ranIndex = 0;
            // HitやMelee SFX の場合、二つの中からランダムで再成
            if (sfx == GameDefine.Sfx.Hit || sfx == GameDefine.Sfx.Melee)
                ranIndex = Random.Range(0, 2);
            
            _channelIndex = loopIndex;
            _sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
            _sfxPlayers[loopIndex].Play();
            break;
        }
    }
}
