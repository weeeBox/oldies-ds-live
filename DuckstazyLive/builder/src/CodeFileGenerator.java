import java.io.File;
import java.io.IOException;
import java.io.PrintStream;
import java.util.List;


public class CodeFileGenerator 
{
	public void generate(File file, List<Package> packs) throws IOException
	{
		PrintStream out = null;
		try 
		{
			out = new PrintStream(file);
			writeHeader(out);
			writeCode(out, packs);
			writeEnding(out);
			System.out.println("File created: " + file);
		} 
		finally 
		{
			if (out != null)
				out.close();
		}
	}

	private void writeHeader(PrintStream out) throws IOException 
	{
		out.println("// This file was generated. Do not modify.");
		out.println("using Framework.core;");
		out.println();
		out.println("namespace DuckstazyLive.app");
		out.println("{");
	}

	private void writeCode(PrintStream out, List<Package> packs) throws IOException
	{
		writePacksIds(out, packs);
		writeResIds(out, packs);
		writeResInfos(out, packs);
	}
	
	private void writePacksIds(PrintStream out, List<Package> packs) 
	{
		out.println("\tpublic class Packs");
		out.println("\t{");
		
		int packIndex;
		for (packIndex = 0; packIndex < packs.size(); packIndex++) 
		{			
			String packName = packs.get(packIndex).getName().toUpperCase();
			out.println("\t\tpublic const int " + packName + " = " + packIndex + ";");
		}
		out.println("\t\tpublic const int PACKS_COUNT = " + packIndex + ";");
		
		out.println("\t}");
	}

	private void writeResIds(PrintStream out, List<Package> packs) 
	{
		out.println("\tpublic class Res");
		out.println("\t{");
		
		int resIndex = 0;
		for (Package pack : packs) 
		{
			out.println("\t\t// " + pack.getName().toUpperCase());
			
			List<Resource> packResources = pack.getResources();
			for (Resource res : packResources) 
			{
				out.println("\t\tpublic const int " + res.getName() + " = " + resIndex + ";");
				resIndex++;
			}
		}
		out.println("\t\tpublic const int RES_COUNT = " + resIndex + ";");
		
		out.println("\t}");
	}

	private void writeResInfos(PrintStream out, List<Package> packs) 
	{
		out.println("\tpublic class DuckstazyResources");
		out.println("\t{");
		out.println("\t\tpublic static ResourceBaseInfo[][] RESOURCES_PACKS =");
		out.println("\t\t{");
		
		for (Package pack : packs) 
		{
			writePackInfo(out, pack);
		}
		
		out.println("\t\t};");
		out.println("\t}");
	}

	private void writePackInfo(PrintStream out, Package pack) 
	{
		out.println("\t\t\t// " + pack.getName().toUpperCase());
		out.println("\t\t\tnew ResourceBaseInfo[]");
		out.println("\t\t\t{");
		
		List<Resource> packResources = pack.getResources();
		for (Resource res : packResources) 
		{
			writeResInfo(out, res);
		}
		out.println("\t\t\t},");
	}

	private void writeResInfo(PrintStream out, Resource res) 
	{
		out.format("\t\t\t\tnew ResourceBaseInfo(Res.%s, ResourceType.%s, \"%s\"),", res.getName(), res.getResourceType(), res.getShortName());
		out.println(); // fix line endings
	}

	private void writeEnding(PrintStream out) throws IOException
	{
		out.println("}");
	}
}
