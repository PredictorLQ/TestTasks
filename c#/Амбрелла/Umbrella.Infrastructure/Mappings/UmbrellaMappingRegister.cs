using Mapster;
using Umbrella.Domain.Dtos;
using Umbrella.Domain.Entities;
using Umbrella.Domain.Models;

namespace Umbrella.Infrastructure.Mappings;

public static class UmbrellaMappingRegister
{
    public static void Configure()
    {
        TypeAdapterConfig<User, UserDto>.NewConfig()
            .Map(dest => dest, src => src);

        TypeAdapterConfig<UserModel, User>.NewConfig()
            .Map(dest => dest, src => src);

        TypeAdapterConfig<DataSource, DataSourceDto>.NewConfig()
            .Map(dest => dest, src => src);

        TypeAdapterConfig<DataSourceModel, DataSource>.NewConfig()
            .Map(dest => dest, src => src);

        TypeAdapterConfig<Report, ReportDto>.NewConfig()
            .Map(dest => dest, src => src)
            .Ignore(dest => dest.ContentSize)
            .Ignore(dest => dest.DataSourceName);

        TypeAdapterConfig<ReportModel, Report>.NewConfig()
            .Map(dest => dest, src => src);

        TypeAdapterConfig<ScheduledReport, ScheduledReportDto>.NewConfig()
            .Map(dest => dest, src => src)
            .Ignore(dest => dest.ReportName)
            .Ignore(dest => dest.DataSourceName);

        TypeAdapterConfig<ScheduledReportModel, ScheduledReport>.NewConfig()
            .Map(dest => dest, src => src);
    }
}