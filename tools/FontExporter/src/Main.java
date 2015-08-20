import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.util.HashMap;
import java.util.Map;
import java.util.Scanner;

import javax.imageio.ImageIO;

class FontProps
{
	private Map<String, String> map;
	
	public FontProps(File file) throws IOException 
	{
		map = read(file);
	}

	private Map<String, String> read(File file) throws IOException 
	{
		Map<String, String> map = new HashMap<String, String>();
		
		Scanner scanner = new Scanner(file);
		while(scanner.hasNextLine())
		{
			String line = scanner.nextLine();
			if (line.startsWith("#"))
			{
				continue;
			}
			
			int index = line.indexOf('=');
			if (index == -1)
			{
				throw new RuntimeException(line);
			}
			
			String name = line.substring(0, index);
			String value = line.substring(index + 1, line.length());
			
			map.put(name, value);
		}
		
		return map;
	}
	
	public String getString(String name)
	{
		return getString(name, null);
	}
	
	public String getString(String name, String defaultValue)
	{
		if (map.containsKey(name))
		{
			return map.get(name);
		}
		return defaultValue;
	}
	
	public int getInt(String name)
	{
		return getInt(name, 0);
	}
	
	public int getInt(String name, int defaultValue)
	{
		if (map.containsKey(name))
		{
			return Integer.parseInt(map.get(name));
		}
		return defaultValue;
	}
}


public class Main 
{
	public static void main(String[] args) throws IOException 
	{
		File propsFile = new File(args[0]);
		FontProps props = new FontProps(propsFile);
		
		File baseDir = propsFile.getParentFile(); 
		File fontFile = new File(baseDir, props.getString("src"));
		File outputFile = new File(baseDir, props.getString("dst"));
		String outputInfoName = outputFile.getName().substring(0, fontFile.getName().lastIndexOf('.')) + ".pixelfont";
		File outputInfoFile = new File(baseDir, outputInfoName);
		String charString = props.getString("chars");
		
		int spaceWidth = props.getInt("space");
		int charOffset = props.getInt("charOffset");
		int lineOffset = props.getInt("lineOffset");
		int fontOffset = props.getInt("fontOffset");
		
		FontInfo fontInfo = FontExtractor.extract(fontFile, charString);
		fontInfo.setLineOffset(lineOffset);
		fontInfo.setCharOffset(charOffset);
		fontInfo.setSpaceWidth(spaceWidth);
		fontInfo.setFontOffset(fontOffset);
		
		BufferedImage fontImage = fontInfo.pack(2048, 4);
		ImageIO.write(fontImage, "png", outputFile);
		
		FontWriter writer = new FontWriter(fontInfo, outputFile.getName());
		writer.write(outputInfoFile);
	}	
}
