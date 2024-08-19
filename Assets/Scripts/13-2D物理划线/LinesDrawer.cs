using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 划线控制器
/// </summary>
public class LinesDrawer : MonoBehaviour
{
    public GameObject linePrefab;

    public LayerMask cantDrawOverLayer;
    private int cantDrawOverLayerIndex;

    /// <summary>
    /// 渐变色
    /// </summary>
    [Space(30)]
    public Gradient lineColor;

    public float linePointsMinDistance;
    public float lineWidth;

    private Line currentLine;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        cantDrawOverLayerIndex = LayerMask.NameToLayer("CantDrawOver");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("鼠标左键按下");
            BeginDraw();
        }

        if (null != currentLine)
        {
            Debug.Log("持续绘制");
            Draw();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("鼠标左键抬起");
            EndDraw();
        }
    }
    
    /// <summary>
    /// 开始画线
    /// </summary>
    private void BeginDraw()
    {
        // 实例化线预设
        currentLine = Instantiate(linePrefab, transform).GetComponent<Line>();
        // 设置参数
        currentLine.UsePhysics(false);
        currentLine.SetLineColor(lineColor);
        currentLine.SetPositionsMinDistance(linePointsMinDistance);
        currentLine.SetLineWidth(lineWidth);
    }
    
    /// <summary>
    /// 画线中
    /// </summary>
    private void Draw()
    {
        Debug.Log("当前鼠标所在位置" + Input.mousePosition);
        var pos = cam.ScreenToWorldPoint(Input.mousePosition);
        // 防止线与线之间交叉
        RaycastHit2D hit = Physics2D.CircleCast(pos, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer);
        Debug.Log("hit : " + hit);
        if (hit)
        {
            Debug.Log("结束绘制");
            EndDraw();
        }
        else
        {
            Debug.Log("添加点" + pos);
            currentLine.AddPoint(pos);
        }
    }

    /// <summary>
    /// 画线结束
    /// </summary>
    private void EndDraw()
    {
        if (null == currentLine)
        {
            return;
        }

        if (currentLine.pointCount < 2)
        {
            Destroy(currentLine.gameObject);
        }
        else
        {
            currentLine.gameObject.layer = cantDrawOverLayerIndex;
            currentLine.UsePhysics(true);
            currentLine = null;
        }
    }

}
