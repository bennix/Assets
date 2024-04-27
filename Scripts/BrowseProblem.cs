using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

using System.Linq;
using System.Security.Cryptography;
using UnityEditor.Experimental;
using Lean.Gui;
using Unity.VisualScripting;


public class BroseProblem : MonoBehaviour
{
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

	//public Entry randomEntry;
	//public List<string> uniqueValues;
	private List<Entry> dictionary; // 词典

	//public List<string> randomSelection;
	[SerializeField] public Button prevBtn; // 引用你的按钮
	[SerializeField] public Button btnNext;
	[SerializeField] public Text uiWord;
	[SerializeField] public Text uiExplain;
	[SerializeField] public Text uiSentence;

	public IEnumerable<words> words;
	public List<string> ExplainList; // 包含重复项目的列表

	public IEnumerable<words> basewords;

	public int currentIdx = 0;
	// Use this for initialization
	void Start()
	{
		ds = new DataService("existing.db");
		//ds.CreateDB ();
		prevBtn.onClick.AddListener(OnPrevlick);
		btnNext.onClick.AddListener(OnNextClick);


		dictionary = new List<Entry>();
		words = ds.GetWords();
		basewords = ds._connection.Query<words>("SELECT * FROM problemwords");
		ToConsole (words,basewords);
	}

	void OnPrevlick()
	{
		currentIdx -= 1;
		if (currentIdx <= 0)
		{
			currentIdx = 0;
		}
	}

	void OnNextClick()
	{
		currentIdx += 1;
		if (currentIdx >= dictionary.Count - 1)
		{
			currentIdx = dictionary.Count - 1;
		}
	}

	private void Update()
	{
		if (dictionary.Count <= 0)
		{
			btnNext.gameObject.SetActive(false);
			prevBtn.gameObject.SetActive(false);
			uiSentence.text = "数据库为空！";
			uiExplain.text = "";
			uiWord.text = "";
			btnNext.gameObject.SetActive(false);
			prevBtn.gameObject.SetActive(false);
			
		}
		else
		{
			uiWord.text = dictionary[currentIdx].Word;
			uiExplain.text = dictionary[currentIdx].Definition;
			uiSentence.text = dictionary[currentIdx].Example;
			
			if (currentIdx == 0)
			{
				prevBtn.gameObject.SetActive(false);
				btnNext.gameObject.SetActive(true);
			}

			if (currentIdx == dictionary.Count-1)
			{
				btnNext.gameObject.SetActive(false);
				prevBtn.gameObject.SetActive(true);
			}
			else
			{
				btnNext.gameObject.SetActive(true);
				prevBtn.gameObject.SetActive(true);
			
			}
		}

	}

	public void ToConsole(IEnumerable<words> words, IEnumerable<words> basewords)
	{
		foreach (var wo in basewords)
		{

			ExplainList.Add(wo.explain);


		}

		foreach (var wo in basewords)
		{


			Entry entry = new Entry(wo.word, wo.explain, wo.example, wo.source);
			dictionary.Add(entry);

		}
	}
}
