using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Inspector で指定したいクリップ
    [SerializeField] private AudioClip m_BGMNormal;
    [Range(0.0f, 1.0f)][SerializeField] private float m_BGMNormalVolume;

    [SerializeField] private AudioClip m_BGMAlert;
    [Range(0.0f, 1.0f)][SerializeField] private float m_BGMAlertVolume;

    [SerializeField] private AudioClip m_SEStrike;
    [Range(0.0f, 1.0f)][SerializeField] private float m_SEStrikeVolume;

    [SerializeField] private AudioClip m_SEDisguise;
    [Range(0.0f, 1.0f)][SerializeField] private float m_SEDisguiseVolume;

    [SerializeField] private AudioClip m_SEDiscovery;
    [Range(0.0f, 1.0f)][SerializeField] private float m_SEDiscoveryVolume;

    [SerializeField] private AudioClip m_SESteal;
    [Range(0.0f, 1.0f)][SerializeField] private float m_SEStealVolume;

    private AudioSource m_BGMSource;
    private List<AudioSource> m_SESources = new List<AudioSource>();

    [SerializeField] private int m_SESourceCount = 4;

    private bool m_IsAlert = false;

    void Awake()
    {
        // BGM 用 AudioSource をコードで生成
        m_BGMSource = gameObject.AddComponent<AudioSource>();
        m_BGMSource.loop = true;

        // SE 用 AudioSource を複数生成
        for (int i = 0; i < m_SESourceCount; i++)
        {
            var src = gameObject.AddComponent<AudioSource>();
            m_SESources.Add(src);
        }
    }

    void Start()
    {
        // 最初は通常 BGM を再生
        m_BGMSource.clip = m_BGMNormal;
        m_BGMSource.volume = m_BGMNormalVolume;
        m_BGMSource.Play();
    }

    public void OnSecurityGuardWake()
    {
        if (!m_IsAlert) return;

        m_BGMSource.Stop();
        m_BGMSource.clip = m_BGMNormal;
        m_BGMSource.volume = m_BGMNormalVolume;
        m_BGMSource.Play();

        m_IsAlert = false;
    }

    public void OnDiscoveredUnconsciousSecurityGuard()
    {
        if (m_IsAlert) return;

        OnDiscovery();

        m_BGMSource.Stop();
        m_BGMSource.clip = m_BGMAlert;
        m_BGMSource.volume = m_BGMAlertVolume;
        m_BGMSource.Play();

        m_IsAlert = true;
    }

    private void FindIdleAudioSourceAndPlay(AudioClip clip, float volume = 1.0f)
    {
        foreach (var src in m_SESources)
        {
            if (!src.isPlaying)
            {
                src.clip = clip;
                src.volume = volume;
                src.Play();
                return;
            }
        }
    }

    public void OnStrike()
    {
        FindIdleAudioSourceAndPlay(m_SEStrike, m_SEStrikeVolume);
    }

    public void OnDisguise()
    {
        FindIdleAudioSourceAndPlay(m_SEDisguise, m_SEDisguiseVolume);
    }

    public void OnDiscovery()
    {
        FindIdleAudioSourceAndPlay(m_SEDiscovery, m_SEDiscoveryVolume);
    }

    public void OnSteal()
    {
        FindIdleAudioSourceAndPlay(m_SESteal, m_SEStealVolume);
    }
}