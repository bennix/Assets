using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

using System.Linq;
using System.Security.Cryptography;
using UnityEditor.Experimental;
using Lean.Gui;
using Unity.VisualScripting;


public class  ProblemTest: MonoBehaviour {
	public class Entry
	{
		public string Word { get; set; } // 词条
		public string Definition { get; set; } // 解释
		public string Example { get; set; } // 例句
		public string Source { get; set; } // 来源

		public Entry(string word, string definition, string example, string source)
		{
			Word = word;
			Definition = definition;
			Example = example;
			Source = source;
		}
	}

	public DataService ds;
	public Entry randomEntry;
	public List<string> uniqueValues;
	private List<Entry> dictionary; // 词典
	public List<string> randomSelection;
	[SerializeField] public Button myButton; // 引用你的按钮
	[SerializeField] public Button btnCheck; 
	[SerializeField] public Text uiText1;
	[SerializeField] public Text uiText2;
	[SerializeField] public Text uiText3;
	[SerializeField] public Text uiText4;
	[SerializeField] public Text uiProblemWord;
	[SerializeField] public Text uiSentence;
	[SerializeField] public LeanToggle Option1;
	[SerializeField] public LeanToggle Option2;
	[SerializeField] public LeanToggle Option3;
	[SerializeField] public LeanToggle Option4;
	[SerializeField] public GameObject Info;
	[SerializeField] public Text InfoCaption;
	[SerializeField] public Text InfoMessage;
	[SerializeField] public Button BtnCloseInfo;
	[SerializeField] public GameObject BtnCheck;
	[SerializeField] public GameObject BtnNext;
	//[SerializeField] public GameObject BtnReset;
	//[SerializeField] public Button ButtonReset;
	public string word;
	public string explain;
	public string example;
	public string source;
	public IEnumerable<words> words; 
	public IEnumerable<words> basewords; 
	public List<string> ExplainList; // 包含重复项目的列表
	// Use this for initialization
	void Start () {
		ds = new DataService ("existing.db");
		//ds.CreateDB ();
		myButton.onClick.AddListener(OnButtonClick);
		btnCheck.onClick.AddListener(OnBtnCheck);
		BtnCloseInfo.onClick.AddListener(OnBtnCloseInfo);
		
		
		dictionary = new List<Entry>();
		basewords = ds._connection.Query<words>("SELECT * FROM basewords");
		words=ds._connection.Query<words>("SELECT * FROM problemwords");
		if (words.Count() <= 0)
		{
			//BtnReset.SetActive(false);
			btnCheck.GameObject().SetActive(false);
			BtnNext.GameObject().SetActive(false);
			Option1.gameObject.SetActive(false);
			Option2.gameObject.SetActive(false);
			Option3.gameObject.SetActive(false);
			Option4.gameObject.SetActive(false);
			uiProblemWord.text = "恭喜你！已经没有不熟悉的词语解释！";
			uiSentence.text = "";
		}
		else
		{
			//BtnReset.SetActive(false);
			btnCheck.GameObject().SetActive(true);
			BtnNext.GameObject().SetActive(true);
			ToConsole (basewords,words);
		}
		
	    
		
	}



	public void OnBtnCloseInfo()
	{
		Info.SetActive(false);
		BtnNext.SetActive(true);
		BtnCheck.SetActive(true);
		randomEntry = GetRandomEntry();
		if (randomEntry != null)
		{
			//Debug.Log("Random Entry:");
			//Debug.Log("Word: " + randomEntry.Word);
			//Debug.Log("Definition: " + randomEntry.Definition);
			//Debug.Log("Example: " + randomEntry.Example);
			//Debug.Log("Source: " + randomEntry.Source);
		}

		
		
		uiProblemWord.gameObject.SetActive(true);
		uiSentence.gameObject.SetActive(true);
		dictionary = new List<Entry>();
		basewords = ds._connection.Query<words>("SELECT * FROM basewords");
		words=ds._connection.Query<words>("SELECT * FROM problemwords");
		if (words.Count() <= 0)
		{
			//BtnReset.SetActive(false);
			btnCheck.GameObject().SetActive(false);
			BtnNext.GameObject().SetActive(false);
			Option1.gameObject.SetActive(false);
			Option2.gameObject.SetActive(false);
			Option3.gameObject.SetActive(false);
			Option4.gameObject.SetActive(false);
			uiProblemWord.text = "恭喜你！已经没有不熟悉的词语解释！";
			uiSentence.text = "";
		}
		else
		{
			//BtnReset.SetActive(false);
			btnCheck.GameObject().SetActive(true);
			BtnNext.GameObject().SetActive(true);
			ToConsole (basewords,words);
		}
	}
	public void OnButtonClick()
	{
		randomEntry = GetRandomEntry();
		if (randomEntry != null)
		{
			//Debug.Log("Random Entry:");
			//Debug.Log("Word: " + randomEntry.Word);
			//Debug.Log("Definition: " + randomEntry.Definition);
			//Debug.Log("Example: " + randomEntry.Example);
			//Debug.Log("Source: " + randomEntry.Source);
		}

		RemoveAndRandomSelect(randomEntry.Definition);
		
	}
	
