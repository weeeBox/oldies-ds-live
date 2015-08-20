
public class Image extends Resource 
{
	private static final String IMPORTER = "TextureImporter";
	private static final String PROCESSOR = "TextureProcessor";

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
		return "RESOURCE_TYPE_TEXTURE";
	}
	
	@Override
	public String getResourceTypePrefix() 
	{
		return "IMG_";
	}
}
