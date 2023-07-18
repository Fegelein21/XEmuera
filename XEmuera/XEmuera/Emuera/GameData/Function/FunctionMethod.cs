using System;
using System.Collections.Generic;
using System.Text;
using EvilMask.Emuera;
using MinorShift.Emuera.GameData.Expression;
using MinorShift.Emuera.GameData.Variable;
using MinorShift.Emuera.Sub;
using trerror = EvilMask.Emuera.Lang.Error;
namespace MinorShift.Emuera.GameData.Function
{
	internal abstract class FunctionMethod
	{
		public Type ReturnType { get; protected set; }
		protected Type[] argumentTypeArray;
		protected string Name { get; private set; }
		#region EM_私家版_Emuera多言語化改造
		protected enum ArgType
		{ 
			Invalid = 0,

			Any = 1,
			Int = 1<<1,
			String = 1<<2,
			Ref = 1<<3,
			Array = 1<<4,
			Array1D = 1<<5,
			Array2D = 1<<6,
			Array3D = 1<<7,
			Variadic = 1<<8,
			SameAsFirst = 1<<9,
			CharacterData = Ref | 1 << 10,
			AllowConstRef = 1 << 11,
			DisallowVoid = 1 << 12,

			RefInt = Ref | Int,
			RefAny = Ref | Any,
			RefString = Ref | String,
			RefAnyArray = RefAny | Array,
			RefIntArray = RefInt | Array,
			RefStringArray = RefString | Array,
			RefAny1D = RefAny | Array1D,
			RefInt1D = RefInt | Array1D,
			RefString1D = RefString | Array1D,
			RefAny2D = RefAny | Array2D,
			RefInt2D = RefInt | Array2D,
			RefString2D = RefString | Array2D,
			RefAny3D = RefAny | Array3D,
			RefInt3D = RefInt | Array3D,
			RefString3D = RefString | Array3D,

			VariadicInt = Variadic | Int,
			VariadicString = Variadic | String,
			VariadicSameAsFirst = Variadic | SameAsFirst,
		}
		protected sealed class _ArgType
		{
			public _ArgType(ArgType t)
			{
				type = t;
			}
			public Type Type { get { return this.Int ? typeof(Int64) : typeof(string); } }
			public ArgType type = ArgType.Invalid;
			public bool AllowConstRef { get { return (type & ArgType.AllowConstRef) != 0; } }
			public bool DisallowVoid { get { return (type & ArgType.DisallowVoid) != 0; } }
			public bool Ref { get { return (type & ArgType.Ref) != 0; } }
			public bool Any { get { return (type & ArgType.Any) != 0; } }
			public bool Int { get { return (type & ArgType.Int) != 0; } }
			public bool Array { get { return (type & ArgType.Array) != 0; } }
			public bool Array1D { get { return (type & ArgType.Array1D) != 0; } }
			public bool Array2D { get { return (type & ArgType.Array2D) != 0; } }
			public bool Array3D { get { return (type & ArgType.Array2D) != 0; } }
			public bool String { get { return (type & ArgType.String) != 0; } }
			public bool Variadic { get { return (type & ArgType.Variadic) != 0; } }
			public bool SameAsFirst { get { return (type & ArgType.SameAsFirst) != 0; } }
			public bool CharacterData { get { return ((int)type & (1 << 10)) != 0; } }
			public int ArrayDims { get { return this.Array ? -1 
						: (this.Array1D ? 1 
						: (this.Array2D ? 2
						: (this.Array3D ? 3 : 0))); } }

			public static implicit operator _ArgType(ArgType value)
			{
				return new _ArgType(value);
			}
		}
		protected sealed class ArgTypeList
		{
			public List<_ArgType> ArgTypes { get; set; } = new List<_ArgType>();
			public int OmitStart { get; set; } = -1;

			public _ArgType Last { get { return ArgTypes.Count > 0 ? ArgTypes[ArgTypes.Count - 1] : null; } }
		}
		protected ArgTypeList[] argumentTypeArrayEx = null;

