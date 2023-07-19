﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Data;

namespace MinorShift.Emuera.Sub
{
	//reader/writer共通のデータはreaderの方に


	/// <summary>
	/// 1808追加 新しいデータ保存形式
	/// Reader と違ってWriterは最新の書き込み方式だけ知っていればよい
	/// WriteHeader -> WriteFileType -> ... -> WriteEFO
	/// </summary>
	internal sealed class EraBinaryDataWriter : IDisposable
	{
		public EraBinaryDataWriter(FileStream fs)
		{
			#region EM_私家版_セーブ圧縮
			// writer = new BinaryWriter(fs, Encoding.Unicode);
			if (Config.SystemSaveInBinary && Config.ZipSaveData)
			{
				ms = new MemoryStream();
				writer = new BinaryWriter(ms, Encoding.Unicode, true);
				fileWriter = new BinaryWriter(fs, Encoding.Unicode, true);
			}
			else
			{
				writer = new BinaryWriter(fs, Encoding.Unicode);
				fileWriter = writer;
			}
			#endregion
		}
		BinaryWriter writer = null;

		#region EM_私家版_セーブ圧縮
		BinaryWriter fileWriter = null;
		MemoryStream ms = null;
		#endregion

		public void WriteHeader()
		{
			#region EM_私家版_セーブ圧縮
			//writer.Write(EraBDConst.Header);
			//writer.Write(EraBDConst.Version1808);
			//writer.Write(EraBDConst.DataCount);
			//for (int i = 0; i < EraBDConst.DataCount; i++)
			//{
			//	writer.Write((UInt32)0);
			//}
			if (Config.SystemSaveInBinary && Config.ZipSaveData)
				fileWriter.Write(EraBDConst.ZipHeader);
			else
				fileWriter.Write(EraBDConst.Header);
			fileWriter.Write(EraBDConst.Version1808);
			fileWriter.Write(EraBDConst.DataCount);
			for (int i = 0; i < EraBDConst.DataCount; i++)
			{
				fileWriter.Write((UInt32)0);
			}
			#endregion
		}

		public void WriteFileType(EraSaveFileType type)
		{
			writer.Write((byte)type);
		}


		/// <summary>
		/// システム用。keyなしでInt64を保存
		/// </summary>
		/// <param name="v"></param>
		public void WriteInt64(Int64 v)
		{
			//圧縮しない
			writer.Write(v);
		}
		/// <summary>
		/// システム用。keyなしでstringを保存
		/// </summary>
		/// <param name="s"></param>
		public void WriteString(string s)
		{
			writer.Write(s);
		}


		public void WriteSeparator()
		{
			writer.Write((byte)EraSaveDataType.Separator);
		}
		public void WriteEOC()
		{
			writer.Write((byte)EraSaveDataType.EOC);
		}
		public void WriteEOF()
		{
			writer.Write((byte)EraSaveDataType.EOF);
		}

		public void WriteWithKey(string key, object v)
		{
			if (v is Int64)
			{
				writer.Write((byte)EraSaveDataType.Int);
				writer.Write(key);
				writeData((Int64)v);
			}
			else if (v is Int64[])
			{
				writer.Write((byte)EraSaveDataType.IntArray);
				writer.Write(key);
				writeData((Int64[])v);
			}
			else if (v is Int64[,])
			{
				writer.Write((byte)EraSaveDataType.IntArray2D);
				writer.Write(key);
				writeData((Int64[,])v);
			}
			else if (v is Int64[, ,])
			{
				writer.Write((byte)EraSaveDataType.IntArray3D);
				writer.Write(key);
				writeData((Int64[, ,])v);
			}
			else if (v is string)
			{
				writer.Write((byte)EraSaveDataType.Str);
				writer.Write(key);
				writeData((string)v);
			}
			else if (v is string[])
			{
				writer.Write((byte)EraSaveDataType.StrArray);
				writer.Write(key);
				writeData((string[])v);
			}
			else if (v is string[,])
			{
				writer.Write((byte)EraSaveDataType.StrArray2D);
				writer.Write(key);
				writeData((string[,])v);
			}
			else if (v is string[, ,])
			{
				writer.Write((byte)EraSaveDataType.StrArray3D);
				writer.Write(key);
				writeData((string[, ,])v);
			}
			#region EM_私家版_セーブ拡張
			else if (v is Dictionary<string, string> map)
			{
				writer.Write((byte)EraSaveDataType.Map);
				writer.Write(key);
				writer.Write(map.Count);
				foreach (var pair in map)
				{
					writer.Write(pair.Key);
					writer.Write(pair.Value);
				}
			}
			else if (v is XmlDocument doc)
			{
				writer.Write((byte)EraSaveDataType.Xml);
				writer.Write(key);
				writer.Write(doc.OuterXml);
			}
			else if (v is DataTable dt)
			{
				writer.Write((byte)EraSaveDataType.DT);
				writer.Write(key);
				var ss = new StringWriter();
				dt.WriteXmlSchema(ss);
				writer.Write(ss.ToString());

				ss = new StringWriter();
				dt.WriteXml(ss);
				writer.Write(ss.ToString());
			}
			#endregion
		}

