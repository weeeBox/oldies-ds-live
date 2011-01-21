import java.io.File;
import java.io.FileFilter;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;

public class FileUtils 
{
	public static String makeRelativePath(File parent, File child)
	{
		String parentPath = parent.getAbsolutePath();
		String childPath = child.getAbsolutePath();

		int len = Math.min(parentPath.length(), childPath.length());
		
		int differenceCharIndex;
		for (differenceCharIndex = 0; differenceCharIndex < len; differenceCharIndex++) 
		{
			if (parentPath.charAt(differenceCharIndex) != childPath.charAt(differenceCharIndex))
				break;
		}
		
		if (differenceCharIndex == 0)
			return childPath;
		
		String childSubPath = childPath.substring(differenceCharIndex);
		String parentSubPath = parentPath.substring(differenceCharIndex);
		
		int parentSubFoldersCount = 0;
		for (int chrIndex = 0; chrIndex < parentSubPath.length(); chrIndex++) 
		{
			if (parentSubPath.charAt(chrIndex) == File.separatorChar)
				parentSubFoldersCount++;
		}
		if (parentSubPath.length() > 0)
			parentSubFoldersCount++;
		
		StringBuilder result = new StringBuilder();
		for (int i = 0; i < parentSubFoldersCount; i++) 
		{
			result.append(".." + File.separatorChar);
		}
		if (childSubPath.startsWith(File.separator))
			childSubPath = childSubPath.substring(1);
		
		result.append(childSubPath);
		return result.toString();
	}
	
	public static File[] listFiles(File file, final String[] extensions) 
	{
		return file.listFiles(new FileFilter() 
		{
			@Override
			public boolean accept(File pathname) 
			{
				if (pathname.isDirectory())
					return false;
				
				String fileExt = getFileExt(pathname);
				for (String ext : extensions) 
				{
					if (fileExt.equalsIgnoreCase(ext))
						return true;
				}
				return false;
			}
		});
	}
	
	public static boolean copy(File src, File dst)
	{
		if (src.isDirectory())
			throw new AssertionError();
		
		if (dst.isDirectory())
			dst = new File(dst, src.getName());
		
		try 
		{
			FileInputStream fis = null;
			FileOutputStream fos = null;
			try 
			{
				fis = new FileInputStream(src);
				fos = new FileOutputStream(dst);
				byte[] buffer = new byte[4096];
				int read;
				while ((read = fis.read(buffer)) != -1)
				{
					fos.write(buffer, 0, read);
				}
			} 
			finally 
			{
				if (fis != null) fis.close();
				if (fos != null) fos.close();
			}
			return true;
		} 
		catch (IOException e) 
		{
			e.printStackTrace();
			return false;
		}
	}
	
	public static void deleteFiles(File dir, final String[] extensions)
	{
		File[] files = listFiles(dir, extensions);
		for (File file : files) 
		{
			file.delete();
		}
	}

	public static String getFileExt(File pathname) 
	{
		String filename = pathname.getName();
		return getFileExt(filename);
	}

	public static String getFileExt(String filename) 
	{
		int dotIndex = filename.lastIndexOf('.');
		
		if (dotIndex == -1)
			return "";
		
		return filename.substring(dotIndex);
	}
	
	public static String getFilenameNoExt(File pathname)
	{
		return getFilenameNoExt(pathname.getName());
	}

	public static String getFilenameNoExt(String filename) 
	{
		int dotIndex = filename.lastIndexOf('.');
		
		if (dotIndex == -1)
			return filename;
		
		return filename.substring(0, dotIndex);
	}
}
