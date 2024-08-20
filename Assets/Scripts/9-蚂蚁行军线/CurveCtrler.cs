using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveCtrler : MonoBehaviour
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
    
    // 画线过程中点与点的最小距离
    private float pointsMinDistance = 0.3f;
    private List<Vector3> points;
    private Vector3 targetDir;
    [SerializeField] private float rotateSpeed = 2;

    // Start is called before the first frame update
    void Start()
    {
        points = new List<Vector3>();
        
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
        points.Add(airplane.position);
    }

    // Update is called once per frame
    void Update()
    {
        // 设置lineRenderer的第一个点
        lineRenderer.SetPosition(0, airplane.position);
        // 保存
        points[0] = airplane.position;
        
        // 计算所有点的总长度
        lineLen = CalculateTotalLen();
        
        // 计算材质的Tiling值
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
        if (Input.GetMouseButton(0))
        {
            var screenPos = Input.mousePosition;
            // 屏幕坐标转世界坐标，注意z轴是距离摄像机的距离
            var worldPos = mainCam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10));
            AddPoint(worldPos);
        }
        
        if (!reachTargetPos)
        {
            // 飞机飞向目标点
            airplane.position += targetDir * flySpeed;
            airplane.up = Vector3.Lerp(airplane.up, targetDir, Time.deltaTime * rotateSpeed);
            
            // 检测是否到达目标点
            // 这里利用点乘 (前--正,后--负)
            if (Vector3.Dot(targetPos, targetPos - airplane.position) < 0)
            {
                airplane.position = targetPos;
                reachTargetPos = true;
                points.RemoveAt(1);
                lineRenderer.positionCount = points.Count;
                lineRenderer.SetPositions(points.ToArray());
                if (points.Count >= 2)
                {
                    ResetTargetPos();
                }
                else
                {
                    lineRenderer.enabled = false;
                }
            }
        }
    }

    /// <summary>
    /// 添加新的点
    /// </summary>
    /// <param name="newPoint"></param>
    private void AddPoint(Vector2 newPoint)
    {
        if (points.Count >= 1 && Vector2.Distance(newPoint, GetLastPoint()) < pointsMinDistance)
        {
            return;
        }
        points.Add(newPoint);
        lineRenderer.enabled = true;
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, newPoint);
        Debug.Log("points.Count : " + points.Count);
        if (2 == points.Count)
        {
            Debug.Log("重新计算目标方向");
            ResetTargetPos();
        }
    }

    /// <summary>
    /// 设置飞机目标点
    /// </summary>
    private void ResetTargetPos()
    {
        targetPos = points[1];
        targetDir = (targetPos - points[0]).normalized;
        reachTargetPos = false;
    }

    /// <summary>
    /// 获取最后一个点
    /// </summary>
    /// <returns></returns>
    private Vector2 GetLastPoint()
    {
        return lineRenderer.GetPosition(points.Count - 1);
    }

    /// <summary>
    /// 计算曲线总长度
    /// </summary>
    /// <returns></returns>
    private float CalculateTotalLen()
    {
        float totalLen = 0;
        for (int i = 1, cnt = lineRenderer.positionCount; i < cnt; ++i)
        {
            totalLen += (lineRenderer.GetPosition(i) - lineRenderer.GetPosition(i - 1)).magnitude;
        }

        return totalLen;
    }
}
