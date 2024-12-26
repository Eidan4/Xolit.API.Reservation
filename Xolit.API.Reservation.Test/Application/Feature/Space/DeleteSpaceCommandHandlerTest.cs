using NUnit.Framework;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xolit.API.Reservation.Application.Contracts.Persistence;
using Xolit.API.Reservation.Application.Features.Space.Handlers.Commands;
using Xolit.API.Reservation.Application.Features.Space.Request.Commands;
using AutoMapper;
using Xolit.API.Reservation.Application.Contracts.Persistence.CrossRepositories;
using Xolit.API.Reservation.Domain.Space;
using System.Linq.Expressions;

namespace Xolit.API.Reservation.Test.Application.Feature.Space.Handlers
{
    [TestFixture]
    public class DeleteSpaceCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private DeleteSpaceCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new DeleteSpaceCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task Handle_ShouldDeleteSpace_ForValidSpaceId()
        {
            // Arrange
            var spaceId = 1;

            var spaces = new List<SpaceEntity>
            {
                new SpaceEntity { Id = spaceId, Name = "Space 1", Description = "Test Space 1" },
                new SpaceEntity { Id = 2, Name = "Space 2", Description = "Test Space 2" }
            };

            // Simula la búsqueda con una expresión LINQ
            _unitOfWorkMock.Setup(u => u.Space.FindAsync(It.IsAny<Expression<Func<SpaceEntity, bool>>>()))
                .ReturnsAsync((Expression<Func<SpaceEntity, bool>> predicate) =>
                    spaces.AsQueryable().Where(predicate).ToList());

            _unitOfWorkMock.Setup(u => u.CompleteAsync())
                .ReturnsAsync(1);

            var command = new DeleteSpaceCommand { Id = spaceId };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Success);
        }

        [Test]
        public async Task Handle_ShouldFail_WhenSpaceDoesNotExist()
        {
            // Arrange
            var spaceId = 999; // ID inexistente

            var command = new DeleteSpaceCommand { Id = spaceId };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.Success);
        }

        [Test]
        public async Task Handle_ShouldFail_WhenUnitOfWorkSaveFails()
        {
            // Arrange
            var spaceId = 1;

            _unitOfWorkMock.Setup(u => u.CompleteAsync())
                .ThrowsAsync(new System.Exception("Database error")); // Simula un error al guardar

            var command = new DeleteSpaceCommand { Id = spaceId };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.Success);
        }
    }
}
