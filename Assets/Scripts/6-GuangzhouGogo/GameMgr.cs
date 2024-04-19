using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    public Player player;
    // 左摇杆
    public JointeArm leftJointedArm;
    // 摄像机的Transform
    private Transform camTrans;
    
    // 右摇杆
    public JointeArm rightJointedArm;
    public CameraControler camCtrler;
    
    private void Awake()
    {
        this.camTrans = this.camCtrler.transform;
        Vector3.Lerp(Vector3.one, Vector3.zero, Time.deltaTime);
    }


    // Start is called before the first frame update
    void Start()
    {

        this.leftJointedArm.onDragCb = (direction) =>
        {
            // Debug.Log("按下 onDragCb");
            // 摇杆向量转世界坐标系下的向量
            var realDirect = this.camTrans.localToWorldMatrix * new Vector3(direction.x, 0, direction.y);
            realDirect.y = 0;
            // 向量归一化
            realDirect = realDirect.normalized;
            // 主角根据向量移动
            this.player.Move(realDirect);
        };
        leftJointedArm.onStopCb = () => { player.Stand(); };

        this.rightJointedArm.onDragCb = (direction) =>
        {
            this.camCtrler.RotateCam(direction);
        };
        this.rightJointedArm.onStopCb = () => { this.camCtrler.StopRotate(); };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
