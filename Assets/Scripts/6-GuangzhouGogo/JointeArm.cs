using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JointeArm : ScrollRect, IPointerDownHandler
{
    public Action<Vector2> onDragCb;
    public Action onStopCb;

    protected float mRadius = 0f;
    
    private Transform trans;
    private RectTransform bgTrans;
    private Camera uiCam;
    private Vector3 originalPos;
    
    protected override void Awake()
    {
        base.Awake();
        this.trans = this.transform;
        this.bgTrans = this.trans.Find("bg") as RectTransform;
        this.uiCam = GameObject.Find("UICamera").GetComponent<Camera>();
        this.originalPos = this.trans.localPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        // 计算摇杆块的半径
        this.mRadius = this.bgTrans.sizeDelta.x * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            // 松手时，遥感复位
            this.trans.localPosition = originalPos;
            this.content.localPosition = Vector3.zero;
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        var contentPosition = this.content.anchoredPosition;
        if (contentPosition.magnitude > this.mRadius)
        {
            contentPosition = contentPosition.normalized * this.mRadius;
            SetContentAnchoredPosition(contentPosition);
        }
        // Debug.Log("遥感滑动方向：" + contentPosition);

        if (null != this.onDragCb)
        {
            this.onDragCb(contentPosition);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        // Debug.Log("遥感拖动结束");
        if (null != this.onStopCb)
        {
            this.onStopCb();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 点击到遥感的区域，遥感移动到点击的位置
        this.trans.position = this.uiCam.ScreenToWorldPoint(eventData.position);
        this.trans.localPosition = new Vector3(this.trans.localPosition.x, this.trans.localPosition.y, 0);
    }
}
