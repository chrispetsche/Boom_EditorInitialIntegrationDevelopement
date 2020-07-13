using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace BubblesManualEditor
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class Placeable : MonoBehaviour
    {
        protected const float SnapDistance = .5f * .5f;
        public static bool ShowBounding = true;

        public float StartPoint { get { return (CenterT - HalfWidthT); } }
        public float EndPoint { get { return (CenterT + HalfWidthT); } }
        public float HalfWidthT { get; protected set; }
        public float CenterT { get; protected set; }
        public WallLogic Wall { get; protected set; }
        public float3 Position { get; protected set; }
        public float4 ClosestPoint { get; protected set; }
        public FeatureData data { get; protected set; }

        protected float3 offset;
        public bool vaild { get; private set; }
        public bool selected { get; private set; }

        public Placeable Next { get; private set; }
        public Placeable Prev { get; private set; }
        protected void Awake()
        {
            transform.parent = Blueprint.Instance.transform;
            var collider = GetComponent<MeshCollider>();
            collider.convex = true;
            collider.isTrigger = true;
        }
        public void SetNext(Placeable next)
        {
            Next = next;
            next.Prev = this;
        }
        public void SetPrev(Placeable prev)
        {
            Prev = Prev;
            prev.Next = this;
        }
        public void RemoveNext()
        {
            Next.Prev = null;
            Next = null;
        }
        public void RemovePrev()
        {
            Prev.Next = null;
            Prev = null;
        }
        public void SetFeature(FeatureData feature)
        {
            data = feature;
            GetComponent<MeshFilter>().mesh = data.BoundingBox;
            GetComponent<MeshRenderer>().material = data.InvaildBoundingBoxMaterial;
            GetComponent<MeshCollider>().sharedMesh = data.BoundingBox;

        }
        public void Rotate(float rotation)
        {
            transform.eulerAngles = new Vector3(0, rotation, 0);
        }
        public void UpdatePosition(float3 point)
        {
            if(Wall!=null)
            {
                Wall.RemoveCutout(this);
                Wall.RemovePlaceable(this);
                Wall = null;
                if (Next != null) RemoveNext();
                if (Prev != null) RemovePrev();
            }
            selected = true;
            var walls = Blueprint.Walls;
            float min = float.MaxValue;
            float t = .5f;
            float3 cachedPoint = point;
            
            Position = point;
            ClosestPoint = new float4(cachedPoint, ClosestPoint.w);
            transform.position = Position;
            vaild = false;
            if(!data.FreeFloating)
            {
                for (int i = 0; i < walls.Count; i++)
                {
                    WallLogic wall = walls[i];
                    float4 closest = wall.GetClosestPoint(point, out t);
                    float distance2 = MathHelper.SqrDistance(closest.xyz, point);
                    float halfWidthT = data.BoundingSize.x / 2 / walls[i].Length;
                    float startPoint = t - halfWidthT;
                    float endPoint = t + halfWidthT;
                    //Debug.Log((distance2 < Placeable.SnapDistance) + " : " + (distance2 < min) + " : " + (0 <= startPoint) + " : " + (endPoint <= 1));
                    if (distance2 < Placeable.SnapDistance && distance2 < min && 0 <= startPoint && endPoint <= 1)
                    {

                        CenterT = t;
                        vaild = true;
                        min = distance2;
                        Wall = wall;
                        ClosestPoint = closest;
                        float sign = math.sign(closest.w);

                        bool moved = false;
                        for (int j = 0; j < Wall.Cutouts.Count; j++)
                        {
                            var other = Wall.Cutouts[j];
                            float otherSign = math.sign(other.ClosestPoint.w);
                            bool intersect = (!data.Offset || !other.data.Offset) || ((data.Offset && other.data.Offset) && sign == otherSign);
                            bool overlap = (other.data.BlockCutsBottom && data.BlockCutsBottom) || (other.data.BlocksCutsTop && data.BlocksCutsTop);
                            //Debug.Log(intersect + " : " + overlap);
                            if (intersect && overlap)
                            {

                                //Debug.Log(StartPoint + " : " + other.EndPoint+":"+(StartPoint <= other.EndPoint) );
                                //Debug.Log(EndPoint + " : " + other.EndPoint + ":" + (EndPoint >= other.EndPoint));
                                //Debug.Log(StartPoint + " : " + other.StartPoint + ":" + (EndPoint >= other.StartPoint));
                                //Debug.Log(EndPoint + " : " + other.StartPoint + ":" + (EndPoint >= other.StartPoint));
                                if (StartPoint <= other.EndPoint && EndPoint >= other.EndPoint)
                                {
                                    if (moved) vaild = false;
                                    var snap = other.EndPoint - StartPoint;

                                    t += snap;
                                    CenterT = t;
                                    cachedPoint = math.lerp(Wall.GetStartPosition(), Wall.GetEndPosition(), t);
                                    ClosestPoint = new float4(cachedPoint, ClosestPoint.w);

                                    if (data.Cutout) SetPrev(other);
                                }
                                else if (StartPoint <= other.StartPoint && EndPoint >= other.StartPoint)
                                {
                                    if (moved) vaild = false;
                                    var snap = other.StartPoint - EndPoint;

                                    t += snap;
                                    CenterT = t;
                                    cachedPoint = math.lerp(Wall.GetStartPosition(), Wall.GetEndPosition(), t);
                                    ClosestPoint = new float4(cachedPoint, ClosestPoint.w);
                                    if (data.Cutout) SetNext(other);
                                }

                            }

                        }
                        for (int j = 0; j < Wall.Placeables.Count; j++)
                        {
                            var other = Wall.Placeables[j];
                            float otherSign = math.sign(other.ClosestPoint.w);
                            bool intersect = (!data.Offset || !other.data.Offset) || ((data.Offset && other.data.Offset) && sign == otherSign);
                            bool overlap = (other.data.BlockCutsBottom && data.BlockCutsBottom) || (other.data.BlocksCutsTop && data.BlocksCutsTop);
                            // Debug.Log(intersect + " : " + overlap);
                            if (intersect && overlap)
                            {

                                //Debug.Log(StartPoint + " : " + other.EndPoint + ":" + (StartPoint <= other.EndPoint));
                                //Debug.Log(EndPoint + " : " + other.EndPoint + ":" + (EndPoint >= other.EndPoint));
                                //Debug.Log(StartPoint + " : " + other.StartPoint + ":" + (EndPoint >= other.StartPoint));
                                //Debug.Log(EndPoint + " : " + other.StartPoint + ":" + (EndPoint >= other.StartPoint));
                                if (StartPoint <= other.EndPoint && EndPoint >= other.EndPoint)
                                {
                                    if (moved) vaild = false;
                                    var snap = other.EndPoint - StartPoint;

                                    t += snap;
                                    cachedPoint = math.lerp(Wall.GetStartPosition(), Wall.GetEndPosition(), t);
                                    ClosestPoint = new float4(cachedPoint, ClosestPoint.w);
                                }
                                else if (StartPoint <= other.StartPoint && EndPoint >= other.StartPoint)
                                {
                                    if (moved) vaild = false;
                                    var snap = other.StartPoint - EndPoint;

                                    t += snap;
                                    cachedPoint = math.lerp(Wall.GetStartPosition(), Wall.GetEndPosition(), t);
                                    ClosestPoint = new float4(cachedPoint, ClosestPoint.w);
                                }

                            }

                        }
                        if (StartPoint < 0 || EndPoint > 1)
                        {
                            vaild = false;
                        }



                        transform.eulerAngles = new Vector3(0, math.abs(closest.w), 0);

                        Position = ClosestPoint.xyz;
                        if (data.Offset)
                        {
                            float offsetT = data.BoundingSize.y / 2 / math.sqrt(distance2);
                            // Debug.Log(offsetT);
                            //Direction = GetPointSide(point);
                            //cachedPoint = math.lerp(Position, point, offsetT);
                            float3 movement = point - closest.xyz;
                            offset = movement * offsetT;
                            Position += offset;
                        }
                        HalfWidthT = data.BoundingSize.x / 2 / Wall.Length;
                        Position = new float3(Position.x, data.ZoffsetinMeters, Position.z);
                        transform.position = Position;
                    }
                    else
                    {
                        
                    }


                }
               
            }
            else
            {
                vaild = true;
                transform.position = Position;
            }
            
           
            UpdateVisuals();
        }
        public void UpdateVisuals()
        {
            if (vaild)
            {
                GetComponent<Renderer>().material = data.VaildBoundingBoxMaterial;
            }
            else
            {
                GetComponent<Renderer>().material = data.InvaildBoundingBoxMaterial;
            }
        }
        public void Deselect()
        {
            selected = false;
        }
        private void OnCollisionStay(Collision collision)
        {
           // if (selected) vaild = false;
        }

    }
}
    


