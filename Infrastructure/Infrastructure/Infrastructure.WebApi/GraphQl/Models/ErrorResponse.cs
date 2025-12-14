namespace Infrastructure.WebApi.GraphQl.Models;

internal record ErrorResponse(List<ErrorDetails?> Errors);

internal record ErrorDetails(string? Message, ErrorExtensions? Extensions);

internal record ErrorExtensions(string? Code);