namespace Cineverse.Mongo.Schemas.Models;

public class Seat
{
    public string SeatId {get; set;} = string.Empty;
    
    public int Row { get; set; }
    
    public int Number { get; set; }
    
    public void GenerateSeatId()
    {
        SeatId = $"{Row}-{Number}";
    }
}