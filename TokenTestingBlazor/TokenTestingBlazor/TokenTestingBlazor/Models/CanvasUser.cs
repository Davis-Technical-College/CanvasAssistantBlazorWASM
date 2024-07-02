namespace TokenTestingBlazor.Models
{
    /// <summary>
    /// Data transfer object for a Canvas User
    /// </summary>
    public class CanvasUser
    {
        public int id { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string name { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
