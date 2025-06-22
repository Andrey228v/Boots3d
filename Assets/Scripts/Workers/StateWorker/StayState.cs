using UnityEngine;

namespace Assets.Scripts.Workers.StateWorker
{
    public class StayState : IStateWorker
    {
        private float _speed = 0f;
        private Animator _animator;

        public StayState(float speed, Animator animator)
        {
            _speed = speed;
            _animator = animator;

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

        }
    }
}
