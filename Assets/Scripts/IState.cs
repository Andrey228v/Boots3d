using System;
using UnityEngine;

namespace Assets.Scripts
{
    public interface IState<T> where T : Component
    {
        public event Action<T> ChangedState;

        public void Enter();

        public void Exit();

        public void UpdateState();

    }
}
