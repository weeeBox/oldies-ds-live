import java.awt.Image;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;

import javax.imageio.ImageIO;


public class AtlasImage implements Packable
{	
	private static final int ALIAS_BORDER = 4;
	
	private BufferedImage image;
	private String id;
	private int x;
	private int y;

	public void setImage(BufferedImage image) 
	{
		this.image = image;
	}

	public void setFile(File file)
	{	
		try 
		{
			image = ImageIO.read(file);
		} 
		catch (IOException e) 
		{
			e.printStackTrace();
		}
	
	}

	public Image getImage()
	{
		return image;
	}

	public void setId(String id)
	{
		this.id = id;
	}

	public String getId()
	{
		return id;
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

}
