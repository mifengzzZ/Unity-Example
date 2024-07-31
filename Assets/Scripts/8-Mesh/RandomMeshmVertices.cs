using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMeshmVertices : MonoBehaviour
{
    /*Mesh的实例 */
    private MeshFilter m_meshFilter;
    
    /*顶点的原始坐标 */
    private Vector3[] m_originalVertices;
    
    /*用于渲染带骨骼动画的模型 蒙皮网格渲染器 */
    private SkinnedMeshRenderer m_skinnedMeshRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        this.m_meshFilter = this.GetComponent<MeshFilter>();
        this.m_originalVertices = this.m_meshFilter.mesh.vertices;
        Debug.Log("输入值：" + this.m_originalVertices);
    }

    // Update is called once per frame
    void Update()
    {
        // 随机修改顶点坐标
        Vector3[] vertices = this.m_meshFilter.mesh.vertices;
        for (int i = 0, len = this.m_originalVertices.Length; i < len; ++i)
        {
            var v = this.m_originalVertices[i];
            vertices[i] = v + Random.Range(-0.1f, 0.1f) * Vector3.one;
        }

        this.m_meshFilter.mesh.vertices = vertices;
        this.m_meshFilter.mesh.RecalculateNormals();
    }
}
