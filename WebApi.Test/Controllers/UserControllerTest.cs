using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Controllers;

namespace WebApi.Test.Controllers
{
    public class UserControllerTest
    {
        [Fact]
        public void Get_OkWithAllUsers()
        {
            //Arrange
            var service = new Mock<ICrudService<User>>();
            var expectedUsers = new Fixture().CreateMany<User>();

            service.Setup(x => x.ReadAsync())
                .ReturnsAsync(expectedUsers);

            var controller = new UsersController(service.Object);

            //Act
            var task = controller.Get();
            task.Wait();
            var result = task.Result;

            //Assert
                task.IsCompletedSuccessfully.Should().BeTrue();
                result.Should().BeOfType<OkObjectResult>()
                    .Subject.Value.Should().BeAssignableTo<IEnumerable<User>>()
                    .Subject.Should().BeSameAs(expectedUsers);
        }

        [Fact]
        public void Get_ExistingId_OkWithUser()
        {
            //Arrange
            var service = new Mock<ICrudService<User>>();
            var expectedUser = new Fixture().Create<User>();

            service.Setup(x => x.ReadAsync(expectedUser.Id))
                .ReturnsAsync(expectedUser);

            var controller = new UsersController(service.Object);

            //Act
            var task = controller.Get(expectedUser.Id);
            task.Wait();
            var result = task.Result;

            //Assert
                task.IsCompletedSuccessfully.Should().BeTrue();
                result.Should().BeOfType<OkObjectResult>()
                    .Subject.Value.Should().BeAssignableTo<User>()
                    .Subject.Should().BeSameAs(expectedUser);
        }

        /*[Fact]
        public void Get_NotExistingId_NotFound()
        {
            //Arrange
            var service = new Mock<ICrudService<User>>();
            service.Setup(x => x.ReadAsync(It.IsAny<int>()))
                   .ReturnsAsync((User?)null)
                   .Verifiable();
            var id = new Fixture().Create<int>();

            var controller = new UsersController(service.Object);

            //Act
            var task = controller.Get(id);
            task.Wait();
            var result = task.Result;

            //Assert
            task.IsCompletedSuccessfully.Should().BeTrue();
            using (new AssertionScope())
            {
                result.Should().BeOfType<NotFoundResult>();
                service.VerifyAll();
            }
        }

        [Fact]
        public void Delete_NotExistingId_NotFound()
        {
            //Arrange
            var service = new Mock<ICrudService<User>>();
            service.Setup(x => x.ReadAsync(It.IsAny<int>()))
                   .ReturnsAsync((User?)null)
                   .Verifiable();
            var id = new Fixture().Create<int>();

            var controller = new UsersController(service.Object);

            //Act
            var task = controller.Delete(id);
            task.Wait();
            var result = task.Result;

            //Assert
            task.IsCompletedSuccessfully.Should().BeTrue();
            using (new AssertionScope())
            {
                result.Should().BeOfType<NotFoundResult>();
                service.VerifyAll();
            }
        }*/

        [Fact]
        public void Get_NotExistingId_NotFound()
        {
            ReturnsNotFound((controller, id) => controller.Get(id));
        }

        [Fact]
        public void Delete_NotExistingId_NotFound()
        {
            ReturnsNotFound((controller, id) => controller.Delete(id));
        }

        [Fact]
        public void Put_NotExistingId_NotFound()
        {
            ReturnsNotFound((controller, id) => controller.Put(id, new Fixture().Create<User>()));
        }

        private void ReturnsNotFound(Func<UsersController, int, Task<IActionResult>> func)
        {
            //Arrange
            var id = new Fixture().Create<int>();
            var service = new Mock<ICrudService<User>>();
            service.Setup(x => x.ReadAsync(It.IsAny<int>()))
                   .ReturnsAsync((User?)null)
                   .Verifiable();
            var controller = new UsersController(service.Object);

            //Act
            var task = func(controller, id);
            task.Wait();
            var result = task.Result;

            //Assert
            task.IsCompletedSuccessfully.Should().BeTrue();
            using (new AssertionScope())
            {
                result.Should().BeOfType<NotFoundResult>();
                service.VerifyAll();
            }
        } 
    }
}
