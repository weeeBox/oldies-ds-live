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
	private File codeFile;
	private List<Package> packages = new ArrayList<Package>();
	
	@Override
	public void execute() throws BuildException 
	{
		if (projFile == null || projFile.isDirectory())
			throw new BuildException("Bad 'projFile': " + projFile);
		if (codeFile == null || codeFile.isDirectory())
			throw new BuildException("Bad 'codeFile': " + codeFile);
		
		updateFiles(projFile.getParentFile());
		processContentProj();
		generateResourcesCode(codeFile);
	}

	private void updateFiles(File contentDir) 
	{
		FileUtils.deleteFiles(contentDir, new String[] {".png", ".mp3", ".wav"});
		for (Package pack : packages) 
		{
			List<Resource> packResources = pack.getResources();
			for (Resource res : packResources) 
			{
				System.out.println("Copy: " + res.getFile() + " to " + contentDir);
				FileUtils.copy(res.getFile(), contentDir);
			}
		}
	}

	private void generateResourcesCode(File file) 
	{
		try 
		{
			new CodeFileGenerator().generate(file, packages);
		} 
		catch (IOException e) 
		{
			e.printStackTrace();
			throw new BuildException(e.getMessage());
		}
	}

	private void processContentProj() 
	{
		try 
		{
			Document doc = new SAXReader().read(projFile);
			processContentProj(doc);
		} 
		catch (DocumentException e) 
		{
			e.printStackTrace();
			throw new BuildException(e.getMessage());
		}
	}
	
	private void processContentProj(Document doc)
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
				if (child.getName().equals("Compile"))
				{
					Element importer = child.element("Importer");
					Element processor = child.element("Processor");
					if (importer != null && processor != null)
					{												
						if (!("TextureImporter".equals(importer.getText())))
							continue;
						if (!("TextureProcessor".equals(processor.getText())))
							continue;
							
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
	
	public void addPackage(Package pack)
	{
		packages.add(pack);
	}
	
	public void setCodeFile(File codeFile) 
	{
		this.codeFile = codeFile;
	}
	
	public void setProjFile(File projFile) 
	{
		this.projFile = projFile;
	}
}
