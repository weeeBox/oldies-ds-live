package font;

import java.io.File;
import java.io.IOException;

public class FontInfoReader 
{
	public FontInfo read(File file) throws IOException
	{		
		FontProps props = new FontProps(file);
		
		File fontFile = new File(file.getParentFile(), props.getString("src"));
		
		String name = props.getString("name");
		String charString = props.getString("chars");		
		int spaceWidth = props.getInt("space");
		int charOffset = props.getInt("charOffset");
		int lineOffset = props.getInt("lineOffset");
		int fontOffset = props.getInt("fontOffset");
		
		FontInfo fontInfo = FontExtractor.extract(fontFile, charString);
		fontInfo.setName(name);
		fontInfo.setLineOffset(lineOffset);
		fontInfo.setCharOffset(charOffset);
		fontInfo.setSpaceWidth(spaceWidth);
		fontInfo.setFontOffset(fontOffset);
		
		return fontInfo;		
	}
}
