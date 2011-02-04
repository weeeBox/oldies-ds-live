package atlas;

import java.awt.image.BufferedImage;

public class AtlasImage implements Packable
{	
	private static final int ALIAS_BORDER = 2;
	
	private BufferedImage image;
	private int x;
	private int y;
	private int ox;
	private int oy;
	
	public AtlasImage(BufferedImage image) 
	{
		this.image = image;
	}

	public BufferedImage getImage()
	{
		return image;
	}

	@Override
	public int getHeight() 
	{
		return image.getHeight() + 2 * ALIAS_BORDER;
	}

	@Override
	public int getWidth() 
	{
		return image.getWidth() + 2 * ALIAS_BORDER;
	}

	@Override
	public int getX() 
	{
		return x + ALIAS_BORDER;
	}

	@Override
	public int getY() 
	{
		return y + ALIAS_BORDER;
	}

	@Override
	public void setX(int x) 
	{
		this.x = x;
	}

	@Override
	public void setY(int y) 
	{
		this.y = y;
	}

	public int getOx() 
	{
		return ox;
	}

	public void setOx(int ox) 
	{
		this.ox = ox;
	}

	public int getOy() 
	{
		return oy;
	}

	public void setOy(int oy) 
	{
		this.oy = oy;
	}
}
