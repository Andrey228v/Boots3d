using Assets.Scripts.Resurses;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.BasesObjects
{
    public class CommandCenter 
    {
        private List<Worker> _allWorker;
        private List<Worker> _freeWorker;
        private List<BaseSlotWorker> _positionsBase;

        public CommandCenter(List<Worker> workers, List<BaseSlotWorker> positions, MapStoreResurs mapStoreResurs)
        {
            _allWorker = new List<Worker>(workers);
            _freeWorker = new List<Worker>(workers);
            _positionsBase = positions;

            Init();
        }

        private void Init()
        {
            for (int i = 0; i < _allWorker.Count; i++) 
            {
                if (TryGetFreeBasePosition(out BaseSlotWorker slot))
                {
                    _allWorker[i].transform.position = slot.Position;
                    slot.SetIsFree(false);
                }
            }
        }

        public void SetCommandTakeResurs(Resource resurs)
        {
            Worker worker = TryGetFreeWorker();
            worker.GetResurs(resurs);
        }

        public void SetCommandBackToBase(Worker worker)
        {
            worker.BackToBase();
        }

        public void SetCommandUploadResurs(Worker worker)
        {
            if (_allWorker.Contains(worker))
            {
                if (worker.View.IsResursTake)
                {
                    worker.UploadObject();
                }

                AddFreeWorker(worker);
            }
        }

        public bool IsFreeWorkerHave()
        {
            return _freeWorker.Count > 0;
        }

        private Worker TryGetFreeWorker()
        {
            Worker worker = _freeWorker[0];
            _freeWorker.RemoveAt(0);
            worker.SetIsFree(false);

            return worker;
        }

        public void AddFreeWorker(Worker worker)
        {
            worker.SetIsFree(true);
            _freeWorker.Add(worker);
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
    }
}
