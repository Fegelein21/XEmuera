using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MinorShift.Emuera.Sub;
using MinorShift.Emuera.GameView;
using MinorShift.Emuera.GameData.Variable;
using System.Linq;
using EvilMask.Emuera;
using XEmuera;
using trerror = EvilMask.Emuera.Lang.Error;
using trsl = EvilMask.Emuera.Lang.SystemLine;

namespace MinorShift.Emuera.GameData
{
	//難読化用属性。enum.ToString()やenum.Parse()を行うなら(Exclude=true)にすること。
	[global::System.Reflection.Obfuscation(Exclude = false)]
	internal enum CharacterStrData
	{
		NAME = 0,
		CALLNAME = 1,
		NICKNAME = 2,
		MASTERNAME = 3,
		CSTR = 4,
	}
	
	//難読化用属性。enum.ToString()やenum.Parse()を行うなら(Exclude=true)にすること。
	[global::System.Reflection.Obfuscation(Exclude = false)]
	internal enum CharacterIntData
	{
		BASE = 0,
		ABL = 1,
		TALENT = 2,
		MARK = 3,
		EXP = 4,
		RELATION = 5,
		CFLAG = 6,
		EQUIP = 7,
		JUEL = 8,
		
	}
	
	internal sealed class ConstantData
	{

		private const int ablIndex = (int)(VariableCode.ABLNAME & VariableCode.__LOWERCASE__);
		private const int expIndex = (int)(VariableCode.EXPNAME & VariableCode.__LOWERCASE__);
		private const int talentIndex = (int)(VariableCode.TALENTNAME & VariableCode.__LOWERCASE__);
		private const int paramIndex = (int)(VariableCode.PALAMNAME & VariableCode.__LOWERCASE__);
		private const int trainIndex = (int)(VariableCode.TRAINNAME & VariableCode.__LOWERCASE__);
		private const int markIndex = (int)(VariableCode.MARKNAME & VariableCode.__LOWERCASE__);
		private const int itemIndex = (int)(VariableCode.ITEMNAME & VariableCode.__LOWERCASE__);
		private const int baseIndex = (int)(VariableCode.BASENAME & VariableCode.__LOWERCASE__);
		private const int sourceIndex = (int)(VariableCode.SOURCENAME & VariableCode.__LOWERCASE__);
		private const int exIndex = (int)(VariableCode.EXNAME & VariableCode.__LOWERCASE__);
		private const int strIndex = (int)(VariableCode.__DUMMY_STR__ & VariableCode.__LOWERCASE__);
		private const int equipIndex = (int)(VariableCode.EQUIPNAME & VariableCode.__LOWERCASE__);
		private const int tequipIndex = (int)(VariableCode.TEQUIPNAME & VariableCode.__LOWERCASE__);
		private const int flagIndex = (int)(VariableCode.FLAGNAME & VariableCode.__LOWERCASE__);
		private const int tflagIndex = (int)(VariableCode.TFLAGNAME & VariableCode.__LOWERCASE__);
		private const int cflagIndex = (int)(VariableCode.CFLAGNAME & VariableCode.__LOWERCASE__);
		private const int tcvarIndex = (int)(VariableCode.TCVARNAME & VariableCode.__LOWERCASE__);
		private const int cstrIndex = (int)(VariableCode.CSTRNAME & VariableCode.__LOWERCASE__);
		private const int stainIndex = (int)(VariableCode.STAINNAME & VariableCode.__LOWERCASE__);
		private const int cdflag1Index = (int)(VariableCode.CDFLAGNAME1 & VariableCode.__LOWERCASE__);
		private const int cdflag2Index = (int)(VariableCode.CDFLAGNAME2 & VariableCode.__LOWERCASE__);
		private const int strnameIndex = (int)(VariableCode.STRNAME & VariableCode.__LOWERCASE__);
		private const int tstrnameIndex = (int)(VariableCode.TSTRNAME & VariableCode.__LOWERCASE__);
		private const int savestrnameIndex = (int)(VariableCode.SAVESTRNAME & VariableCode.__LOWERCASE__);
		private const int globalIndex = (int)(VariableCode.GLOBALNAME & VariableCode.__LOWERCASE__);
		private const int globalsIndex = (int)(VariableCode.GLOBALSNAME & VariableCode.__LOWERCASE__);
		private const int countNameCsv = (int)VariableCode.__COUNT_CSV_STRING_ARRAY_1D__;

		#region EE_CSV変数拡張
		private const int dayIndex = (int)(VariableCode.DAYNAME & VariableCode.__LOWERCASE__);
		private const int timeIndex = (int)(VariableCode.TIMENAME & VariableCode.__LOWERCASE__);
		private const int moneyIndex = (int)(VariableCode.MONEYNAME & VariableCode.__LOWERCASE__);
		#endregion


		public int[] MaxDataList = new int[countNameCsv];
        readonly HashSet<VariableCode> changedCode = new HashSet<VariableCode>();
		
		public int[] VariableIntArrayLength;
		public int[] VariableStrArrayLength;
		public Int64[] VariableIntArray2DLength;
		public Int64[] VariableStrArray2DLength;
		public Int64[] VariableIntArray3DLength;
		public Int64[] VariableStrArray3DLength;
		public int[] CharacterIntArrayLength;
		public int[] CharacterStrArrayLength;
		public Int64[] CharacterIntArray2DLength;
		public Int64[] CharacterStrArray2DLength;

		#region EM_私家版_セーブ拡張
		public HashSet<string> GlobalSaveMaps { get; private set; } = new HashSet<string>();
		public HashSet<string> SaveMaps { get; private set; } = new HashSet<string>();
		public HashSet<string> GlobalSaveXmls { get; private set; } = new HashSet<string>();
		public HashSet<string> SaveXmls { get; private set; } = new HashSet<string>();
		public HashSet<string> GlobalSaveDTs { get; private set; } = new HashSet<string>();
		public HashSet<string> SaveDTs { get; private set; } = new HashSet<string>();

		public HashSet<string> StaticMaps { get; private set; } = new HashSet<string>();
		public HashSet<string> StaticXmls { get; private set; } = new HashSet<string>();
		public HashSet<string> StaticDTs { get; private set; } = new HashSet<string>();
		#endregion

		//private readonly GameBase gamebase;
		//private readonly string[][] names = new string[(int)VariableCode.__COUNT_CSV_STRING_ARRAY_1D__][];
		#region EE_ERD
		private const int ERD_NAMES_INDEX = (int)VariableCode.__COUNT_CSV_STRING_ARRAY_1D__; 
		private readonly string[][] names = new string[(int)VariableCode.__COUNT_CSV_STRING_ARRAY_1D__ + 1][]; // 元より1増やす
		private readonly Dictionary<string, Dictionary<string, int>> erdNameToIntDics = new Dictionary<string, Dictionary<string, int>>();
		#endregion
		private readonly Dictionary<string, int>[] nameToIntDics = new Dictionary<string, int>[(int)VariableCode.__COUNT_CSV_STRING_ARRAY_1D__];
		private readonly Dictionary<string, int> relationDic = new Dictionary<string, int>();
		public string[] GetCsvNameList(VariableCode code)
		{
			return names[(int)(code & VariableCode.__LOWERCASE__)];
		}

		public Int64[] ItemPrice;
		
		private readonly List<CharacterTemplate> CharacterTmplList;
		private EmueraConsole output;
		
		public ConstantData()
		{
			//this.gamebase = gamebase;
			setDefaultArrayLength();

			CharacterTmplList = new List<CharacterTemplate>();
			useCompatiName = Config.CompatiCALLNAME;
		}

		readonly bool useCompatiName;

