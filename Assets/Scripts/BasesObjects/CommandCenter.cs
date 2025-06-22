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

        public void SetCommandTakeResurs(Resurs resurs)
        {
            Worker worker = TryGetFreeWorker();
            worker.SetCoomandGetResurs(resurs);
        }

        public bool IsFreeWorkerHave()
        {
            return _freeWorker.Count > 0;
        }

        public int GetFreeWorkersCount()
        {
            return _freeWorker.Count;
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
