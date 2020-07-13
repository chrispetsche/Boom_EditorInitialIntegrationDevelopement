using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System.Linq;

namespace BubblesManualEditor
{
    [RequireComponent(typeof(MeshCollider))]
    public class WallLogic : MonoBehaviour, IComparable<WallLogic>
    {
        public MeshData WallMesh;

        private MeshCollider collider;
        public CornerLogic StartCorner { get; private set; }
        public CornerLogic EndCorner { get; private set; }
        public float Length { get; private set; }

        private Transform VisualHolder;
        private List<Transform> Visuals = new List<Transform>();
        private List<float2> visParts = new List<float2>();
        public List<Placeable> Cutouts { get { return cutouts.ToList(); } }
        private HashSet<Placeable> cutouts = new HashSet<Placeable>();

        public List<Placeable> Placeables { get { return placeables.ToList(); } }
        private HashSet<Placeable> placeables = new HashSet<Placeable>();

        public List<Dragable> Dragables { get { return dragables.ToList(); } }
        private HashSet<Dragable> dragables = new HashSet<Dragable>();

        private void Awake()
        {

            collider = GetComponent<MeshCollider>();
            collider.convex = true;
            collider.isTrigger = true;
            GameObject go = new GameObject("VisualParent");
            VisualHolder = go.transform;
            go.transform.parent = transform;
            transform.parent = Blueprint.Instance.transform;


        }
        public void SetStartCorner(CornerLogic corner)//need to run calcs when swaping
        {
            if (corner == StartCorner) return;
            if (StartCorner != null) StartCorner.RemoveWall(this);
            StartCorner = corner;

            if (StartCorner != null && EndCorner != null) Length = math.distance(GetStartPosition(), GetEndPosition());

            corner.AddWall(this);
            if (StartCorner == null && EndCorner == null) return;
            UpdateLength();
        }
        public void SetEndCorner(CornerLogic corner)
        {
            if (corner == EndCorner) return;
            if (EndCorner != null) EndCorner.RemoveWall(this);
            EndCorner = corner;

            if (StartCorner != null && EndCorner != null) Length = math.distance(GetStartPosition(), GetEndPosition());

            corner.AddWall(this);
            if (StartCorner == null && EndCorner == null) return;
            UpdateLength();


        }
        public void Vaildate()
        {
            float3 foo = StartCorner.Position - EndCorner.Position;
            if(foo.x<0&&foo.x<foo.z)
            {
                var temp = EndCorner;
                EndCorner = StartCorner;
                StartCorner = temp;
            }
            else if(foo.z < 0 && foo.z < foo.x)
            {
                var temp = EndCorner;
                EndCorner = StartCorner;
                StartCorner = temp;
            }
        }
        public bool AddCutout(Placeable cutout)
        {
            bool add = cutouts.Add(cutout);
            if (add) Cutouts.Add(cutout);
            UpdateLength();
            return add;
        }
        public bool RemoveCutout(Placeable cutout)
        {
            bool add = cutouts.Remove(cutout);
            Cutouts.Remove(cutout);
            UpdateLength();
            return add;
        }
        public bool AddPlaceable(Placeable placeable)
        {
            bool add = placeables.Add(placeable);
            if (add) Placeables.Add(placeable);
            
            return add;
        }
        public bool RemovePlaceable(Placeable placeable)
        {
            bool add = placeables.Remove(placeable);
            Placeables.Remove(placeable);
            
            return add;
        }

