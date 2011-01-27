
public class VectorFont extends Resource 
{	
	private static final String IMPORTER = "FontDescriptionImporter";
	private static final String PROCESSOR = "FontDescriptionProcessor";

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
		return "RESOURCE_TYPE_VECTOR_FONT";
	}
	
	@Override
	public String getResourceTypePrefix() 
	{
		return "FNT_";
	}
}
