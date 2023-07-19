using System;
using System.Collections.Generic;
using System.Text;
using MinorShift.Emuera.GameData.Expression;

namespace MinorShift.Emuera.Sub
{
	/// <summary>
	/// 字句解析結果の保存場所。Listとその現在位置を結びつけるためのもの。
	/// 基本的に全てpublicで
	/// </summary>
	internal sealed class WordCollection
	{
		public List<Word> Collection = new List<Word>();
		public int Pointer = 0;
		private static Word nullToken = new NullWord();
		public void Add(Word token)
		{
			Collection.Add(token);
		}
		public void Add(WordCollection wc)
		{
			Collection.AddRange(wc.Collection);
		}

		public void Clear()
		{
			Collection.Clear();
		}

		public void ShiftNext() { Pointer++; }
		public Word Current
		{
			get
			{
				if (Pointer >= Collection.Count)
					return nullToken;
				return Collection[Pointer]; 
			} 
		}
		#region EM_私家版_HTMLパラメータ拡張
		public Word Next
		{
			get
			{
				if (Pointer + 1 >= Collection.Count)
					return nullToken;
				return Collection[Pointer + 1];
			}
		}
		#endregion
		public bool EOL { get { return Pointer >= Collection.Count; } }

		public void Insert(Word w)
		{
			Collection.Insert(Pointer, w);
		}
		public void InsertRange(WordCollection wc)
		{
			Collection.InsertRange(Pointer, wc.Collection);
		}
		public void Remove()
		{
			Collection.RemoveAt(Pointer);
		}
		
		public void SetIsMacro()
		{
			foreach(Word word in Collection)
			{
				word.SetIsMacro();
			}
		}
		
		public WordCollection Clone()
		{
			WordCollection ret = new WordCollection();
			for(int i = 0;i < this.Collection.Count;i++)
			{
				ret.Collection.Add(this.Collection[i]);
			} 
			return ret;
		}
		public WordCollection Clone(int start, int count)
		{
			WordCollection ret = new WordCollection();
			if (start > this.Collection.Count)
				return ret;
			int end = start + count;
			if (end > this.Collection.Count)
				end = this.Collection.Count;
			for(int i = start;i < end;i++)
			{
				ret.Collection.Add(this.Collection[i]);
			} 
			return ret;
		}
		
	}
}
















