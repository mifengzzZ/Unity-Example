using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCreateMesh : MonoBehaviour
{
    public MeshFilter mf;
    
    // Start is called before the first frame update
    void Start()
    {
        /*构建顶点坐标 */
        var mesh = new Mesh();
        var vertices = new List<Vector3>();
        vertices.Add(new Vector3(-0.5f, -0.5f, 0));
        vertices.Add(new Vector3(-0.5f, 0.5f, 0));
        vertices.Add(new Vector3(0.5f, 0.5f, 0));
        vertices.Add(new Vector3(0.5f, -0.5f, 0));
        /*将顶点坐标设置给Mesh */
        mesh.SetVertices(vertices);

        /*构建UV坐标 */
        var uvs = new List<Vector2>();
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        /*将UV坐标设置给Mesh */
        mesh.SetUVs(0, uvs);
        
        /*三角形序列 */
        var triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        mesh.SetTriangles(triangles, 0);
        
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        this.mf.mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
