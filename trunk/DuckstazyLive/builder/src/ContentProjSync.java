import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import org.apache.tools.ant.BuildException;
import org.dom4j.Document;
import org.dom4j.DocumentException;
import org.dom4j.Element;
import org.dom4j.io.OutputFormat;
import org.dom4j.io.SAXReader;
import org.dom4j.io.XMLWriter;


public class ContentProjSync 
{
	private List<Resource> resources;
	
	public ContentProjSync()
	{
		resources = new ArrayList<Resource>();
	}
	
	public void addResource(Resource res)
	{
		resources.add(res);
	}
	
	public void sync(File projFile)
	{
		try 
		{
			Document doc = new SAXReader().read(projFile);
			processContentProj(doc, projFile);
		} 
		catch (DocumentException e) 
		{
			e.printStackTrace();
			throw new BuildException(e.getMessage());
		}
	}
	
	private void processContentProj(Document doc, File projFile)
	{	
		clearOldItems(doc);
		addNewItems(doc);
		writeDocument(doc, projFile);
	}

	private void clearOldItems(Document doc) 
	{
		List<ContentPair> contentPairs = Resource.getContentPairs();
		System.out.println("Content pairs:");
		for (ContentPair contentPair : contentPairs) 
		{
			System.out.println(contentPair);
		}
		
		List<Element> itemGroups = doc.getRootElement().elements("ItemGroup");
		for (Element e : itemGroups) 
		{
			List<Element> children = e.elements();
			for (Element child : children) 
			{
				if (child.getName().equals("Compile"))
				{
					Element importerElement = child.element("Importer");
					Element processorElement = child.element("Processor");
					if (importerElement != null && processorElement != null)
					{	
						String importer = importerElement.getText();
						String processor = processorElement.getText();
						ContentPair pair = new ContentPair(importer, processor);
						if (contentPairs.contains(pair))							
							e.remove(child);
					}					
				}
			}
			if (e.elements().isEmpty())
				e.getParent().remove(e);
		}
	}
	
	private void addNewItems(Document doc) 
	{
		Element parent = doc.getRootElement().addElement("ItemGroup");
		for (Resource res : resources) 
		{
			addResource(res, parent);
		}
	}

	private void addResource(Resource res, Element parent) 
	{
		Element element = parent.addElement("Compile");
		element.addAttribute("Include", res.getFile().getName());
		element.addElement("Name").addText(res.getShortName());
		element.addElement("Importer").addText(res.getImporter());
		element.addElement("Processor").addText(res.getProcessor());
	}

	private void writeDocument(Document doc, File file)
	{
		try 
		{
			FileOutputStream stream = new FileOutputStream(file);
			
			OutputFormat format = OutputFormat.createPrettyPrint();
			XMLWriter writer = new XMLWriter(stream, format);
			writer.write(doc);
			writer.flush();
			
			stream.close();
			writer.close();
		} 
		catch (IOException e) 
		{
			e.printStackTrace();
		}
	}
}
