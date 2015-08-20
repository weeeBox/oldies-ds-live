import java.awt.Graphics;
import java.awt.image.BufferedImage;
import java.util.ArrayList;
import java.util.List;

public class FontInfo 
{	
	private int charOffset;
	private int lineOffset;
	private int spaceWidth;
	private int fontOffset;
	private List<CharImage> charsImages;
	private List<CharInfo> packedChars;
	
	public FontInfo()
	{
		charsImages = new ArrayList<CharImage>();
		packedChars = new ArrayList<CharInfo>();		
	}
	
	public void addCharImage(CharImage c)
	{		
		charsImages.add(c);
	}	
	
	public BufferedImage pack(int packWidth, int aliasBorder)
	{
		int totalHeight = 0;
		int totalWidth = 0;		
		int lineHeight = 0;
		int px = aliasBorder;
		int py = aliasBorder;
		packedChars.clear();
		for (CharInfo c : charsImages) 
		{
			int charWidth = aliasBorder + c.getWidth();
			int charHeight = c.getY() + c.getHeight();
			if (lineHeight < charHeight)
			{
				lineHeight = charHeight;
			}
			
			packedChars.add(new CharInfo(c.getChar(), px, py, c.getWidth(), c.getHeight(), c.getOffX(), c.getOffY()));
			
			if (totalWidth < px + charWidth)
				totalWidth = px + charWidth;
			if (totalHeight < py + charHeight)
				totalHeight = py + charHeight;
			
			px += charWidth;
			
			if (px > packWidth)
			{		
				px = aliasBorder;
				py += lineHeight;
				lineHeight = 0;				
			}			
		}		
		
		BufferedImage packedImage = new BufferedImage(totalWidth, totalHeight, BufferedImage.TYPE_INT_ARGB);
		Graphics g = packedImage.getGraphics();		
		for (int i = 0; i < charsImages.size(); ++i) 
		{
			CharImage img = charsImages.get(i);
			CharInfo packed = packedChars.get(i);
						
			g.drawImage(img.getImage(), packed.x, packed.y, null);			
		}
		g.dispose();
		
		return packedImage;
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

	public void setFontOffset(int fontOffset) 
	{
		this.fontOffset = fontOffset;
	}

	public List<CharInfo> getPackedChars() 
	{
		return packedChars;
	}
}
