
public class Song extends Resource 
{
	private static final String IMPORTER = "Mp3Importer";
	private static final String PROCESSOR = "SongProcessor";

	static
	{
		registerResource(IMPORTER, PROCESSOR);
	}
	
	@Override
	public String getImporter() 
	{
		return IMPORTER;
	}

	@Override
	public String getProcessor() 
	{
		return PROCESSOR;
	}

	@Override
	public String getResourceType() 
	{
		return "RESOURCE_TYPE_SONG";
	}

	@Override
	public String getResourceTypePrefix() 
	{
		return "SONG_";
	}

}
