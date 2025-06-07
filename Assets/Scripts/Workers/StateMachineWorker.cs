using Assets.Scripts.Workers.StateWorker;
using System;
using UnityEngine;

namespace Assets.Scripts.Workers
{
    [RequireComponent(typeof(Worker), typeof(WorkerView))]
    [RequireComponent(typeof(Animator))]
    public class StateMachineWorker : MonoBehaviour, IStateMachine<Worker>
    {
        [SerializeField] private float _speedStayState = 0f;
        [SerializeField] private float _speedStayMove = 5f;
        

        private WorkerView _view;
        private Animator _animator;
        private StayState _stayState;
        private MoveState _moveState;

        public event Action<string> ChangedState;

        public IStateWorker CurrentState { get; private set; }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _view = GetComponent<WorkerView>();

            _stayState = new StayState(_speedStayState, _animator);
            _moveState = new MoveState(_animator, _view, _speedStayMove);

            
        }

        private void Start()
        {
            CurrentState = _stayState;
            SelectState(WorkerStateType.Wait);
        }

        private void Update()
        {
            CurrentState.UpdateState();
        }

        private void ChangeState(IStateWorker newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }

        public void SelectState(WorkerStateType stateType)
        {
            switch (stateType)
            {
                case WorkerStateType.Wait:
                    ChangeState(_stayState);
                    ChangedState?.Invoke(WorkerStateType.Wait.ToString());
                    break;

                case WorkerStateType.Run:
                    ChangeState(_moveState);
                    ChangedState?.Invoke(WorkerStateType.Run.ToString());
                    break;

                default:
                    Console.WriteLine("None State");
                    break;
            }
        }
    }
}
