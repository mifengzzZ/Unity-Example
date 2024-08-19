using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private AudioSource m_audiosource;
    
    public AudioClip audioclip;
    public float[] samples;
    public LineRenderer linerenderer;

    private readonly int LINERENDERER_POINT_CNT = 32;
    
    void Awake()
    {
        m_audiosource = gameObject.GetComponent<AudioSource>();
        m_audiosource.clip = audioclip;
        m_audiosource.Play();
        samples = new float[1024];
        linerenderer.positionCount = LINERENDERER_POINT_CNT;
        linerenderer.startWidth = 0.02f;
        linerenderer.endWidth = 0.02f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_audiosource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
        for (int i = 0, cnt = LINERENDERER_POINT_CNT; i < cnt; ++i)
        {
            var v = samples[i];
            linerenderer.SetPosition(i, new Vector3((i - LINERENDERER_POINT_CNT / 2) * 0.2f, v*20, -5));
        }
    }
}