		//引数の数・型が一致するかどうかのテスト
		//正しくない場合はエラーメッセージを返す。
		//引数の数が不定である場合や引数の省略を許す場合にはoverrideすること。
		private string CheckArgumentTypeEx(string name, IOperandTerm[] arguments)
		{
			string[] errMsg = new string[argumentTypeArrayEx.Length];
			for (int idx = 0; idx < argumentTypeArrayEx.Length; idx++)
			{
				var list = argumentTypeArrayEx[idx];
				bool variadic = list.ArgTypes.Count > 0 && list.Last.Variadic;
				bool argsNotMoreThanRule = variadic ? true : arguments.Length <= list.ArgTypes.Count;
				bool argsNotLessThanRule = list.OmitStart > -1 ? arguments.Length >= list.OmitStart : arguments.Length >= list.ArgTypes.Count;
				if (argsNotMoreThanRule && argsNotLessThanRule)
				{
					// 引数の数が有効
					for (int i = 0; i < (variadic ? arguments.Length : Math.Min(arguments.Length, list.ArgTypes.Count)); i++)
					{
						var rule = variadic && i + 1 >= list.ArgTypes.Count ? list.Last : list.ArgTypes[i];
						if (arguments[i] == null)
						{
							if (i < list.OmitStart || (list.OmitStart > -1 && i >= list.OmitStart && rule.DisallowVoid))
							{
								errMsg[idx] = string.Format(Lang.Error.ArgCanNotBeNull.Text, name, i + 1);
								break;
							}
							else continue;
						}
						bool typeNotMatch = rule.SameAsFirst
							? arguments[0].GetOperandType() != arguments[i].GetOperandType()
							: !rule.Any && rule.Type != arguments[i].GetOperandType();
						if (rule.Ref)
						{
							if (rule.CharacterData && (!(arguments[i] is VariableTerm cvarTerm) || !cvarTerm.Identifier.IsCharacterData))
							{
								// キャラ変数ではない
								errMsg[idx] = string.Format(Lang.Error.ArgIsNotCharacterVar.Text, name, i + 1);
								break;
							}
							// 引数の型が違う
							bool error = false;
							string errText;
							var dims = rule.ArrayDims;
							switch (dims)
							{
								case 0:
									{ 
										// 普通の場合
										var err = rule.String ? Lang.Error.ArgIsNotStrVar
											: (rule.Int ? Lang.Error.ArgIsNotIntVar : Lang.Error.ArgIsNotVar);
										errText = string.Format(err.Text, name, i + 1);
										break;
									}
								case -1:
									{
										// 任意配列の場合
										var err = rule.String ? Lang.Error.ArgIsNotStrArray
											: (rule.Int ? Lang.Error.ArgIsNotIntArray : Lang.Error.ArgIsNotArray);
										errText = string.Format(err.Text, name, i + 1);
										break;
									}
								default:
									{
										// 1-3次元配列の場合
										var err = rule.String ? Lang.Error.ArgIsNotNDStrArray
											: (rule.Int ? Lang.Error.ArgIsNotNDIntArray : Lang.Error.ArgIsNotNDArray);
										errText = string.Format(err.Text, name, i + 1, dims);
										break;
									}
							}
							// 引数が引用系
							if ((arguments[i] is VariableTerm varTerm && !(varTerm.Identifier.IsCalc || (!rule.AllowConstRef && varTerm.Identifier.IsConst))))
							{
								// 変数の場合
								switch (dims)
								{
									case 0: error = typeNotMatch; break;
									case -1: error = (!varTerm.Identifier.IsArray1D && !varTerm.Identifier.IsArray2D && !varTerm.Identifier.IsArray3D) || typeNotMatch; break;
									case 1: error = !varTerm.Identifier.IsArray1D || typeNotMatch; break;
									case 2: error = !varTerm.Identifier.IsArray2D || typeNotMatch; break;
									case 3: error = !varTerm.Identifier.IsArray3D || typeNotMatch; break;
								}
							}
							else error = true; // 変数ではない
							if (error)
							{
								errMsg[idx] = errText;
								break;
							}
						}
						else if (typeNotMatch)
						{
							var type = rule.SameAsFirst ? arguments[0].GetOperandType() : rule.Type;
							// 引数の型が違う
							errMsg[idx] = type == typeof(string) ? string.Format(Lang.Error.ArgIsNotStr.Text, name, i + 1)
								: string.Format(Lang.Error.ArgIsNotInt.Text, name, i + 1);
							break;
						}
					}
					if (errMsg[idx] == null)
						return null;
				}
				else if (list.OmitStart == -1 && list.ArgTypes.Count > 0 && !list.Last.Variadic)
				{
					// 数固定の引数が必要
					if (list.ArgTypes.Count > 0)
						errMsg[idx] = string.Format(Lang.Error.ArgsCountNotMatches.Text, name, list.ArgTypes.Count, arguments.Length);
					else
						errMsg[idx] = string.Format(Lang.Error.ArgsNotNeeded.Text, name);
					continue;
				}
				// 可変長引数
				else if (!argsNotMoreThanRule)
				{
					// 引数が多すぎる
					errMsg[idx] = string.Format(Lang.Error.TooManyFuncArgs.Text, name);
					continue;
				}
				else
				{
					// 引数が足りない
					errMsg[idx] = string.Format(Lang.Error.NotEnoughArgs.Text, name, list.OmitStart < 0 ? list.ArgTypes.Count : list.OmitStart);
					continue;
				}
			}
			if (argumentTypeArrayEx.Length == 1) return errMsg[0];

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < errMsg.Length; i++)
			{
				sb.Append(string.Format(Lang.Error.NotValidArgsReason.Text, i + 1, errMsg[i]));
				if (i + 1 < errMsg.Length) sb.Append(" | ");
			}
			return string.Format(Lang.Error.NotValidArgs.Text, name, sb.ToString());
		}
		public virtual string CheckArgumentType(string name, IOperandTerm[] arguments)
		{
			if (argumentTypeArrayEx != null)
			{
				return CheckArgumentTypeEx(name, arguments);
			}
			else if (argumentTypeArray!=null)
			{
				if (arguments.Length != argumentTypeArray.Length)
				// return string.Format(Properties.Resources.SyntaxErrMesMethodDefaultArgumentNum0, name);
				{
					if (argumentTypeArray.Length > 0)
						return string.Format(Lang.Error.ArgsCountNotMatches.Text, name, argumentTypeArray.Length, arguments.Length);
					else
						return string.Format(Lang.Error.ArgsNotNeeded.Text, name);
				}
				for (int i = 0; i < argumentTypeArray.Length; i++)
				{
					if (arguments[i] == null)
						// return string.Format(Properties.Resources.SyntaxErrMesMethodDefaultArgumentNotNullable0, name, i + 1);
						return string.Format(Lang.Error.ArgCanNotBeNull.Text, name, i + 1);
					if (argumentTypeArray[i] != arguments[i].GetOperandType())
						// return string.Format(Properties.Resources.SyntaxErrMesMethodDefaultArgumentType0, name, i + 1);
						return argumentTypeArray[i] == typeof(string) ? string.Format(Lang.Error.ArgIsNotStr.Text, name, i + 1)
								: string.Format(Lang.Error.ArgIsNotInt.Text, name, i + 1);
				}
			}
			return null;
		}
		#endregion

