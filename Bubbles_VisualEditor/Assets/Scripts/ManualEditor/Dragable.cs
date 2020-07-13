using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
namespace BubblesManualEditor
{

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class Dragable : MonoBehaviour//just to not copy code
    {
        public float StartPoint { get; private set; }
        public float EndPoint { get; private set; }
        public float Length { get; private set; }
        public float3 startPosition { get; private set; }
        public float3 endPosition{ get; private set; }

        private Transform VisualHolder;
        private List<Transform> Visuals = new List<Transform>();
        private List<float2> visParts = new List<float2>();
        private MeshCollider collider;

        protected const float SnapDistance = .5f * .5f;
        public static bool ShowBounding = true;

        public float HalfWidthT { get; protected set; }
        public float CenterT { get; protected set; }
        public WallLogic Wall { get; protected set; }
        public float3 Position { get; protected set; }
        public float4 ClosestPoint { get; protected set; }
        public FeatureData data { get; protected set; }

        protected float3 offset;
        public bool vaild { get; private set; }
        public bool selected { get; private set; }
        private new void Awake()
        {
            startPosition = new float3(1, 0, 0);
            endPosition = new float3(-1, 0, 0);
            transform.parent = Blueprint.Instance.transform;
            collider = GetComponent<MeshCollider>();
            collider.convex = true;
            collider.isTrigger = true;
            GameObject go = new GameObject("VisualParent");
            VisualHolder = go.transform;
            go.transform.parent = transform;
            transform.parent = Blueprint.Instance.transform;
        }
        public void SetFeature(FeatureData feature)
        {
            data = feature;
            GetComponent<MeshFilter>().mesh = data.BoundingBox;
            GetComponent<MeshRenderer>().material = data.InvaildBoundingBoxMaterial;
            GetComponent<MeshCollider>().sharedMesh = data.BoundingBox;

        }
        public float MoveStart(float3 point)
        {
            if(data.FreeFloating)
            {
                startPosition = point;
                StartPoint = 0;
                return StartPoint;
            }
            else
            {
                if (Wall == null) return StartPoint;

                float t = 0;
                float4 closest = MathHelper.ClosestPoint(Wall.GetStartPosition(), Wall.GetEndPosition(), point, out t);
                if(t<0)
                {
                    StartPoint = 0;
                    startPosition = Wall.GetStartPosition() + offset - Position;
                }
                else
                {
                    startPosition = closest.xyz + offset - Position;
                    StartPoint = t;
                }
                UpdateVisuals();
                return t;
            }
            
        }
        public float MoveEnd(float3 point)
        {
            if (data.FreeFloating)
            {
                endPosition = point;
                EndPoint = 0;
                return StartPoint;
            }
            else
            {
                if (Wall == null) return EndPoint;

                float t = 0;
                float4 closest = MathHelper.ClosestPoint(Wall.GetStartPosition(), Wall.GetEndPosition(), point, out t);
                if (t < 0)
                {
                    EndPoint = 0;
                    endPosition = Wall.GetStartPosition() + offset-Position;
                }
                else
                {
                    endPosition = closest.xyz + offset - Position;
                    EndPoint = t;
                }
                UpdateVisuals();
                return t;
            }

        }
        public void FlipOffset()
        {
            float4 c = new float4(ClosestPoint.xyz, -ClosestPoint.w);
            ClosestPoint = c;
            UpdateVisuals(); 
        }
        public void UpdatePosition(float3 point)
        {
            selected = true;
            var walls = Blueprint.Walls;
            float min = float.MaxValue;
            float t = .5f;
            float3 cachedPoint = point;

            Position = point;
            ClosestPoint = new float4(cachedPoint, ClosestPoint.w);
            transform.position = Position;
            vaild = false;
            if (!data.FreeFloating)
            {
                for (int i = 0; i < walls.Count; i++)
                {
                    WallLogic wall = walls[i];
                    float4 closest = wall.GetClosestPoint(point, out t);
                    float distance2 = MathHelper.SqrDistance(closest.xyz, point);
                    float halfWidthT = (data.BoundingSize.x+1) / 2 / walls[i].Length;
                    float startPoint = t - halfWidthT;
                    float endPoint = t + halfWidthT;
                    //Debug.Log((distance2 < Placeable.SnapDistance) + " : " + (distance2 < min) + " : " + (0 <= startPoint) + " : " + (endPoint <= 1));
                    if (distance2 < Dragable.SnapDistance && distance2 < min && 0 <= startPoint && endPoint <= 1)
                    {

                        CenterT = t;
                        vaild = true;
                        min = distance2;
                        Wall = wall;
                        ClosestPoint = closest;
                        float sign = math.sign(closest.w);
                        StartPoint = startPoint;
                        EndPoint = endPoint;

                        
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



                        //transform.eulerAngles = new Vector3(0, math.abs(closest.w), 0);

                        Position = ClosestPoint.xyz;
                        if (data.Offset)
                        {
                            float offsetT = data.BoundingSize.x / 2 / math.sqrt(distance2);
                            // Debug.Log(offsetT);
                            //Direction = GetPointSide(point);
                            //cachedPoint = math.lerp(Position, point, offsetT);
                            float3 movement = point - closest.xyz;
                            offset = movement * offsetT;
                            Position += offset;
                        }
                        startPosition = math.lerp(wall.GetStartPosition(), wall.GetEndPosition(), StartPoint);
                        endPosition = math.lerp(wall.GetStartPosition(), wall.GetEndPosition(), EndPoint);
                        startPosition += offset - Position;
                        endPosition += offset - Position;
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
        public void SetWall(WallLogic wall, WallLogic prev, float start, float end)
        {
            Wall = wall;
            StartPoint = start;
            EndPoint = end;
            float3 prevCenter = (prev.GetEndPosition() + prev.GetStartPosition()) / 2;
            ClosestPoint = MathHelper.ClosestPoint(wall.GetStartPosition(), wall.GetEndPosition(), prevCenter);
            Position = (startPosition + endPosition) / 2;

            float distance2 = MathHelper.SqrDistance(ClosestPoint.xyz, prevCenter);
            float offsetT = data.BoundingSize.x / 2 / math.sqrt(distance2);
            // Debug.Log(offsetT);
            //Direction = GetPointSide(point);
            //cachedPoint = math.lerp(Position, point, offsetT);
            float3 movement = prevCenter - ClosestPoint.xyz;
            offset = movement * offsetT;
            Position += offset;




            startPosition = math.lerp(wall.GetStartPosition(), wall.GetEndPosition(), start)+ offset - Position;
            endPosition = math.lerp(wall.GetStartPosition(), wall.GetEndPosition(), end)+ offset - Position;
            Position = new float3(Position.x, data.ZoffsetinMeters, Position.z);
            transform.position = Position;
            vaild = true;
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
            UpdateLength();
            if (!data.FreeFloating&&Wall!=null)
            {
                CalcVisible();
                //visParts.Clear();
                //float2 section = new float2(0, 1);
                //visParts.Add(section);
            }
            else
            {
                visParts.Clear();
                float2 section = new float2(0, 1);
                visParts.Add(section);
            }
           // RenderVisuals();
        }
        public void UpdateLength()
        {
            Length = math.distance(startPosition, endPosition);
            if (Length <= 0) return;

            Mesh orginalMesh = data.BoundingBox;
            float startPoint = data.StartOffset;
            float meshLength = data.BoundingSize.y;
            float3 start = startPosition;
            float3 end = endPosition;
            //if(data.Offset)
            //{
            //    start += offset;
            //    end += offset;
            //}
            var alteredmesh = Instantiate(orginalMesh);

            Vector3[] verts = alteredmesh.vertices;
            float angle = Mathf.Atan((end.z - start.z) / (end.x - start.x)) + 90 * Mathf.Deg2Rad;
            for (int i = 0; i < verts.Length; i++)
            {
                float t = ((startPoint - verts[i].z) / meshLength);
                Debug.Log(t);
                verts[i] = new Vector3(verts[i].x * Mathf.Cos(angle) - verts[i].z * Mathf.Sin(angle), verts[i].y, verts[i].x * Mathf.Sin(angle) + verts[i].z * Mathf.Cos(angle)) + Vector3.Lerp(end, start, t);
                //new Vector3(verts[i].x * Mathf.Cos(angle) - verts[i].z * Mathf.Sin(angle), verts[i].y, verts[i].x * Mathf.Sin(angle) + verts[i].z * Mathf.Cos(angle))
            }

            alteredmesh.vertices = verts;
            collider.sharedMesh = alteredmesh;
            GetComponent<MeshFilter>().mesh = alteredmesh;
        }

        private void CalcVisible()//i was just going to get the stuff from wall logic but well
        {

            visParts.Clear();
            float2 section = new float2(StartPoint, EndPoint);
            visParts.Add(section);
            var cutoutsList = Wall.Cutouts;
            float tempT = 1;
            for (int i = 0; i < cutoutsList.Count; i++)//i dont want to double loop
            {
                for (int j = 0; j < visParts.Count; j++)
                {
                    if (cutoutsList[i].StartPoint >= visParts[j].x && cutoutsList[i].EndPoint <= visParts[j].y)
                    {
                        float2 part = visParts[j];
                        tempT = part.y;
                        part.y = cutoutsList[i].StartPoint;
                        visParts[j] = part;
                        section = new float2(cutoutsList[i].EndPoint, tempT);
                        visParts.Add(section);
                    }
                    else if (cutoutsList[i].StartPoint <= visParts[j].x && cutoutsList[i].EndPoint >= visParts[j].y)
                    {
                        float2 part = visParts[j];
                        part.y = cutoutsList[i].EndPoint;
                        visParts[j] = part;
                    }
                    else if (cutoutsList[i].StartPoint >= visParts[j].x && cutoutsList[i].EndPoint <= visParts[j].y)
                    {
                        float2 part = visParts[j];
                        part.x = cutoutsList[i].EndPoint;
                        visParts[j] = part;
                    }
                }
            }


            //UpdateVisuals();


        }
        private void RenderVisuals()
        {
            if (Visuals.Count > visParts.Count)
            {
                for (int i = Visuals.Count; i > visParts.Count; i--)
                {
                    GameObject.Destroy(Visuals[i - 1].gameObject);
                }
            }

            else if (Visuals.Count < visParts.Count)
            {
                for (int i = Visuals.Count; i < visParts.Count ; i++)
                {
                    GameObject go = new GameObject("DragableVisual", typeof(MeshFilter), typeof(MeshRenderer));
                    go.GetComponent<MeshRenderer>().sharedMaterial = DataManager.Instance.WallMesh.material;
                    Visuals.Add(go.transform);
                    go.transform.parent = VisualHolder;
                    go.transform.position = Vector3.zero;
                }
            }
            Mesh orginalMesh = data.BoundingBox;
            float startPoint = data.StartOffset;
            float meshLength = data.BoundingSize.y;



            float3 start = startPosition+ Position;
            float3 end = endPosition+ Position;
            if(!data.FreeFloating&&Wall!=null)
            {
                start = Wall.GetStartPosition();
                end = Wall.GetEndPosition();
            }
            for (int v = 0; v < Visuals.Count; v++)
            {
                float2 part = visParts[v];

                var alteredmesh = Instantiate(orginalMesh);


                Vector3[] verts = alteredmesh.vertices;
                float angle = Mathf.Atan((end.z - start.z) / (end.x - start.x)) + 90 * Mathf.Deg2Rad;
                for (int i = 0; i < verts.Length; i++)
                {
                    float t = ((startPoint - verts[i].z) / meshLength) * (part.y - part.x) + part.x;//to add again
                    verts[i] = new Vector3(verts[i].x * Mathf.Cos(angle) - verts[i].z * Mathf.Sin(angle), verts[i].y, verts[i].x * Mathf.Sin(angle) + verts[i].z * Mathf.Cos(angle)) + Vector3.Lerp(start, end, t);
                    //new Vector3(verts[i].x * Mathf.Cos(angle) - verts[i].z * Mathf.Sin(angle), verts[i].y, verts[i].x * Mathf.Sin(angle) + verts[i].z * Mathf.Cos(angle))
                }

                alteredmesh.vertices = verts;
                Visuals[v].GetComponent<MeshFilter>().mesh = alteredmesh;



            }
        }
        public void Deselect()
        {
            selected = false;
        }
    }
    
    

}
