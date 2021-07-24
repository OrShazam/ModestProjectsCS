using System;			
public class Program
{
	// kind of dumb to make my own implementation and not just use convert.frombase64/tobase64 
        // and yet use fancy system methods like .replace and .substring, yet I felt like doing something today and share it to the world.
	// I have no Idea why it doesn't actually work - if you do please enlight me.
	public static string addPadding(string binary){
		string binary_copy = binary;
		int length = binary.Length;
		while ((length++) < 8){
			binary_copy = "0" + binary_copy;
		}
		return binary_copy;
	}
	public static char decodeBinary(string binary){
		int value = 0; int count = 7;
		foreach(char ch in binary){
			if (ch == '1')
				value += (int)Math.Pow(2,(count));
			count--;
		}
		return (char)value;
	}
	public static string encodeBinary(char ch){
		string binary = "0"; int count = 6;
		int value = (int)ch;
		for (int i = 0; i < 7; i++){
			if (value >= (int)Math.Pow(2,(count--))){
				value -= (int)Math.Pow(2,count+1);
				binary += "1"; 
			}
			else {
				binary += "0";
			}	
		}
		return binary;	
	}
	public static string binaryString(string s){
		string binary_string = "";
		foreach (char ch in s){
			binary_string += encodeBinary(ch);
		}
		return binary_string;
	}
	public static string base64Encode(string s){
		string result = "";
		string binary = binaryString(s);
		for(int i = 0; i + 6 < binary.Length; i+= 6){
			result += String.Join("",decodeBinary("00" + binary.Substring(i,6)));
		}
		if (binary.Length % 6 != 0)
			result += String.Join("",decodeBinary(
				addPadding(binary.Substring(binary.Length / 6 * 6,binary.Length % 6))));
		return result;	
	}
	public static string base64Decode(string s){
		string result = "";
		string binary = binaryString(s);
		string binaryOriginal = binary.Replace("00","");
		// we don't need to specifically remove the added padding from the last binary value
		// as multiplies of 8 minus multiplies of 6 can only result in multiplies of 2, therefore the padding was already 
		// removed thanks to the string replace method
		for (int i = 0; i < binaryOriginal.Length; i+=8){
			result += String.Join("",decodeBinary(binaryOriginal.Substring(i,8)));
		}
		return result;		
	}
	public static void Main()
	{
		Console.WriteLine(@"Why should any of these things that happen externally, so much distract thee? 
		Give thyself leisure to learn some good thing, and cease roving and wandering to and fro.
		Thou must also take heed of another kind of wandering,
		for they are idle in their actions, who toil and labour in this life, and have no certain scope to
		which to direct all their motions, and desires.");
	}
}
