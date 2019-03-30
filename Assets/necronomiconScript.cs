using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;

public class necronomiconScript : MonoBehaviour 
{
	public KMBombInfo bomb;
	public KMAudio Audio;

	static System.Random rnd = new System.Random();

	public KMSelectable pageTurner;
	public KMSelectable cover;

	public GameObject[] pages;
	public TextMesh coverText;
	public GameObject axis;

	public GameObject[] coverParts;
	public Material[] mat;

	static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

	bool isOpen;
	int currentPage;
	int correctPage;
	int lastUpdatedPage;

	Stopwatch watch;

	int place;
	GreatOldOne god;

	int[] chapters;
	int[] selectedChapters;
	List<int> validChapters;
	List<int> priorityOrder;

	int[][] priority = new int[][] { new int[] {2, 11, 21, 26, 16},
									 new int[] {30, 3, 19, 24, 23},
									 new int[] {33, 7, 35, 12, 6},
									 new int[] {38, 31, 13, 22, 32},
									 new int[] {28, 37, 1, 39, 5},
									 new int[] {29, 25, 15, 17, 36},
									 new int[] {18, 40, 10, 34, 14},
									 new int[] {8, 27, 20, 4, 9} };

	bool playing;
	bool animating;

	void Awake()
	{
		moduleId = moduleIdCounter++;

		cover.OnInteract += delegate () { OpenBook(); return false; };
		pageTurner.OnInteract += delegate () { PageTurn(); return false; };
	}

	void Start () 
	{
		isOpen = false;
		playing = true;
		animating = false;
		currentPage = 0;
		lastUpdatedPage = -1;

		chapters = new int [] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40};
		chapters = chapters.OrderBy(x => rnd.Next()).ToArray();

		ChoosePlace();
		god = new GreatOldOne();
		coverText.text = god.coverText;
		SelectChapters();
		EvalChapters();
		
