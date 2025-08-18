using Assets.Scripts.BasesObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class BaseBuilder : MonoBehaviour
    {
        [SerializeField] private Base _basePrefab;
        [SerializeField] private Transform _basePerent;
        [SerializeField] private MapStoreResurs _mapStoreResurs;
        [SerializeField] private List<Base> _baseList;

        private void Start()
        {
            foreach (Base baseUnit in _baseList) 
            {
                baseUnit.OnWorkerCreated += AddEventToUnit;

                foreach (Worker worker in baseUnit.CommandCenter.AllWorker)
                {
                    worker.FlagTrigger.OnFlagTrigger += BuildBase;
                }
            }
        }

        private void OnDestroy()
        {
            foreach (Base baseUnit in _baseList)
            {
                foreach (Worker worker in baseUnit.CommandCenter.AllWorker)
                {
                    worker.FlagTrigger.OnFlagTrigger -= BuildBase;
                }
            }
        }

        private void BuildBase(Flag flag, Worker worker)
        {
            Base baseBuild = Instantiate(_basePrefab, _basePerent);
            baseBuild.transform.localPosition = flag.transform.position;
            baseBuild.SetMapStoreResurs(_mapStoreResurs);
            _baseList.Add(baseBuild);
            baseBuild.CommandCenter.AddWorker(worker);
            worker.SetBase(baseBuild);
            baseBuild.OnWorkerCreated += AddEventToUnit;
        }

        private void AddEventToUnit(Worker worker)
        {
            worker.FlagTrigger.OnFlagTrigger += BuildBase;
        }
    }
}
