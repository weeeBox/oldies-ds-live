import java.io.File;

import org.dom4j.Document;
import org.dom4j.DocumentException;
import org.dom4j.io.SAXReader;

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
		return "RESOURCE_TYPE_FONT";
	}
	
	@Override
	public String getResourceTypePrefix() 
	{
		return "FNT_";
	}
}