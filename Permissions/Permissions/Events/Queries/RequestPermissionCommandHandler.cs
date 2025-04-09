using global::Permissions.Interfaces;
using global::Permissions.Models;
using MediatR;
using Permissions.Events.Commands;

namespace Permissions.Events.Queries
{
    public class RequestPermissionCommandHandler : IRequestHandler<RequestPermissionCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElasticsearchService _elasticsearchService;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly ILogger<RequestPermissionCommandHandler> _logger;

        public RequestPermissionCommandHandler(
             IUnitOfWork unitOfWork,
             IElasticsearchService elasticsearchService,
             IKafkaProducer kafkaProducer,
             ILogger<RequestPermissionCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _elasticsearchService = elasticsearchService;
            _kafkaProducer = kafkaProducer;
            _logger = logger;
        }

        public async Task<int> Handle(RequestPermissionCommand request, CancellationToken cancellationToken)
        {
            var permission = new Permission
            {
                EmployeeForeName = request.EmployeeFirstName,
                EmployeeSurName = request.EmployeeLastName,
                PermissionTypeId = request.PermissionTypeId,
                PermissionDate = request.Date
            };

            await _unitOfWork.Permissions.AddAsync(permission);
            await _unitOfWork.CommitAsync();

            await _elasticsearchService.IndexPermissionAsync(permission);
            await _kafkaProducer.ProduceOperationAsync("request", permission.Id);

            _logger.LogInformation("Permission created with ID: {PermissionId}", permission.Id);
            return permission.Id;
        }
    }
}