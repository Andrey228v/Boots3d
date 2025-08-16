using System;
using UnityEngine;

namespace Assets.Scripts.BasesObjects.BaseState
{
    public class StateMachineBase : MonoBehaviour, IStateMachine<Base>
    {
        [SerializeField] private Base _base;
        [SerializeField] private Store _store;
        [SerializeField] private MapStoreResurs _mapStoreResurs;
        [SerializeField] private FlagController _flagController;

        private MineState _mineState;
        private CreateNewBaseState _createNewBaseState;

        public event Action<string> ChangedState;

        public IStateBase CurrentState { get; private set; }

        private void Start()
        {
            _mineState = new MineState(_base, _store, _mapStoreResurs, _base.CommandCenter, _base.Commands);
            _createNewBaseState = new CreateNewBaseState(_base, _store, _mapStoreResurs, _base.CommandCenter, _base.Commands, _flagController);

            CurrentState = _mineState;
            SelectState(BaseStateType.Main);
        }

        public void SetMapStoreResurs(MapStoreResurs mapStoreResurs)
        {
            _mapStoreResurs = mapStoreResurs;
        }

        private void Update()
        {
            CurrentState.UpdateState();
        }

        private void ChangeState(IStateBase newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }

        public void SelectState(BaseStateType stateType)
        {
            switch (stateType)
            {
                case BaseStateType.Main:
                    ChangeState(_mineState);
                    ChangedState?.Invoke(BaseStateType.Main.ToString());
                    break;

                case BaseStateType.Create:
                    ChangeState(_createNewBaseState);
                    ChangedState?.Invoke(BaseStateType.Create.ToString());
                    break;

                default:
                    Console.WriteLine("None State");
                    break;
            }
        }
    }
}
