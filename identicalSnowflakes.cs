using System;
// Basically translated Daniel Zingaro's C code from the book 'Algorithmic Thinking' to C# code
public class snowflakeNode {
	public snowflakeNode (){;}
	public int[] snowflake = new int[6];
	public snowflakeNode next;
}
public class Program
{
	public static bool are_any_identical(int[,] snowflakes){
		snowflakeNode[] snowflake_nodes = snowflakes_as_nodes(snowflakes);
		snowflakeNode snowflake_node1, snowflake_node2;
		for (int i =0; i < 100000; i++){
			snowflake_node1 = snowflake_nodes[i];
			while (snowflake_node1 != null){
				snowflake_node2 = snowflake_node1.next;
				while (snowflake_node2 != null){
					if (are_identical(snowflake_node1.snowflake, snowflake_node2.snowflake))
						return true;
					snowflake_node2 = snowflake_node2.next;
				}
				snowflake_node1 = snowflake_node1.next;
			}
		}
		return false;
	}
	public static snowflakeNode[] snowflakes_as_nodes(int[,] snowflakes){
		snowflakeNode[] snowflake_nodes = new snowflakeNode[100000]; // good enough
		int[] curr_snowflake = new int[6];
		snowflakeNode curr_snowflake_node = new snowflakeNode();
		int curr_snowflake_code;
		for (int i =0; i < snowflakes.GetLength(0); i++){
			for (int j = 0; j < 6; j++)
				curr_snowflake[j] = snowflakes[i,j];
			curr_snowflake_code = snowflake_code(curr_snowflake);
			curr_snowflake_node.snowflake = curr_snowflake;
			curr_snowflake_node.next = snowflake_nodes[curr_snowflake_code];
			snowflake_nodes[curr_snowflake_code] = curr_snowflake_node;
		}
		return snowflake_nodes;
		
	}
	public static int snowflake_code(int[] snow){
		return snow[0] + snow[1] + snow[2] + snow[3] + snow[4] + snow[5] %100000 ;
	}
	public static bool identical_right(int[] snow1, int[] snow2, int start){
		int offset;
		for (offset = 0; offset < 6; offset++){
			if (snow1[offset] != snow2[(start+offset) % 6]){
				return false;
			}
		}
		return true;
	}
	public static bool identical_left(int[] snow1, int[] snow2, int start){
		int offset, snow2_index;
		for (offset =0; offset < 6; offset++){
			snow2_index = start -offset;
			if (snow2_index < 0)
				snow2_index += 6;
			if (snow1[offset] != snow2[snow2_index])
				return false;
		}
		return true;
	}
	public static bool are_identical(int[] snow1, int[] snow2){
		for (int start = 0; start < 6; start++){
			if (identical_left(snow1,snow2,start) || identical_right(snow1,snow2,start))
				return true;
		}
		return false;
	}
	public static void Main()
	{
		// we got the complexity from the mighty O(n^2) to O(n * something), by only making effective comparisons 
		
	}