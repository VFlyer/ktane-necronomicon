using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rnd = UnityEngine.Random;

public class GreatOldOne {

	public int index;
	public String name;
	public String coverText;
	public String coverTextClean;
	public String coverTextTranlation;
	public int coverTextType;

	static Dictionary<int, String> nameDecoder = new Dictionary<int, String>
	{
		{0, "Azathoth"},
		{1, "Nyarlathotep"},
		{2, "Tsathoggua"},
		{3, "Yog-Sothoth"},
		{4, "Shub-Niggurath"},
		{5, "Nug and Yeb"},
		{6, "Cthulhu"},
		{7, "Hastur"}
	};

	static Dictionary<int, String[]> coverTextDecoder = new Dictionary<int, String[]>
	{
		{0, new String[]{"azathoth", "shogg'toth\nsathoth", "n'ghft\ngnaiih"} },
		{1, new String[]{"nyarla)\n(thotep", "krnug\nruugnah", "ugga-naach\ngnaiih", "azathoth\ngof'nn"} },
		{2, new String[]{"tsatho)\n(ggua", "n'kai\nfhtagn", "zvilpo)\n(gghua\ngnaiih", "ghisguth\ngof'nn", "zstylz)\n(hemghi\ngof'nn"} },
		{3, new String[]{"yog-)\n(sothoth", "yog'tah\nngulch'", "nug ngyeb\ngnaiih"} },
		{4, new String[]{"shub-)\n(niggurath", "n'ghft\nmnahn'ul", "nug ngyeb\n'fhalma", "n'ghft\ngof'nn"} },
		{5, new String[]{"nug ngyeb", "ah'ha\nhlirgh", "cthulhu\ngnaiih", "hastur\ngnaiih", "yog-)\n(sothoth\ngof'nn", "shub-)\n(niggurath\ngof'nn"} },
		{6, new String[]{"cthulhu", "r'lyeh\nfhtagn", "cthylla\ngnaiih"} },
		{7, new String[]{"Hastur", "haast'r\nsathoth", "ithaqua\ngnaiih"} }
	};

	static Dictionary<int, String[]> coverTextCleanDecoder = new Dictionary<int, String[]>
	{
		{0, new String[]{"Azathoth", "Shogg'toth Sathoth", "N'ghft Gnaiih"} },
		{1, new String[]{"Nyarlathotep", "Krnug Ruugnah", "Ugga-Naach Gnaiih", "Azathoth Gof'nn"} },
		{2, new String[]{"Tsathoggua", "N'Kai Fhtagn", "Zvilpogghua Gnaiih", "Ghisguth Gof'nn", "Zstylzhemghi Gof'nn"} },
		{3, new String[]{"Yog-Sothoth", "Yog'tah Ngulch'", "Nug Ngyeb Gnaiih"} },
		{4, new String[]{"Shub-Niggurath", "N'ghft Mnahn'ul", "Nug Ngyeb 'Fhalma", "N'ghft Gof'nn"} },
		{5, new String[]{"Nug Ngyeb", "Ah'ha Hlirgh", "Cthulhu Gnaiih", "Hastur Gnaiih", "Yog-Sothoth Gof'nn", "Shub-Niggurath Gof'nn"} },
		{6, new String[]{"Cthulhu", "R'lyeh Fhtagn", "Cthylla Gnaiih"} },
		{7, new String[]{"Hastur", "Haast'r Sathoth", "Ithaqua Gnaiih"} }
	};

	static Dictionary<int, String[]> translationDecoder = new Dictionary<int, String[]>
	{
		{0, new String[]{"Azathoth", "The Daemon Sultan", "Father of Darkness"} },
		{1, new String[]{"Nyarlathotep", "The Crawling Chaos", "Father of Ugga-Naach", "Son of Azathoth"} },
		{2, new String[]{"Tsathoggua", "The Sleeper of N'Kai", "Father of Zvilpogghua", "Son of Ghisguth", "Son of Zstylzhemghi"} },
		{3, new String[]{"Yog-Sothoth", "The Key and the Gate", "Father of Nug and Yeb"} },
		{4, new String[]{"Shub-Niggurath", "The Black Goat", "Mother of Nug and Yeb", "Daughter of Darkness"} },
		{5, new String[]{"Nug and Yeb", "The Twin Blasphemies", "Father of Cthulhu", "Father of Hastur", "Sons of Yog-Sothoth", "Sons of Shub-Niggurath"} },
		{6, new String[]{"Cthulhu", "The Sleeper of R'lyeh", "Father of Cthylla"} },
		{7, new String[]{"Hastur", "The King in Yellow", "Father of Ithaqua"} }
	};

	static Dictionary<int, int[]> typeDecoder = new Dictionary<int, int[]>
	{
		{0, new int[]{0, 1, 2} },
		{1, new int[]{0, 1, 2, 2} },
		{2, new int[]{0, 1, 2, 2, 2} },
		{3, new int[]{0, 1, 2} },
		{4, new int[]{0, 1, 2, 2} },
		{5, new int[]{0, 1, 2, 2, 2, 2} },
		{6, new int[]{0, 1, 2} },
		{7, new int[]{0, 1, 2} }
	};

	public GreatOldOne()
	{
		index = rnd.Range(0, 8);

		String[] coverTexts, coverTextsClean, translations;
		int[] types;

		nameDecoder.TryGetValue(index, out name);
		coverTextDecoder.TryGetValue(index, out coverTexts);
		coverTextCleanDecoder.TryGetValue(index, out coverTextsClean);
		translationDecoder.TryGetValue(index, out translations);
		typeDecoder.TryGetValue(index, out types);

		int pos = rnd.Range(0, coverTexts.Length);

		coverText = coverTexts[pos];
		coverTextClean = coverTextsClean[pos];
		coverTextTranlation = translations[pos];
		coverTextType = types[pos];
	}
}
