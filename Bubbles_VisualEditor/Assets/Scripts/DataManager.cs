using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubblesManualEditor
{
    public class DataManager : Singleton<DataManager>
    {
        public MeshData WallCollider;
        public MeshData WallMesh;
        public FeatureData[] Doors;
        public FeatureData[] Windows;
        public FeatureData[] Fridges;
        public FeatureData[] LowerCabinet;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
