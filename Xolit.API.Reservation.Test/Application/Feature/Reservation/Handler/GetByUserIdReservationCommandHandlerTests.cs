using NUnit.Framework;
using Moq;
using Xolit.API.Reservation.Application.Features.Reservation.Handlers.Queries;
using Xolit.API.Reservation.Application.Contracts.Persistence;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Xolit.API.Reservation.Application.Contracts.Persistence.CrossRepositories;
using Xolit.API.Reservation.Application.Features.Reservation.Request.Queries;
using Xolit.API.Reservation.Domain.Reservation;
using AutoMapper;


namespace Xolit.API.Reservation.Test.Application.Feature.Reservation.Handlers
{
    [TestFixture]
    public class GetByUserIdReservationCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mockMapper;
        private GetByUserIdReservationCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetByUserIdReservationCommandHandler(_unitOfWorkMock.Object, _mockMapper.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnReservations_ForValidUserId()
        {
            // Arrange
            var userId = 1;
            var reservations = new List<ReservationEntity>
            {
                new ReservationEntity { Id = 1, UserId = userId, SpaceId = 2, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) },
                new ReservationEntity { Id = 2, UserId = userId, SpaceId = 3, StartTime = DateTime.Now.AddHours(2), EndTime = DateTime.Now.AddHours(3) }
            };

            var mappedReservations = new List<object> // Mock del mapeo
            {
                new { Id = 1, UserId = userId, SpaceId = 2, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) },
                new { Id = 2, UserId = userId, SpaceId = 3, StartTime = DateTime.Now.AddHours(2), EndTime = DateTime.Now.AddHours(3) }
            };

            _unitOfWorkMock.Setup(u => u.Reservations.FindAsync(r => r.UserId == userId))
                .ReturnsAsync(reservations);

            _mockMapper.Setup(m => m.Map<List<object>>(It.IsAny<List<ReservationEntity>>()))
                .Returns(mappedReservations);

            var command = new GetByUserIdReservationCommand { UserId = userId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(1, result.Parameters.Count);
        }

        [Test]
        public async Task Handle_ShouldReturnEmpty_ForInvalidUserId()
        {
            // Arrange
            var userId = 999; // No reservations for this ID
            _unitOfWorkMock.Setup(u => u.Reservations.FindAsync(r => r.UserId == userId))
                .ReturnsAsync(new List<ReservationEntity>());

            _mockMapper.Setup(m => m.Map<List<object>>(It.IsAny<List<ReservationEntity>>()))
                .Returns(new List<object>());

            var command = new GetByUserIdReservationCommand { UserId = userId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.Success);
        }
    }
}
