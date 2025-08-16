
namespace Assets.Scripts.BasesObjects.BaseCommands
{
    public interface ICommand
    {
        public bool CanExecute();

        public void Execute();


    }
}
