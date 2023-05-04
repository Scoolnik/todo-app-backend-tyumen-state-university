namespace TODOAppBackend;
public class AppAuthSettings
{
	public string Secret { get; set; }
	public string TokenLifetimeValue { get; set; }
	public TimeSpan TokenLifetime
	{
		get 
		{ 
			if (_tokenLifetime == TimeSpan.MinValue) 
				_tokenLifetime = TimeSpan.Parse(TokenLifetimeValue); 
			return _tokenLifetime; 
		}
		set => _tokenLifetime = value;
	}

	private TimeSpan _tokenLifetime = TimeSpan.MinValue;
}
