using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [Header("[ BGM ]")]
    [SerializeField] private AudioClip[] bgmClips;
    [SerializeField] private float bgmVolume;
    private AudioSource _bgmPlayer;
    
    [Header("[ SFX ]")]
    [SerializeField] private AudioClip[] sfxClips;
    [SerializeField] private float sfxCVolume;
    [SerializeField] private int channels;
    private AudioSource[] _sfxPlayers;
    private int _channelIndex;
    
    public enum Bgm { Title, Battle }
    public enum Sfx { Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win }
    
    private void Awake()
    {
        Instance = this;
        Init();
    }

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

    public void PlayBgm(Bgm bgm)
    {
        var clip = bgmClips[(int)bgm];
        _bgmPlayer.clip = clip;
        _bgmPlayer.Play();
    }

    public void StopBgm() => _bgmPlayer.Stop();

    public void PlaySfx(Sfx sfx)
    {
        for (int i = 0; i < _sfxPlayers.Length; i++)
        {
            int loopIndex = (i + _channelIndex) % _sfxPlayers.Length;

            if (_sfxPlayers[loopIndex].isPlaying)
                continue;

            int ranIndex = 0;
            if (sfx == Sfx.Hit || sfx == Sfx.Melee)
                ranIndex = Random.Range(0, 2);
            
            _channelIndex = loopIndex;
            _sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
            _sfxPlayers[loopIndex].Play();
            break;
        }
    }
}
