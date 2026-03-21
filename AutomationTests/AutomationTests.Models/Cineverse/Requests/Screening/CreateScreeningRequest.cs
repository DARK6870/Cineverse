using System.Text.Json.Serialization;
using AutomationTests.Models.Converters;

namespace AutomationTests.Models.Cineverse.Requests.Screening;

public record CreateScreeningRequest(
    string MovieId,
    string HallId,
    DateOnly Date,
    [property: JsonConverter(typeof(JsonTimeOnlyConverter))] TimeOnly StartTime,
    [property: JsonConverter(typeof(JsonTimeOnlyConverter))] TimeOnly EndTime,
    int TicketPrice
);