using System.Text.Json;
using Microsoft.Extensions.Logging;
using UltimateHelloWorld.Library.Models;

namespace UltimateHelloWorld.Library.BusinessLogic;
public class Messages
{
	private readonly ILogger<Messages> _logger;
	public Messages(ILogger<Messages> logger)
	{
		_logger = logger;
	}
	
	public string Greeting(string language)
	{
		return LookUpCustomText(nameof(Greeting), language);
	}

	private string LookUpCustomText(string key, string language)
	{
		JsonSerializerOptions options = new()
		{
			PropertyNameCaseInsensitive = true
		};

		try
		{
            List<CustomText>? messageSets = JsonSerializer
            .Deserialize<List<CustomText>>
            (
                File.ReadAllText("CustomText.json"), options
            );
			CustomText? messages = messageSets?.Where(x => x.Language == language).First();

			if(messages is null)
			{
				throw new NullReferenceException("The specific language was not found in the json file.");
			}
			return messages.Translations[key];
        }
		catch (Exception ex)
		{
			_logger.LogError("Error looking up the custom text", ex);
			throw;
		}
		
	}
}
