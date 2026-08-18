using LUPA.Api.Common.Exceptions;
using LUPA.Api.Data;
using LUPA.Api.Entities;
using LUPA.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;

namespace LUPA.Api.Services.Reports;

public class ReportExecutionService : IReportExecutionService
{
    private readonly IReportRepository _reportRepository;
    private readonly ApplicationDbContext _context;

    public ReportExecutionService(IReportRepository reportRepository, ApplicationDbContext context)
    {
        _reportRepository = reportRepository;
        _context = context;
    }

    public async Task<List<Dictionary<string, object?>>> ExecuteAsync(
        int reportId, Dictionary<string, string> parameterValues)
    {
        var report = await _reportRepository.GetByIdAsync(reportId)
            ?? throw new NotFoundException("Reporte no encontrado.");

        if (!report.IsActive)
        {
            throw new ValidationException("Este reporte está desactivado.");
        }

        var connection = _context.Database.GetDbConnection();

        await using var command = connection.CreateCommand();
        command.CommandText = report.StoredProcedureName;
        command.CommandType = CommandType.StoredProcedure;

        foreach (var parameter in report.Parameters.OrderBy(p => p.SortOrder))
        {
            // El nombre configurado puede venir con o sin "@"; el usuario manda el valor
            // usando el nombre "limpio" (sin @) como key del diccionario.
            string key = parameter.Name.TrimStart('@');
            parameterValues.TryGetValue(key, out var rawValue);

            rawValue ??= parameter.DefaultValue;

            var dbParameter = command.CreateParameter();
            dbParameter.ParameterName = parameter.Name;

            if (string.IsNullOrWhiteSpace(rawValue))
            {
                if (parameter.IsRequired)
                {
                    throw new ValidationException($"El parámetro '{parameter.Label}' es obligatorio.");
                }

                dbParameter.Value = DBNull.Value;
            }
            else
            {
                dbParameter.Value = ConvertValue(parameter, rawValue);
            }

            command.Parameters.Add(dbParameter);
        }

        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        var results = new List<Dictionary<string, object?>>();

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var row = new Dictionary<string, object?>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                row[reader.GetName(i)] = await reader.IsDBNullAsync(i) ? null : reader.GetValue(i);
            }

            results.Add(row);
        }

        return results;
    }

    private static object ConvertValue(ReportParameter parameter, string rawValue)
    {
        try
        {
            return parameter.DataType switch
            {
                ReportParameterType.Int => int.Parse(rawValue, CultureInfo.InvariantCulture),
                ReportParameterType.Decimal => decimal.Parse(rawValue, CultureInfo.InvariantCulture),
                ReportParameterType.DateTime => DateTime.Parse(rawValue, CultureInfo.InvariantCulture),
                ReportParameterType.Bool => bool.Parse(rawValue),
                _ => rawValue
            };
        }
        catch (FormatException)
        {
            throw new ValidationException(
                $"El valor '{rawValue}' no es válido para el parámetro '{parameter.Label}' (se esperaba {parameter.DataType}).");
        }
    }
}