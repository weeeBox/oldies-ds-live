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
		addResource(image);
	}
	
	public void addAtlas(AtlasResource atlas)
	{
		addResource(atlas);
	}
	
	public void addPixelFont(PixelFont font)
	{
		addResource(font);
	}
	
	public void addVectorFont(VectorFont font)
	{
		addResource(font);
	}
	
	public void addSound(Sound sound)
	{
		addResource(sound);
	}
	
	public void addSong(Song song)
	{
		addResource(song);
	}
	
	private void addResource(Resource res)
	{
		res.setPackage(this);
		resources.add(res);
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
