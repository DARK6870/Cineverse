namespace Cineverse.Mongo.Schemas.Models;

public class Seat
{
    public string SeatId => $"{Row}-{Number}";
    
    public int Row { get; set; }
    
    public int Number { get; set; }
}