package atlas;

import java.awt.image.BufferedImage;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.io.UnsupportedEncodingException;
import java.util.List;

import javax.imageio.ImageIO;

import org.dom4j.Document;
import org.dom4j.DocumentHelper;
import org.dom4j.Element;
import org.dom4j.io.OutputFormat;
import org.dom4j.io.XMLWriter;

public class AtlasWriter 
{
	public void write(Atlas atlas, File atlasFile, File textureFile) throws IOException
	{
		writeXml(atlas, atlasFile);
		writePng(atlas, textureFile);
	}

	private void writeXml(Atlas atlas, File file) throws IOException 
	{
		FileOutputStream fos = null;
		try
		{
			fos = new FileOutputStream(file);
			writeXml(atlas, fos);
		}
		finally
		{
			if (fos != null)
				fos.close();
		}
	}
	
	private void writeXml(Atlas atlas, OutputStream stream) throws IOException
	{
		Document doc = DocumentHelper.createDocument();
		
		Element root = doc.addElement("atlas");
		root.addAttribute("filename", atlas.getTexName());
		
		List<AtlasImage> images = atlas.getImages();
		for (AtlasImage img : images) 
		{
			if (!img.isExportInfo())
			{
				continue;
			}
			
			Element e = root.addElement("image");			
			e.addAttribute("x", Integer.toString(img.getRealX()));
			e.addAttribute("y", Integer.toString(img.getRealY()));
			e.addAttribute("w", Integer.toString(img.getRealWidth()));
			e.addAttribute("h", Integer.toString(img.getRealHeight()));
			e.addAttribute("ox", Integer.toString(img.getOx()));
			e.addAttribute("oy", Integer.toString(img.getOy()));
		}
		
		try 
		{
			XMLWriter writer = new XMLWriter(stream, OutputFormat.createPrettyPrint());
			writer.write(doc);
		} 
		catch (UnsupportedEncodingException e) 
		{
			e.printStackTrace();
		}
	}
	
	private void writePng(Atlas atlas, File file) throws IOException 
	{
		BufferedImage image = atlas.createImage();
		ImageIO.write(image, "png", file);
	}
}
