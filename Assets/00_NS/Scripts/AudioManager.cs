using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [Header("[ BGM ]")]
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private float bgmVolume;
    private AudioSource _bgmPlayer;
    
    [Header("[ SFX ]")]
    [SerializeField] private AudioClip[] sfxClip;
    [SerializeField] private float sfxCVolume;
    [SerializeField] private int channels;
    private AudioSource[] _sfxPlayers;
    private int _channelIndex;


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
        _bgmPlayer.clip = bgmClip;
        
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
}
