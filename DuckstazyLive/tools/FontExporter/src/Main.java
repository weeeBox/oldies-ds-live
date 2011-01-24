import java.awt.image.BufferedImage;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.util.Properties;

import javax.imageio.ImageIO;

public class Main 
{
	public static void main(String[] args) throws IOException 
	{
		File propsFile = new File(args[0]);
		
		Properties props = new Properties();
		FileInputStream fis = null;
		try
		{
			fis = new FileInputStream(propsFile);
			props.load(fis);
		}
		finally
		{
			if (fis != null)
				fis.close();
		}
		
		File baseDir = propsFile.getParentFile(); 
		File fontFile = new File(baseDir, props.getProperty("src"));
		File outputFile = new File(baseDir, props.getProperty("dst"));
		String outputInfoName = outputFile.getName().substring(0, fontFile.getName().lastIndexOf('.')) + ".pixelfont";
		File outputInfoFile = new File(baseDir, outputInfoName);
		String charString = props.getProperty("chars");
		
		int spaceWidth = Integer.parseInt(props.getProperty("space"));
		int charOffset = Integer.parseInt(props.getProperty("charOffset"));
		int lineOffset = Integer.parseInt(props.getProperty("lineOffset"));
		int fontOffset = Integer.parseInt(props.getProperty("fontOffset"));
		
		FontInfo fontInfo = FontExtractor.extract(fontFile, charString);
		fontInfo.setLineOffset(lineOffset);
		fontInfo.setCharOffset(charOffset);
		fontInfo.setSpaceWidth(spaceWidth);
		fontInfo.setFontOffset(fontOffset);
		
		BufferedImage fontImage = fontInfo.pack(1024, 4);
		ImageIO.write(fontImage, "png", outputFile);
		
		FontWriter writer = new FontWriter(fontInfo, outputFile.getName());
		writer.write(outputInfoFile);
	}
}