		UnityEngine.Debug.LogFormat("[The Necronomicon #{0}] The book's place of origin is {1}.", moduleId, GetPlaceOfOrigin());
		UnityEngine.Debug.LogFormat("[The Necronomicon #{0}] The cover's expression is {1} (translated: {2}) [{3}].", moduleId, god.coverTextClean, god.coverTextTranlation, GetType(god.coverTextType));
		UnityEngine.Debug.LogFormat("[The Necronomicon #{0}] The worshiped Great Old One is {1}.", moduleId, god.name);
		UnityEngine.Debug.LogFormat("[The Necronomicon #{0}] The chapters present are {1}.", moduleId, String.Join(",", selectedChapters.Select(p => p.ToString()).ToArray()));		
		UnityEngine.Debug.LogFormat("[The Necronomicon #{0}] The valid chapters are {1}.", moduleId, String.Join(",", validChapters.ToArray().Select(p => p.ToString()).ToArray()));
		UnityEngine.Debug.LogFormat("[The Necronomicon #{0}] The chapter priority is {1}.", moduleId, String.Join(",", priorityOrder.ToArray().Select(p => p.ToString()).ToArray()));
		UnityEngine.Debug.LogFormat("[The Necronomicon #{0}] The correct chapter is {1} (page {2}).", moduleId, selectedChapters[correctPage - 1], correctPage);
	}
	
	void Update () 
	{
		if(!isOpen)
		{
			lastUpdatedPage = -1;
			watch = null;
		}
		else if(currentPage == lastUpdatedPage)
		{
			if(watch.ElapsedMilliseconds >= 5000)
			{
				if(currentPage == correctPage)
				{
					playing = false;
					isOpen = false;
					GetComponent<KMBombModule>().HandlePass();
					UnityEngine.Debug.LogFormat("[The Necronomicon #{0}] Solved! The selected chapter was {1} (page {2}).", moduleId, selectedChapters[currentPage - 1], currentPage);
					StartCoroutine("CloseBookAnim");
				}
				else
				{
					isOpen = false;
					GetComponent<KMBombModule>().HandleStrike();

					if(currentPage == 8)
					{
						UnityEngine.Debug.LogFormat("[The Necronomicon #{0}] Strike! The book was opened in the forbidden page.", moduleId);
					}
					else
					{
						UnityEngine.Debug.LogFormat("[The Necronomicon #{0}] Strike! The selected chapter was {1} (page {2}).", moduleId, selectedChapters[currentPage - 1], currentPage);
					}

					StartCoroutine("CloseBookAnim");
				}
			}
		}
		else
		{
			lastUpdatedPage = currentPage;
			watch = System.Diagnostics.Stopwatch.StartNew();
		}
	}

	void OpenBook()
	{
		if(!isOpen && !animating && playing)
		{
			isOpen = true;
			currentPage++;
			StartCoroutine("OpenBookAnim");
		}
	}

	void PageTurn()
	{
		if(animating || !playing)
			return;

		if(currentPage == 8)
		{
			isOpen = false;
			StartCoroutine("CloseBookAnim");
		}
		else
		{
			Audio.PlaySoundAtTransform("pageTurn", transform);			
			StartCoroutine(PageTurnAnim(pages[currentPage - 1]));
			currentPage++;
		}
	}

	void ChoosePlace()
	{
		place = rnd.Next() % 5;
		SetBookCover();
	}

	void SelectChapters()
	{
		int pos;

		for(pos = 0; pos < chapters.Length; pos++)
		{
			if(IsValidChapter(chapters[pos]))
				break;
		}

		selectedChapters = new int[] {chapters[pos], chapters[pos+1], chapters[pos+2], chapters[pos+3], chapters[pos+4], chapters[pos+5], chapters[pos+6]};
		Array.Sort(selectedChapters);

		for(int i = 0; i < pages.Length; i++)
		{
			pages[i].GetComponentInChildren<TextMesh>().text = selectedChapters[i].ToString();
		}
	}

	bool IsValidChapter(int chapter)
	{
		switch(chapter)
		{
			case 1:
			{
				return (god.index == 2 || god.index == 4);
			}
			case 2:
			{
				return god.index == 0;
			}
			case 3:
			{
				return (place == 1 || place == 3);
			}
			case 4:
			{
				return (god.index == 4 || god.index == 7);
			}
			case 5:
			{
				return (place == 1 || place == 4);
			}
			case 6:
			{
				return god.index != 1;
			}
			case 7:
			{
				return god.index == 2;				
			}
			case 8:
			{
				return place == 0;				
			}
			case 9:
			{
				return (place == 0 || place == 4);				
			}
			case 10:
			{
				return place == 2;				
			}
			case 11:
			{
				return place == 1;				
			}
			case 12:
			{
				return (place == 2 || place == 3);				
			}
			case 13:
			{
				return (place == 1 || place == 2);				
			}
			case 14:
			{
				return (place == 4 || god.index == 6);				
			}
			case 15:
			{
				return (place == 2 || place == 4);				
			}
			case 16:
			{
				return (god.index == 0 || god.index == 1);				
			}
			case 17:
			{
				return (god.index == 3 || god.index == 5);				
			}
			case 18:
			{
				return (place == 0 || place == 2);				
			}
			case 19:
			{
				return god.index == 1;				
			}
			case 20:
			{
				return (god.index == 6 || god.index == 7);				
			}
			case 21:
			{
				return (god.index == 0 || god.index == 1 || god.index == 3 || god.index == 4);				
			}
			case 22:
			{
				return god.index == 3;				
			}
			case 23:
			{
				return god.index == 1;				
			}
			case 24:
			{
				return (place == 0 || place == 3);				
			}
			case 25:
			{
				return (god.index == 2 || god.index == 5);				
			}
			case 26:
			{
				return god.coverTextType == 0;
			}
			case 27:
			{
				return (place == 0 || place == 1);				
			}
			case 28:
			{
				return (god.index == 4 || god.index == 5 || god.index == 6 || god.index == 7);				
			}
			case 29:
			{
				return god.index == 5;				
			}
			case 30:
			{
				return god.index == 1;				
			}
			case 31:
			{
				return god.index == 3;				
			}
			case 32:
			{
				return (place == 3 || place == 4);				
			}
			case 33:
			{
				return (god.index == 2 || god.index == 7);				
			}
			case 34:
			{
				return (god.index == 0 || god.index == 6);				
			}
			case 35:
			{
				return (place == 0 || place == 1 || place == 2);				
			}
			case 36:
			{
				return (place == 3 || place == 4);				
			}
			case 37:
			{
				return god.coverTextType == 2;
			}
			case 38:
			{
				return god.coverTextType == 1;
			}
			case 39:
			{
				return place == 3;				
			}
			case 40:
			{
				return true;
			}
		}

		return false;
	}

	void SetBookCover()
	{
		foreach(GameObject g in coverParts)
		{
			g.GetComponentsInChildren<Renderer>()[0].material = mat[place];
		}
	}

	String GetPlaceOfOrigin()
	{
		switch(place)
		{
			case 0:
			{
				return "Plateau of Leng";
			}
			case 1:
			{
				return "Sarnath";
			}
			case 2:
			{
				return "Dylath-Leen";
			}
			case 3:
			{
				return "Yuggoth";
			}
			case 4:
			{
				return "R'lyeh";
			}
		}

		return "";
	}

	String GetType(int type)
	{
		switch(type)
		{
			case 0:
				return "true name";
			case 1:
				return "alias";
			case 2:
				return "family relationship";
		}

		return "";
	}

	void EvalChapters()
	{
		validChapters = new List<int>();

		foreach(int chapter in selectedChapters)
		{
			if(IsValidChapter(chapter))
				validChapters.Add(chapter);
		}

		Array.Sort(chapters);

		SelectSolution();
	}

	void SelectSolution()
	{
		priorityOrder = new List<int>();
		bool found = false;

		for(int i = 0; i <= place; i++)
		{
			if(!found && validChapters.Exists(x => x == priority[god.index][i]))
			{
				correctPage = Array.FindIndex<int>(selectedChapters, x => x == priority[god.index][i]) + 1;
				found = true;
			}

			priorityOrder.Add(priority[god.index][i]);
		}

		for(int i = god.index + 1; i < priority.Length; i++)
		{
			if(!found && validChapters.Exists(x => x == priority[i][place]))
			{
				correctPage = Array.FindIndex<int>(selectedChapters, x => x == priority[i][place]) + 1;
				found = true;
			}
			
			priorityOrder.Add(priority[i][place]);
		}

		for(int i = 0; i < god.index; i++)
		{
			if(!found && validChapters.Exists(x => x == priority[i][place]))
			{
				correctPage = Array.FindIndex<int>(selectedChapters, x => x == priority[i][place]) + 1;
				found = true;
			}

			priorityOrder.Add(priority[i][place]);
		}

		if(!found)
			correctPage = Array.FindIndex<int>(selectedChapters, x => x == validChapters[0]) + 1;
	}

	IEnumerator OpenBookAnim()
	{
		animating = true;

		for(int i = 0; i < 20; i++)
		{
			cover.transform.RotateAround(axis.transform.position, axis.transform.up, 5f);
			yield return new WaitForSeconds(0.01f);
		}

		animating = false;

	}

	IEnumerator CloseBookAnim()
	{
		animating = true;

		int pageIdx = currentPage;

		while(pageIdx > 1)
		{
			Audio.PlaySoundAtTransform("pageTurn", transform);
			StartCoroutine(PageBackAnim(pages[pageIdx - 2]));
			pageIdx--;
			yield return new WaitForSeconds(0.05f);
		}

		StartCoroutine("CloseCoverAnim");

		currentPage = 0;

		animating = false;
	}

	IEnumerator PageTurnAnim(GameObject page)
	{
		animating = true;

		for(int i = 0; i < 20; i++)
		{
			page.transform.RotateAround(axis.transform.position, axis.transform.up, 5f);
			yield return new WaitForSeconds(0.01f);
		}

		animating = false;
	}

	IEnumerator PageBackAnim(GameObject page)
	{
		for(int i = 0; i < 20; i++)
		{
			page.transform.RotateAround(axis.transform.position, axis.transform.up, -5f);
			yield return new WaitForSeconds(0.01f);
		}
	}

	IEnumerator CloseCoverAnim()
	{
		for(int i = 0; i < 20; i++)
		{
			if(i == 5)
				Audio.PlaySoundAtTransform("bookClose", transform);

			cover.transform.RotateAround(axis.transform.position, axis.transform.up, -5f);
			yield return new WaitForSeconds(0.01f);
		}
	}
}
