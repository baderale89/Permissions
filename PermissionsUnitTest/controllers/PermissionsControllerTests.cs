using FluentAssertions;
using Moq;
using Permissions.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Permissions.Controllers.Permissions.Api.Controllers;
using Microsoft.Extensions.Logging;
using Permissions.Events.Commands;

namespace PermissionsUnitTest.controllers
{
  
    
    public class PermissionsControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMediator> _mockMediator;
        private readonly PermissionsController _controller;

        public PermissionsControllerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMediator = new Mock<IMediator>();
            _controller = new PermissionsController(_mockMediator.Object, Mock.Of<ILogger<PermissionsController>>());
        }

        [Fact]
        public async Task RequestPermission_ReturnsOkResult()
        {
            // Arrange
            var command = new RequestPermissionCommand
            {
                EmployeeFirstName = "John",
                EmployeeLastName = "Doe",
                PermissionTypeId = 1
            };
            _mockMediator.Setup(m => m.Send(It.IsAny<RequestPermissionCommand>(), default))
                        .ReturnsAsync(1);

            // Act
            var result = await _controller.RequestPermission(command);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().NotBeNull();
        }
    }
}
