using NUnit.Framework;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xolit.API.Reservation.Application.Contracts.Persistence;
using Xolit.API.Reservation.Application.Features.User.Handlers.Commands;
using Xolit.API.Reservation.Application.Features.User.Request.Commands;
using Xolit.API.Reservation.Domain.User;
using AutoMapper;
using Xolit.API.Reservation.Application.Contracts.Persistence.CrossRepositories;

namespace Xolit.API.Reservation.Test.Application.Feature.User.Handlers
{
    [TestFixture]
    public class CreateUserCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private CreateUserCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateUserCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task Handle_ShouldCreateUser_ForValidRequest()
        {
            // Arrange
            var userDto = new API.Reservation.Application.DTOs.User.UserDto
            {
                Name = "Test User",
                Email = "testuser@example.com",
                Password = "password123",
                Role = "User"
            };

            var command = new CreateUserCommand { UserDto = userDto };

            _unitOfWorkMock.Setup(u => u.User.AddAsync(It.IsAny<UserEntity>()))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(response.Success);
        }

        [Test]
        public async Task Handle_ShouldFail_WhenUserAlreadyExists()
        {
            // Arrange
            var userDto = new API.Reservation.Application.DTOs.User.UserDto
            {
                Name = "Test User",
                Email = "testuser@example.com",
                Password = "password123"
            };

            var command = new CreateUserCommand { UserDto = userDto };


            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(response.Success);
        }

        [Test]
        public async Task Handle_ShouldFail_WhenSavingThrowsException()
        {
            // Arrange
            var userDto = new API.Reservation.Application.DTOs.User.UserDto
            {
                Name = "Test User",
                Email = "testuser@example.com",
                Password = "password123"
            };

            var command = new CreateUserCommand { UserDto = userDto };

            _unitOfWorkMock.Setup(u => u.User.AddAsync(It.IsAny<UserEntity>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.CompleteAsync())
                .ThrowsAsync(new System.Exception("Database save failed"));

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(response.Success);
        }
    }
}
