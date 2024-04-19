using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    // 摄像机看向的物体
    public Transform lookAt;
    // 摄像机自身的Transform
    public Transform camTransform;
    // 摄像机与目标物体的距离
    public float distance = 1.2f;

    private float currentX = 0.0f;
    private float currentY = 20.0f;
    
    // 旋转速度
    public float rotateSpeed = 0.01f;
    
    private bool rotating;
    private Vector2 rotateDelta;
    
    // 限制旋转范围
    private const float Y_ANGLE_MIN = 10f;
    private const float Y_ANGLE_MAX = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.camTransform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.rotating)
        {
            // 限制旋转范围
            this.currentX += this.rotateDelta.x;
            this.currentY += this.rotateDelta.y;
            this.currentY = Mathf.Clamp(this.currentY, CameraControler.Y_ANGLE_MIN, CameraControler.Y_ANGLE_MAX);
        }
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(this.currentY, this.currentX, 0);
        // 设置相机坐标
        this.camTransform.position = this.lookAt.position + rotation * dir;
        // 设置相机角度看向目标物体
        this.camTransform.LookAt(this.lookAt.position);
    }

    public void RotateCam(Vector2 delta)
    {
        this.rotateDelta = delta * this.rotateSpeed;
        this.rotating = true;
    }

    public void StopRotate()
    {
        this.rotating = false;
    }
    
}
