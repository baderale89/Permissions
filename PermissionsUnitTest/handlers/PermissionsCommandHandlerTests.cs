using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Permissions.Events.Commands;
using Permissions.Events.Queries;
using Permissions.Interfaces;
using Permissions.Models;


namespace PermissionsUnitTest.handlers
{
    public class RequestPermissionCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCommand_ReturnsPermissionId()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockElasticsearch = new Mock<IElasticsearchService>();
            var mockKafka = new Mock<IKafkaProducer>();

            mockUnitOfWork.Setup(u => u.Permissions.AddAsync(It.IsAny<Permission>()))
                         .Returns(Task.CompletedTask);
            mockUnitOfWork.Setup(u => u.CommitAsync())
                         .ReturnsAsync(1);

            var handler = new RequestPermissionCommandHandler(
                mockUnitOfWork.Object,
                mockElasticsearch.Object,
                mockKafka.Object,
                Mock.Of<ILogger<RequestPermissionCommandHandler>>());

            var command = new RequestPermissionCommand
            {
                EmployeeFirstName = "John",
                EmployeeLastName = "Doe",
                PermissionTypeId = 1
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BePositive();
            mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
            mockElasticsearch.Verify(e => e.IndexPermissionAsync(It.IsAny<Permission>()), Times.Once);
        }
    }
}
