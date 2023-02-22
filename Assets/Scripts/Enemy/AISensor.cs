using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISensor : MonoBehaviour
{
    public float m_distance = 5;
    public float m_angle = 30;
    public float m_height = 1.0f;
    public Color m_meshColor = Color.red;
    public int scanFrequency = 30;

    Collider[] colliders = new Collider[50];
    Mesh m_mesh;
    int count;
    float scanInterval;
    float scanTimer;

    // Start is called before the first frame update
    void Start()
    {
        scanInterval = 1.0f / scanFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        scanTimer -= Time.deltaTime;
        if(scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }

    private void Scan()
    {
        //count = Physics.OverlapSphereNonAlloc()
    }


    Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();
        //Need to split the mesh into segments like a pizza slice.
        //Each slice has 4 triangles, two for the far size and two for the top and bottom.
        //The 2 + 2 part is for the left and right side
        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -m_angle, 0) * Vector3.forward * m_distance;
        Vector3 bottomRight = Quaternion.Euler(0, m_angle, 0) * Vector3.forward * m_distance;


        Vector3 topCenter = bottomCenter + Vector3.up * m_height;
        Vector3 topLeft = bottomLeft + Vector3.up * m_height;
        Vector3 topRight = bottomRight + Vector3.up * m_height;

        int vertInArray = 0;

        //left side of wedge
        vertices[vertInArray++] = bottomCenter;
        vertices[vertInArray++] = bottomLeft;
        vertices[vertInArray++] = topLeft;

        vertices[vertInArray++] = topLeft;
        vertices[vertInArray++] = topCenter;
        vertices[vertInArray++] = bottomCenter;

        //right side of wedge
        vertices[vertInArray++] = bottomCenter;
        vertices[vertInArray++] = topCenter;
        vertices[vertInArray++] = topRight;

        vertices[vertInArray++] = topRight;
        vertices[vertInArray++] = bottomRight;
        vertices[vertInArray++] = bottomCenter;

        //subdividing the wedge so it isn't just a triangle but more in a cone shape
        float currentAngle = -m_angle;
        float deltaAngle = (m_angle * 2) / segments;
        for(int i = 0; i < segments; i++)
        {

            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * m_distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * m_distance;

            topLeft = bottomLeft + Vector3.up * m_height;
            topRight = bottomRight + Vector3.up * m_height;

            //far side of wedge
            vertices[vertInArray++] = bottomLeft;
            vertices[vertInArray++] = bottomRight;
            vertices[vertInArray++] = topRight;

            vertices[vertInArray++] = topRight;
            vertices[vertInArray++] = topLeft;
            vertices[vertInArray++] = bottomLeft;

            //top of wedge
            vertices[vertInArray++] = topCenter;
            vertices[vertInArray++] = topLeft;
            vertices[vertInArray++] = topRight;

            //bottom of wedge
            vertices[vertInArray++] = bottomCenter;
            vertices[vertInArray++] = bottomRight;
            vertices[vertInArray++] = bottomLeft;

            currentAngle += deltaAngle;
        }
        

        for(int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        m_mesh = CreateWedgeMesh();
        scanInterval = 1.0f / scanFrequency;
    }

    private void OnDrawGizmos()
    {
        if(m_mesh)
        {
            Gizmos.color = m_meshColor;
            Gizmos.DrawMesh(m_mesh, transform.position, transform.rotation);
        }
    }
}
