package atlas;

import java.awt.image.BufferedImage;

public class AtlasImage implements Packable
{	
	static final int ALIAS_BORDER = 4;
	
	private BufferedImage image;
	private int x;
	private int y;
	private int ox;
	private int oy;
	private boolean exportInfo;
	
	public AtlasImage(BufferedImage image) 
	{
		this.image = image;
		setExportInfo(true);
	}

	public BufferedImage getImage()
	{
		return image;
	}

	public int getRealX()
	{
		return x + ALIAS_BORDER;
	}
	
	public int getRealY()
	{
		return y + ALIAS_BORDER;
	}
	
	public int getRealWidth() 
	{
		return image.getWidth();
	}
	
	public int getRealHeight() 
	{
		return image.getHeight();
	}
	
	@Override
	public int getHeight() 
	{
		return getRealHeight() + 2 * ALIAS_BORDER;
	}	

	@Override
	public int getWidth() 
	{
		return getRealWidth() + 2 * ALIAS_BORDER;
	}

	@Override
	public int getX() 
	{
		return x;
	}

	@Override
	public int getY() 
	{
		return y;
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

	public boolean isExportInfo() 
	{
		return exportInfo;
	}

	public void setExportInfo(boolean exportInfo) 
	{
		this.exportInfo = exportInfo;
	}
}
