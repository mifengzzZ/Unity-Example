using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCtrler : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    private Material mat;
    private Vector2 matTiling;
    private Vector2 matOffset;
    private int mainTexProperty;

    /// <summary>
    /// 飞机
    /// </summary>
    [SerializeField]
    private Transform airplane;
    
    // 线长
    private float lineLen;
    // 密度
    [SerializeField] private float density = 2f;
    
    // 定时器
    private float timer = 0;
    // 频率间隔
    [SerializeField] private float frequency = 0.03f;
    // 小蚂蚁爬行速度
    [SerializeField] private float moveSpeed = 0.04f;
    
    // 主摄像机
    private Camera mainCam;
    // 目标坐标
    private Vector3 targetPos;
    // 飞机飞行速度
    [SerializeField] private float flySpeed = 0.01f;
    // 是否到达目标位置
    private bool reachTargetPos = false;

    // Start is called before the first frame update
    void Start()
    {
        // 缓存材质示例
        mat = lineRenderer.material;
        // 缓存属性id，防止下面设置属性的时候重复计算名字的哈希
        mainTexProperty = Shader.PropertyToID("_MainTex");

        matTiling = new Vector2(20, 0);
        matOffset = new Vector2(0, 0);
        
        // 设置Tiling
        // mat.SetTextureScale(mainTexProperty, matTiling);
        // 设置Offset
        // mat.SetTextureOffset(mainTexProperty, matOffset);
        
        mainCam = Camera.main;
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, airplane.position);
        
        //----------------------------------------------------------------------------------------
        // 计算长度
        
        // 计算线长度
        lineLen = (lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0)).magnitude;
        // 根据线段长度计算Tiling
        matTiling = new Vector2(lineLen * density, 0);
        // 设置Tiling
        mat.SetTextureScale(mainTexProperty, matTiling);
        
        //----------------------------------------------------------------------------------------
        // 让线动起来
        timer += Time.deltaTime;
        if (timer >= frequency)
        {
            timer = 0;
            matOffset -= new Vector2(moveSpeed, 0);
            mat.SetTextureOffset(mainTexProperty, matOffset);
        }
        
        //----------------------------------------------------------------------------------------
        // 点击屏幕，让飞机沿着线飞过去
        if (Input.GetMouseButtonDown(0))
        {
            var screenPos = Input.mousePosition;
            // 屏幕坐标转世界坐标，注意z轴是距离摄像机的距离
            targetPos = mainCam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10));
            // 这里用up是因为飞机的朝向的方向是y轴的方向，如果你的飞机的朝向是z轴的，则用forward 
            airplane.up = targetPos - airplane.position;
            // 设置lineRenderer的终点
            lineRenderer.SetPosition(1, targetPos);
            reachTargetPos = false;
            lineRenderer.enabled = true;
        }

        if (!reachTargetPos)
        {
            // 飞机飞向目标点
            airplane.position += airplane.up * flySpeed;
            
            // 检测是否到达目标点
            // 这里利用点乘 (前--正,后--负)
            if (Vector3.Dot(airplane.up, targetPos - airplane.position) < 0)
            {
                airplane.position = targetPos;
                reachTargetPos = true;
                lineRenderer.enabled = false;
            }
        }
    }
}
