using System.Net;
using Infrastructure.Common.Exceptions.Base;

namespace Infrastructure.Common.Exceptions;

public class ValidationException(string error) : BaseException(error, HttpStatusCode.BadRequest);