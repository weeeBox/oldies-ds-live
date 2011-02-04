package font;

import java.util.ArrayList;
import java.util.List;

public class FontInfo 
{	
	private int charOffset;
	private int lineOffset;
	private int spaceWidth;
	private int fontOffset;
	private String name;
	private List<CharImage> charsImages;
	
	public FontInfo()
	{
		charsImages = new ArrayList<CharImage>();
	}
	
	public void addCharImage(CharImage c)
	{		
		charsImages.add(c);
	}
	
	public List<CharImage> getCharsImages()
	{
		return charsImages;
	}
	
	public int getCharOffset() 
	{
		return charOffset;
	}

	public void setCharOffset(int charOffset) 
	{
		this.charOffset = charOffset;
	}

	public int getLineOffset() 
	{
		return lineOffset;
	}

	public void setLineOffset(int lineOffset) 
	{
		this.lineOffset = lineOffset;
	}	
	
	public int getSpaceWidth() 
	{
		return spaceWidth;
	}
	
	public void setSpaceWidth(int spaceWidth) 
	{
		this.spaceWidth = spaceWidth;
	}	
	
	public int getFontOffset() 
	{
		return fontOffset;
	}

	public String getName() 
	{
		return name;
	}

	public void setName(String name) 
	{
		this.name = name;
	}

	public void setFontOffset(int fontOffset) 
	{
		this.fontOffset = fontOffset;
	}
}
