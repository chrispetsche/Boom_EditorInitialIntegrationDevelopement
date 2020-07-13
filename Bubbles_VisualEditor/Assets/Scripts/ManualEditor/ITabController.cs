using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
namespace BubblesManualEditor
{
    public abstract class TabController : MonoBehaviour
    {
        public abstract void HandleInput(bool tap, bool hold, float2 mousePosition);
        public abstract void HandleRelease(float2 mousePosition);
        public abstract void Delete();
        public abstract void Rotate(float rot);
    }
}