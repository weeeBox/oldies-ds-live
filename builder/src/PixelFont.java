
public class PixelFont extends Resource 
{	
	private static final String IMPORTER = "PixelFontImporter";
	private static final String PROCESSOR = "PixelFontProcessor";

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
		return "RESOURCE_TYPE_PIXEL_FONT";
	}
	
	@Override
	public String getResourceTypePrefix() 
	{
		return "FNT_";
	}
}