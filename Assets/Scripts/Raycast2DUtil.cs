using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Utilities
{
    public static class Raycast2DUtil
    {
        public static List<RaycastHit2D> SortHits(RaycastHit2D[] hits) 
        {
            List<RaycastHit2D> hitList = new List<RaycastHit2D>(hits);
            hitList.Sort(new RaycastHit2dComparer());
            return hitList;
        }
    }

    public class RaycastHit2dComparer : IComparer<RaycastHit2D>
    {

        public int Compare(RaycastHit2D x, RaycastHit2D y)
        {
            if (x.distance < y.distance)
            {
                return -1;
            }
            else if (x.distance > y.distance)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

}
