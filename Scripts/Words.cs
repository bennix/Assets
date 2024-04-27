using SQLite4Unity3d;

public class words  {

	
	public string word { get; set; }
	public string explain { get; set; }
	public string example { get; set; }
	public string source { get; set; }

	public override string ToString ()
	{
		return string.Format ("[word: word={0}, explain={1},  example={2}, source={3}]", word, explain, example, source);
	}
}
