
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

}
