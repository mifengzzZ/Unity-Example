using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1f;
    public float turnSpeed = 20f;

    public Animator anim;
    public Transform rootTrans;
    public Transform modelTrans;

    private bool moving = false;
    private Vector3 moveDirection = Vector3.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.moving)
        {
            // 播放动画
            this.anim.SetInteger("AnimationPar", 1);
            // 更新主角坐标
            rootTrans.position += this.moveDirection * this.speed * Time.deltaTime;
            // 更新主角朝向，使用Vector3.lerp进行插值运算，使得角度变化不那么生硬
            this.modelTrans.forward =
                Vector3.Lerp(this.modelTrans.forward, this.moveDirection, this.turnSpeed * Time.deltaTime);
        }
        else
        {
            // 播放站立
            this.anim.SetInteger("AnimationPar", 0);
        }
    }

    public void Move(Vector3 direction)
    {
        this.moveDirection = direction;
        this.moving = true;
    }

    public void Stand()
    {
        this.moving = false;
    }
}
