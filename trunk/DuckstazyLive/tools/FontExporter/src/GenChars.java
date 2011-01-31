import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;


public class GenChars 
{
	public static void main(String[] args) 
	{
		List<String> strings = new ArrayList<String>();
		strings.add("DAMMIT!");
		strings.add("DEAD...SORRY");
		
		Set<Character> chars = new HashSet<Character>();
		for (String str : strings) 
		{
			for (int i = 0; i < str.length(); i++) 
			{
				chars.add(str.charAt(i));
			}
		}
		
		StringBuilder result = new StringBuilder();
		for (Character character : chars) 
		{
			result.append(character.charValue());
		}
		
		System.out.println(result);
	}
}
