namespace TCPI_PR_Portal.Data
{
	public class Error
	{
		public int code { get; set; }
		public Message message { get; set; }
	}

	public class Message
	{
		public string lang { get; set; }
		public string value { get; set; }
	}
}
