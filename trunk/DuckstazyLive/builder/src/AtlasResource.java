import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import javax.imageio.ImageIO;

import org.apache.tools.ant.BuildException;

import atlas.Atlas;
import atlas.AtlasImage;
import atlas.AtlasPacker;
import atlas.AtlasWriter;
import atlas.Packable;
import font.CharImage;
import font.FontInfo;
import font.FontInfoReader;
import font.FontWriter;

public class AtlasResource extends Resource 
{
	private static final String IMPORTER = "AtlasImporter";
	private static final String PROCESSOR = "AtlasProcessor";

	static
	{
		registerResource(IMPORTER, PROCESSOR);
	}
	
	private List<Resource> resources;
	private List<FontInfo> fonts;
	
	public AtlasResource() 
	{
		resources = new ArrayList<Resource>();
		fonts = new ArrayList<FontInfo>();
	}
	
	@Override
	public void process() 
	{		
		List<Packable> packables = process(resources);
		
		AtlasPacker packer = new AtlasPacker();
		packer.doPacking(packables, AtlasPacker.FAST);
		
		Atlas atlas = new Atlas(getName());
		for (int imageIndex = 0; imageIndex < packables.size(); ++imageIndex)
		{			
			Packable packable = packables.get(imageIndex);
			if (packable instanceof CharImage)
			{
				continue;
			}
			
			AtlasImage img = (AtlasImage) packable;
			atlas.add(img);
		}
		
		try 
		{
			File parentDir = ContentProjTask.resDir;
			File atlasfFile = new File(parentDir, atlas.getName() + ".atlas");
			File textureFile = new File(parentDir, atlas.getTexName() + ".png");
			
			setFile(atlasfFile);
			
			AtlasWriter writer = new AtlasWriter();
			writer.write(atlas, atlasfFile, textureFile);
			
			Image textureImage = new Image();
			textureImage.setFile(textureFile);
			textureImage.setName(atlas.getTexName());
			textureImage.process();
			
			ContentProjTask.fileSync.addFile(atlasfFile);
			ContentProjTask.projSync.addResource(this);
			
			exportFonts(resources);
		} 
		catch (IOException e) 
		{
			e.printStackTrace();
		}
	}
	
	private void exportFonts(List<Resource> resources) 
	{
		try 
		{
			for (FontInfo info : fonts) 
			{
				exportFont(info);
			}
		} 
		catch (IOException e) 
		{
			e.printStackTrace();
			throw new BuildException(e.getMessage());
		}
	}

	public void exportFont(FontInfo info) throws IOException 
	{
		String fontName = info.getName();
		
		File fontOutput = new File(ContentProjTask.resDir, fontName + ".pixelfont");
		FontWriter writer = new FontWriter(info, "tex_" + getName());
		writer.write(fontOutput);
		
		PixelFont pixelFont = new PixelFont();
		pixelFont.setName(fontName);
		pixelFont.setFile(fontOutput);
		pixelFont.process();
		
		Package pack = getPackage();
		pack.addPixelFont(pixelFont);
	}

	private List<Packable> process(List<Resource> resources) 
	{
		List<Packable> packables = new ArrayList<Packable>();
		for (Resource res : resources) 
		{
			if (res instanceof Image)
			{
				try 
				{
					BufferedImage image = ImageIO.read(res.getFile());
					AtlasImage atlasImage = new AtlasImage(image);
					packables.add(atlasImage);
				} 
				catch (IOException e) 
				{
					e.printStackTrace();
				}
			}
			else if (res instanceof PixelFont)
			{
				File file = res.getFile();
				try 
				{
					FontInfoReader reader = new FontInfoReader();
					FontInfo info = reader.read(file);
					fonts.add(info);
					
					List<CharImage> images = info.getCharsImages();
					for (CharImage charImage : images) 
					{
						packables.add(charImage);
					}					
				} 
				catch (IOException e) 
				{
					e.printStackTrace();
				}
			}
		}
		
		return packables;
	}

	public void addImage(Image res)
	{
		resources.add(res);
	}
	
	public void addPixelFont(PixelFont font)
	{
		resources.add(font);
	}
	
	public List<Resource> resources()
	{
		return resources;
	}
	
	@Override
	public String getImporter() 
	{
		return IMPORTER;
	}

	@Override
	public String getProcessor() 
	{
		return PROCESSOR;
	}

	@Override
	public String getResourceType() 
	{
		return "RESOURCE_TYPE_ATLAS";
	}

	@Override
	public String getResourceTypePrefix() 
	{
		return "ATLAS_";
	}
}
