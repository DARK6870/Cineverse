using System.Net;
using Infrastructure.Common.Exceptions.Base;

namespace Infrastructure.Common.Exceptions;

public class UnauthorizedException(string error) : BaseException(error, HttpStatusCode.Unauthorized);