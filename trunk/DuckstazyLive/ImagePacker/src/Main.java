import java.awt.Color;
import java.awt.Graphics;
import java.awt.image.BufferedImage;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.IOException;
import java.util.ArrayList;

import javax.imageio.ImageIO;

public class Main 
{	
	static ArrayList<Packable> imageList = new ArrayList<Packable>();
	
	public static void main(String[] args) throws IOException 
	{
		File dir = new File(args[0]);
		File[] files = dir.listFiles();
		for (File f : files) 
		{
			BufferedImage img = ImageIO.read(f);
			AtlasImage packable = new AtlasImage();
			packable.setImage(img);
			
			imageList.add(packable);
		}		
		
		AtlasPacker packer = new AtlasPacker();
		packer.doPacking(imageList, AtlasPacker.BEST);

		BufferedImage atlas = new BufferedImage(getAtlasWidth(), getAtlasHeight(), BufferedImage.TYPE_INT_ARGB);

		Graphics g = atlas.createGraphics();		
		for (Packable img : imageList) 
		{
			AtlasImage image  = (AtlasImage)img;
			g.drawImage(image.getImage(), image.getX(), image.getY(), null);
		}
				
		ImageIO.write(atlas, "png", new File("d:/output.png"));
	}	
	
	public static int getAtlasWidth()
	{
		int w = 0;
		for (Packable img : imageList)
		{
			if (img.getX() + img.getWidth() > w)
			{
				w = img.getX() + img.getWidth();
			}
		}
		return w;
	}

	public static int getAtlasHeight()
	{
		int h = 0;
		for (Packable img : imageList)
		{
			if (img.getY() + img.getHeight() > h)
			{
				h = img.getY() + img.getHeight();
			}
		}
		return h;
	}
}
