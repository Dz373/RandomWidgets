using UnityEngine;
using System.Collections.Generic;

public class DrawMesh : MonoBehaviour
{
    public float minDistance = .1f;
    public float lineThickness = .1f;
    public Color lineColor;
    public GameObject drawnObject;

    private Mesh mesh;
    private Vector3 lastMousePosition;

    private List<GameObject> meshes = new List<GameObject>();

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            mesh = new Mesh();

            GameObject drawnLine = Instantiate(drawnObject);
            drawnLine.GetComponent<MeshFilter>().mesh = mesh;
            meshes.Add(drawnLine);

            Vector3[] vertices = new Vector3[4];
            Vector2[] uv = new Vector2[4];
            int[] triangles = new int[6];
            Color[] colors = new Color[4];

            vertices[0] = GetMousePosition();
            vertices[1] = GetMousePosition();
            vertices[2] = GetMousePosition();
            vertices[3] = GetMousePosition();

            uv[0] = Vector2.zero;
            uv[1] = Vector2.zero;
            uv[2] = Vector2.zero;
            uv[3] = Vector2.zero;

            triangles[0] = 0;
            triangles[1] = 3;
            triangles[2] = 1;

            triangles[3] = 1;
            triangles[4] = 3;
            triangles[5] = 2;

            for (int i = 0; i < colors.Length; i++) {
                colors[i] = lineColor;
            }

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.colors = colors;
            mesh.MarkDynamic();

            lastMousePosition = GetMousePosition();
        }

        if (Input.GetMouseButton(0)) {
            if (Vector3.Distance(GetMousePosition(), lastMousePosition) > minDistance) {
                Vector3[] vertices = new Vector3[mesh.vertices.Length + 2];
                Vector2[] uv = new Vector2[mesh.uv.Length + 2];
                int[] triangles = new int[mesh.triangles.Length + 6];
                Color[] colors = new Color[mesh.colors.Length + 2];

                mesh.vertices.CopyTo(vertices, 0);
                mesh.uv.CopyTo(uv, 0);
                mesh.triangles.CopyTo(triangles, 0);
                mesh.colors.CopyTo(colors, 0);

                int vIndex = vertices.Length - 4;
                int vIndex0 = vIndex;
                int vIndex1 = vIndex + 1;
                int vIndex2 = vIndex + 2;
                int vIndex3 = vIndex + 3;

                Vector3 mouseForwardVector = (GetMousePosition() - lastMousePosition).normalized;
                Vector3 normal2D = new Vector3(0, 0, -1f);

                Vector3 newVertexUp = GetMousePosition() + Vector3.Cross(mouseForwardVector, normal2D) * lineThickness;
                Vector3 newVertexDown = GetMousePosition() + Vector3.Cross(mouseForwardVector, normal2D * -1f) * lineThickness;

                vertices[vIndex2] = newVertexUp;
                vertices[vIndex3] = newVertexDown;

                uv[vIndex2] = Vector2.zero;
                uv[vIndex3] = Vector2.zero;

                int tIndex = triangles.Length - 6;

                triangles[tIndex + 0] = vIndex0;
                triangles[tIndex + 1] = vIndex2;
                triangles[tIndex + 2] = vIndex1;

                triangles[tIndex + 3] = vIndex1;
                triangles[tIndex + 4] = vIndex2;
                triangles[tIndex + 5] = vIndex3;

                colors[vIndex2] = lineColor;
                colors[vIndex3] = lineColor;

                mesh.vertices = vertices;
                mesh.uv = uv;
                mesh.triangles = triangles;
                mesh.colors = colors;

                lastMousePosition = GetMousePosition();
            }
        }
    }

    private Vector3 GetMousePosition() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        return mousePos;
    }
}
