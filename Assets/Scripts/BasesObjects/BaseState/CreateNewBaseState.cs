using Assets.Scripts.BasesObjects.BaseCommands;
using Assets.Scripts.Resurses;
using System.Collections.Generic;

namespace Assets.Scripts.BasesObjects.BaseState
{
    public class CreateNewBaseState : IStateBase
    {
        private Base _base;
        private Store _store;
        private MapStoreResurs _mapStoreResours;
        private CommandCenter _commandCenter;
        private Stack<ICommand> _commands;
        private FlagController _flagController;

        private bool IsAccomulate = false;

        public CreateNewBaseState(Base baseUnit, Store store, MapStoreResurs mapStoreResurs, CommandCenter commandCenter, Stack<ICommand> commands, FlagController flagController)
        {
            _base = baseUnit;
            _store = store;
            _mapStoreResours = mapStoreResurs;
            _commandCenter = commandCenter;
            _commands = commands;
            _flagController = flagController;

        }

        public void Enter()
        {
            IsAccomulate = false;
            _store.OnAccumulatedForBase += BuyBase;
            _store.OnSpent += NotifyBuy;
        }

        public void Exit()
        {
            _store.OnAccumulatedForBase -= BuyBase;
            _store.OnSpent -= NotifyBuy;
        }

        public void UpdateState()
        {
            if (_commandCenter.HasFreeWorkers())
            {

                if (IsAccomulate == false)
                {
                    if (_mapStoreResours.TryGetFreeResurs(_base.transform.position, out Resource resurs))
                    {
                        CommandTakeResurs commandTakeResurs = new CommandTakeResurs(resurs, _base, _commandCenter);
                        _commands.Push(commandTakeResurs);
                    }
                }
                else
                {
                    _base.SelectState(BaseStateType.Main);
                }

            }
        }

        public void BuyBase()
        {
            IsAccomulate = true;
            _store.SpentForBuyBase();
            CommandCreateBase commandCreateBase = new CommandCreateBase(_commandCenter, _flagController);
            _commands.Push(commandCreateBase);
        }

        public void NotifyBuy(Resource resurs)
        {
            _mapStoreResours.RemoveResource(resurs);
        }
    }
}
