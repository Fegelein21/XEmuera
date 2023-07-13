using System;
using System.Collections.Generic;
using System.Text;
using MinorShift.Emuera.Sub;
using MinorShift.Emuera.GameProc;
using MinorShift.Emuera.GameData.Expression;

namespace MinorShift.Emuera.GameData.Variable
{
	
	//変数の引数のうち文字列型のもの。
	internal sealed class VariableStrArgTerm : IOperandTerm
	{
		#region EE_ERD
		// public VariableStrArgTerm(VariableCode code, IOperandTerm strTerm, int index)
		public VariableStrArgTerm(VariableCode code, IOperandTerm strTerm, int index, string varname)
		#endregion
			: base(typeof(Int64))
		{
			this.strTerm = strTerm;
			parentCode = code;
			this.index = index;
			#region EE_ERD
			this.varname = varname;
			#endregion

		}
		IOperandTerm strTerm;
		readonly VariableCode parentCode;
		readonly int index;
		#region EE_ERD
		readonly string varname;
		#endregion
		Dictionary<string, int> dic = null;
		string errPos = null;
		
        public override Int64 GetIntValue(ExpressionMediator exm)
		{
			if (dic == null)
				#region EE_ERD
				// dic = exm.VEvaluator.Constant.GetKeywordDictionary(out errPos, parentCode, index);
				dic = exm.VEvaluator.Constant.GetKeywordDictionary(out errPos, parentCode, index, varname);
				#endregion
			string key = strTerm.GetStrValue(exm);
			if (key == "")
				throw new CodeEE("キーワードを空には出来ません");
			#region EE_ERD
			if (dic == null && key != "")
				throw new CodeEE("配列変数" + parentCode.ToString() + "に" + key + "の定義はありません");
			#endregion

			if (!dic.TryGetValue(key, out int i))
            {
                if (errPos == null)
                    throw new CodeEE("配列変数" + parentCode.ToString() + "の要素を文字列で指定することはできません");
                else
                    throw new CodeEE(errPos + "の中に\"" + key + "\"の定義がありません");
            }
            return i;
        }
		
        public override IOperandTerm Restructure(ExpressionMediator exm)
        {
			if (dic == null)
				#region EE_ERD
				// dic = exm.VEvaluator.Constant.GetKeywordDictionary(out errPos, parentCode, index);
				dic = exm.VEvaluator.Constant.GetKeywordDictionary(out errPos, parentCode, index, null);
				#endregion

			strTerm = strTerm.Restructure(exm);
			if (!(strTerm is SingleTerm))
				return this;
			return new SingleTerm(this.GetIntValue(exm));
        }
	}

}