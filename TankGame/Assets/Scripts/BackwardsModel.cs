using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moltensoft.TankGame {
  public class BackwardsModel : MonoBehaviour {

    public void Start() {
        // Get the mesh filter component
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        // Get the mesh from the mesh filter
        Mesh mesh = meshFilter.mesh;
        // Rotate the mesh vertices 90 degrees around Y axis
        mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++) {
            Vector3 v = vertices[i];
            v = Quaternion.Euler(0, 90, 0) * v;
            vertices[i] = v;
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
  }
}
