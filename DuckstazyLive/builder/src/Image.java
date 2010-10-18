
public class Image extends Resource 
{
	@Override
	public String getImporter() 
	{
		return "TextureImporter";
	}

	@Override
	public String getProcessor() 
	{
		return "TextureProcessor";
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
