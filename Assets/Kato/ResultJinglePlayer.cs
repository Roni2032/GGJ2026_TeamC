using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultJinglePlayer : MonoBehaviour
{
    [SerializeField] private AudioClip m_SuccessJingle;
    [Range(0.0f, 1.0f)][SerializeField] private float m_SuccessJingleVolume;

    [SerializeField] private AudioClip m_FailureJingle;
    [Range(0.0f, 1.0f)][SerializeField] private float m_FailureJingleVolume;

    private AudioSource m_JingleSource;

    // Start is called before the first frame update
    void Start()
    {
        m_JingleSource = gameObject.AddComponent<AudioSource>();

        if (GameManager.Instance.ResultType == ResultType.Clear)
        {
            m_JingleSource.clip = m_SuccessJingle;
            m_JingleSource.volume = m_SuccessJingleVolume;
            m_JingleSource.Play();
        }
        else
        {
            m_JingleSource.clip = m_FailureJingle;
            m_JingleSource.volume = m_FailureJingleVolume;
            m_JingleSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