		#region private

		private void m_WriteInt(Int64 v)
		{
			//セーブデータ容量の爆発を避けるためにできるだけWrite(Int64)はしない
			if (v >= 0 && v <= Ebdb.Byte)//0～207まではそのままbyteに詰め込む
				writer.Write((byte)v);
			else if (v >= Int16.MinValue && v <= Int16.MaxValue)//整数の範囲に応じて適当に
			{
				writer.Write(Ebdb.Int16);
				writer.Write((Int16)v);
			}
			else if (v >= Int32.MinValue && v <= Int32.MaxValue)
			{
				writer.Write(Ebdb.Int32);
				writer.Write((Int32)v);
			}
			else
			{
				writer.Write(Ebdb.Int64);
				writer.Write(v);
			}
		}

		private void writeData(Int64 v)
		{
			m_WriteInt(v);
		}

		private void writeData(Int64[] array)
		{
			//配列の記憶。0が連続する場合には圧縮を試みる。
			writer.Write((Int32)array.Length);
			int countZero = 0;//0については0が連続する数を記憶する。その他の数はそのまま記憶する。
			for(int x = 0; x < array.Length; x++)
			{
				if (array[x] == 0)
					countZero++;
				else
				{
					if (countZero > 0)
					{
						writer.Write(Ebdb.Zero);
						this.m_WriteInt(countZero);
						countZero = 0;
					}
					this.m_WriteInt(array[x]);
				}
			}
			//記憶途中で配列の残りが全部0であるなら0の数も記憶せず配列の終わりを記憶
			writer.Write(Ebdb.EoD);
		}

		private void writeData(Int64[,] array)
		{
			int countZero = 0;//0については0が連続する数を記憶する。その他はそのまま記憶する。
			int countAllZero = 0;//列の要素が全て0である列の連続する数を記憶する。列の要素に一つでも非0があるなら通常の記憶方式。
			int length0 = array.GetLength(0);
			int length1 = array.GetLength(1);
			writer.Write(length0);
			writer.Write(length1);
			
			for(int x = 0; x < length0; x++)
			{
				for(int y = 0; y < length1; y++)
				{
					if (array[x,y] == 0)
						countZero++;
					else
					{
						if (countAllZero > 0)
						{
							writer.Write(Ebdb.ZeroA1);
							this.m_WriteInt(countAllZero);
							countAllZero = 0;
						}
						if (countZero > 0)
						{
							writer.Write(Ebdb.Zero);
							this.m_WriteInt(countZero);
							countZero = 0;
						}
						this.m_WriteInt(array[x,y]);
					}
				}
				if (countZero == length1)//列の要素が全部0
					countAllZero++;
				else
					writer.Write(Ebdb.EoA1);//非0があるなら列終端記号を記憶
				countZero = 0;
			}
			writer.Write(Ebdb.EoD);
		}

		private void writeData(Int64[, ,] array)
		{
			int countZero = 0;//0については0が連続する数を記憶する。その他はそのまま記憶する。
			int countAllZero = 0;//列の要素が全て0である列の連続する数を記憶する。列の要素に一つでも非0があるなら通常の記憶方式。
			int countAllZero2D = 0;//行列の要素が全て0である行列の･･･
			int length0 = array.GetLength(0);
			int length1 = array.GetLength(1);
			int length2 = array.GetLength(2);
			writer.Write(length0);
			writer.Write(length1);
			writer.Write(length2);
			for(int x = 0; x < length0; x++)
			{
				for(int y = 0; y < length1; y++)
				{
					for(int z = 0; z < length2; z++)
					{
						if (array[x,y,z] == 0)
							countZero++;
						else
						{
							if (countAllZero2D > 0)
							{
								writer.Write(Ebdb.ZeroA2);
								this.m_WriteInt(countAllZero2D);
								countAllZero2D = 0;
							}
							if (countAllZero > 0)
							{
								writer.Write(Ebdb.ZeroA1);
								this.m_WriteInt(countAllZero);
								countAllZero = 0;
							}
							if (countZero > 0)
							{
								writer.Write(Ebdb.Zero);
								this.m_WriteInt(countZero);
								countZero = 0;
							}
							this.m_WriteInt(array[x,y,z]);
						}
					}
					if (countZero == length2)
						countAllZero++;
					else
						writer.Write(Ebdb.EoA1);
					countZero = 0;
				}
				if (countAllZero == length1)
					countAllZero2D++;
				else
					writer.Write(Ebdb.EoA2);
				countAllZero = 0;
			}
			writer.Write(Ebdb.EoD);
		}

