using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
namespace BubblesManualEditor
{
    public class CornerLogic : MonoBehaviour
    {
        public float3 Position { get; private set; }
        private List<WallLogic> walls = new List<WallLogic>();
        private HashSet<WallLogic> hashedWalls = new HashSet<WallLogic>();//to Keep Unique

        private void Awake()
        {
            transform.parent = Blueprint.Instance.transform;
        }
        public void ChangePosition(float3 point)
        {
            Position = point;
            //Debug.Log(point);
            UpdateWalls();
        }
        public bool AddWall(WallLogic wall)
        {
            bool add = hashedWalls.Add(wall);
            if (add) walls.Add(wall);
            walls.Sort();
           //UpdateWalls();
            return add;
        }
        public bool RemoveWall(WallLogic wall)
        {
            bool add = hashedWalls.Remove(wall);
            walls.Remove(wall);
            if(walls.Count==0)
            {
                Blueprint.Instance.RemoveCorner(this);
                GameObject.Destroy(gameObject);
            }
            walls.Sort();
            
            return add;
        }
        public WallLogic GetWall(int i)
        {
            return walls[i];
        }
        public WallLogic GetNextWall(WallLogic wall)
        {
            if (walls.Count == 1) return null;
            int i = walls.IndexOf(wall);
            if (i == walls.Count - 1) return walls[0];
            else return walls[i+1];
        }
        public WallLogic GetPreviousWall(WallLogic wall)
        {
            if (walls.Count == 1) return null;
            int i = walls.IndexOf(wall);
            if (i == 0) return walls[walls.Count - 1];
            else return walls[i-1];
        }
        public int GetWallsCount()
        {
            return walls.Count;
        }
        public void MergeOther(CornerLogic other)
        {
            int index = Blueprint.Corners.IndexOf(this);
            for (int i = 0; i < other.walls.Count; i++)
            {
                bool add = hashedWalls.Add(other.walls[i]);
                if (add)
                {
                    walls.Add(other.walls[i]);
                    if (other.walls[i].StartCorner.Position.Equals( Position))
                    {
                        other.walls[i].SetStartCorner(this);
                    }
                    else
                    {
                        other.walls[i].SetEndCorner(this);
                    }
                }
                else
                {//not sure what i was thinking
                    //hashedWalls.Remove(other.walls[i]);
                    //walls.Remove(other.walls[i]);
                    //Blueprint.Instance.RemoveWall(other.walls[i]);
                }
            }
            Blueprint.Instance.RemoveCorner(other);
           
            walls.Sort();
            UpdateWalls();
        }
        public void AxisAlignWithOther(CornerLogic other)
        {
            float x = Mathf.Abs(Position.x - other.Position.x);
            float z = Mathf.Abs(Position.z - other.Position.z);
            if (x < z) Position = new float3(other.Position.x, Position.yz);
            else Position = new float3(Position.xy, other.Position.z);
            UpdateWalls();
        }
        public void SortWalls()
        {
            walls.Sort();
        }
        public void SnapToOther(CornerLogic other)
        {
            Position = other.Position;
            UpdateWalls();
        }
        private void UpdateWalls()
        {
            
            for (int i = 0; i < walls.Count; i++)
            {
                walls[i].UpdateLength();
            }
        }
        private void OnDestroy()
        {
            for (int i = 0; i < walls.Count; i++)
            {
                GameObject.Destroy(walls[i]);
            }
        }
    }
}