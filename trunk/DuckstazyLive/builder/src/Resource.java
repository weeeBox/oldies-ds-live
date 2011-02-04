import java.io.File;
import java.util.ArrayList;
import java.util.List;

public abstract class Resource 
{	
	private String name;
	private File file;
	private Package pack;
		
	private static List<ContentPair> contentPairs = new ArrayList<ContentPair>();
	
	public static void registerResource(String importer, String processor)
	{
		ContentPair pair = new ContentPair(importer, processor);
		if (!contentPairs.contains(pair))
			contentPairs.add(pair);
	}
	
	public static List<ContentPair> getContentPairs() 
	{
		return contentPairs;
	}
	
	public Package getPackage() 
	{
		return pack;
	}

	public void setPackage(Package parent) 
	{
		this.pack = parent;
	}

	public String getName()
	{
		return name;
	}
	
	public String getLongName() 
	{
		return (getResourceTypePrefix() + name).toUpperCase();
	}
	
	public void setName(String name) 
	{
		this.name = name;
	}
	
	public String getShortName()
	{
		return FileUtils.getFilenameNoExt(file);
	}
	
	public File getFile() 
	{
		return file;
	}
	
	public void setFile(File file) 
	{
		this.file = file;
	}
	
	public abstract String getImporter();
	public abstract String getProcessor();
	public abstract String getResourceType();
	public abstract String getResourceTypePrefix();
	
	@Override
	public String toString() 
	{
		return String.format("name='%s' path='%s'", name, file);
	}	

	@Override
	public boolean equals(Object obj) 
	{
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		Resource other = (Resource) obj;
		if (!file.equals(other.file))
			return false;
		if (!name.equals(other.name))
			return false;
		return true;
	}

	public void process() 
	{
		ContentProjTask.fileSync.addFile(getFile());
		ContentProjTask.projSync.addResource(this);
	}	
}

class ContentPair
{
	private String importer;
	private String processor;

	public ContentPair(String importer, String processor) 
	{
		this.importer = importer;
		this.processor = processor;
	}

	@Override
	public int hashCode() 
	{
		final int prime = 31;
		int result = 1;
		result = prime * result
				+ ((importer == null) ? 0 : importer.hashCode());
		result = prime * result
				+ ((processor == null) ? 0 : processor.hashCode());
		return result;
	}

	@Override
	public boolean equals(Object obj) 
	{
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		ContentPair other = (ContentPair) obj;
		if (importer == null) 
		{
			if (other.importer != null)
				return false;
		} 
		else if (!importer.equals(other.importer))
			return false;
		if (processor == null) 
		{
			if (other.processor != null)
				return false;
		} 
		else if (!processor.equals(other.processor))
			return false;
		return true;
	}
	
	@Override
	public String toString() 
	{
		return String.format("importer=%s processor=%s", importer, processor);
	}
}
