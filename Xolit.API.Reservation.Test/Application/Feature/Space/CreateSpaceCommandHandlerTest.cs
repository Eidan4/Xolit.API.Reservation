using NUnit.Framework;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xolit.API.Reservation.Application.Contracts.Persistence;
using Xolit.API.Reservation.Application.Features.Space.Handlers.Commands;
using Xolit.API.Reservation.Application.Features.Space.Request.Commands;
using Xolit.API.Reservation.Application.DTOs.Space;
using Xolit.API.Reservation.Domain.Space;
using AutoMapper;
using Xolit.API.Reservation.Application.Contracts.Persistence.CrossRepositories;

namespace Xolit.API.Reservation.Test.Application.Feature.Space.Handlers
{
    [TestFixture]
    public class CreateSpaceCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private CreateSpaceCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateSpaceCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task Handle_ShouldCreateSpace_ForValidRequest()
        {
            // Arrange
            var spaceDto = new SpaceDto
            {
                Name = "Meeting Room",
                Description = "A large meeting room",
                Capacity = 10
            };

            var spaceEntity = new SpaceEntity
            {
                Id = 1,
                Name = spaceDto.Name,
                Description = spaceDto.Description,
                Capacity = spaceDto.Capacity
            };

            _mapperMock.Setup(m => m.Map<SpaceEntity>(It.IsAny<SpaceDto>()))
                .Returns(spaceEntity);

            _unitOfWorkMock.Setup(u => u.Space.AddAsync(It.IsAny<SpaceEntity>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.CompleteAsync())
                .ReturnsAsync(1); // Simula éxito en la base de datos

            var command = new CreateSpaceCommand { SpaceDto = spaceDto };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Success);
        }

        [Test]
        public async Task Handle_ShouldFail_WhenUnitOfWorkSaveFails()
        {
            // Arrange
            var spaceDto = new SpaceDto
            {
                Name = "Meeting Room",
                Description = "A large meeting room",
                Capacity = 10
            };

            var spaceEntity = new SpaceEntity
            {
                Id = 1,
                Name = spaceDto.Name,
                Description = spaceDto.Description,
                Capacity = spaceDto.Capacity
            };

            _mapperMock.Setup(m => m.Map<SpaceEntity>(It.IsAny<SpaceDto>()))
                .Returns(spaceEntity);

            _unitOfWorkMock.Setup(u => u.Space.AddAsync(It.IsAny<SpaceEntity>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.CompleteAsync())
                .ThrowsAsync(new System.Exception("Database error"));

            var command = new CreateSpaceCommand { SpaceDto = spaceDto };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.Success);
        }

        [Test]
        public async Task Handle_ShouldFail_WhenSpaceDtoIsNull()
        {
            // Arrange
            var command = new CreateSpaceCommand { SpaceDto = null };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.Success);
        }
    }
}
