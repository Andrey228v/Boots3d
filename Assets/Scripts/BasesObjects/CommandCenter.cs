using Assets.Scripts.Resurses;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.BasesObjects
{
    public class CommandCenter 
    {
        private List<Worker> _freeWorker;
        private List<BaseSlotWorker> _positionsBase;

        public event Action<Worker> OnWorkerReturn;

        public CommandCenter(List<BaseSlotWorker> positions)
        {
            AllWorker = new List<Worker>();
            _freeWorker = new List<Worker>();
            _positionsBase = positions;

            Init();
        }

        public List<Worker> AllWorker { get; private set; }

        public void SetCommandTakeResurs(Resource resurs)
        {
            Worker worker = GetFreeWorker();
            worker.GetResurs(resurs);
        }

        public void ReturnWorker(Worker worker)
        {
            if (worker.View.IsResursTake)
            {
                if (AllWorker.Contains(worker))
                {
                    AddFreeWorker(worker);
                    OnWorkerReturn?.Invoke(worker);
                }
            }            
        }

        public bool HasFreeWorkers()
        {
            return _freeWorker.Count > 0;
        }

        public bool HasUndoFreeWorkers()
        {
            return AllWorker.Count > 1;
        }

        public void AddWorker(Worker worker)
        {
            AllWorker.Add(worker);
            AddFreeWorker(worker);
        }

        public void AddFreeWorker(Worker worker)
        {
            worker.SetIsFree(true); 
            _freeWorker.Add(worker);
        }

        public Worker SetCommandBildBase(Flag flag)
        {
            Worker worker = GetFreeWorker();
            worker.GetFlag(flag);
            AllWorker.Remove(worker);

            return worker;
        }

        private Worker GetFreeWorker()
        {
            Worker worker = _freeWorker[0];
            _freeWorker.Remove(worker);
            worker.SetIsFree(false);

            return worker;
        }

        private bool TryGetFreeBasePosition(out BaseSlotWorker slot)
        {
            bool isFind = false;
            slot = null;

            for (int i = 0; i <_positionsBase.Count; i++)
            {
                if (_positionsBase[i].IsFree == true)
                {
                    isFind = true;
                    slot = _positionsBase[i];
                    break;
                }
            }

            return isFind;
        }

        private void Init()
        {
            for (int i = 0; i < AllWorker.Count; i++)
            {
                if (TryGetFreeBasePosition(out BaseSlotWorker slot))
                {
                    AllWorker[i].transform.position = slot.Position;
                    slot.SetIsFree(false);
                }
            }
        }
    }
}
