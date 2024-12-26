using NUnit.Framework;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xolit.API.Reservation.Application.Contracts.Persistence;
using Xolit.API.Reservation.Application.Contracts.Persistence.CrossRepositories;
using Xolit.API.Reservation.Application.Features.Reservation.Handlers.Commands;
using Xolit.API.Reservation.Application.Features.Reservation.Request.Commands;
using Xolit.API.Reservation.Domain.Reservation;
using AutoMapper;

namespace Xolit.API.Reservation.Test.Application.Feature.Reservation.Handlers
{
    [TestFixture]
    public class CreateReservationCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IGenericRepository<ReservationEntity>> _reservationsRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private CreateReservationCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _reservationsRepositoryMock = new Mock<IGenericRepository<ReservationEntity>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(u => u.Reservations).Returns(_reservationsRepositoryMock.Object);

            _mapperMock = new Mock<IMapper>();
            _handler = new CreateReservationCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task Handle_ShouldCreateReservation_ForValidRequest()
        {
            // Arrange
            var reservationDto = new API.Reservation.Application.DTOs.Recervation.ReservationDto
            {
                SpaceId = 1,
                UserId = 1,
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(2)
            };

            var command = new CreateReservationCommand { ReservationDto = reservationDto };

            _reservationsRepositoryMock.Setup(r => r.AddAsync(It.IsAny<ReservationEntity>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(response.Success);
        }

        [Test]
        public async Task Handle_ShouldFail_WhenUnitOfWorkSaveFails()
        {
            // Arrange
            var reservationDto = new API.Reservation.Application.DTOs.Recervation.ReservationDto
            {
                SpaceId = 1,
                UserId = 2,
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(2)
            };

            var command = new CreateReservationCommand { ReservationDto = reservationDto };


            _unitOfWorkMock.Setup(u => u.Reservations.AddAsync(It.IsAny<ReservationEntity>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.CompleteAsync())
                .ThrowsAsync(new Exception("Database save failed"));

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(response.Success);
        }
    }
}
