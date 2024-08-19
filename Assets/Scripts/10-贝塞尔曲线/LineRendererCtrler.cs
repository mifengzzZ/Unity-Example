using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BezierCurve))]
public class LineRendererCtrler : MonoBehaviour
{
    [SerializeField]
    private int nodeCount = 20;

    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private BezierCurve bezier;

    private void Awake()
    {
        lineRenderer.positionCount = nodeCount + 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 更新LineRenderer的点
        for (int i = 0; i <= nodeCount; ++i)
        {
            Vector3 to = bezier.formula(i / (float)nodeCount);
            lineRenderer.SetPosition(i, to);
        }
    }
}
