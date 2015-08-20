import java.awt.image.BufferedImage;

public class CharImage extends CharInfo
{	
	private BufferedImage image;
	
	public CharImage(BufferedImage source, char chr, int x, int y, int width, int height, int offX, int offY)
	{		
		super(chr, x, y, width, height, offX, offY);
		image = source.getSubimage(x, y, width, height);
	}
	
	public BufferedImage getImage() 
	{
		return image;
	}	
}