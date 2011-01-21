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
		int lineWidth = 0;
		int lineHeight = 0;
		packedChars.clear();
		for (CharInfo c : charsImages) 
		{
			int charHeight = c.getY() + c.getHeight();
			if (lineHeight < charHeight)
			{
				lineHeight = charHeight;
			}
			
			int px, py;
			int charWidth = aliasBorder + c.getWidth();
			if (lineWidth + charWidth > packWidth)
			{
				if (totalWidth < lineWidth)
				{
					totalWidth = lineWidth;
				}
				
				totalHeight += lineHeight + aliasBorder;
				lineWidth = 0;
				lineHeight = 0;
				px = 0;				
			}
			else
			{
				px = lineWidth;				 
			}
			lineWidth += charWidth;				
			py = totalHeight;
			
			packedChars.add(new CharInfo(c.getChar(), px, py, c.getWidth(), c.getHeight(), c.getOffX(), c.getOffY()));
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
