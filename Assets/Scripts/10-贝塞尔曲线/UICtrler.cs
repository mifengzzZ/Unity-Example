using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICtrler : MonoBehaviour
{
    public GameObject bezierCurve2;

    public Toggle toggleBezier;
    
    // Start is called before the first frame update
    void Start()
    {
        toggleBezier.onValueChanged.AddListener((v) =>
        {
            bezierCurve2.SetActive(v);
        });
        
        // 默认显示三阶贝塞尔曲线
        bezierCurve2.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