	public void OnBtnCheck()
	{   
		
		
	
		string selAnswer="";
		if (Option1.On)
		{
			selAnswer = randomSelection[0];
		} else if (Option2.On)
		{
			selAnswer = randomSelection[1];
		} else if (Option3.On)
		{
			selAnswer = randomSelection[2];
		}
		else if (Option4.On)
		{
			selAnswer = randomSelection[3];
		}
		if (selAnswer == explain)
		{
			
			Debug.Log("CORRECT!");
			Info.SetActive(true);
			Info.GetComponent<Image>().color = Color.green;
			InfoCaption.text = word +" 答对了！";
			InfoCaption.color = Color.black;
			InfoMessage.text = selAnswer;
			InfoMessage.color =Color.black;
			uiProblemWord.gameObject.SetActive(false);
			uiSentence.gameObject.SetActive(false);

			string sqlcommand = "DELETE FROM problemwords WHERE word='" +
			                    word +
			                    "' AND explain='" +
			                    explain +
			                    "' AND example='" +
			                    example +
			                    "' AND source='" +
			                    source+
			                    "';";
			Debug.Log(sqlcommand);
			ds._connection.Execute(sqlcommand,new object[] { });
			ds = new DataService ("existing.db");
			dictionary = new List<Entry>();
			words=ds._connection.Query<words>("SELECT * FROM problemwords");
			basewords = ds._connection.Query<words>("SELECT * FROM basewords");
			
			words=ds._connection.Query<words>("SELECT * FROM problemwords");
			if (words.Count() <= 0)
			{
				//BtnReset.SetActive(false);
				btnCheck.GameObject().SetActive(false);
				BtnNext.GameObject().SetActive(false);
				Option1.gameObject.SetActive(false);
				Option2.gameObject.SetActive(false);
				Option3.gameObject.SetActive(false);
				Option4.gameObject.SetActive(false);
				uiProblemWord.text = "恭喜你！已经没有不熟悉的词语解释！";
				uiSentence.text = "";
			}
			else
			{
				//BtnReset.SetActive(false);
				btnCheck.GameObject().SetActive(true);
				BtnNext.GameObject().SetActive(true);
				ToConsole (basewords,words);
			}
			
		}
		else
		{
			Info.SetActive(true);
			Info.GetComponent<Image>().color = Color.blue;
			InfoCaption.text = "答错了！正确解释是：";
			
			InfoCaption.color = Color.white;
			InfoMessage.text = explain;
			InfoMessage.color = Color.white;
			
		}
		BtnNext.SetActive(false);
		BtnCheck.SetActive(false);
		
		
	}
	public void ToConsole(IEnumerable<words> basewords,IEnumerable<words> words){
		foreach (var wo in basewords) {
			
			ExplainList.Add (wo.explain);
			
			
		}
        foreach (var wo in words) {
		    Entry entry = new Entry(wo.word, wo.explain, wo.example, wo.source);
			dictionary.Add(entry);
		}
		uniqueValues = GetUniqueValues(ExplainList);

		foreach (var item in uniqueValues) {
			//Debug.Log(item);
		}

		//Debug.Log(dictionary.Count);
		randomEntry = GetRandomEntry();
		if (randomEntry != null)
		{
			//Debug.Log("Random Entry:");
			//Debug.Log("Word: " + randomEntry.Word);
			//Debug.Log("Definition: " + randomEntry.Definition);
			//Debug.Log("Example: " + randomEntry.Example);
			//Debug.Log("Source: " + randomEntry.Source);
		}

		RemoveAndRandomSelect(randomEntry.Definition);
		
	}


	
	
    private List<string> GetUniqueValues(List<string> list)
    {
        // 使用LINQ的Distinct()方法获取唯一值
        List<string> uniqueValues = list.Distinct().ToList();
        
        return uniqueValues;
    }

    public Entry GetRandomEntry()
    {
	    if (dictionary.Count == 0)
	    {
		    Debug.LogWarning("Dictionary is empty.");
		    return null;
	    }

	    int randomIndex = Random.Range(0, dictionary.Count);
	    return dictionary[randomIndex];
    }
    
    private void RemoveAndRandomSelect(string valueToRemove)
    {
	   
        if(uniqueValues.Contains(valueToRemove))
        {
	        uniqueValues.Remove(valueToRemove);
        }
	   

	    randomSelection = new List<string>();
        randomSelection.Add((valueToRemove));
	    int count = 0;
	    int randomIndex = 0;
	    while(count<3)
	    {
		    randomIndex  = Random.Range(0, uniqueValues.Count);
		    while (randomSelection.Contains(uniqueValues[randomIndex]))
		    {
			    randomIndex  = Random.Range(0, uniqueValues.Count);
			    
		    }

		    randomSelection.Add(uniqueValues[randomIndex]);
		    count += 1;
	    }

	    uniqueValues.Add(valueToRemove);
	    Debug.Log("Random Selection:");
	    foreach (string value in randomSelection)
	    {
		    //Debug.Log(value);
	    }
        ShuffleList(randomSelection);
	    //Debug.Log("After Random Selection:");
	    foreach (string value in randomSelection)
	    {
		    //Debug.Log(value);
	    }

	    uiText1.text = randomSelection[0];
	    uiText2.text = randomSelection[1];
	    uiText3.text = randomSelection[2];
        uiText4.text = randomSelection[3];
	    uiProblemWord.text = randomEntry.Word;
	    uiSentence.text = randomEntry.Example;
	    word= randomEntry.Word;
	    explain = randomEntry.Definition;
	    example = randomEntry.Example;
	    source = randomEntry.Source;

    }
    
    private void ShuffleList<T>(List<T> list)
    {
	    int n = list.Count;
	    while (n > 1)
	    {
		    n--;
		    int k = Random.Range(0, n + 1);
		    T value = list[k];
		    list[k] = list[n];
		    list[n] = value;
	    }
    }
}
