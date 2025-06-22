using UnityEngine;

namespace Assets.Scripts.Workers.StateWorker
{
    [RequireComponent(typeof(WorkerView))]
    public class MoveState : IStateWorker
    {
        private Animator _animator;
        private WorkerView _view;
        private float _speed;

        public MoveState(Animator animator, WorkerView view, float speed)
        {
            _animator = animator;
            _view = view;
            _speed = speed;
        }

        public void Enter()
        {
            _animator.SetFloat(PlayerAnimations.AnimatorParameterSpeed, _speed);
        }

        public void Exit()
        {
            
        }

        public void UpdateState()
        {
            _view.SetPosition(_speed);
        }
    }
}
