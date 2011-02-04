package font;

import java.awt.Rectangle;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import javax.imageio.ImageIO;

public class FontExtractor 
{	
	public static FontInfo extract(File fontFile, String chars) throws IOException 
	{
		BufferedImage fontImage = ImageIO.read(fontFile);
		int width = fontImage.getWidth();
		int height = fontImage.getHeight();
		int[] argb = new int[width * height];
		fontImage.getRGB(0, 0, width, height, argb, 0, width);
		
		List<Rectangle> rects = process(argb, width, height);
		FontInfo info = new FontInfo();
		for (int i = 0; i < rects.size(); ++i) 
		{			
			Rectangle r = rects.get(i);
			char c = chars.charAt(i);
			info.addCharImage(new CharImage(fontImage, c, r.x, r.y, r.width, r.height, 0, r.y));	
		}
		
		return info;
	}

	private static List<Rectangle> process(int[] argb, int width, int height) 
	{
		List<Rectangle> rects = new ArrayList<Rectangle>();
		
		final int MAX = Integer.MAX_VALUE;
		final int MIN = Integer.MIN_VALUE;
		
		int startX, startY, endX, endY;		
		startX = startY = MAX;
		endX = endY = MIN;
		boolean transparentCol;
		for (int x = 0; x < width; ++x)
		{
			transparentCol = true;
			
			for (int y = 0; y < height; ++y)
			{
				int index = y * width + x;
				int pixel = argb[index];				
				
				if (!isTransparent(pixel))
				{
					transparentCol = false;
					if (x < startX)
					{
						startX = x;
					}
					if (y < startY)
					{
						startY = y;
					}
					if (startX != MAX && x > endX)
					{
						endX = x;
					}
					if (startY != MAX && y > endY)
					{
						endY = y;
					}					
				}
			}
			
			if (x == width - 1)
				transparentCol = true;
			
			if (transparentCol && endX != MIN && endY != MIN)
			{				
				rects.add(new Rectangle(startX, startY, endX - startX + 1, endY - startY + 1));
				startX = startY = MAX;
				endX = endY = MIN;
			}
		}		
		
		return rects;
	}
	
	private static boolean isTransparent(int pixel)
	{
		return (pixel & 0xff000000) == 0;
	}
}
