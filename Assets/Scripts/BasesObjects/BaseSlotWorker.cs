using UnityEngine;

namespace Assets.Scripts.BasesObjects
{
    public class BaseSlotWorker
    {
        public Vector3 Position { get; private set; }
        public bool IsFree { get; private set; }

        public BaseSlotWorker(Vector3 position, bool isFree)
        {
            Position = position;
            IsFree = isFree;
        }

        public void SetIsFree(bool isFree) 
        {
            IsFree = isFree;
        }
    }
}
