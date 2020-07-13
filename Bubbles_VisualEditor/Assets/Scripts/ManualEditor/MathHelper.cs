using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
namespace BubblesManualEditor
{

    public static class MathHelper
    {

        public static float AngleSigned(float3 from, float3 to) => AngleSigned(from, to, new float3(0, 1, 0));

        public static float AngleSigned(float3 from, float3 to, float3 axis)
        {
            float angle = math.acos(math.dot(math.normalize(from), math.normalize(to)));
            float sign = math.sign(math.dot(axis, math.cross(from, to)));
            return math.degrees(angle * sign);
        }
        // w is rotation and sign
        public static float4 ClosestPoint(float3 lineStart,float3 lineEnd,float3 point)
        {
            //i got this off stack overflow https://stackoverflow.com/questions/3120357/get-closest-point-to-a-line
            float3 s2p = lineStart - point;
            float3 s2e = lineStart - lineEnd;
            float s2eMag = s2e.x * s2e.x + s2e.z * s2e.z;

            float s2pDots2b = s2p.x * s2e.x + s2p.z * s2e.z;

            float t = s2pDots2b / s2eMag;

            float3 ClosestPoint = lineStart - s2e * t;
            float rotation = -Mathf.Atan(s2e.z / s2e.x) * Mathf.Rad2Deg;
            rotation = rotation < 0 ? rotation + 360 : rotation;
            float sign = math.sign(AngleSigned(s2e, s2p))*rotation;
            
            return new float4(ClosestPoint, sign);
        }
        public static float4 ClosestPoint(float3 lineStart, float3 lineEnd, float3 point, out float t)
        {
            //i got this off stack overflow https://stackoverflow.com/questions/3120357/get-closest-point-to-a-line
            float3 s2p = lineStart - point;
            float3 s2e = lineStart - lineEnd;
            float s2eMag = s2e.x * s2e.x + s2e.z * s2e.z;

            float s2pDots2b = s2p.x * s2e.x + s2p.z * s2e.z;

            t = s2pDots2b / s2eMag;

            float3 ClosestPoint = lineStart - s2e * t;
            float rotation = -Mathf.Atan(s2e.z / s2e.x) * Mathf.Rad2Deg;
            rotation = rotation < 0 ? rotation + 360 : rotation;
            float sign = math.sign(AngleSigned(s2e, s2p)) * rotation;

            return new float4(ClosestPoint, sign);
        }
        public static float SqrDistance(float3 v1, float3 v2)
        {
            float3 v3 = v2 - v1;
            v3 *= v3;
            return v3.x + v3.y + v3.z;
        }
        
    }

}
