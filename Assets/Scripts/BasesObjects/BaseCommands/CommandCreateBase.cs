using System;


namespace Assets.Scripts.BasesObjects.BaseCommands
{
    public class CommandCreateBase : ICommand
    {
        private CommandCenter _commandCenter;
        private FlagController _flagController;

        public CommandCreateBase(CommandCenter commandCenter, FlagController flagController) 
        {
            _commandCenter = commandCenter;
            _flagController = flagController;
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
            Worker worker = _commandCenter.SetCommandBildBase(_flagController.GetFlagPosition());
        }
    }
}
