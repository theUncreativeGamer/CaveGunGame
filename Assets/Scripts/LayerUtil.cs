using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class LayerUtil
    {
        public static bool LayerIsInMask(int layer, LayerMask mask)
        {
            return mask == (mask | (1 << layer));
        }
    }
    
}