		private void setDefaultArrayLength()
		{
			MaxDataList[ablIndex] = 100;
			MaxDataList[talentIndex] = 1000;
			MaxDataList[expIndex] = 100;
			MaxDataList[markIndex] = 100;
			MaxDataList[trainIndex] = 1000;
			MaxDataList[paramIndex] = 200;
			MaxDataList[itemIndex] = 1000;
			MaxDataList[baseIndex] = 100;
			MaxDataList[sourceIndex] = 1000;
			MaxDataList[exIndex] = 100;
			MaxDataList[equipIndex] = 100;
			MaxDataList[tequipIndex] = 100;
			MaxDataList[flagIndex] = 10000;
			MaxDataList[tflagIndex] = 1000;
			MaxDataList[cflagIndex] = 1000;
			MaxDataList[tcvarIndex] = 100;
			MaxDataList[cstrIndex] = 100;
			MaxDataList[stainIndex] = 1000;
			MaxDataList[strIndex] = 20000;
			MaxDataList[cdflag1Index] = 1;
			MaxDataList[cdflag2Index] = 1;
			MaxDataList[strnameIndex] = 20000;
			MaxDataList[tstrnameIndex] = 100;
			MaxDataList[savestrnameIndex] = 100;
			MaxDataList[globalIndex] = 1000;
			MaxDataList[globalsIndex] = 100;

			#region EE_CSV拡張機能
			MaxDataList[dayIndex] = 100;
			MaxDataList[timeIndex] = 100;
			MaxDataList[moneyIndex] = 100;
			#endregion

			VariableIntArrayLength = new int[(int)VariableCode.__COUNT_INTEGER_ARRAY__];
			VariableStrArrayLength = new int[(int)VariableCode.__COUNT_STRING_ARRAY__];
			VariableIntArray2DLength = new Int64[(int)VariableCode.__COUNT_INTEGER_ARRAY_2D__];
			VariableStrArray2DLength = new Int64[(int)VariableCode.__COUNT_STRING_ARRAY_2D__];
			VariableIntArray3DLength = new Int64[(int)VariableCode.__COUNT_INTEGER_ARRAY_3D__];
			VariableStrArray3DLength = new Int64[(int)VariableCode.__COUNT_STRING_ARRAY_3D__];
			CharacterIntArrayLength = new int[(int)VariableCode.__COUNT_CHARACTER_INTEGER_ARRAY__];
			CharacterStrArrayLength = new int[(int)VariableCode.__COUNT_CHARACTER_STRING_ARRAY__];
			CharacterIntArray2DLength = new Int64[(int)VariableCode.__COUNT_CHARACTER_INTEGER_ARRAY_2D__];
			CharacterStrArray2DLength = new Int64[(int)VariableCode.__COUNT_CHARACTER_STRING_ARRAY_2D__];
			for (int i = 0; i < VariableIntArrayLength.Length; i++)
				VariableIntArrayLength[i] = 1000;
			VariableIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.FLAG)] = 10000;
			VariableIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.ITEMPRICE)] = MaxDataList[itemIndex];

			VariableIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.RANDDATA)] = 625;

			for (int i = 0; i < VariableStrArrayLength.Length; i++)
				VariableStrArrayLength[i] = 100;
			VariableStrArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.STR)] = MaxDataList[strIndex];

			for (int i = 0; i < VariableIntArray2DLength.Length; i++)
				VariableIntArray2DLength[i] = (100L << 32) + 100L;
			for (int i = 0; i < VariableStrArray2DLength.Length; i++)
				VariableStrArray2DLength[i] = (100L << 32) + 100L;

			for (int i = 0; i < VariableIntArray3DLength.Length; i++)
				VariableIntArray3DLength[i] = (100L << 40) + (100L << 20) + 100L;
			for (int i = 0; i < VariableStrArray3DLength.Length; i++)
				VariableStrArray3DLength[i] = (100L << 40) + (100L << 20) + 100L;

			for (int i = 0; i < CharacterIntArrayLength.Length; i++)
				CharacterIntArrayLength[i] = 100;
			CharacterIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.TALENT)] = 1000;
			CharacterIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.CFLAG)] = 1000;
			CharacterIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.JUEL)] = 200;
			CharacterIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.GOTJUEL)] = 200;

			for (int i = 0; i < CharacterStrArrayLength.Length; i++)
				CharacterStrArrayLength[i] = 100;

			for (int i = 0; i < CharacterIntArray2DLength.Length; i++)
				CharacterIntArray2DLength[i] = (1L << 32) + 1L;
			for (int i = 0; i < CharacterStrArray2DLength.Length; i++)
				CharacterStrArray2DLength[i] = (1L << 32) + 1L;
		}

		private void loadVariableSizeData(string csvPath, bool disp)
		{
			if (!FileUtils.Exists(ref csvPath))
				return;
			EraStreamReader eReader = new EraStreamReader(false);
			if (!eReader.Open(csvPath))
			{
				output.PrintError(string.Format(trerror.FailedOpenFile.Text, eReader.Filename));
				return;
			}
			ScriptPosition position = null;
			if (disp)
				output.PrintSystemLine(string.Format(trsl.LoadingFile.Text, eReader.Filename));
			try
			{
				StringStream st = null;
				while ((st = eReader.ReadEnabledLine()) != null)
				{
					position = new ScriptPosition(eReader.Filename, eReader.LineNo);
					changeVariableSizeData(st.Substring(), position);
				}
				position = new ScriptPosition(eReader.Filename, -1);
			}
			catch
			{
				//System.Media.SystemSounds.Hand.Play();
				if (position != null)
					ParserMediator.Warn(trerror.UnexpectedError.Text, position, 3);
				else
					output.PrintError(trerror.UnexpectedError.Text);
				return;
			}
			finally
			{
				eReader.Close();
			}
			decideActualArraySize(position);
		}


		private void changeVariableSizeData(string line, ScriptPosition position)
		{
			string[] tokens = line.Split(',');
			if (tokens.Length < 2)
			{
				ParserMediator.Warn(trerror.MissingComma.Text, position, 1);
				return;
			}
			string idtoken = tokens[0].Trim();
			VariableIdentifier id = VariableIdentifier.GetVariableId(idtoken);
			if (id == null)
			{
				ParserMediator.Warn(string.Format(trerror.CanNotInterpretVarName.Text, "1"), position, 1);
				return;
			}
			if ((!id.IsArray1D) && (!id.IsArray2D) && (!id.IsArray3D))
			{
				ParserMediator.Warn(string.Format(trerror.CanNotChange0DVarSize.Text, id.ToString()), position, 1);
				return;
			}
			if ((id.IsCalc) || (id.Code == VariableCode.RANDDATA))
			{
				ParserMediator.Warn(string.Format(trerror.CanNotChangeVarSize.Text, id.ToString()), position, 1);
				return;
			}
            int length2 = 0;
            int length3 = 0;
			if (!int.TryParse(tokens[1], out int length))
			{
				ParserMediator.Warn(string.Format(trerror.CanNotInterpretVarName.Text, "2"), position, 1);
				return;
			}
            //1820a16 変数禁止指定 負の値を指定する
			if (length <= 0)
			{
				if (length == 0)
				{
					ParserMediator.Warn(trerror.ArrayLengthIs0.Text, position, 2);
					return;
				}
				if(!id.CanForbid)
				{
					ParserMediator.Warn(trerror.CanNotDisableVarArrayLengthIsNegative.Text, position, 2);
					return;
				}
                if (tokens.Length > 2 && tokens[2].Length > 0 && tokens[2].Trim().Length > 0 && char.IsDigit((tokens[2].Trim())[0]))
                {
                    ParserMediator.Warn(string.Format(trerror.IgnoreNDData.Text, "1"), position, 0);
                }
				length = 0;
				goto check1break;
			}
			if (id.IsArray1D)
			{
                if (tokens.Length > 2 && tokens[2].Length > 0 && tokens[2].Trim().Length > 0 && char.IsDigit((tokens[2].Trim())[0]))
                {
					ParserMediator.Warn(string.Format(trerror.IgnoreNDData.Text, "1"), position, 0);
				}
				if (id.IsLocal && length < 1)
				{
					ParserMediator.Warn(trerror.LocalVarSizeCanNotLessThan1.Text, position, 1);
					return;
				}
				if (!id.IsLocal && length < 100)
				{
					ParserMediator.Warn(trerror.InternalVarSizeCanNotLessThan100.Text, position, 1);
					return;
				}
				if (length > 1000000)
				{
					ParserMediator.Warn(trerror.OneDVarSizeCanNotGreaterThan1M.Text, position, 1);
					return;
				}
			}
			else if (id.IsArray2D)
			{
				if (tokens.Length < 3)
				{
					ParserMediator.Warn(string.Format(trerror.MissingVarSizeArg.Text, "2"), position, 1);
					return;
				}
                if (tokens.Length > 3 && tokens[3].Length > 0 && tokens[3].Trim().Length > 0 && char.IsDigit((tokens[3].Trim())[0]))
                {
					ParserMediator.Warn(string.Format(trerror.IgnoreNDData.Text, "2"), position, 0);
				}
				if (!int.TryParse(tokens[2], out length2))
				{
					ParserMediator.Warn(string.Format(trerror.CanNotInterpretVarName.Text, "3"), position, 1);
					return;
				}
				if ((length < 1) || (length2 < 1))
				{
					ParserMediator.Warn(trerror.VarSizeCanNotLessThan1.Text, position, 1);
					return;
				}
				if ((length > 1000000) || (length2 > 1000000))
				{
					ParserMediator.Warn(trerror.VarSizeCanNotGreaterThan1M.Text, position, 1);
					return;
				}
				if (length * length2 > 1000000)
				{
					ParserMediator.Warn(string.Format(trerror.VarSizeLimitIs1M.Text, "2"), position, 1);
					return;
				}
			}
			else if (id.IsArray3D)
			{
				if (tokens.Length < 4)
				{
					ParserMediator.Warn(string.Format(trerror.MissingVarSizeArg.Text, "3"), position, 1);
					return;
				}
                if (tokens.Length > 4 && tokens[4].Length > 0 && tokens[4].Trim().Length > 0 && char.IsDigit((tokens[4].Trim())[0]))
                {
					ParserMediator.Warn(string.Format(trerror.IgnoreNDData.Text, "3"), position, 0);
				}
				if (!int.TryParse(tokens[2], out length2))
				{
					ParserMediator.Warn(string.Format(trerror.CanNotInterpretVarName.Text, "3"), position, 1);
					return;
				}
				if (!int.TryParse(tokens[3], out length3))
				{
					ParserMediator.Warn(string.Format(trerror.CanNotInterpretVarName.Text, "4"), position, 1);
					return;
				}
				if ((length < 1) || (length2 < 1) || (length3 < 1))
				{
					ParserMediator.Warn(trerror.VarSizeCanNotLessThan1.Text, position, 1);
					return;
				}
				//1802 サイズ保存の都合上、2^20超えるとバグる
				if ((length > 1000000) || (length2 > 1000000) || (length3 > 1000000))
				{
					ParserMediator.Warn(trerror.VarSizeCanNotGreaterThan1M.Text, position, 1);
					return;
				}
				if (length * length2 * length3 > 10000000)
				{
					ParserMediator.Warn(string.Format(trerror.VarSizeLimitIs1M.Text, "3"), position, 1);
					return;
				}
			}
			check1break:
			switch (id.Code)
			{
				//1753a PALAMだけ仕様が違うのはかえって問題なので、変数と要素文字列配列数の同期は全部バックアウト
				//基本的には旧来の処理に戻しただけ
				case VariableCode.ITEMNAME:
				case VariableCode.ITEMPRICE:
					VariableIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.ITEMPRICE)] = length;
					MaxDataList[itemIndex] = length;
					break;
				case VariableCode.STR:
					VariableStrArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.STR)] = length;
					MaxDataList[strIndex] = length;
					break;
				case VariableCode.ABLNAME:
				case VariableCode.TALENTNAME:
				case VariableCode.EXPNAME:
				case VariableCode.MARKNAME:
				case VariableCode.PALAMNAME:
				case VariableCode.TRAINNAME:
				case VariableCode.BASENAME:
				case VariableCode.SOURCENAME:
				case VariableCode.EXNAME:
				case VariableCode.EQUIPNAME:
				case VariableCode.TEQUIPNAME:
				case VariableCode.FLAGNAME:
				case VariableCode.TFLAGNAME:
				case VariableCode.CFLAGNAME:
				case VariableCode.TCVARNAME:
				case VariableCode.CSTRNAME:
				case VariableCode.STAINNAME:
				case VariableCode.CDFLAGNAME1:
				case VariableCode.CDFLAGNAME2:
				case VariableCode.TSTRNAME:
				case VariableCode.SAVESTRNAME:
				case VariableCode.STRNAME:
				case VariableCode.GLOBALNAME:
				case VariableCode.GLOBALSNAME:
				#region EE_CSV機能拡張
				case VariableCode.DAYNAME:
				case VariableCode.TIMENAME:
				case VariableCode.MONEYNAME:
				#endregion
					MaxDataList[(int)(id.Code & VariableCode.__LOWERCASE__)] = length;
					break;
				default:
					{
						if (id.IsCharacterData)
						{
							if (id.IsArray2D)
							{
								Int64 length64 = (((Int64)length) << 32) + ((Int64)length2);
								if (id.IsInteger)
									CharacterIntArray2DLength[id.CodeInt] = length64;
								else if (id.IsString)
									CharacterStrArray2DLength[id.CodeInt] = length64;
							}
							else
							{
								if (id.IsInteger)
									CharacterIntArrayLength[id.CodeInt] = length;
								else if (id.IsString)
									CharacterStrArrayLength[id.CodeInt] = length;
							}
						}
						else if (id.IsArray2D)
						{
							Int64 length64 = (((Int64)length) << 32) + ((Int64)length2);
							if (id.IsInteger)
								VariableIntArray2DLength[id.CodeInt] = length64;
							else if (id.IsString)
								VariableStrArray2DLength[id.CodeInt] = length64;
						}
						else if (id.IsArray3D)
						{
							//Int64 length3d = ((Int64)length << 32) + ((Int64)length2 << 16) + (Int64)length3;
							Int64 length3d = ((Int64)length << 40) + ((Int64)length2 << 20) + (Int64)length3;
							if (id.IsInteger)
								VariableIntArray3DLength[id.CodeInt] = length3d;
							else
								VariableStrArray3DLength[id.CodeInt] = length3d;
						}
						else
						{
							if (id.IsInteger)
								VariableIntArrayLength[id.CodeInt] = length;
							else if (id.IsString)
								VariableStrArrayLength[id.CodeInt] = length;
						}
					}
					break;
			}
			//1803beta004 二重定義を警告対象に
			if (!changedCode.Add(id.Code))
				ParserMediator.Warn(string.Format(trerror.VarSizeAlreadyDefined.Text, id.Code.ToString()), position, 1);
		}

		private void _decideActualArraySize_sub(VariableCode mainCode, VariableCode nameCode, int[] arraylength, ScriptPosition position)
		{
			int nameIndex = (int)(nameCode & VariableCode.__LOWERCASE__);
			int mainLengthIndex = (int)(mainCode & VariableCode.__LOWERCASE__);
			if (changedCode.Contains(nameCode) && changedCode.Contains(mainCode))
			{
				if (MaxDataList[nameIndex] != arraylength[mainLengthIndex])
				{
					int i = Math.Max(MaxDataList[nameIndex], arraylength[mainLengthIndex]);
					arraylength[mainLengthIndex] = i;
					MaxDataList[nameIndex] = i;
					//1803beta004 不適切な指定として警告Lv1の対象にする
					if (MaxDataList[nameIndex] == 0 || arraylength[mainLengthIndex] == 0)
						ParserMediator.Warn(string.Format(trerror.DifferentVarProhibitSetting.Text, mainCode.ToString(), nameCode.ToString()), position, 1);
					else
						ParserMediator.Warn(string.Format(trerror.DifferentVarSize.Text, mainCode.ToString(), nameCode.ToString()), position, 1);
				}
			}
			else if (changedCode.Contains(nameCode) && !changedCode.Contains(mainCode))
				arraylength[mainLengthIndex] = MaxDataList[nameIndex];
			else if (!changedCode.Contains(nameCode) && changedCode.Contains(mainCode))
				MaxDataList[nameIndex] = arraylength[mainLengthIndex];
		}
		
		private void decideActualArraySize(ScriptPosition position)
		{
			_decideActualArraySize_sub(VariableCode.ABL, VariableCode.ABLNAME, CharacterIntArrayLength, position);
			_decideActualArraySize_sub(VariableCode.TALENT, VariableCode.TALENTNAME, CharacterIntArrayLength, position);
			_decideActualArraySize_sub(VariableCode.EXP, VariableCode.EXPNAME, CharacterIntArrayLength, position);
			_decideActualArraySize_sub(VariableCode.MARK, VariableCode.MARKNAME, CharacterIntArrayLength, position);
			_decideActualArraySize_sub(VariableCode.BASE, VariableCode.BASENAME, CharacterIntArrayLength, position);
            _decideActualArraySize_sub(VariableCode.SOURCE, VariableCode.SOURCENAME, CharacterIntArrayLength, position);
			_decideActualArraySize_sub(VariableCode.EX, VariableCode.EXNAME, CharacterIntArrayLength, position);
			_decideActualArraySize_sub(VariableCode.EQUIP, VariableCode.EQUIPNAME, CharacterIntArrayLength, position);
			_decideActualArraySize_sub(VariableCode.TEQUIP, VariableCode.TEQUIPNAME, CharacterIntArrayLength, position);
			_decideActualArraySize_sub(VariableCode.FLAG, VariableCode.FLAGNAME, VariableIntArrayLength, position);
			_decideActualArraySize_sub(VariableCode.TFLAG, VariableCode.TFLAGNAME, VariableIntArrayLength, position);
			_decideActualArraySize_sub(VariableCode.CFLAG, VariableCode.CFLAGNAME, CharacterIntArrayLength, position);
			_decideActualArraySize_sub(VariableCode.TCVAR, VariableCode.TCVARNAME, CharacterIntArrayLength, position);
			_decideActualArraySize_sub(VariableCode.CSTR, VariableCode.CSTRNAME, CharacterStrArrayLength, position);
			_decideActualArraySize_sub(VariableCode.STAIN, VariableCode.STAINNAME, CharacterIntArrayLength, position);
			_decideActualArraySize_sub(VariableCode.STR, VariableCode.STRNAME, VariableStrArrayLength, position);
			_decideActualArraySize_sub(VariableCode.TSTR, VariableCode.TSTRNAME, VariableStrArrayLength, position);
			_decideActualArraySize_sub(VariableCode.SAVESTR, VariableCode.SAVESTRNAME, VariableStrArrayLength, position);
			_decideActualArraySize_sub(VariableCode.GLOBAL, VariableCode.GLOBALNAME, VariableIntArrayLength, position);
			_decideActualArraySize_sub(VariableCode.GLOBALS, VariableCode.GLOBALSNAME, VariableStrArrayLength, position);
			#region EE_CSV機能拡張
			_decideActualArraySize_sub(VariableCode.DAY, VariableCode.DAYNAME, VariableIntArrayLength, position);
			_decideActualArraySize_sub(VariableCode.TIME, VariableCode.TIMENAME, VariableIntArrayLength, position);
			_decideActualArraySize_sub(VariableCode.MONEY, VariableCode.MONEYNAME, VariableIntArrayLength, position);
            #endregion


            //PALAM(JUEL込み)
            //PALAMかJUELが変わっているときは大きい方をとる
            if (changedCode.Contains(VariableCode.PALAM) || changedCode.Contains(VariableCode.JUEL))
			{
				int palamJuelMax = Math.Max(CharacterIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.PALAM)]
						, CharacterIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.JUEL)]);
				//PALAMNAMEが変わっているなら、それと比較して大きい方を採用
				if(changedCode.Contains(VariableCode.PALAMNAME))
				{
					if (MaxDataList[paramIndex] != palamJuelMax)
					{
						int i = Math.Max(MaxDataList[paramIndex], palamJuelMax);
						MaxDataList[paramIndex] = i;
						if(CharacterIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.PALAM)] == palamJuelMax)
							CharacterIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.PALAM)] = i;
						if(CharacterIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.JUEL)] == palamJuelMax)
							CharacterIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.JUEL)] = i;
						//1803beta004 不適切な指定として警告Lv1の対象にする
						ParserMediator.Warn(trerror.InappropriatePalamJuelPalamname.Text, position, 1);
					}
				}
				else//PALAMNAMEの指定がないなら大きい方にPALAMNAMEをあわせる
					MaxDataList[paramIndex] = palamJuelMax;
			}
			//PALAMとJUEL不変でPALAMNAMEが変わっている場合
			else if (changedCode.Contains(VariableCode.PALAMNAME))
			{
				//PALAMを指定のPALAMNAMEにあわせる
				CharacterIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.PALAM)] = MaxDataList[paramIndex];
				//指定のPALAMNAMEがJUELより小さければ警告出してJUELにあわせる
				if (MaxDataList[paramIndex] < CharacterIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.JUEL)])
				{
					ParserMediator.Warn(trerror.PalamnameSizeLessThanJuelSize.Text, position, 1);
					MaxDataList[paramIndex] = CharacterIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.JUEL)];
				}
			}
			//CDFLAG
			//一部変更されたら双方変更されたと扱う
			bool cdflagNameLengthChanged = changedCode.Contains(VariableCode.CDFLAGNAME1) || changedCode.Contains(VariableCode.CDFLAGNAME2);
			int mainLengthIndex = (int)(VariableCode.__LOWERCASE__ & VariableCode.CDFLAG);
			Int64 length64 = CharacterIntArray2DLength[mainLengthIndex];
			int length1 = (int)(length64 >> 32);
			int length2 = (int)(length64 & 0x7FFFFFFF);
			if (changedCode.Contains(VariableCode.CDFLAG) && cdflagNameLengthChanged)
			{
				//調整が面倒なので投げる
				if ((length1 != MaxDataList[cdflag1Index]) || (length2 != MaxDataList[cdflag2Index]))
					throw new CodeEE(Lang.Error.DoesNotMatchCdflagElements.Text, position);
			}
			else if (cdflagNameLengthChanged && !changedCode.Contains(VariableCode.CDFLAG))
			{
				length1 = MaxDataList[cdflag1Index];
				length2 = MaxDataList[cdflag2Index];
				if (length1 * length2 > 1000000)
				{
					//調整が面倒なので投げる
					throw new CodeEE(Lang.Error.TooManyCdflagElements.Text, position);
				}
				CharacterIntArray2DLength[mainLengthIndex] = (((Int64)length1) << 32) + ((Int64)length2);
			}
			else if (!cdflagNameLengthChanged && changedCode.Contains(VariableCode.CDFLAG))
			{
				MaxDataList[cdflag1Index] = length1;
				MaxDataList[cdflag2Index] = length2;
			}
			//もう使わないのでデータ破棄
			changedCode.Clear();
		}


		public void LoadData(string csvDir, EmueraConsole console, bool disp)
		{
			output = console;
			loadVariableSizeData(csvDir + "VariableSize.CSV", disp);
			for(int i = 0; i< countNameCsv;i++)
			{
				names[i] = new string[MaxDataList[i]];
				nameToIntDics[i] = new Dictionary<string, int>();
			}
			ItemPrice = new Int64[MaxDataList[itemIndex]];
			#region EE_ERD
			loadDataTo(csvDir + "ABL.CSV", ablIndex, null, disp);
			loadDataTo(csvDir + "EXP.CSV", expIndex, null, disp);
			loadDataTo(csvDir + "TALENT.CSV", talentIndex, null, disp);
			loadDataTo(csvDir + "PALAM.CSV", paramIndex, null,  disp);
			loadDataTo(csvDir + "TRAIN.CSV", trainIndex, null,  disp);
			loadDataTo(csvDir + "MARK.CSV", markIndex, null,  disp);
			loadDataTo(csvDir + "ITEM.CSV", itemIndex, ItemPrice,  disp);
			loadDataTo(csvDir + "BASE.CSV", baseIndex, null,  disp);
			loadDataTo(csvDir + "SOURCE.CSV", sourceIndex, null,  disp);
			loadDataTo(csvDir + "EX.CSV", exIndex, null,  disp);
			loadDataTo(csvDir + "STR.CSV", strIndex, null,  disp);
			loadDataTo(csvDir + "EQUIP.CSV", equipIndex, null,  disp);
			loadDataTo(csvDir + "TEQUIP.CSV", tequipIndex, null,  disp);
			loadDataTo(csvDir + "FLAG.CSV", flagIndex, null,  disp);
			loadDataTo(csvDir + "TFLAG.CSV", tflagIndex, null,  disp);
			loadDataTo(csvDir + "CFLAG.CSV", cflagIndex, null,  disp);
			loadDataTo(csvDir + "TCVAR.CSV", tcvarIndex, null,  disp);
			loadDataTo(csvDir + "CSTR.CSV", cstrIndex, null,  disp);
			loadDataTo(csvDir + "STAIN.CSV", stainIndex, null,  disp);
			loadDataTo(csvDir + "CDFLAG1.CSV", cdflag1Index, null,  disp);
			loadDataTo(csvDir + "CDFLAG2.CSV", cdflag2Index, null,  disp);
			
			loadDataTo(csvDir + "STRNAME.CSV", strnameIndex, null,  disp);
			loadDataTo(csvDir + "TSTR.CSV", tstrnameIndex, null,  disp);
			loadDataTo(csvDir + "SAVESTR.CSV", savestrnameIndex, null,  disp);
			loadDataTo(csvDir + "GLOBAL.CSV", globalIndex, null,  disp);
			loadDataTo(csvDir + "GLOBALS.CSV", globalsIndex, null,  disp);
			#endregion
			#region EE_CSV機能拡張
			loadDataTo(csvDir + "DAY.CSV", dayIndex, null,  disp);
			loadDataTo(csvDir + "TIME.CSV", timeIndex, null,  disp);
			loadDataTo(csvDir + "MONEY.CSV", moneyIndex, null,  disp);
			#endregion
			//逆引き辞書を作成
			for (int i = 0; i < names.Length; i++)
			{
				if (i == 10)//Strは逆引き無用
					continue;
				#region EE_ERD
				if (names[i] == null)
					continue;
				#endregion
				string[] nameArray = names[i];
				for (int j = 0; j < nameArray.Length; j++)
				{
					if (!string.IsNullOrEmpty(nameArray[j]) && !nameToIntDics[i].ContainsKey(nameArray[j]))
						nameToIntDics[i].Add(nameArray[j], j);
				}
			}
			//if (!Program.AnalysisMode)
			loadCharacterData(csvDir, disp);
			
			#region EM_私家版_セーブ拡張
			loadGlobalVarExSetting(csvDir, disp);
			#endregion
			//逆引き辞書を作成2 (RELATION)
			for (int i = 0; i < CharacterTmplList.Count; i++)
			{
				CharacterTemplate tmpl = CharacterTmplList[i];
				if (!string.IsNullOrEmpty(tmpl.Name) && !relationDic.ContainsKey(tmpl.Name))
					relationDic.Add(tmpl.Name, (int)tmpl.No);
				if (!string.IsNullOrEmpty(tmpl.Callname) && !relationDic.ContainsKey(tmpl.Callname))
                    relationDic.Add(tmpl.Callname, (int)tmpl.No);
				if (!string.IsNullOrEmpty(tmpl.Nickname) && !relationDic.ContainsKey(tmpl.Nickname))
                    relationDic.Add(tmpl.Nickname, (int)tmpl.No);
			}
		}
		#region EE_ERD
		private struct ErdDictInfo 
		{
			public int num;
			public string path;
		};
		public void UserDefineLoadData(List<string> filepaths, string varname, int varlength, bool disp, ScriptPosition sc)
		{

			Dictionary<string, ErdDictInfo> preDict = new Dictionary<string, ErdDictInfo>();

			foreach (var filepath in filepaths)
			{
				string[] nameArray = new string[varlength];
				names[ERD_NAMES_INDEX] = nameArray;
				loadDataTo(filepath, ERD_NAMES_INDEX, null, disp);
				names[ERD_NAMES_INDEX] = null;
				//逆引き辞書を作成
				for (int j = 0; j < nameArray.Length; j++)
				{
					if (!string.IsNullOrEmpty(nameArray[j]))
					{
						if (!preDict.ContainsKey(nameArray[j]))
						{ 
							preDict[nameArray[j]] = new ErdDictInfo() { num = j, path = filepath };
						}
						else
						{
							throw new CodeEE(string.Format(Lang.Error.DuplicateErdKey.Text,
								varname, nameArray[j], preDict[nameArray[j]].path, filepath));
						}
					}
				}
			}
			// ここで発生しないと思うが一応書いておく
			if (erdNameToIntDics.ContainsKey(varname))
				throw new CodeEE(string.Format(Lang.Error.DuplicateVariableDefine.Text, varname), sc);

			var dict = new Dictionary<string, int>();
			foreach(var pair in preDict)
				dict.Add(pair.Key, pair.Value.num);
			erdNameToIntDics.Add(varname, dict);
		}

		public bool isDefined(VariableCode varCode, string str)
		{
			if (string.IsNullOrEmpty(str))
				return false;
            Dictionary<string, int> dic;
            if (varCode == VariableCode.CDFLAG)
            {
                // dic = GetKeywordDictionary(out _, VariableCode.CDFLAGNAME1, -1);
				dic = GetKeywordDictionary(out _, VariableCode.CDFLAGNAME1, -1, null);
				if ((dic == null) || (!dic.ContainsKey(str)))
                   // dic = GetKeywordDictionary(out _, VariableCode.CDFLAGNAME2, -1);
					dic = GetKeywordDictionary(out _, VariableCode.CDFLAGNAME2, -1, null);
				if (dic == null)
                    return false;
                return dic.ContainsKey(str);
            }
            // dic = GetKeywordDictionary(out _, varCode, -1);
			dic = GetKeywordDictionary(out _, varCode, -1, null);
			if (dic == null)
				return false;
			return dic.ContainsKey(str);
		}

		public bool isUserDefined(string varname, string str, int dim)
		{
			if (string.IsNullOrEmpty(str))
				return false;
			if (dim == 1 && (!erdNameToIntDics.ContainsKey(varname) || !erdNameToIntDics[varname].ContainsKey(str)))
				throw new CodeEE(string.Format(Lang.Error.NotDefinedErdKey.Text, varname, str));
			//CDFLAGの判定も割とガバガバなのでこれで良い（暴論）
			if (dim == 2)
            {
				if (!erdNameToIntDics.ContainsKey(varname + "@1") || !erdNameToIntDics[varname + "@1"].ContainsKey(str))
                {
					if (!erdNameToIntDics.ContainsKey(varname + "@2") || !erdNameToIntDics[varname + "@2"].ContainsKey(str))
						throw new CodeEE(string.Format(Lang.Error.NotDefinedErdKey.Text, varname, str));
				}
			}
			if (dim == 3)
			{
				if (!erdNameToIntDics.ContainsKey(varname + "@1") || !erdNameToIntDics[varname + "@1"].ContainsKey(str))
				{
					if (!erdNameToIntDics.ContainsKey(varname + "@2") || !erdNameToIntDics[varname + "@2"].ContainsKey(str))
                    {
						if (!erdNameToIntDics.ContainsKey(varname + "@3") || !erdNameToIntDics[varname + "@3"].ContainsKey(str))
							throw new CodeEE(string.Format(Lang.Error.NotDefinedErdKey.Text, varname, str));
					}
				}
			}
			return true;
		}
		#endregion


		#region EE_ERD
		// public bool TryKeywordToInteger(out int ret, VariableCode code, string key, int index)
		public bool TryKeywordToInteger(out int ret, VariableCode code, string key, int index, string varname)
		{
			ret = 0;
			if (string.IsNullOrEmpty(key))
				return false;
			Dictionary<string, int> dic;
			try
			{
				// dic = GetKeywordDictionary(out string errPos, code, index);
				dic = GetKeywordDictionary(out string errPos, code, index, null);

				//ここで見つからなかったら下の処理でも通す
				if (dic.TryGetValue(key, out ret))
					return (dic.TryGetValue(key, out ret));
			}
			catch { }
			if (!string.IsNullOrEmpty(varname))
			{
				if (!erdNameToIntDics.ContainsKey(varname))
					return false;
				return erdNameToIntDics[varname].TryGetValue(key, out ret);
			}
			return false;
		}
		#endregion
		#region EE_ERDNAME
		public bool TryIntegerToKeyword(out string ret, long value, string varname)
		{
			ret = "";
			if (value < 0)
				return false;
			Dictionary<string, int> dic;
			if (!string.IsNullOrEmpty(varname))
			{
				if (!erdNameToIntDics.ContainsKey(varname))
					return false;
				dic = erdNameToIntDics[varname];
                try
                {
					ret = dic.First(x => x.Value == value).Key;
				}
                catch
                {
					return false;
                }
				if (!string.IsNullOrEmpty(ret))
					return true;
			}
			return false;
		}
		#endregion

		public int KeywordToInteger(VariableCode code, string key, int index)
		{
			if (string.IsNullOrEmpty(key))
				throw new CodeEE(Lang.Error.KeywordsCannotBeEmpty.Text);
			#region EE_ERD
			// Dictionary<string, int> dic = GetKeywordDictionary(out string errPos, code, index);
			Dictionary<string, int> dic = GetKeywordDictionary(out string errPos, code, index, null);
			#endregion
			if (dic.TryGetValue(key, out int ret))
                return ret;
            if (errPos == null)
				throw new CodeEE(string.Format(Lang.Error.CanNotSpecifiedByString.Text, code.ToString()));
			else
				throw new CodeEE(string.Format(Lang.Error.NotDefinedKey.Text, errPos, key));
		}

		#region EE_ERD
		//public Dictionary<string, int> GetKeywordDictionary(out string errPos, VariableCode code, int index)
		public Dictionary<string, int> GetKeywordDictionary(out string errPos, VariableCode code, int index, string varname)
		#endregion
		{
			errPos = null;
			int allowIndex = -1;
			Dictionary<string, int> ret = null;
			switch (code)
			{
				case VariableCode.ABL:
					ret = nameToIntDics[ablIndex];//AblName;
					errPos = "abl.csv";
					allowIndex = 1;
					break;
				case VariableCode.EXP:
					ret = nameToIntDics[expIndex];//ExpName;
					errPos = "exp.csv";
					allowIndex = 1;
					break;
				case VariableCode.TALENT:
					ret = nameToIntDics[talentIndex];//TalentName;
					errPos = "talent.csv";
					allowIndex = 1;
					break;
				case VariableCode.UP:
				case VariableCode.DOWN:
					ret = nameToIntDics[paramIndex];//ParamName　１;
					errPos = "palam.csv";
					allowIndex = 0;
					break;
				case VariableCode.PALAM:
				case VariableCode.JUEL:
				case VariableCode.GOTJUEL:
				case VariableCode.CUP:
				case VariableCode.CDOWN:
					ret = nameToIntDics[paramIndex];//ParamName　２;
					errPos = "palam.csv";
					allowIndex = 1;
					break;

				case VariableCode.TRAINNAME:
					ret = nameToIntDics[trainIndex];//TrainName;
					errPos = "train.csv";
					allowIndex = 0;
					break;
				case VariableCode.MARK:
					ret = nameToIntDics[markIndex];//MarkName;
					errPos = "mark.csv";
					allowIndex = 1;
					break;
				case VariableCode.ITEM:
				case VariableCode.ITEMSALES:
				case VariableCode.ITEMPRICE:
					ret = nameToIntDics[itemIndex];//ItemName;
					errPos = "Item.csv";
					allowIndex = 0;
					break;
				case VariableCode.LOSEBASE:
					ret = nameToIntDics[baseIndex];//BaseName;
					errPos = "base.csv";
					allowIndex = 0;
					break;
				case VariableCode.BASE:
				case VariableCode.MAXBASE:
				case VariableCode.DOWNBASE:
					ret = nameToIntDics[baseIndex];//BaseName;
					errPos = "base.csv";
					allowIndex = 1;
					break;
				case VariableCode.SOURCE:
					ret = nameToIntDics[sourceIndex];//SourceName;
					errPos = "source.csv";
					allowIndex = 1;
					break;
				case VariableCode.EX:
				case VariableCode.NOWEX:
					ret = nameToIntDics[exIndex];//ExName;
					errPos = "ex.csv";
					allowIndex = 1;
					break;


				case VariableCode.EQUIP:
					ret = nameToIntDics[equipIndex];//EquipName;
					errPos = "equip.csv";
					allowIndex = 1;
					break;
				case VariableCode.TEQUIP:
					ret = nameToIntDics[tequipIndex];//TequipName;
					errPos = "tequip.csv";
					allowIndex = 1;
					break;
				case VariableCode.FLAG:
					ret = nameToIntDics[flagIndex];//FlagName;
					errPos = "flag.csv";
					allowIndex = 0;
					break;
				case VariableCode.TFLAG:
					ret = nameToIntDics[tflagIndex];//TFlagName;
					errPos = "tflag.csv";
					allowIndex = 0;
					break;
				case VariableCode.CFLAG:
					ret = nameToIntDics[cflagIndex];//CFlagName;
					errPos = "cflag.csv";
					allowIndex = 1;
					break;
				case VariableCode.TCVAR:
					ret = nameToIntDics[tcvarIndex];//TCVarName;
					errPos = "tcvar.csv";
					allowIndex = 1;
					break;
				case VariableCode.CSTR:
					ret = nameToIntDics[cstrIndex];//CStrName;
					errPos = "cstr.csv";
					allowIndex = 1;
					break;

				case VariableCode.STAIN:
					ret = nameToIntDics[stainIndex];//StainName;
					errPos = "stain.csv";
					allowIndex = 1;
					break;
				case VariableCode.CDFLAGNAME1:
					ret = nameToIntDics[cdflag1Index];
					errPos = "cdflag1.csv";
					allowIndex = 0;
					break;
				case VariableCode.CDFLAGNAME2:
					ret = nameToIntDics[cdflag2Index];
					errPos = "cdflag2.csv";
					allowIndex = 0;
					break;
				case VariableCode.CDFLAG:
				{
					if (index == 1)
					{
						ret = nameToIntDics[cdflag1Index];//CDFlagName1
						errPos = "cdflag1.csv";
					}
					else if (index == 2)
					{
						ret = nameToIntDics[cdflag2Index];//CDFlagName2
						errPos = "cdflag2.csv";
					}
					else if (index >= 0)
						throw new CodeEE(string.Format(Lang.Error.CannotIndexSpecifiedByString.Text, code.ToString(), (index + 1).ToString()));
					else
						throw new CodeEE(Lang.Error.UseCdflagname.Text);
					return ret;
				}
				case VariableCode.STR:
					ret = nameToIntDics[strnameIndex];
					errPos = "strname.csv";
					allowIndex = 0;
					break;
				case VariableCode.TSTR:
					ret = nameToIntDics[tstrnameIndex];
					errPos = "tstr.csv";
					allowIndex = 0;
					break;
				case VariableCode.SAVESTR:
					ret = nameToIntDics[savestrnameIndex];
					errPos = "savestr.csv";
					allowIndex = 0;
					break;
				case VariableCode.GLOBAL:
					ret = nameToIntDics[globalIndex];
					errPos = "global.csv";
					allowIndex = 0;
					break;
				case VariableCode.GLOBALS:
					ret = nameToIntDics[globalsIndex];
					errPos = "globals.csv";
					allowIndex = 0;
					break;
				case VariableCode.RELATION:
					ret = relationDic;
					errPos = "chara*.csv";
					allowIndex = 1;
					break;
				case VariableCode.NAME:
					ret = relationDic;
					errPos = "chara*.csv";
					allowIndex = -1;
					break;
				#region EE_CSV機能拡張
				case VariableCode.DAY:
					ret = nameToIntDics[dayIndex];
					errPos = "day.csv";
					allowIndex = 0;
					break;
				case VariableCode.TIME:
					ret = nameToIntDics[timeIndex];
					errPos = "time.csv";
					allowIndex = 0;
					break;
				case VariableCode.MONEY:
					ret = nameToIntDics[moneyIndex];
					errPos = "money.csv";
					allowIndex = 0;
					break;
				#endregion

			}

			#region EE_ERD
			if (ret == null && Config.UseERD)
			{
				if (string.IsNullOrEmpty(varname))
					return ret;
				else
				{
					switch (code)
					{
						case VariableCode.VAR:
						case VariableCode.VARS:
						case VariableCode.CVAR:
						case VariableCode.CVARS:
							if (!erdNameToIntDics.ContainsKey(varname))
								return ret;
							ret = erdNameToIntDics[varname];
							errPos = varname + ".csv";
							allowIndex = 0;
							if (code == VariableCode.CVAR || code == VariableCode.CVARS)
								allowIndex = 1;
							break;
						case VariableCode.VAR2D:
						case VariableCode.VARS2D:
						case VariableCode.CVAR2D:
						case VariableCode.CVARS2D:
							{
								if ((code == VariableCode.VAR2D && index == 0) || (code == VariableCode.CVAR2D && index == 1))
								{
									string varnamed = varname + "@1";
									if (!erdNameToIntDics.ContainsKey(varnamed))
										return ret;

									ret = erdNameToIntDics[varnamed];
									errPos = varnamed + ".csv";
									allowIndex = 0;
									if (code == VariableCode.CVAR2D || code == VariableCode.CVARS2D)
										allowIndex = 1;
								}
								//if ((code == VariableCode.VAR2D && index == 1) || (code == VariableCode.CVAR2D && index == 2))
								else
								{
									string varnamed = varname + "@2";
									if (!erdNameToIntDics.ContainsKey(varnamed))
										return ret;

									ret = erdNameToIntDics[varnamed];
									errPos = varnamed + ".csv";
									allowIndex = 1;
									if (code == VariableCode.CVAR2D || code == VariableCode.CVARS2D)
										allowIndex = 2;
								}
								break;
							}
						case VariableCode.VAR3D:
						case VariableCode.VARS3D:
							string varname3d = varname + "@" + (index+1);
							if (!erdNameToIntDics.ContainsKey(varname3d))
								return ret;

							ret = erdNameToIntDics[varname3d];
							errPos = varname3d + ".csv";
							allowIndex = index;
							break;
					}
				}
			}
			#endregion
			if (index < 0)
				return ret;
			if (ret == null)
				throw new CodeEE(string.Format(Lang.Error.CanNotSpecifiedByString.Text, code.ToString()));
			if ((index != allowIndex))
			{
				if (allowIndex < 0)//GETNUM専用
					throw new CodeEE(string.Format(Lang.Error.CanNotSpecifiedByString.Text, code.ToString()));
				throw new CodeEE(string.Format(Lang.Error.CannotIndexSpecifiedByString.Text, code.ToString(), (index + 1).ToString()));
			}
			return ret;
		}

		public CharacterTemplate GetCharacterTemplate(Int64 index)
		{
			foreach (CharacterTemplate chara in CharacterTmplList)
			{
				if (chara.No == index)
					return chara;
			}
			return null;
		}
		
		public CharacterTemplate GetCharacterTemplate_UseSp(Int64 index, bool sp)
		{
			foreach (CharacterTemplate chara in CharacterTmplList)
			{
				if (chara.No != index)
					continue;
				if (Config.CompatiSPChara && sp != chara.IsSpchara)
					continue;
				return chara;
			}
			return null;
		}

		public CharacterTemplate GetCharacterTemplateFromCsvNo(Int64 index)
		{
			foreach (CharacterTemplate chara in CharacterTmplList)
			{
				if (chara.csvNo != index)
					continue;
				return chara;
			}
			return null;
		}

		public CharacterTemplate GetPseudoChara()
		{
			return new CharacterTemplate(0, this);
		}

		//private CharacterData dummyChara = null;
		//public CharacterData DummyChara
		//{
		//    get { if (dummyChara == null) dummyChara = new CharacterData(GlobalStatic.VEvaluator.Constant, GetPseudoChara(),varData); return dummyChara; }
		//    set { dummyChara = value; }
		//}

		private void loadCharacterData(string csvDir, bool disp)
		{
			if (!Directory.Exists(csvDir))
				return;
			List<KeyValuePair<string, string>> csvPaths = Config.GetFiles(csvDir, "CHARA*.CSV");
			for (int i = 0; i < csvPaths.Count; i++)
				loadCharacterDataFile(csvPaths[i].Value, csvPaths[i].Key, disp);
			if (useCompatiName)
			{
				foreach (CharacterTemplate tmpl in CharacterTmplList)
					if (string.IsNullOrEmpty(tmpl.Callname))
						tmpl.Callname = tmpl.Name;
			}
			foreach (CharacterTemplate tmpl in CharacterTmplList)
				tmpl.SetSpFlag();
			Dictionary<Int64, CharacterTemplate> nList = new Dictionary<Int64, CharacterTemplate>();
			Dictionary<Int64, CharacterTemplate> spList = new Dictionary<Int64, CharacterTemplate>();
			foreach (CharacterTemplate tmpl in CharacterTmplList)
			{
				Dictionary<Int64, CharacterTemplate>  targetList = nList;
				if(Config.CompatiSPChara && tmpl.IsSpchara)
				{
					targetList = spList;
				}
				if (targetList.TryGetValue(tmpl.No, out var template))
				{

					if (!Config.CompatiSPChara && (tmpl.IsSpchara!= template.IsSpchara))
						ParserMediator.Warn(string.Format(trerror.DuplicateCharaDefine1.Text, tmpl.No.ToString()), null, 1);
					else
						ParserMediator.Warn(string.Format(trerror.DuplicateCharaDefine2.Text, tmpl.No.ToString()), null, 1);
				}
				else
					targetList.Add(tmpl.No, tmpl);
			}
		}

		#region EM_私家版_セーブ拡張
		private void loadGlobalVarExSetting(string csvPath, bool disp)
		{
			GlobalSaveXmls.Clear();
			SaveXmls.Clear();
			GlobalSaveMaps.Clear();
			SaveMaps.Clear();
			StaticMaps.Clear();
			StaticXmls.Clear();
			foreach (var path in FileUtils.GetFiles(csvPath, "VarExt*.csv", SearchOption.AllDirectories))
			{
				EraStreamReader eReader = new EraStreamReader(false);
				if (!eReader.Open(path))
				{
					output.PrintError(string.Format(trerror.FailedOpenFile.Text, eReader.Filename));
					return;
				}
				ScriptPosition position = null;
				if (disp)
					output.PrintSystemLine(string.Format(trsl.LoadingFile.Text, eReader.Filename));
				try
				{
					StringStream st = null;
					while ((st = eReader.ReadEnabledLine()) != null)
					{
						position = new ScriptPosition(eReader.Filename, eReader.LineNo);
						string[] tokens = st.Substring().Split(',');
						if (tokens.Length < 2)
						{
							ParserMediator.Warn(trerror.MissingComma.Text, position, 1);
							continue;
						}
						if (tokens[0].Length == 0)
						{
							ParserMediator.Warn(trerror.StartedComma.Text, position, 1);
							continue;
						}
						if (tokens[0].Equals("GLOBAL_MAPS", Config.SCVariable))
						{
							for (int i = 1; i < tokens.Length; i++)
								GlobalSaveMaps.Add(tokens[i].Trim());
							continue;
						}
						if (tokens[0].Equals("SAVE_MAPS", Config.SCVariable))
						{
							for (int i = 1; i < tokens.Length; i++)
								SaveMaps.Add(tokens[i].Trim());
							continue;
						}
						if (tokens[0].Equals("GLOBAL_XMLS", Config.SCVariable))
						{
							for (int i = 1; i < tokens.Length; i++)
								GlobalSaveXmls.Add(tokens[i].Trim());
							continue;
						}
						if (tokens[0].Equals("SAVE_XMLS", Config.SCVariable))
						{
							for (int i = 1; i < tokens.Length; i++)
								SaveXmls.Add(tokens[i].Trim());
							continue;
						}
						if (tokens[0].Equals("GLOBAL_DTS", Config.SCVariable))
						{
							for (int i = 1; i < tokens.Length; i++)
								GlobalSaveDTs.Add(tokens[i].Trim());
							continue;
						}
						if (tokens[0].Equals("SAVE_DTS", Config.SCVariable))
						{
							for (int i = 1; i < tokens.Length; i++)
								SaveDTs.Add(tokens[i].Trim());
							continue;
						}
						if (tokens[0].Equals("STATIC_XMLS", Config.SCVariable))
						{
							for (int i = 1; i < tokens.Length; i++)
								StaticXmls.Add(tokens[i].Trim());
							continue;
						}
						if (tokens[0].Equals("STATIC_MAPS", Config.SCVariable))
						{
							for (int i = 1; i < tokens.Length; i++)
								StaticMaps.Add(tokens[i].Trim());
							continue;
						}
						if (tokens[0].Equals("STATIC_DTS", Config.SCVariable))
						{
							for (int i = 1; i < tokens.Length; i++)
								StaticDTs.Add(tokens[i].Trim());
							continue;
						}
					}
				}
				catch
				{
					//System.Media.SystemSounds.Hand.Play();
					if (position != null)
						ParserMediator.Warn(trerror.UnexpectedError.Text, position, 3);
					else
						output.PrintError(trerror.UnexpectedError.Text);
					return;
				}
				finally
				{
					eReader.Dispose();
				}
			}
		}
		#endregion
		private void loadCharacterDataFile(string csvPath, string csvName, bool disp)
		{
			CharacterTemplate tmpl = null;
			EraStreamReader eReader = new EraStreamReader(false);
			if (!eReader.Open(csvPath, csvName))
			{
				output.PrintError(string.Format(trerror.FailedOpenFile.Text, eReader.Filename));
				return;
			}
			ScriptPosition position = null;
			if (disp)
				output.PrintSystemLine(string.Format(trsl.LoadingFile.Text, eReader.Filename));
			try
			{
				Int64 index = -1;
				StringStream st = null;
				while ((st = eReader.ReadEnabledLine()) != null)
				{
					position = new ScriptPosition(eReader.Filename, eReader.LineNo);
					string[] tokens = st.Substring().Split(',');
					if (tokens.Length < 2)
					{
						ParserMediator.Warn(trerror.MissingComma.Text, position, 1);
						continue;
					}
					if (tokens[0].Length == 0)
					{
						ParserMediator.Warn(trerror.StartedComma.Text, position, 1);
						continue;
					}
					if ((tokens[0].Equals("NO", Config.SCVariable))
						|| (tokens[0].Equals("番号", Config.SCVariable)))
					{
						if (tmpl != null)
						{
							ParserMediator.Warn(trerror.CharaNoDefinedTwice.Text, position, 1);
							continue;
						}
						if (!Int64.TryParse(tokens[1].TrimEnd(), out index))
						{
							ParserMediator.Warn(string.Format(trerror.CanNotConvertToInt.Text, tokens[1]), position, 1);
							continue;
						}
						tmpl = new CharacterTemplate(index, this);
						string no = eReader.Filename.ToUpper();
						no = no.Substring(no.IndexOf("CHARA") + 5);
						StringBuilder sb = new StringBuilder();
						StringStream ss = new StringStream(no);
						while (!ss.EOS && char.IsDigit(ss.Current))
						{
							sb.Append(ss.Current);
							ss.ShiftNext();
						}
						if (sb.Length > 0)
							tmpl.csvNo = Convert.ToInt64(sb.ToString());
						else
							tmpl.csvNo = 0;
							//tmpl.csvNo = index;
						CharacterTmplList.Add(tmpl);
						continue;
					}
					if (tmpl == null)
					{
						ParserMediator.Warn(trerror.StartedDataBeforeCharaNo.Text, position, 1);
						continue;
					}
					toCharacterTemplate(position, tmpl, tokens);
				}
			}
			catch
			{
				//System.Media.SystemSounds.Hand.Play();
				if (position != null)
					ParserMediator.Warn(trerror.UnexpectedError.Text, position, 3);
				else
					output.PrintError(trerror.UnexpectedError.Text);
				return;
			}
			finally
			{
				eReader.Dispose();
			}
		}

		private bool tryToInt64(string str, out Int64 p)
		{
			p = -1;
			if (string.IsNullOrEmpty(str))
				return false;
			StringStream st = new StringStream(str);
			int sign = 1;
			if (st.Current == '+')
				st.ShiftNext();
			else if (st.Current == '-')
			{
				sign = -1;
				st.ShiftNext();
			}
			//1803beta005 char.IsDigitは全角数字とかまでひろってしまうので･･･
			//if (!char.IsDigit(st.Current))
			// return false;
			switch (st.Current)
			{
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					break;
				default:
					return false;
			}
			try
			{
				p = LexicalAnalyzer.ReadInt64(st, false);
				p *= sign;
			}
			catch
			{
				return false;
			}
			return true;
		}

		private void toCharacterTemplate(ScriptPosition position, CharacterTemplate chara, string[] tokens)
		{
			if (chara == null)
				return;
			int length;
            Dictionary<int, Int64> intArray = null;
            Dictionary<int, string> strArray = null;
			Dictionary<string, int> namearray;

			string errPos = null;
			string varname = tokens[0].ToUpper();
			switch (varname)
			{
				case "NAME":
				case "名前":
					chara.Name = tokens[1];
					return;
				case "CALLNAME":
				case "呼び名":
					chara.Callname = tokens[1];
					return;
				case "NICKNAME":
				case "あだ名":
					chara.Nickname = tokens[1];
					return;
				case "MASTERNAME":
				case "主人の呼び方":
					chara.Mastername = tokens[1];
					return;
				case "MARK":
				case "刻印":
					length = CharacterIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.MARK)];
					intArray = chara.Mark;
					namearray = nameToIntDics[markIndex];
					errPos = "mark.csv";
					break;
				case "EXP":
				case "経験":
					length = CharacterIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.EXP)];
					intArray = chara.Exp;
					namearray = nameToIntDics[expIndex];//ExpName;
					errPos = "exp.csv";
					break;
				case "ABL":
				case "能力":
					length = CharacterIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.ABL)];
					intArray = chara.Abl;
					namearray = nameToIntDics[ablIndex];//AblName;
					errPos = "abl.csv";
					break;
				case "BASE":
				case "基礎":
					length = CharacterIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.MAXBASE)];
					intArray = chara.Maxbase;
					namearray = nameToIntDics[baseIndex];//BaseName;
					errPos = "base.csv";
					break;
				case "TALENT":
				case "素質":
					length = CharacterIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.TALENT)];
					intArray = chara.Talent;
					namearray = nameToIntDics[talentIndex];//TalentName;
					errPos = "talent.csv";
					break;
				case "RELATION":
				case "相性":
					length = CharacterIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.RELATION)];
					intArray = chara.Relation;
					namearray = null;
					break;
				case "CFLAG":
				case "フラグ":
					length = CharacterIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.CFLAG)];
					intArray = chara.CFlag;
					namearray = nameToIntDics[cflagIndex];//CFlagName;
					errPos = "cflag.csv";
					break;
				case "EQUIP":
				case "装着物":
					length = CharacterIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.EQUIP)];
					intArray = chara.Equip;
					namearray = nameToIntDics[equipIndex];//EquipName;
					errPos = "equip.csv";
					break;
				case "JUEL":
				case "珠":
					length = CharacterIntArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.JUEL)];
					intArray = chara.Juel;
					namearray = nameToIntDics[paramIndex];//ParamName;
					errPos = "palam.csv";
					break;
				case "CSTR":
					length = CharacterStrArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.CSTR)];
					strArray = chara.CStr;
					namearray = nameToIntDics[cstrIndex];//CStrName;
					errPos = "cstr.csv";
					break;
				case "ISASSI":
				case "助手":
					return;
				default:
					ParserMediator.Warn(string.Format(trerror.CanNotInterpreted.Text, tokens[0]), position, 1);
					return;
			}
			if (length < 0)
			{
				ParserMediator.Warn(trerror.ProgramError.Text, position, 3);
				return;
			}
			if (length == 0)
			{
				ParserMediator.Warn(string.Format(trerror.IsProhibitedVar.Text, varname), position, 2);
				return;
			}
			bool p1isNumeric = tryToInt64(tokens[1].TrimEnd(), out long p1);
			if (p1isNumeric && ((p1 < 0) || (p1 >= length)))
			{
				ParserMediator.Warn(string.Format(trerror.OoRArray.Text, p1.ToString()), position, 1);
				return;
			}
			int index = (int)p1;
			if ((!p1isNumeric) && (namearray != null))
			{
				if (!namearray.TryGetValue(tokens[1], out index))
				{
					ParserMediator.Warn(string.Format(trerror.NotDefinedKey.Text, errPos, tokens[1]), position, 1);
					//ParserMediator.Warn("\"" + tokens[1] + "\"は解釈できない識別子です", position, 1);
					return;
				}
				else if (index >= length)
				{
					ParserMediator.Warn(string.Format(trerror.OoRArray.Text, tokens[1]), position, 1);
					return;
				}
			}

			if ((index < 0) || (index >= length))
			{
				if (p1isNumeric)
					ParserMediator.Warn(string.Format(trerror.OoRArray.Text, index.ToString()), position, 1);
				else if (tokens[1].Length == 0)
					ParserMediator.Warn(trerror.MissingSecondIdentifier.Text, position, 1);
				else
					ParserMediator.Warn(string.Format(trerror.CanNotInterpreted.Text, tokens[1]), position, 1);
				return;
			}
			if (strArray != null)
			{
				if (tokens.Length < 3)
					ParserMediator.Warn(trerror.MissingThirdIdentifier.Text, position, 1);
				if (strArray.ContainsKey(index))
					ParserMediator.Warn(string.Format(trerror.VarKeyAreadyDefined.Text, varname, index.ToString()), position, 1);
				strArray[index] = tokens[2];
			}
			else
			{
				if ((tokens.Length < 3) || !tryToInt64(tokens[2], out long p2))
					p2 = 1;
				if (intArray.ContainsKey(index))
					ParserMediator.Warn(string.Format(trerror.VarKeyAreadyDefined.Text, varname, index.ToString()), position, 1);
				intArray[index] = p2;
			}
		}

		private void loadDataTo(string csvPath, int targetIndex, Int64[] targetI, bool disp)
		{

			if (!FileUtils.Exists(ref csvPath))
				return;
			string[] target = names[targetIndex];
            HashSet<int> defined = new HashSet<int>();
			EraStreamReader eReader = new EraStreamReader(false);
			#region EE_ERD
			// if (!eReader.Open(csvPath))
			if (!eReader.Open(csvPath) && output != null)
			#endregion
			{
				output.PrintError(string.Format(trerror.FailedOpenFile.Text, eReader.Filename));
				return;
			}
			ScriptPosition position = null;
			#region EE_ERD
			// if (disp || Program.AnalysisMode)
			if ((disp || Program.AnalysisMode) && output != null)
			#endregion
				output.PrintSystemLine(string.Format(trsl.LoadingFile.Text, eReader.Filename));
			try
			{
				StringStream st = null;
				while ((st = eReader.ReadEnabledLine()) != null)
				{
					position = new ScriptPosition(eReader.Filename, eReader.LineNo);
					string[] tokens = st.Substring().Split(',');
					if (tokens.Length < 2)
					{
						ParserMediator.Warn(trerror.MissingComma.Text, position, 1);
						continue;
					}
                    if (!Int32.TryParse(tokens[0], out int index))
                    {
                        ParserMediator.Warn(trerror.FirstValueCanNotConvertToInt.Text, position, 1);
                        continue;
                    }
                    if (target.Length == 0)
					{
						ParserMediator.Warn(trerror.ProhibitedArrayName.Text, position, 2);
						break;
					}
					if (((index < 0) || (target.Length <= index)))
					{
						ParserMediator.Warn(string.Format(trerror.OoRArray.Text, index.ToString()), position, 1);
						continue;
					}
                    if (!defined.Add(index))
                        ParserMediator.Warn(string.Format(trerror.VarKeyAreadyDefined.Text, index.ToString()), position, 1);
					target[index] = tokens[1];
					if ((targetI != null) && (tokens.Length >= 3))
					{

                        if (!Int64.TryParse(tokens[2].TrimEnd(), out long price))
                        {
                            ParserMediator.Warn(trerror.CanNotReadAmountOfMoney.Text, position, 1);
                            continue;
                        }

                        targetI[index] = price;
					}
				}
			}
			catch
			{
				//System.Media.SystemSounds.Hand.Play();
				if (position != null)
					ParserMediator.Warn(trerror.UnexpectedError.Text, position, 3);
				else
					output.PrintError(trerror.UnexpectedError.Text);
				return;
			}
			finally
			{
				eReader.Close();
			}


		}
	}

	internal sealed class CharacterTemplate
	{
        readonly int[] arraySize;
        readonly int cstrSize;

		public string Name;
		public string Callname;
		public string Nickname;
		public string Mastername;
		public readonly Int64 No;
		public readonly Dictionary<Int32, Int64> Maxbase = new Dictionary<Int32, Int64>();
		public readonly Dictionary<Int32, Int64> Mark = new Dictionary<Int32, Int64>();
		public readonly Dictionary<Int32, Int64> Exp = new Dictionary<Int32, Int64>();
		public readonly Dictionary<Int32, Int64> Abl = new Dictionary<Int32, Int64>();
		public readonly Dictionary<Int32, Int64> Talent = new Dictionary<Int32, Int64>();
		public readonly Dictionary<Int32, Int64> Relation = new Dictionary<Int32, Int64>();
		public readonly Dictionary<Int32, Int64> CFlag = new Dictionary<Int32, Int64>();
		public readonly Dictionary<Int32, Int64> Equip = new Dictionary<Int32, Int64>();
		public readonly Dictionary<Int32, Int64> Juel = new Dictionary<Int32, Int64>();
		public readonly Dictionary<Int32, string> CStr = new Dictionary<Int32, string>();
		public Int64 csvNo;
		public bool IsSpchara { get; private set; }
		
		public CharacterTemplate(Int64 index, ConstantData constant)
		{
			arraySize = constant.CharacterIntArrayLength;
			cstrSize = constant.CharacterStrArrayLength[(int)(VariableCode.__LOWERCASE__ & VariableCode.CSTR)];
			No = index;
		}
		public int ArrayStrLength(CharacterStrData type)
		{
			switch (type)
			{
				case CharacterStrData.CSTR:
					return cstrSize;
				default:
					throw new CodeEE(Lang.Error.NotExistKey.Text);
			}
		}

		public int ArrayLength(CharacterIntData type)
		{
			switch (type)
			{
				case CharacterIntData.BASE:
					{
						int size = arraySize[(int)(VariableCode.__LOWERCASE__ & VariableCode.BASE)];
						int maxSize = arraySize[(int)(VariableCode.__LOWERCASE__ & VariableCode.MAXBASE)];
						return size > maxSize ? size : maxSize;
					}
				case CharacterIntData.MARK:
					return arraySize[(int)(VariableCode.__LOWERCASE__ & VariableCode.MARK)];
				case CharacterIntData.ABL:
					return arraySize[(int)(VariableCode.__LOWERCASE__ & VariableCode.ABL)];
				case CharacterIntData.EXP:
					return arraySize[(int)(VariableCode.__LOWERCASE__ & VariableCode.EXP)];
				case CharacterIntData.RELATION:
					return arraySize[(int)(VariableCode.__LOWERCASE__ & VariableCode.RELATION)];
				case CharacterIntData.TALENT:
					return arraySize[(int)(VariableCode.__LOWERCASE__ & VariableCode.TALENT)];
				case CharacterIntData.CFLAG:
					return arraySize[(int)(VariableCode.__LOWERCASE__ & VariableCode.CFLAG)];
				case CharacterIntData.EQUIP:
					return arraySize[(int)(VariableCode.__LOWERCASE__ & VariableCode.EQUIP)];
				case CharacterIntData.JUEL:
					return arraySize[(int)(VariableCode.__LOWERCASE__ & VariableCode.JUEL)];
				default:
					throw new CodeEE(trerror.NotExistKey.Text);
			}
		}

		internal void SetSpFlag()
		{
			//bool res;
			if (CFlag.ContainsKey(0) && CFlag[0] != 0L)
				IsSpchara = true;
		}
	}
}
