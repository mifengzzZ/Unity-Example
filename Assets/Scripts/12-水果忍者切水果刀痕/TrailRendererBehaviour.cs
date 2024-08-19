using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRendererBehaviour : MonoBehaviour
{
    public TrailRenderer trailrenderer;

    private Transform m_selfTras;

    private const float LINE_POS_Z = 10;

    private void Awake()
    {
        m_selfTras = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 启用此属性后，Unity 会在轨迹中添加新点。禁用此属性后，Unity 不会向轨迹中添加新点。使用此属性可暂停和恢复轨迹生成功能
            trailrenderer.emitting = false;
            var mousPos = Input.mousePosition;
            m_selfTras.position = Camera.main.ScreenToWorldPoint(new Vector3(mousPos.x, mousPos.y, LINE_POS_Z));
            return;
        }

        if (Input.GetMouseButton(0))
        {
            trailrenderer.emitting = true;
            var mousPos = Input.mousePosition;
            m_selfTras.position = Camera.main.ScreenToWorldPoint(new Vector3(mousPos.x, mousPos.y, LINE_POS_Z));
        }
    }
}
