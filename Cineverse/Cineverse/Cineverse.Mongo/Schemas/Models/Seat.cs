namespace Cineverse.Mongo.Schemas.Models;

public record Seat
{
    public string SeatId => $"{Row}-{Number}";
    
    public int Row { get; init; }
    
    public int Number { get; init; }
}