		//Argumentが全て定数の時にMethodを解体してよいかどうか。RANDやCharaを参照するものなどは不可
		public bool CanRestructure { get; protected set; }

		//FunctionMethodが固有のRestructure()を持つかどうか
		public bool HasUniqueRestructure { get; protected set; }

		//実際の計算。
		public virtual Int64 GetIntValue(ExpressionMediator exm, IOperandTerm[] arguments) { throw new ExeEE(trerror.ReturnTypeDifferentOrNotImpelemnt.Text); }
		public virtual string GetStrValue(ExpressionMediator exm, IOperandTerm[] arguments) { throw new ExeEE(trerror.ReturnTypeDifferentOrNotImpelemnt.Text); }
		public virtual SingleTerm GetReturnValue(ExpressionMediator exm, IOperandTerm[] arguments)
		{
			if (ReturnType == typeof(Int64))
				return new SingleTerm(GetIntValue(exm, arguments));
			else
				return new SingleTerm(GetStrValue(exm, arguments));
		}

		/// <summary>
		/// 戻り値は全体をRestructureできるかどうか
		/// </summary>
		/// <param name="exm"></param>
		/// <param name="arguments"></param>
		/// <returns></returns>
		public virtual bool UniqueRestructure(ExpressionMediator exm, IOperandTerm[] arguments)
		{ throw new ExeEE(trerror.NotImplement.Text); }


		internal void SetMethodName(string name)
		{
			Name = name;
		}
	}
}
