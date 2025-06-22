using System;
using UnityEngine;

namespace Assets.Scripts
{
    public interface IStateMachine<T> where T : Component
    {
        public event Action<string> ChangedState;
    }
}