        public bool AddDragable(Dragable dragable)
        {
            bool add = dragables.Add(dragable);
            if (add) Dragables.Add(dragable);

            return add;
        }
        public bool RemoveDragable(Dragable dragable)
        {
            bool add = dragables.Remove(dragable);
            Dragables.Remove(dragable);

            return add;
        }
        public void UpdateLength()
        {
            if(StartCorner == null || EndCorner == null) return;
            Length = math.distance(GetStartPosition(), GetEndPosition());
            if (Length<=0) return;
            
            Mesh orginalMesh = WallMesh.mesh;
            float startPoint = WallMesh.StartPoint;
            float meshLength = WallMesh.MeshLength;
            float3 start = GetStartPosition();
            float3 end = GetEndPosition();
            var alteredmesh = Instantiate(orginalMesh);

            Vector3[] verts = alteredmesh.vertices;
            float angle = Mathf.Atan((end.z - start.z) / (end.x - start.x)) + 90 * Mathf.Deg2Rad;
            for (int i = 0; i < verts.Length; i++)
            {
                float t = ((startPoint - verts[i].z) / meshLength);
                verts[i] = new Vector3(verts[i].x * Mathf.Cos(angle) - verts[i].z * Mathf.Sin(angle), verts[i].y, verts[i].x * Mathf.Sin(angle) + verts[i].z * Mathf.Cos(angle)) + Vector3.Lerp(start, end, t);
                //new Vector3(verts[i].x * Mathf.Cos(angle) - verts[i].z * Mathf.Sin(angle), verts[i].y, verts[i].x * Mathf.Sin(angle) + verts[i].z * Mathf.Cos(angle))
            }

            alteredmesh.vertices = verts;
            collider.sharedMesh = alteredmesh;

            CalcVisible();
        }
        private void CalcVisible()
        {

            if (StartCorner == null || EndCorner == null) return;

            visParts.Clear();
            float2 section = new float2(0, 1);
            visParts.Add(section);
            var cutoutsList = Cutouts;
            float tempT = 1;
            for(int i=0; i< cutoutsList.Count;i++)//i dont want to double loop
            {
                for(int j=0;j< visParts.Count;j++)
                {
                    if(cutoutsList[i].StartPoint >= visParts[j].x && cutoutsList[i].EndPoint <= visParts[j].y)
                    {
                        float2 part = visParts[j];
                        tempT = part.y;
                        part.y = cutoutsList[i].StartPoint;
                        visParts[j] = part;
                        section = new float2(cutoutsList[i].EndPoint, tempT);
                        visParts.Add(section);
                    }
                    else if (cutoutsList[i].StartPoint >= visParts[j].x && cutoutsList[i].EndPoint >= visParts[j].y && cutoutsList[i].StartPoint <= visParts[j].y)
                    {
                        float2 part = visParts[j];
                        part.y = cutoutsList[i].StartPoint;
                        visParts[j] = part;
                    }
                    else if (cutoutsList[i].StartPoint <= visParts[j].x && cutoutsList[i].EndPoint <= visParts[j].y && cutoutsList[i].EndPoint >= visParts[j].x)
                    {
                        float2 part = visParts[j];
                        part.x = cutoutsList[i].EndPoint;
                        visParts[j] = part;
                    }
                }
            }
            List<float2> remove = new List<float2>();
            for (int j = 0; j < visParts.Count; j++)
            {
                if(math.distance(visParts[j].x, visParts[j].y)<.0001f)
                {
                    remove.Add(visParts[j]);
                }
            }
            for (int j = 0; j < remove.Count; j++)
            {
                visParts.Remove(remove[j]);
            }


            UpdateVisuals();


        }
        
        private void UpdateVisuals()
        {

            if (Visuals.Count > visParts.Count)
            {
                for (int i = Visuals.Count; i > cutouts.Count; i--)
                {
                    GameObject.Destroy(Visuals[i-1]);
                }
            }

            else if (Visuals.Count < visParts.Count)
            {
                for (int i = Visuals.Count; i < cutouts.Count + 1; i++)
                {
                    GameObject go = new GameObject("WallVisual", typeof(MeshFilter), typeof(MeshRenderer));
                    go.GetComponent<MeshRenderer>().sharedMaterial = DataManager.Instance.WallMesh.material;
                    Visuals.Add(go.transform);
                    go.transform.parent = VisualHolder;
                }
            }
            Mesh orginalMesh = WallMesh.mesh;
            float startPoint = WallMesh.StartPoint;
            float meshLength = WallMesh.MeshLength;


            var start = GetStartPosition();
            var end = GetEndPosition();
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
        public float3 GetStartPosition()
        {
            return StartCorner.Position;
        }
        public float3 GetEndPosition()
        {
            return EndCorner.Position;
        }
        public float4 GetClosestPoint(float3 point)
        {
            return MathHelper.ClosestPoint(GetStartPosition(), GetEndPosition(), point);
        }
        public float4 GetClosestPoint(float3 point, out float t)
        {
            return MathHelper.ClosestPoint(GetStartPosition(), GetEndPosition(), point, out t);
        }





        void OnDestroy()//clean up stuff
        {
          StartCorner.RemoveWall(this);
           EndCorner.RemoveWall(this);
            foreach(var p in placeables)
            {
                GameObject.Destroy(p.gameObject);
            }
            foreach (var p in cutouts)
            {
                GameObject.Destroy(p.gameObject);
            }
        }


        public int CompareTo(WallLogic other)
        {
            if (other == null) return 1;
            float angle = 0, otherAngle = 0;
            if (StartCorner == other.StartCorner)
            {

                angle = MathHelper.AngleSigned(StartCorner.Position, EndCorner.Position);
                otherAngle = MathHelper.AngleSigned(other.StartCorner.Position, other.EndCorner.Position);
            }
            else if (StartCorner == other.EndCorner)
            {
                angle = MathHelper.AngleSigned(StartCorner.Position, EndCorner.Position);
                otherAngle = MathHelper.AngleSigned(other.EndCorner.Position, other.StartCorner.Position);
            }
            else if (EndCorner == other.EndCorner)
            {
                angle = MathHelper.AngleSigned(EndCorner.Position, StartCorner.Position);
                otherAngle = MathHelper.AngleSigned(other.EndCorner.Position, other.StartCorner.Position);
            }
            else
            {
                angle = MathHelper.AngleSigned(EndCorner.Position, StartCorner.Position);
                otherAngle = MathHelper.AngleSigned(other.StartCorner.Position, other.EndCorner.Position);
            }
            if (angle < 0) angle += 360;
            if (otherAngle < 0) otherAngle += 360;
            return angle.CompareTo(otherAngle);
        }
    }

}
