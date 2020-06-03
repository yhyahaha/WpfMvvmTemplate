using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace ViewModel
{
    public partial class MainWindowViewModel
    {
        // CRUD

        private void ExecuteClearItemCommand()
        {
            Reset();
        }

        private bool CanExecuteClearItemCommand() => true;

        private void ExecuteUpdateItemCommand()
        {
            // 新規登録
            if (Person.Id == 0)
            {
                Model.Person person = reverseMapper.Map<PersonViewModel, Model.Person>(Person);
                int res = personRepository.AddItem(person);

                if(res != 0)
                {
                    Person.Id = res;
                    People.Add(Person);
                    SelectedIndex = -1;
                }
            }
            // 修正
            else
            {
                Model.Person person = reverseMapper.Map<PersonViewModel, Model.Person>(Person);
                int res = personRepository.UpdateItem(person);

                if(res != 0)
                {
                    var per = People.Where(x => x.Id == person.Id).FirstOrDefault();
                    if(per != null)
                    {
                        per = mapper.Map<Model.Person, PersonViewModel>(person);
                    }
                }
            }
        }

        private bool CanExecuteUpdateItemCommand() => !Person.HasErrors && !string.IsNullOrEmpty(Person.PersonName);

        private void ExecuteDeleteItemCommand()
        {
            Model.Person person = reverseMapper.Map<PersonViewModel, Model.Person>(Person);
            int res = personRepository.DeleteItem(person);

            if(res != 0)
            {
                SelectedIndex = -1;
                Person = new PersonViewModel();
                var per = People.Where(x => x.Id == person.Id).FirstOrDefault();
                if (per != null)
                {
                    People.Remove(per);
                }
            }
        }

        private bool CanExecuteDeleteItemCommand() => SelectedIndex > -1;

        // 検索

        private void ExecuteSearchCommand()
        {
            SelectedIndex = -1;
            People.Clear();

            IList<Model.Person> res = personRepository.GetItemsByKeyword(this.keyword);

            if(res != null)
            {
                People = mapper.Map<IList<Model.Person>, ObservableCollection<PersonViewModel>>(res);
                OnPropertyChanged(nameof(People));
                SelectedIndex = 0;
            }
        }

        private void ExecuteClearKeywordCommand()
        {
            this.Keyword = string.Empty;
        }
    }
}
