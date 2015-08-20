
public class Sound extends Resource 
{
	private static final String IMPORTER = "WavImporter";
	private static final String PROCESSOR = "SoundEffectProcessor";

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
		return "RESOURCE_TYPE_SOUND";
	}

	@Override
	public String getResourceTypePrefix() 
	{
		return "SND_";
	}

}
