using Assets.Scripts.Resurses;
using UnityEngine;

namespace Assets.Scripts.BasesObjects.BaseCommands
{
    public class CommandTakeResurs : ICommand
    {
        private CommandCenter _commandCenter;
        private Resource _resource;
        private Base _base;

        public CommandTakeResurs(Resource resource, Base mainBase, CommandCenter commandCenter)
        {
            _resource = resource;
            _base = mainBase;
            _commandCenter = commandCenter;
        }

        public bool CanExecute()
        {
            bool isExecute = false; 

            if (_commandCenter.HasFreeWorkers())
            {
                isExecute = true;
            }

            return isExecute;
        }

        public void Execute()
        {
            Debug.DrawRay(_base.transform.position, _resource.transform.position - _base.transform.position, Color.yellow, 3f);
            _commandCenter.SetCommandTakeResurs(_resource);
        }
    }
}
