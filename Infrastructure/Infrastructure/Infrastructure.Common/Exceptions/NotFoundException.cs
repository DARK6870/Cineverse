using System.Net;
using Infrastructure.Common.Exceptions.Base;

namespace Infrastructure.Common.Exceptions;

public class NotFoundException(string error) : BaseException(error, HttpStatusCode.NotFound);