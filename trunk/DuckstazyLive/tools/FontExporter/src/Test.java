import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.Properties;

public class Test 
{
	public static void main(String[] args) 
	{
		Properties props = new Properties();
		props.put("name", "big");
		props.put("src", "font_big_src.png");
		props.put("dst", "font_big.png");
		props.put("chars", "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`{|}~");
		props.put("space", "20");
		props.put("charOffset", "-6");
		props.put("lineOffset", "-6");
		try 
		{
			props.store(new FileOutputStream(new File("d:/temp.txt")), null);
		} 
		catch (FileNotFoundException e) 
		{
			e.printStackTrace();
		}
		catch (IOException e) 
		{
			e.printStackTrace();
		}
	}
}