		private void writeData(string v)
		{
			if (v != null)
				writer.Write(v);
			else
				writer.Write("");
		}

		private void writeData(string[] array)
		{
			int countZero = 0;
			writer.Write((int)array.Length);
			for(int x = 0; x < array.Length; x++)
			{
				if (array[x] == null || array[x].Length == 0)
					countZero++;
				else
				{
					if (countZero > 0)
					{
						writer.Write(Ebdb.Zero);
						this.m_WriteInt(countZero);
						countZero = 0;
					}
					writer.Write(Ebdb.String);
					writer.Write(array[x]);
				}
			}
			writer.Write(Ebdb.EoD);
		}

		private void writeData(string[,] array)
		{
			int countZero = 0;
			int countAllZero = 0;
			int length0 = array.GetLength(0);
			int length1 = array.GetLength(1);
			writer.Write(length0);
			writer.Write(length1);
			for(int x = 0; x < length0; x++)
			{
				for(int y = 0; y < length1; y++)
				{
					if (array[x,y] == null || array[x,y].Length == 0)
						countZero++;
					else
					{
						if (countAllZero > 0)
						{
							writer.Write(Ebdb.ZeroA1);
							this.m_WriteInt(countAllZero);
							countAllZero = 0;
						}
						if (countZero > 0)
						{
							writer.Write(Ebdb.Zero);
							this.m_WriteInt(countZero);
							countZero = 0;
						}
						writer.Write(Ebdb.String);
						writer.Write(array[x,y]);
					}
				}
				if (countZero == length1)
					countAllZero++;
				else
					writer.Write(Ebdb.EoA1);
				countZero = 0;
			}
			writer.Write(Ebdb.EoD);
		}

		private void writeData(string[, ,] array)
		{
			int countZero = 0;
			int countAllZero = 0;
			int countAllZero2D = 0;
			int length0 = array.GetLength(0);
			int length1 = array.GetLength(1);
			int length2 = array.GetLength(2);
			writer.Write(length0);
			writer.Write(length1);
			writer.Write(length2);
			for(int x = 0; x < length0; x++)
			{
				for(int y = 0; y < length1; y++)
				{
					for(int z = 0; z < length2; z++)
					{
						if (array[x,y,z] == null || array[x,y,z].Length == 0)
							countZero++;
						else
						{
							if (countAllZero2D > 0)
							{
								writer.Write(Ebdb.ZeroA2);
								this.m_WriteInt(countAllZero2D);
								countAllZero2D = 0;
							}
							if (countAllZero > 0)
							{
								writer.Write(Ebdb.ZeroA1);
								this.m_WriteInt(countAllZero);
								countAllZero = 0;
							}
							if (countZero > 0)
							{
								writer.Write(Ebdb.Zero);
								this.m_WriteInt(countZero);
								countZero = 0;
							}
							writer.Write(Ebdb.String);
							writer.Write(array[x,y,z]);
						}
					}
					if (countZero == length2)
						countAllZero++;
					else
						writer.Write(Ebdb.EoA1);
					countZero = 0;
				}
				if (countAllZero == length1)
					countAllZero2D++;
				else
					writer.Write(Ebdb.EoA2);
				countAllZero = 0;
			}
			writer.Write(Ebdb.EoD);
		}
		#endregion
		#region IDisposable メンバ

		public void Dispose()
		{
			#region EM_私家版_セーブ圧縮
			if (Config.SystemSaveInBinary && Config.ZipSaveData && writer != null)
			{
				var st = writer.BaseStream;
				st.Seek(0, SeekOrigin.Begin);
				writer.Close(); writer = null;
				var compressor = new GZipStream(fileWriter.BaseStream, CompressionMode.Compress);
				st.CopyTo(compressor);
				fileWriter.Close();
				compressor.Close();
				st.Close();
			}
			fileWriter = null;
			#endregion
			if (writer != null)
				writer.Close();
			writer = null;
		}

		#endregion
		public void Close()
		{
			Dispose();
		}

	}
}
