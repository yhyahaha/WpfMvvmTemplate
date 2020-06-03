using System;
using System.Windows.Input;

namespace ViewModel
{
    internal class DelegateCommand : ICommand
    {
        // DeledateCommand コンストラクター
        public DelegateCommand(Action execute)
        {
            this.execute = x => execute();
            this.canExecute = x => true;
        }

        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = x => execute();
            this.canExecute = x => canExecute();
        }

        public DelegateCommand(Action<object> execute)
        {
            this.execute = execute;
            this.canExecute = x => true;
        }

        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        // ICommand実装
        private readonly Action<object> execute;
        public void Execute(object value) => this.execute(value);

        private readonly Func<object, bool> canExecute;
        public bool CanExecute(object value) => this.canExecute(value);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
