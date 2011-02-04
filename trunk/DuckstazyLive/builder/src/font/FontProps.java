package font;

import java.io.File;
import java.io.IOException;
import java.util.HashMap;
import java.util.Map;
import java.util.Scanner;

public class FontProps
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