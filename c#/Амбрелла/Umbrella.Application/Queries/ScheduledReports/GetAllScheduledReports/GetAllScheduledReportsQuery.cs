using MediatR;
using Umbrella.Domain.Dtos;

namespace Umbrella.Application.Queries.ScheduledReports.GetAllScheduledReports;

/// <summary>
/// Запрос для получения всех запланированных отчетов
/// </summary>
public sealed partial class GetAllScheduledReportsQuery() : IRequest<List<ScheduledReportDto>>
{
}