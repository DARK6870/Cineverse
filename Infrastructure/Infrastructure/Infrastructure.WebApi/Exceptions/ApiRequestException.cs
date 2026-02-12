using System.Net;
using Infrastructure.Common.Exceptions.Base;

namespace Infrastructure.WebApi.Exceptions;

public class ApiRequestException(string error, HttpStatusCode statusCode) : BaseException(error, statusCode);