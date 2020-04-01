using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;

namespace Core
{
	public static class ConsoleUtilities
	{
		static string RemoveDiacritics(string text)
		{
			return string.Concat( 
				text.Normalize(NormalizationForm.FormD)
				.Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch)!= UnicodeCategory.NonSpacingMark)
				).Normalize(NormalizationForm.FormC);
		}

		static string NormalizeIdent( string ident, bool trace = false )
		{
			var lower = ident.ToLower();
			var repped = Regex.Replace( lower, @"[\s\p{P}\p{S}]+", "");
			var stripped = RemoveDiacritics( repped );

			if (trace)
			{
				Debug.LogFormat( "{0} => {1} => {2} => {3}", ident, lower, repped, stripped );
			}

			return stripped;
		}

		static bool LooseMatch( string ident, string candidate, bool trace = false )
		{
			// flatten case and collapse spaces & punctuation
			var normalizedIdent = NormalizeIdent( ident, trace );
			var normalizedCandidate = NormalizeIdent( candidate, trace );

			return normalizedCandidate.StartsWith( normalizedIdent );
		}

		// Given a dictionary of string to any type, and an identifier,
		// return a list of dictionary keys which the identifier might
		// be referring to.
		public static string[] Lookup<T>( string ident, Dictionary<string,T> source, bool trace = false )
		{
			List<string> result = new List<string>();
			foreach (var kv in source)
			{
				if (LooseMatch(ident,kv.Key,trace))
				{
					result.Add(kv.Key);
				}
			}

			return result.OrderBy(k=>k).ToArray();
		}

		static bool TestLookupCase( Dictionary<string,string> dict, string ident, int expect )
		{
			var hits = Lookup(ident,dict);
			if (hits.Length == expect)
			{
				return true;
			}

			Debug.LogWarningFormat("Loose lookup test failed, key '{0}' expected {1} hit {2}:", ident, expect, hits.Length );
			foreach (var h in hits)
			{
				Debug.LogWarning( "  "+h);
			}

			// redo the test with trace on
			Lookup(ident,dict,trace:true);

			return false;
		}

		public static void TestLookup()
		{
			var dict = new Dictionary<string,string>()
			{
				{ "simple", "x" },
				{ "SIM card", "x" },
				{ "sPoNgeCaSe", "x"},
				{ "spongecake", "x"},
				{ "ALLCAPS", "x"},
				{ "all-stars", "x"},
				{ "T'Pau", "x"},
				{ "T-Pain", "x"},
				{ "Rémi Jacquinot", "x" },
				{ "Remington Steele", "x" },
				{ "Jean-Claude l'Epouvante", "x" },
				{ "Jean-Claude Lungfish", "x" },
			};

			int pass = 0;
			int fail = 0;

			if (TestLookupCase( dict, "simple", 1 )) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "zimple", 0 )) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "sIm", 2 )) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "spon", 2)) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "spongecases", 0)) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "spongecase", 1)) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "spöngecäse", 1)) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "al", 2)) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "all ", 2)) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "åll ", 2)) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "all/", 2)) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "all-", 2)) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "....all", 2)) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "all.......!>!>!>st", 1)) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "tp", 2)) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "t'p", 2)) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "t-p", 2)) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "t*p", 2)) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "----tpa---", 2)) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "tpâü", 1)) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "remi", 2)) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "remij", 1)) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "rémin", 1)) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "jean-cl", 2)) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "jeancl", 2)) {pass++;} else {fail++;}
			if (TestLookupCase( dict, "jeanclaudel'epou", 1)) {pass++;} else {fail++;}

			Debug.LogFormat("Tested liberal string matching, {0} passes, {1} failures.", pass, fail);
		}
	}
}