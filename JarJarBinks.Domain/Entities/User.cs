using FluentValidation;
using FluentValidation.Results;
using JarJarBinks.Domain.Validation;
using System;
using System.Threading.Tasks;

namespace JarJarBinks.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; protected set; }
        public int Age { get; protected set; }

        protected User()
        {

        }

        public User(string name, int age, bool isActive = true)
        {
            this.Name = name;
            this.Age = age;
            this.IsActive = isActive;
        }

        public async Task<ValidationResult> Validate()
        {
            var validator = new UserValidator();
            return await validator.ValidateAsync(this);
        }

        public bool IsAboveAge()
        {
            return (this.Age - DateTime.Now.Year) >= 18;
        }

        public void UpdateValues(string name, int age)
        {
            this.Name = name;
            this.Age = age;
        }
    }
}
