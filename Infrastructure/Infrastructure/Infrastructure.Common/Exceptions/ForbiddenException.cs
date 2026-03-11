using System.Net;
using Infrastructure.Common.Exceptions.Base;

namespace Infrastructure.Common.Exceptions;

public class ForbiddenException(string error) : BaseException(error, HttpStatusCode.Forbidden);