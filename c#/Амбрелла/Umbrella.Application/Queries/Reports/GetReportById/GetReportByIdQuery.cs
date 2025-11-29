using MediatR;
using Umbrella.Domain.Dtos;

namespace Umbrella.Application.Queries.Reports.GetReportById;

/// <summary>
/// Запрос для получения отчета по идентификатору
/// </summary>
public sealed partial class GetReportByIdQuery(Guid id) : IRequest<ReportDto?>
{
    public Guid Id { get; } = id;
}