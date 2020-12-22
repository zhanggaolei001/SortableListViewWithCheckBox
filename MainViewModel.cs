using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Documents;
using System.Windows.Input;

namespace SortableListViewWithCheckBox
{
    public class SimpleCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the Predicate to execute when the CanExecute of the command gets called
        /// </summary>
        public Predicate<object> CanExecuteDelegate { get; set; }

        /// <summary>
        /// Gets or sets the action to be called when the Execute method of the command gets called
        /// </summary>
        public Action<object> ExecuteDelegate { get; set; }


        public SimpleCommand(Predicate<object> canExecuteDelegate, Action<object> executeDelegate)
        {
            CanExecuteDelegate = canExecuteDelegate;
            ExecuteDelegate = executeDelegate;
        }

        public SimpleCommand(Action<object> executeDelegate)
        {
            ExecuteDelegate = executeDelegate;
        }


        #region ICommand Members

        /// <summary>
        /// Checks if the command Execute method can run
        /// </summary>
        /// <param name="parameter">THe command parameter to be passed</param>
        /// <returns>Returns true if the command can execute. By default true is returned so that if the user of SimpleCommand does not specify a CanExecuteCommand delegate the command still executes.</returns>
        public bool CanExecute(object parameter)
        {
            if (CanExecuteDelegate != null)
                return CanExecuteDelegate(parameter);

            return true;// if there is no can execute default to true
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Executes the actual command
        /// </summary>
        /// <param name="parameter">THe command parameter to be passed</param>
        public void Execute(object parameter)
        {
            if (ExecuteDelegate != null)
                ExecuteDelegate(parameter);
        }

        #endregion
    }
    public class Data : INotifyPropertyChanged
    {
        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                RaisePropertyChanged(nameof(IsChecked));
            }
        }
        public string Name { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            DataList = new List<Data>()
            {
                new Data(){IsChecked = false,Name = "aaaaa"},
                new Data(){IsChecked = true,Name = "bbbbb"},
                new Data(){IsChecked = true,Name = "ccccc"},
                new Data(){IsChecked = false,Name = "ddddd"},
            };
            SortCommand = new SimpleCommand(Sort);
        }

        private void Sort(object obj)
        {
            DataList = DataList.OrderBy(d => d.IsChecked).ToImmutableList().ToList();
        }

        private List<Data> dataList;
        public List<Data> DataList
        {
            get { return dataList; }
            set
            {
                dataList = value;
                RaisePropertyChanged(nameof(DataList));
            }
        }

        public SimpleCommand SortCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}