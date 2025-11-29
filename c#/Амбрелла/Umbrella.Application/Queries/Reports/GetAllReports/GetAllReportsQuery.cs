using MediatR;
using Umbrella.Domain.Dtos;

namespace Umbrella.Application.Queries.Reports.GetAllReports;

/// <summary>
/// Запрос для получения всех отчетов
/// </summary>
public sealed partial class GetAllReportsQuery() : IRequest<List<ReportDto>>
{
}