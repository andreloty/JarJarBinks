using JarJarBinks.Domain.Entities;
using JarJarBinks.Domain.Interfaces;
using JarJarBinks.Infra.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JarJarBinks.Test.Tests
{
    public class CRUDUserTest: BaseTest
    {
        private IUserRepository _userRepo;

        public CRUDUserTest()
        {
            // seta o contexto na variável ctx
            // se passar true, usa o InMemory
            // se passar false, usa o SQL Server
            GetContext(false);

            _userRepo = new UserRepository(ctx);

            _userRepo.CreateAsync(new User("Primeiro Usuário", 20));
        }

        #region THEORY
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Nome Maior Que Cento e Cinquenta Caracteres Nome Maior Que Cento e Cinquenta Caracteres Nome Maior Que Cento e Cinquenta Caracteres Nome Maior Que Cento e Cinquenta Caracteres")]
        public async Task Deve_Validar_Nome_Usuario_Nulo_Vazio_Ou_Maior_Que_150(string name)
        {
            var user = new User(name, 40, true);

            var val = await user.Validate();

            Assert.False(val.IsValid);
        }

        [Theory]
        [InlineData(null, "Null")]
        [InlineData("", "Empty")]
        [InlineData("Nome Maior Que Cento e Cinquenta Caracteres Nome Maior Que Cento e Cinquenta Caracteres Nome Maior Que Cento e Cinquenta Caracteres Nome Maior Que Cento e Cinquenta Caracteres", "GratherThanMaximumLength")]
        public async Task Deve_Validar_Nome_Usuario_Nulo_Vazio_Ou_Maior_Que_150_Com_Codigo_De_Erro(string name, string errorCode)
        {
            var user = new User(name, 40, true);

            var val = await user.Validate();

            Assert.False(val.IsValid);
            Assert.NotNull(val.Errors.FirstOrDefault(f => f.ErrorCode == errorCode));
        }

        [Theory]
        [InlineData("José Maria da Silva")]
        [InlineData("Maria Helena Bragança")]
        [InlineData("Andre Lima")]
        public async Task Deve_Validar_Nome_Usuario_Com_Nome_Correto(string name)
        {
            var user = new User(name, 40, true);

            var val = await user.Validate();

            Assert.True(val.IsValid);
        }

        [Theory]
        [InlineData(17)]
        [InlineData(15)]
        [InlineData(10)]
        public void Deve_Validar_Se_Usuario_Nao_E_Maior_De_Idade(int age)
        {
            var user = new User("André Lima", age, true);

            var isAboveAge = user.IsAboveAge();

            Assert.False(isAboveAge);
        }

        [Theory]
        [InlineData(18)]
        [InlineData(25)]
        [InlineData(20)]
        public void Deve_Validar_Se_Usuario_E_Maior_De_Idade(int age)
        {
            var user = new User("André Lima", age, true);

            var isAboveAge = user.IsAboveAge();

            Assert.False(isAboveAge);
        }
        #endregion

        #region FACT
        #region CREATE
        [Fact]
        public async Task Deve_Salvar_Usuario_Sem_Parametro_SaveChanges()
        {
            var user = new User("André Lima", 40, true);

            await _userRepo.CreateAsync(user);

            var userDb = await _userRepo.GetByIdAsync(2);

            // ASSERT
            Assert.NotNull(userDb);
        }

        [Fact]
        public async Task Deve_Salvar_Usuario_Com_Parametro_SaveChanges_True()
        {
            var user = new User("André Lima", 40, true);

            await _userRepo.CreateAsync(user, true);

            var userDb = await _userRepo.GetByIdAsync(2);

            // ASSERT
            Assert.NotNull(userDb);
        }

        [Fact]
        public async Task Nao_Deve_Salvar_Usuario_Com_Parametro_SaveChanges_False()
        {
            var user = new User("André Lima", 40, true);

            await _userRepo.CreateAsync(user, false);

            var userDb = await _userRepo.GetByIdAsync(2);

            // ASSERT
            Assert.Null(userDb);
        }

        [Fact]
        public async Task Deve_Salvar_Usuario_Com_Parametro_SaveChanges_False_E_Com_Chamada_Do_SaveChanges()
        {
            var user = new User("André Lima", 40, true);

            // nesse momento, não deve confirmar a gravação no banco
            await _userRepo.CreateAsync(user, false);
            // porém nesse momento, a gravação no banco deve ser confirmada
            await _userRepo.SaveChangesAsync();

            var userDb = await _userRepo.GetByIdAsync(2);

            // ASSERT
            Assert.NotNull(userDb);
        }
        #endregion

        #region UPDATE
        [Fact]
        public async Task Deve_Alterar_Usuario_Sem_Parametro_SaveChanges()
        {
            var user = await _userRepo.GetByIdAsync(1);

            var newName = "André Ricardo de Lima";
            var newAge = 25;

            user.UpdateValues(newName, newAge);

            await _userRepo.UpdateAsync(user);

            var updatedUserDb = await _userRepo.GetByIdAsync(1);

            // ASSERT
            Assert.Equal(updatedUserDb.Name, newName);
            Assert.Equal(updatedUserDb.Age, newAge);
        }

        [Fact]
        public async Task Deve_Alterar_Usuario_Com_Parametro_SaveChanges_True()
        {
            var user = await _userRepo.GetByIdAsync(1);

            var newName = "André Ricardo de Lima";
            var newAge = 25;

            user.UpdateValues(newName, newAge);

            await _userRepo.UpdateAsync(user, true);

            var updatedUserDb = await _userRepo.GetByIdAsync(1);

            // ASSERT
            Assert.Equal(updatedUserDb.Name, newName);
            Assert.Equal(updatedUserDb.Age, newAge);
        }

        [Fact]
        public async Task Nao_Deve_Alterar_Usuario_Com_Parametro_SaveChanges_False()
        {
            var user = await _userRepo.GetByIdAsync(1);

            var newName = "André Ricardo de Lima";
            var newAge = 25;

            user.UpdateValues(newName, newAge);

            await _userRepo.UpdateAsync(user, false);

            ctx.Dispose();

            GetContext(false, false);
            _userRepo = new UserRepository(ctx);

            var updatedUserDb = await _userRepo.GetByIdAsync(1);

            // ASSERT
            Assert.NotEqual(updatedUserDb.Name, newName);
            Assert.NotEqual(updatedUserDb.Age, newAge);

            var newUser = await _userRepo.GetByIdAsync(1);

            newUser.UpdateValues(newName, newAge);

            await _userRepo.UpdateAsync(newUser, false);
            await _userRepo.SaveChangesAsync();

            Assert.Equal(updatedUserDb.Name, newName);
            Assert.Equal(updatedUserDb.Age, newAge);
        }

        [Fact]
        public async Task Deve_Alterar_Usuario_Com_Parametro_SaveChanges_False_E_Com_Chamada_Do_SaveChanges()
        {
            var user = await _userRepo.GetByIdAsync(1);

            var newName = "André Ricardo de Lima";
            var newAge = 25;

            user.UpdateValues(newName, newAge);

            await _userRepo.UpdateAsync(user, false);
            await _userRepo.SaveChangesAsync();

            var updatedUserDb = await _userRepo.GetByIdAsync(1);

            // ASSERT
            Assert.Equal(updatedUserDb.Name, newName);
            Assert.Equal(updatedUserDb.Age, newAge);
        }
        #endregion
        #endregion
    }
}
