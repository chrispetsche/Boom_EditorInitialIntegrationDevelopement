using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BubblesManualEditor
{
    public class Blueprint : Singleton<Blueprint>
    {
        public static List<CornerLogic> Corners { get => Blueprint.Instance.corners; }
        private List<CornerLogic> corners = new List<CornerLogic>();
        private HashSet<CornerLogic> hashedCorners = new HashSet<CornerLogic>();

        public static List<WallLogic> Walls { get => Blueprint.Instance.walls; }
        private List<WallLogic> walls = new List<WallLogic>();//lists are faster to iterate threw then hashsets and dont want to convert constantly
        private HashSet<WallLogic> hashedWalls = new HashSet<WallLogic>();//to Keep Unique

        public static List<Placeable> Cutouts { get => Blueprint.Instance.cutouts; }
        private List<Placeable> cutouts = new List<Placeable>();//lists are faster to iterate threw then hashsets and dont want to convert constantly
        private HashSet<Placeable> hashedCutouts = new HashSet<Placeable>();//to Keep Unique

        public static List<Placeable> Placeables { get => Blueprint.Instance.placeables; }
        private List<Placeable> placeables = new List<Placeable>();//lists are faster to iterate threw then hashsets and dont want to convert constantly
        private HashSet<Placeable> hashedPlaceables = new HashSet<Placeable>();//to Keep Unique

        public static List<Dragable> Dragables { get => Blueprint.Instance.dragables; }
        private List<Dragable> dragables = new List<Dragable>();//lists are faster to iterate threw then hashsets and dont want to convert constantly
        private HashSet<Dragable> hashedDragables = new HashSet<Dragable>();//to Keep Unique

        public bool AddPlaceable(Placeable placeable)
        {
            bool add = hashedPlaceables.Add(placeable);
            if (add) placeables.Add(placeable);

            return add;
        }
        public bool RemovePlaceable(int index) => RemovePlaceable(placeables[index]);
        public bool RemovePlaceable(Placeable placeable)
        {
            bool add = hashedPlaceables.Remove(placeable);
            placeables.Remove(placeable);
            GameObject.Destroy(placeable.gameObject);
            return add;
        }
        public bool AddDragable(Dragable dragable)
        {
            bool add = hashedDragables.Add(dragable);
            if (add) dragables.Add(dragable);

            return add;
        }
        public bool RemoveDragable(int index) => RemoveDragable(dragables[index]);
        public bool RemoveDragable(Dragable dragable)
        {
            bool add = hashedDragables.Remove(dragable);
            dragables.Remove(dragable);
            GameObject.Destroy(dragable.gameObject);
            return add;
        }

        public bool AddCutout(Placeable cutout)
        {
            bool add = hashedCutouts.Add(cutout);
            if (add) Cutouts.Add(cutout);

            return add;
        }
        public bool RemoveCutout(int index) => RemoveCutout(cutouts[index]);
        public bool RemoveCutout(Placeable cutout)
        {
            bool add = hashedCutouts.Remove(cutout);
            Cutouts.Remove(cutout);
            GameObject.Destroy(cutout.gameObject);
            return add;
        }

        public bool AddWall(WallLogic wall)
        {
            bool add = hashedWalls.Add(wall);
            if (add) walls.Add(wall);
          
            return add;
        }
        public bool RemoveWall(int index) => RemoveWall(walls[index]);
        public bool RemoveWall(WallLogic wall)
        {
            bool add = hashedWalls.Remove(wall);
            walls.Remove(wall);
            GameObject.Destroy(wall.gameObject);
            return add;
        }
        public bool AddCorner(CornerLogic corner)
        {
            bool add = hashedCorners.Add(corner);
            if (add) corners.Add(corner);
            
            return add;
        }
        public bool RemoveCorner(int index) => RemoveCorner(corners[index]);
        public bool RemoveCorner(CornerLogic corner)
        {
            bool add = hashedCorners.Remove(corner);
            corners.Remove(corner);
            GameObject.Destroy(corner.gameObject);
            return add;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            for (int i=0;i<corners.Count;i++)
            {
                Gizmos.DrawSphere(corners[i].Position, .1f);
            }
        }
    }
    
}
