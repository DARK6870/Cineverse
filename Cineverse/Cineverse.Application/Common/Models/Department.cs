using System.ComponentModel;

namespace Cineverse.Application.Common.Models;

public enum Department
{
    [Description("Collaboration")]
    Collaboration,
    
    [Description("Marketing")]
    Marketing,
    
    [Description("Customer Service")]
    CustomerService,
    
    [Description("IT & Support")]
    ItSupport
}