import java.io.File;

public abstract class Resource 
{
	private String name;
	private File file;
	
	public String getName() 
	{
		return name;
	}
	
	public void setName(String name) 
	{
		this.name = name;
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
	
	@Override
	public String toString() 
	{
		return String.format("name='%s' path='%s'", name, file);
	}
}
