import java.util.ArrayList;
import java.util.List;

public class Package 
{
	private List<Resource> resources = new ArrayList<Resource>();
	private String name;
	
	public String getName() 
	{
		return name;
	}

	public void setName(String name) 
	{
		this.name = name;
	}

	public void addImage(Image image)
	{
		resources.add(image);
	}
	
	public void addPixelFont(PixelFont font)
	{
		resources.add(font);
	}
	
	public void addVectorFont(VectorFont font)
	{
		resources.add(font);
	}
	
	public void addSound(Sound sound)
	{
		resources.add(sound);
	}
	
	public void addSong(Song song)
	{
		resources.add(song);
	}
	
	public List<Resource> getResources()
	{
		return resources;
	}
	
	@Override
	public String toString() 
	{
		StringBuilder buf = new StringBuilder();
		buf.append("Package: " + name );
		for (Resource res : resources) 
		{
			buf.append("\n\t" + res);
		}
		return buf.toString();
	}
}
