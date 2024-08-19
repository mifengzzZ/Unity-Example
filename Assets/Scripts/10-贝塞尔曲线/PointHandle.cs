using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointHandle : MonoBehaviour
{
    private Transform targetTrans;
    private Camera cam;
    private float posz;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // 鼠标左键按下
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                // 缓存射线检测到的物体
                targetTrans = hit.transform;
                // 缓存物体与摄像机的距离
                posz = targetTrans.position.z - cam.transform.position.z;
            }
        }
        // 鼠标左键抬起
        if (Input.GetMouseButtonUp(0))
        {
            // 释放碰撞体缓存
            targetTrans = null;
        }
        // 鼠标按住中
        if (null != targetTrans && Input.GetMouseButton(0))
        {
            // 鼠标的屏幕坐标转成世界坐标
            // 由于鼠标的屏幕坐标的z轴是0，所以需要使用物体 离 摄像机的距离 为z轴的值
            var targetPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, posz));
            targetTrans.position = targetPos;
        }
    }
}
