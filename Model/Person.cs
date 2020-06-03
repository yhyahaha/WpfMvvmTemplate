using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Model
{
    public class Person
    {
        // This class is wrapped by the PersonViewModel class.
        
        // Constructor

        public Person() { }

        public Person(int id, string name, int? age)
        {
            Id = id;
            PersonName = name;
            Age = age;
        }

        // State Properties
        [Key]
        public int Id { get; set; } = 0;

        [Required(AllowEmptyStrings = true)]
        [StringLength(10, ErrorMessage = "名前は{1}文字以内で入力してください。")]
        public string PersonName { get; set; } = string.Empty;

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Range(0, 120, ErrorMessage = "年齢は{1}～{2}の数値を入力してください。")]
        public int? Age { get; set; } = null;


        // Override Equality, ToString 

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            int a = this.GetHashCode();
            int b = obj.GetHashCode();
            return a == b;
        }

        public static bool operator ==(Person a, Person b)
        {
            object objectA = a;
            object objectB = b;
            if (objectA == null && objectB == null) return true;
            if (objectA == null) return false;
            if (objectB == null) return false;
            return objectA.Equals(objectB);
        }

        public static bool operator != (Person a, Person b)
        {
            object objectA = a;
            object objectB = b;
            if (objectA == null && objectB == null) return false;
            if (objectA == null) return true;
            if (objectB == null) return true;
            return !objectA.Equals(objectB);
        }

        public override string ToString()
        {
            return this.PersonName ?? string.Empty;
        }

    }
}
