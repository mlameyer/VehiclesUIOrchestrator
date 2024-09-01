namespace VehiclesUIOrchestrator.api.Dtos
{
    public record class VehicleByTypeAndOrManufacturerDto
    {
        public string? VehicleType { get; set; }
        public List<Manufacturer>? Manufacturers { get; set; } = new List<Manufacturer>(); 
    }

    public record class Manufacturer
    {
        public int Id { get; set; }
        public string? Shortname { get; set; }
        public string? FullName { get; set; }
        public string? Country { get; set; }
    }
}
