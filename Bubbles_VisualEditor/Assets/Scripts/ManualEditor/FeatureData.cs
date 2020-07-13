using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace BubblesManualEditor
{
    [CreateAssetMenu(fileName = "FeatureData", menuName = "Bubbles/FeatureData", order = 2)]
    public class FeatureData : ScriptableObject
    {
        public MeshData Cap, Middle;
        public bool BlocksCutsTop, BlockCutsBottom, Offset, Cutout, FreeFloating = false;
        public Mesh BoundingBox;
        //this is bad practice to be swaping mats todo change to matpropbox and colors
        public Material VaildBoundingBoxMaterial;
        public Material InvaildBoundingBoxMaterial;
        public float2 BoundingSize;
        public float HeightOffset;
        public float StartOffset;
        private void OnValidate()
        {
            float maxx = float.MinValue;
            float minx = float.MaxValue;

            float maxz = float.MinValue;
            float minz = float.MaxValue;

            for (int i = 0; i < BoundingBox.vertices.Length; i++)
            {
                if (BoundingBox.vertices[i].z > maxz) maxz = BoundingBox.vertices[i].z;
                if (BoundingBox.vertices[i].z < minz) minz = BoundingBox.vertices[i].z;

                if (BoundingBox.vertices[i].x > maxx) maxx = BoundingBox.vertices[i].x;
                if (BoundingBox.vertices[i].x < minx) minx = BoundingBox.vertices[i].x;
            }
            BoundingSize.x = maxx - minx;
            BoundingSize.y = maxz - minz;
            StartOffset = -minz;
        }
        public float WidthInMeters, ZoffsetinMeters;
    }
}
