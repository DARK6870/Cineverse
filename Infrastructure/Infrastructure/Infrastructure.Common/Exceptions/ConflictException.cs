using System.Net;
using Infrastructure.Common.Exceptions.Base;

namespace Infrastructure.Common.Exceptions;

public class ConflictException(string error) : BaseException(error, HttpStatusCode.Conflict);