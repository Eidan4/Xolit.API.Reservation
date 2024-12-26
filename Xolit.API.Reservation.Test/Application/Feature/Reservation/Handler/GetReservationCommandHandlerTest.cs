using NUnit.Framework;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xolit.API.Reservation.Application.Contracts.Persistence;
using Xolit.API.Reservation.Application.Features.Reservation.Handlers.Queries;
using Xolit.API.Reservation.Application.Features.Reservation.Request.Queries;
using Xolit.API.Reservation.Domain.Reservation;
using Xolit.API.Reservation.Application.Contracts.Persistence.CrossRepositories;
using AutoMapper;

namespace Xolit.API.Reservation.Test.Application.Feature.Reservation.Handlers
{
    [TestFixture]
    public class GetReservationByDayCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mockMapper;
        private GetReservationCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetReservationCommandHandler(_unitOfWorkMock.Object, _mockMapper.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnAvailableIntervals_ForValidDayAndSpaceId()
        {
            // Arrange
            var day = "26/12/2024";
            var spaceId = 1;

            var reservations = new List<ReservationEntity>
            {
                new ReservationEntity
                {
                    Id = 1,
                    SpaceId = spaceId,
                    UserId = 1,
                    StartTime = new DateTime(2024, 12, 26, 10, 0, 0),
                    EndTime = new DateTime(2024, 12, 26, 12, 0, 0)
                },
                new ReservationEntity
                {
                    Id = 2,
                    SpaceId = spaceId,
                    UserId = 2,
                    StartTime = new DateTime(2024, 12, 26, 14, 0, 0),
                    EndTime = new DateTime(2024, 12, 26, 16, 0, 0)
                }
            };

            _unitOfWorkMock.Setup(u => u.Reservations.FindAsync(r =>
                r.StartTime.Date == DateTime.ParseExact(day, "dd/MM/yyyy", null).Date &&
                r.SpaceId == spaceId))
                .ReturnsAsync(reservations);

            var command = new GetReservationByDayCommand { Day = day, SpaceId = spaceId };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(response.Success);
        }

        [Test]
        public async Task Handle_ShouldFail_WhenInvalidDateFormatIsProvided()
        {
            // Arrange
            var day = "2024-12-26"; // Invalid format
            var spaceId = 1;

            var command = new GetReservationByDayCommand { Day = day, SpaceId = spaceId };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(response.Success);

        }
    }
}
