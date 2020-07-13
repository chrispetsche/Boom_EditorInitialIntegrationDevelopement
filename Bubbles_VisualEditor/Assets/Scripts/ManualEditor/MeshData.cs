using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubblesManualEditor
{
    [CreateAssetMenu(fileName = "MeshData", menuName = "Bubbles/MeshData", order = 1)]
    public class MeshData : ScriptableObject
    {
        public Mesh mesh;
        public Material material;
        public float MeshLength = 0, MeshDepth = 0, StartPoint = 0;
        private void OnValidate()
        {
            float maxx = float.MinValue;
            float minx = float.MaxValue;

            float maxz = float.MinValue;
            float minz = float.MaxValue;

            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                if (mesh.vertices[i].z > maxz) maxz = mesh.vertices[i].z;
                if (mesh.vertices[i].z < minz) minz = mesh.vertices[i].z;

                if (mesh.vertices[i].x > maxx) maxx = mesh.vertices[i].x;
                if (mesh.vertices[i].x < minx) minx = mesh.vertices[i].x;
            }
            MeshDepth = maxx - minx;
            MeshLength = maxz - minz;
            StartPoint = maxz;//idk why max
        }
    }
}