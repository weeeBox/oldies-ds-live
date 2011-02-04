package font;

import java.awt.image.BufferedImage;

import atlas.AtlasImage;
import atlas.Packable;

public class CharImage extends AtlasImage implements Packable
{	
	private char chr;
	
	public CharImage(BufferedImage source, char chr, int x, int y, int width, int height, int offX, int offY)
	{		
		super(source.getSubimage(x, y, width, height));
		this.chr = chr;
		setX(x);
		setY(y);
		setOx(offX);
		setOy(offY);	
		setExportInfo(false);
	}

	public char getChar() 
	{
		return chr;
	}
}