
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Client.Utils
{
   public class BoyerMooreStringMatcher
	{
		public string Pattern;

		private Dictionary<char, int> badCharactorShifts = new Dictionary<char, int>();

		private int[] goodSuffixShifts;

		private static int dstringmax = Convert.ToInt32("9fff", 16);

		private static int dstringmin = Convert.ToInt32("4e00", 16);

		public BoyerMooreStringMatcher(string tmpStr)
		{
			if (Pattern == null)
			{
				Pattern = tmpStr.ToLower();
				BuildBadCharactorHeuristic();
				BuildGoodSuffixHeuristic();
			}
		}

		public void UnLoad()
		{
			Pattern = null;
			badCharactorShifts.Clear();
			goodSuffixShifts = null;
		}

		private static int Max(int a, int b)
		{
			return (a <= b) ? b : a;
		}

		private static bool IsChinese(char mchar)
		{
			int num = Convert.ToInt32(mchar);
			return num >= BoyerMooreStringMatcher.dstringmin && num < BoyerMooreStringMatcher.dstringmax;
		}

		private static bool CheckWord(char mchar)
		{
			return char.IsWhiteSpace(mchar) || char.IsSymbol(mchar) || char.IsPunctuation(mchar) ||
			       BoyerMooreStringMatcher.IsChinese(mchar);
		}

		private void BuildBadCharactorHeuristic()
		{
			int length = Pattern.Length;
			for (int i = 0; i < length; i++)
			{
				if (!badCharactorShifts.ContainsKey(Pattern[i]))
				{
					badCharactorShifts.Add(Pattern[i], length - 1 - i);
				}
				else
				{
					badCharactorShifts[Pattern[i]] = length - 1 - i;
				}
			}
		}

		private void BuildGoodSuffixHeuristic()
		{
			int length = Pattern.Length;
			goodSuffixShifts = new int[length];
			int[] suffixLengthArray = GetSuffixLengthArray();
			for (int i = 0; i < length; i++)
			{
				goodSuffixShifts[i] = length;
			}

			int j = 0;
			for (int k = length - 1; k >= -1; k--)
			{
				if (k == -1 || suffixLengthArray[k] == k + 1)
				{
					while (j < length - 1 - k)
					{
						if (goodSuffixShifts[j] == length)
						{
							goodSuffixShifts[j] = length - 1 - k;
						}

						j++;
					}
				}
			}

			for (int l = 0; l < length - 1; l++)
			{
				goodSuffixShifts[length - 1 - suffixLengthArray[l]] = length - 1 - l;
			}
		}

		private int[] GetSuffixLengthArray()
		{
			int length = Pattern.Length;
			int[] array = new int[length];
			int num = 0;
			array[length - 1] = length;
			int num2 = length - 1;
			for (int i = length - 2; i >= 0; i--)
			{
				if (i > num2 && array[i + length - 1 - num] < i - num2)
				{
					array[i] = array[i + length - 1 - num];
				}
				else
				{
					if (i < num2)
					{
						num2 = i;
					}

					num = i;
					while (num2 >= 0 && Pattern[num2] == Pattern[num2 + length - 1 - num])
					{
						num2--;
					}

					array[i] = num - num2;
				}
			}

			return array;
		}

		private int GetBadCharactorShift(char tmp)
		{
			if (badCharactorShifts.ContainsKey(tmp))
			{
				return badCharactorShifts[tmp];
			}

			return Pattern.Length;
		}

		public bool TryMatch(string text, bool Accurate = false)
		{
			int length = text.Length;
			int length2 = Pattern.Length;
			int i = 0;
			while (i <= length - length2)
			{
				int num = length2 - 1;
				while (num >= 0 && Pattern[num] == char.ToLower(text[i + num]))
				{
					num--;
				}

				if (num < 0)
				{
					if (!Accurate || ((i == 0 || CheckWord(text[i - 1])) &&
					                  (i + length2 == length || CheckWord(text[i + length2]))))
					{
						return true;
					}

					i += goodSuffixShifts[0];
				}
				else
				{
					i += Max(goodSuffixShifts[num],
						GetBadCharactorShift(char.ToLower(text[i + num])) - (length2 - 1) + num);
				}
			}

			return false;
		}

		public void CheckAndRePlace(char[] text, bool Accurate = false)
		{
			int num = text.Length;
			int length = Pattern.Length;
			int i = 0;
			while (i <= num - length)
			{
				int num2 = length - 1;
				while (num2 >= 0 && Pattern[num2] == char.ToLower(text[i + num2]))
				{
					num2--;
				}

				if (num2 < 0)
				{
					if (!Accurate || ((i == 0 || CheckWord(text[i - 1])) &&
					                  (i + length == num || CheckWord(text[i + length]))))
					{
						for (int j = 0; j < length; j++)
						{
							text[i + j] = '*';
						}
					}

					i += goodSuffixShifts[0];
				}
				else
				{
					i += Max(goodSuffixShifts[num2],
						GetBadCharactorShift(char.ToLower(text[i + num2])) - (length - 1) + num2);
				}
			}
		}

		public unsafe void CheckAndRePlace(string text, bool Accurate = false)
		{
			int length = text.Length;
			int length2 = Pattern.Length;
			int i = 0;
			while (i <= length - length2)
			{
				int num = length2 - 1;
				while (num >= 0 && Pattern[num] == char.ToLower(text[i + num]))
				{
					num--;
				}

				if (num < 0)
				{
					if (!Accurate || ((i == 0 || CheckWord(text[i - 1])) &&
					                  (i + length2 == length || CheckWord(text[i + length2]))))
					{
					fixed (char* ptr = text)
					{
						for (int j = 0; j < length2; j++)
						{
							ptr[i + j] = '*';
						}
					}
					}

					i += goodSuffixShifts[0];
				}
				else
				{
					i += Max(goodSuffixShifts[num],
						GetBadCharactorShift(char.ToLower(text[i + num])) - (length2 - 1) + num);
				}
			}
		}
	}
}