namespace AlertEmailAPI.Dto;

public class EmailRequest
{
    public string Subject { get; set; }
    public string Message { get; set; }
    public List<Recipient> Recipients { get; set; }
    
    public class Recipient
    {
        public string Email { get; set; }
    }
}