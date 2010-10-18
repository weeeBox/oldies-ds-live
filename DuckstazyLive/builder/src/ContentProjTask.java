import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import org.apache.tools.ant.BuildException;
import org.apache.tools.ant.Task;
import org.dom4j.Document;
import org.dom4j.DocumentException;
import org.dom4j.Element;
import org.dom4j.io.OutputFormat;
import org.dom4j.io.SAXReader;
import org.dom4j.io.XMLWriter;

public class ContentProjTask extends Task
{
	private File projFile;
	private List<Package> packages = new ArrayList<Package>();
	
	@Override
	public void execute() throws BuildException 
	{
		if (projFile == null || projFile.isDirectory())
			throw new BuildException("Bad 'projFile': " + projFile);
		
		try 
		{
			Document doc = new SAXReader().read(projFile);
			process(doc);
		} 
		catch (DocumentException e) 
		{
			e.printStackTrace();
			throw new BuildException(e.getMessage());
		}
	}
	
	private void process(Document doc)
	{	
		clearOldItems(doc);
		addNewItems(doc);
		writeDocument(doc, projFile);
	}

	private void clearOldItems(Document doc) 
	{
		List<Element> itemGroups = doc.getRootElement().elements("ItemGroup");
		for (Element e : itemGroups) 
		{
			List<Element> children = e.elements();
			for (Element child : children) 
			{
				if (!child.getName().equals("Reference"))
				{
					e.remove(child);
				}
			}
			if (e.elements().isEmpty())
				e.getParent().remove(e);
		}
	}
	
	private void addNewItems(Document doc) 
	{
		Element parent = doc.getRootElement().addElement("ItemGroup");
		for (Package pack : packages) 
		{
			addPackage(pack, parent);
		}
	}

	private void addPackage(Package pack, Element parent) 
	{
		List<Resource> resources = pack.getResources();
		for (Resource res : resources) 
		{
			addResource(res, parent);
		}
	}

	private void addResource(Resource res, Element parent) 
	{
		Element element = parent.addElement("Compile");
		element.addAttribute("Include", res.getFile().getName());
		element.addElement("Name").addText(res.getName());
		element.addElement("Importer").addText(res.getImporter());
		element.addElement("Processor").addText(res.getProcessor());
	}

	private void writeDocument(Document doc, File file)
	{
		try 
		{
			FileOutputStream stream = new FileOutputStream("d:/temp.xml");
			
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
	
	public void addPackage(Package pack)
	{
		packages.add(pack);
	}
	
	public void setProjFile(File projFile) 
	{
		this.projFile = projFile;
	}
}
