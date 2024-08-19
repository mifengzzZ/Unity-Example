using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityDebug;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class LineRendererBehaviour : MonoBehaviour
{
    public LineRenderer linerenderer;
    
    /// <summary>
    /// 线段的Z轴坐标
    /// </summary>
    private const float LINE_POS_Z = 10;

    /// <summary>
    /// 线段的点的数量
    /// </summary>
    private const int LINE_POS_CNT = 10;

    /// <summary>
    /// 坐标点的数组
    /// </summary>
    private Vector3[] m_linePosList = new Vector3[LINE_POS_CNT];

    /// <summary>
    /// 鼠标的屏幕坐标
    /// </summary>
    private Vector3 m_mouseScreenPos;

    /// <summary>
    /// 是否是按下
    /// </summary>
    private bool m_fire = false;

    /// <summary>
    /// 上一次是否是按下
    /// </summary>
    private bool m_firePre = false;

    /// <summary>
    /// 是否是刚按下
    /// </summary>
    private bool m_fireDown = false;

    /// <summary>
    /// 是否是抬起
    /// </summary>
    private bool m_fireUp = false;

    /// <summary>
    /// 坐标的起始和终止坐标点
    /// </summary>
    private Vector2 m_start, m_end;

    /// <summary>
    /// 坐标点索引
    /// </summary>
    private int m_linePosIndex = 0;

    /// <summary>
    /// 线段的alpha通道值
    /// </summary>
    private float m_trailAlpha = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_mouseScreenPos = Input.mousePosition;
        m_fireDown = false;
        m_fireUp = false;
    
        // 点击鼠标左键
        m_fire = Input.GetMouseButton(0);
        if (m_fire && !m_firePre)
        {
            m_fireDown = true;
        }

        if (!m_fire && m_firePre)
        {
            m_fireUp = true;
        }

        m_firePre = m_fire;
        
        // 画线
        DrawLine();
        // 设置线段颜色，主要是设置alpha值，慢慢变淡
        SetLineColor();
    }

    /// <summary>
    /// 设置线段颜色
    /// </summary>
    private void SetLineColor()
    {
        if (m_trailAlpha > 0)
        {
            // 黄色
            linerenderer.startColor = new Color(1f, 1f, 0f, m_trailAlpha);
            // 红色
            linerenderer.endColor = new Color(1f, 0f, 0f, m_trailAlpha);
            // 慢慢变透明
            m_trailAlpha -= Time.deltaTime * 2;
        }
    }

    private void DrawLine()
    {
        if (m_fireDown)
        {
            m_start = m_mouseScreenPos;
            m_end = m_mouseScreenPos;
            m_linePosIndex = 0;
            m_trailAlpha = 1;    
            AddTrailPoint();
        }
        
        // 鼠标滑动中
        if (m_fire)
        {
            m_end = m_mouseScreenPos;
            var pos1 = Camera.main.ScreenToWorldPoint(new Vector3(m_start.x, m_start.y, LINE_POS_Z));
            var pos2 = Camera.main.ScreenToWorldPoint(new Vector3(m_end.x, m_end.y, LINE_POS_Z));
            // 滑动距离超过0.1才算一次有效的滑动
            if (Vector3.Distance(pos1, pos2) > 0.01f)
            {
                m_trailAlpha = 1;
                ++m_linePosIndex;
                AddTrailPoint();
            }

            m_start = m_mouseScreenPos;
        }
        
        SetLineRendererPos();
    }

    /// <summary>
    /// 添加坐标点到数组中
    /// </summary>
    private void AddTrailPoint()
    {
        if (m_linePosIndex < LINE_POS_CNT)
        {
            for (int i = m_linePosIndex; i < LINE_POS_CNT; ++i)
            {
                m_linePosList[i] = Camera.main.ScreenToWorldPoint(new Vector3(m_end.x, m_end.y, LINE_POS_Z));
            }
        }
        else
        {
            for (int i = 0; i < LINE_POS_CNT - 1; ++i)
            {
                m_linePosList[i] = m_linePosList[i + 1];
            }

            m_linePosList[LINE_POS_CNT - 1] = Camera.main.ScreenToWorldPoint(new Vector3(m_end.x, m_end.y, LINE_POS_Z));
        }

    }
    
    /// <summary>
    /// 将坐标数组赋值给LineRenderer组件
    /// </summary>
    private void SetLineRendererPos()
    {
        for (int i = 0; i < LINE_POS_CNT; ++i)
        {  
            linerenderer.SetPosition(i, m_linePosList[i]);
        }
    }
}
