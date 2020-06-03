using System;
using System.Collections.Generic;
using InterFaces;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Model;


namespace ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase,  IOperation
    {
        private static readonly log4net.ILog logger
            = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        // Constructor
        public MainWindowViewModel(IDataAccess<Person> repository, IObjectMapping mapper, IObjectMapping reverseMapper)
        {
            this.personRepository = repository;
            this.mapper = mapper;
            this.reverseMapper = reverseMapper;

            Reset();

            logger.Info("Login");
        }

        // ICrud Commands
        public ICommand ClearItemCommand
        {
            get
            {
                if(this.clearItemCommand == null)
                {
                    this.clearItemCommand = new DelegateCommand(ExecuteClearItemCommand, CanExecuteClearItemCommand);
                }
                return this.clearItemCommand;
            }
        }

        public ICommand UpdateItemCommand
        {
            get
            {
                if (this.updateItemCommand == null)
                {
                    this.updateItemCommand = new DelegateCommand(ExecuteUpdateItemCommand,CanExecuteUpdateItemCommand);
                }
                return this.updateItemCommand;
            }
        }

        public ICommand DeleteItemCommand
        {
            get
            {
                if(this.deleteItemCommand == null)
                {
                    this.deleteItemCommand = new DelegateCommand(ExecuteDeleteItemCommand, CanExecuteDeleteItemCommand);
                }
                return this.deleteItemCommand;
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                if(this.searchCommand == null)
                {
                    this.searchCommand = new DelegateCommand(ExecuteSearchCommand);
                }
                return this.searchCommand;
            }
        }

        public ICommand ClearKeywordCommand
        {
            get
            {
                if(this.clearKeywordCommand == null)
                {
                    this.clearKeywordCommand = new DelegateCommand(ExecuteClearKeywordCommand);
                }
                return this.clearKeywordCommand;
            }
        }

        // ICrud Properties
        public string Keyword
        {
            get { return this.keyword; }
            set
            {
                if (this.keyword == value) return;
                this.keyword = value;
                OnPropertyChanged();
            }
        }

        public int SelectedIndex
        {
            get { return this.selectedIndex; }
            set
            {
                if (value == this.selectedIndex) return;
                this.selectedIndex = value;

                OnPropertyChanged();

                if(this.selectedIndex > -1)
                {
                    Person = People[selectedIndex];
                }
            }
        }

        // People
        public ObservableCollection<PersonViewModel> People { get; private set; }
        = new ObservableCollection<PersonViewModel>();

        // Person
        public PersonViewModel Person
        {
            get { return this.person; }
            set
            {
                this.person = value;
                OnPropertyChanged();
            }
        }

        // Private Helpers
        private void GetAllItems()
        {
            IList<Person> allitem = personRepository.GetAllItems();
            People = mapper.Map<IList<Person>, ObservableCollection<PersonViewModel>>(allitem);
            OnPropertyChanged(nameof(People));
        }

        private void Reset()
        {
            Keyword = string.Empty;
            SelectedIndex = -1;
            Person = new PersonViewModel();
            GetAllItems();
        }

        // App Commands
        private ICommand closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                if(closeCommand == null)
                {
                    closeCommand = new DelegateCommand(RequestCloseCommand);
                }
                return closeCommand;
            }
        }


        // RequestClose [event]　App.xamlでイベント登録
        public event EventHandler RequestClose;

        private void RequestCloseCommand()
        {
            EventHandler handler = this.RequestClose;　
            handler?.Invoke(this, EventArgs.Empty);
        }

        // Fields
        readonly IDataAccess<Person> personRepository;
        readonly IObjectMapping mapper;
        readonly IObjectMapping reverseMapper;
        PersonViewModel person = new PersonViewModel();
        private ICommand clearItemCommand;
        private ICommand updateItemCommand;
        private ICommand deleteItemCommand;
        private ICommand searchCommand;
        private ICommand clearKeywordCommand;
        private string keyword;
        private int selectedIndex;
    }
}
