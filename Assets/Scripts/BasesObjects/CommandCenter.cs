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
        private MapStoreResurs _mapStoreResurs;

        public CommandCenter(List<Worker> workers, List<BaseSlotWorker> positions, MapStoreResurs mapStoreResurs)
        {
            _allWorker = new List<Worker>(workers);
            _freeWorker = new List<Worker>(workers);
            _positionsBase = positions;
            _mapStoreResurs = mapStoreResurs;

            Init();
        }

        private void Init()
        {
            for (int i = 0; i < _allWorker.Count; i++) 
            {
                _allWorker[i].RealisedWorker += AddFreeWorker;
                _allWorker[i].View.OnTakeResurs += _mapStoreResurs.TakeResurs;
                _allWorker[i].View.OnUploadObject += _mapStoreResurs.UploadResurs;

                if (TryGetFreeBasePosition(out BaseSlotWorker slot))
                {
                    _allWorker[i].transform.position = slot.Position;
                    slot.SetIsFree(false);
                }
            }
        }

        public void Dispose()
        {
            for(int i = 0; i < _allWorker.Count; i++)
            {
                _allWorker[i].RealisedWorker -= AddFreeWorker;
                _allWorker[i].View.OnTakeResurs -= _mapStoreResurs.TakeResurs;
                _allWorker[i].View.OnUploadObject -= _mapStoreResurs.UploadResurs;
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

        public bool IsFreeWorkerHave()
        {
            return _freeWorker.Count > 0;
        }

        public int GetFreeWorkersCount()
        {
            return _freeWorker.Count;
        }

        public bool TryFindWorkerTrackingResurs(Resource resurs, out Worker worker)
        {
            bool isFind = false;
            worker = null;

            for(int i = 0; i < _allWorker.Count; i++)
            {
                if (_allWorker[i].TargetResurs == resurs)
                {
                    worker = _allWorker[i];
                    isFind = true;
                }
            }

            return isFind;
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
