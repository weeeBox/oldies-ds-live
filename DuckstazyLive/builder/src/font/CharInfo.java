package font;

public class CharInfo 
{
	protected char chr;
	protected int x;
	protected int y;
	protected int width;
	protected int height;
	protected int offX, offY;

	public CharInfo(char chr, int x, int y, int width, int height, int offX, int offY) 
	{
		this.chr = chr;
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
		this.offX = offX;
		this.offY = offY;
	}
	
	public char getChar() 
	{
		return chr;
	}
	
	public int getOffX() 
	{
		return offX;
	}
	
	public int getOffY() 
	{
		return offY;
	}

	public int getX() 
	{
		return x;
	}

	public int getY() 
	{
		return y;
	}

	public int getWidth() 
	{
		return width;
	}

	public int getHeight() 
	{
		return height;
	}
}