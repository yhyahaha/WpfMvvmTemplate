using System;
using System.Collections.Generic;
using System.ComponentModel;
using Model;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace ViewModel
{
    // this class wraps Person class.


    public sealed class PersonViewModel : ViewModelBase, IDataErrorInfo
    {
        // Fields
        readonly Person person;
        private readonly Dictionary<string, string> errors = new Dictionary<string, string>();


        // Constructor
        public PersonViewModel() : this(new Person())
        {
        }

        public PersonViewModel(Person person)
        {
            this.person = person;
        }

        // Person Properties
        public int Id
        {
            get { return this.person.Id; }
            set
            {
                if (value == this.person.Id) return;
                this.person.Id = value;

                OnPropertyChanged();
            }
        }

        public string PersonName
        {
            get { return this.person.PersonName; }
            set
            {
                if (value == this.person.PersonName) return;
                this.person.PersonName = value;

                ValidationVaue(nameof(PersonName), value);

                OnPropertyChanged();
            }
        }

        public int? Age
        {
            get { return this.person.Age; }
            set
            {
                if (value == this.person.Age) return;
                this.person.Age = value;

                ValidationVaue(nameof(Age), value);

                OnPropertyChanged();
            }
        }

        // Presentation Properties



        // public Methods
        public bool HasErrors
        {
            get
            {
                return errors.Count != 0 || !string.IsNullOrEmpty(Error);
            }
        }

        // IDataErroeInfo Members
        public string Error
        {
            //get { return (person as IDataErrorInfo).Error; }
            get
            {
                if (errors.Count > 0) return Properties.MessageStrings.errMsg_PersonViewModelHasError;
                else return null;
            }
        }

        public string this[string propertyName]
        {
            get
            {
                string error = errors.ContainsKey(propertyName) ? errors[propertyName] : null;
                CommandManager.InvalidateRequerySuggested();
                return error;
            }
        }

        // Private Helper

        private void ValidationVaue(string propertyName,object value)
        {
            try
            {
                ValidationContext context = new ValidationContext(person, null, null);
                context.MemberName = propertyName;
                Validator.ValidateProperty(value, context);
                errors.Remove(propertyName);
            }
            catch (ValidationException ex)
            {
                errors[propertyName] = ex.Message;
            }
            catch (ArgumentNullException)
            {
                errors[propertyName] = Properties.MessageStrings.errMsg_NoWords;
            }
        }
    }
}
