using Assets.Scripts.BasesObjects.BaseCommands;
using Assets.Scripts.Resurses;
using System.Collections.Generic;

namespace Assets.Scripts.BasesObjects.BaseState
{
    public class MineState : IStateBase
    {
        private Base _base;
        private Store _store;
        private MapStoreResurs _mapStoreResours;
        private CommandCenter _commandCenter;
        private Stack<ICommand> _commands;

        public MineState(Base baseUnit, Store store, MapStoreResurs mapStoreResurs, CommandCenter commandCenter, Stack<ICommand> commands)
        {
            _base = baseUnit;
            _store = store;
            _mapStoreResours = mapStoreResurs;
            _commandCenter = commandCenter;
            _commands = commands;
        }

        public void Enter()
        {
            _store.OnAccumulatedForUnit += BuyUnit;
            _store.OnSpent += NotifyBuy;
        }

        public void Exit()
        {
            _store.OnAccumulatedForUnit -= BuyUnit;
            _store.OnSpent -= NotifyBuy;
        }

        public void UpdateState()
        {
            if (_commandCenter.HasFreeWorkers())
            {
                if (_mapStoreResours.TryGetFreeResurs(_base.transform.position, out Resource resurs))
                {
                    CommandTakeResurs commandTakeResurs = new CommandTakeResurs(resurs, _base, _commandCenter);
                    _commands.Push(commandTakeResurs);
                }
            } 
        }

        public void BuyUnit()
        {
            Worker worker = _base.CreateWorker();
            _store.SpentForBuyWorker();
        }

        public void NotifyBuy(Resource resurs)
        {
            _mapStoreResours.RemoveResource(resurs);
        }
    }
}
