using global::Permissions.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Permissions.Models;

namespace Permissions.Events.Queries
{

    public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, IEnumerable<PermissionDto>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetPermissionsQueryHandler> _logger;

        public GetPermissionsQueryHandler(AppDbContext context, ILogger<GetPermissionsQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<PermissionDto>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all permissions");

            return await _context.Permissions
                .Include(p => p.PermissionType)
                .Select(p => new PermissionDto
                {
                    Id = p.Id,
                    EmployeeForeName = p.EmployeeForeName,
                    EmployeeSurName = p.EmployeeSurName,
                    PermissionTypeId = p.PermissionTypeId,
                    PermissionTypeDescription = p.PermissionType.Description,
                    Date = p.PermissionDate
                })
                .ToListAsync(cancellationToken);
        }
    }
   
}
