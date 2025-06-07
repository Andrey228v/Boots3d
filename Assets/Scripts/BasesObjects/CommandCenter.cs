using Assets.Scripts.Resurses;
using System;
using System.Collections.Generic;


namespace Assets.Scripts.BasesObjects
{
    public class CommandCenter : IDisposable
    {
        private List<Worker> _allWorker;
        private List<Worker> _freeWorker;
        private List<BaseSlotWorker> _positionsBase;
        private HashSet<Resurs> _arrivedResursPosition;

        public CommandCenter(List<Worker> workers, List<BaseSlotWorker> positions)
        {
            _allWorker = workers;
            _freeWorker = workers;
            _positionsBase = positions;

            Init();
        }

        private void Init()
        {
            for (int i = 0; i < _freeWorker.Count; i++) 
            {
                _freeWorker[i].RealisedWorker += AddFreeWorker;

                if (TryGetFreeBasePosition(out BaseSlotWorker slot))
                {
                    _freeWorker[i].transform.position = slot.Position;
                    slot.SetIsFree(false);
                }
            }
        }

        public void Dispose()
        {
            for(int i = 0; i < _allWorker.Count; i++)
            {
                _allWorker[i].RealisedWorker -= AddFreeWorker;
            }
        }

        public bool SetCommandTakeResurs(Resurs resurs)
        {
            bool isCommandExecute = false;

            if (TryGetFreeWorker(out Worker worker)) 
            {
                worker.SetCoomandGetResurs(resurs);
                isCommandExecute = true;
            }


            return isCommandExecute;
        }

        public bool IsFreeWorkerHave()
        {
            return _freeWorker.Count > 0;
        }

        private bool TryGetFreeWorker(out Worker worker)
        {
            bool isFind = false;
            worker = null;

            if (_freeWorker.Count > 0)
            {
                worker = _freeWorker[0];
                isFind = true;
                _freeWorker.RemoveAt(0);
            }

            return isFind;
        }

        public void AddFreeWorker(Worker worker)
        {
            _freeWorker.Add(worker);

            if(TryGetFreeBasePosition(out BaseSlotWorker slot))
            {
                worker.transform.position = slot.Position;
            }
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
