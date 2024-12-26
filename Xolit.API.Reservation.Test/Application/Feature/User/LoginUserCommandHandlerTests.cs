using NUnit.Framework;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xolit.API.Reservation.Application.Contracts.Persistence;
using Xolit.API.Reservation.Application.Features.User.Handlers.Commands;
using Xolit.API.Reservation.Application.Features.User.Request.Commands;
using Xolit.API.Reservation.Application.DTOs.User;
using Xolit.API.Reservation.Domain.User;
using AutoMapper;
using Xolit.API.Reservation.Application.Contracts.Persistence.CrossRepositories;
using System.Linq.Expressions;

namespace Xolit.API.Reservation.Test.Application.Feature.User.Handlers
{
    [TestFixture]
    public class LoginUserCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private LoginUserCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new LoginUserCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task Handle_ShouldLoginSuccessfully_WithValidCredentials()
        {
            // Arrange
            var userLoginDto = new UserLoginDto
            {
                Email = "testuser@example.com",
                Password = "password123"
            };

            var user = new UserEntity
            {
                Id = 1,
                Name = "Test User",
                Email = userLoginDto.Email,
                Password = userLoginDto.Password,
                Role = "Admin"
            };

            _unitOfWorkMock.Setup(u => u.User.FindAsync(It.IsAny<Expression<Func<UserEntity, bool>>>()))
                .ReturnsAsync(new List<UserEntity> { user });

            _mapperMock.Setup(m => m.Map<UserEntity>(It.IsAny<UserLoginDto>()))
                .Returns(user);

            var command = new LoginUserCommand { UserLoginDto = userLoginDto };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(response.Success);
        }

        [Test]
        public async Task Handle_ShouldFail_WithInvalidCredentials()
        {
            // Arrange
            var userLoginDto = new UserLoginDto
            {
                Email = "testuser@example.com",
                Password = "wrongpassword"
            };


            var command = new LoginUserCommand { UserLoginDto = userLoginDto };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(response.Success);
        }

        [Test]
        public async Task Handle_ShouldFail_WhenExceptionIsThrown()
        {
            // Arrange
            var userLoginDto = new UserLoginDto
            {
                Email = "testuser@example.com",
                Password = "password123"
            };


            var command = new LoginUserCommand { UserLoginDto = userLoginDto };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(response.Success);
        }
    }
}
