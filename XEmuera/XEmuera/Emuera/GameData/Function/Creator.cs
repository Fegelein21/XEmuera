﻿using System;
using System.Collections.Generic;
using System.Text;
using MinorShift.Emuera.GameData.Expression;
using MinorShift.Emuera.Sub;
using MinorShift.Emuera.GameProc;


namespace MinorShift.Emuera.GameData.Function
{
	internal static partial class FunctionMethodCreator
	{
		static FunctionMethodCreator()
		{
            methodList = new Dictionary<string, FunctionMethod>
            {
                //キャラクタデータ系
                ["GETCHARA"] = new GetcharaMethod(),
                ["GETSPCHARA"] = new GetspcharaMethod(),
                ["CSVNAME"] = new CsvStrDataMethod(CharacterStrData.NAME),
                ["CSVCALLNAME"] = new CsvStrDataMethod(CharacterStrData.CALLNAME),
                ["CSVNICKNAME"] = new CsvStrDataMethod(CharacterStrData.NICKNAME),
                ["CSVMASTERNAME"] = new CsvStrDataMethod(CharacterStrData.MASTERNAME),
                ["CSVCSTR"] = new CsvcstrMethod(),
                ["CSVBASE"] = new CsvDataMethod(CharacterIntData.BASE),
                ["CSVABL"] = new CsvDataMethod(CharacterIntData.ABL),
                ["CSVMARK"] = new CsvDataMethod(CharacterIntData.MARK),
                ["CSVEXP"] = new CsvDataMethod(CharacterIntData.EXP),
                ["CSVRELATION"] = new CsvDataMethod(CharacterIntData.RELATION),
                ["CSVTALENT"] = new CsvDataMethod(CharacterIntData.TALENT),
                ["CSVCFLAG"] = new CsvDataMethod(CharacterIntData.CFLAG),
                ["CSVEQUIP"] = new CsvDataMethod(CharacterIntData.EQUIP),
                ["CSVJUEL"] = new CsvDataMethod(CharacterIntData.JUEL),
                ["FINDCHARA"] = new FindcharaMethod(false),
                ["FINDLASTCHARA"] = new FindcharaMethod(true),
                ["EXISTCSV"] = new ExistCsvMethod(),

                //汎用処理系
                ["VARSIZE"] = new VarsizeMethod(),
                ["CHKFONT"] = new CheckfontMethod(),
                ["CHKDATA"] = new CheckdataMethod(EraSaveFileType.Normal),
                ["ISSKIP"] = new IsSkipMethod(),
                ["MOUSESKIP"] = new MesSkipMethod(true),
                ["MESSKIP"] = new MesSkipMethod(false),
                ["GETCOLOR"] = new GetColorMethod(false),
                ["GETDEFCOLOR"] = new GetColorMethod(true),
                ["GETFOCUSCOLOR"] = new GetFocusColorMethod(),
                ["GETBGCOLOR"] = new GetBGColorMethod(false),
                ["GETDEFBGCOLOR"] = new GetBGColorMethod(true),
                ["GETSTYLE"] = new GetStyleMethod(),
                ["GETFONT"] = new GetFontMethod(),
                ["BARSTR"] = new BarStringMethod(),
                ["CURRENTALIGN"] = new CurrentAlignMethod(),
                ["CURRENTREDRAW"] = new CurrentRedrawMethod(),
                ["COLOR_FROMNAME"] = new ColorFromNameMethod(),
                ["COLOR_FROMRGB"] = new ColorFromRGBMethod(),

                //TODO:1810
                //methodList["CHKVARDATA"] = new CheckdataStrMethod(EraSaveFileType.Var);
                ["CHKCHARADATA"] = new CheckdataStrMethod(EraSaveFileType.CharVar),
                //methodList["CHKGLOBALDATA"] = new CheckdataMethod(EraSaveFileType.Global);
                //methodList["FIND_VARDATA"] = new FindFilesMethod(EraSaveFileType.Var);
                ["FIND_CHARADATA"] = new FindFilesMethod(EraSaveFileType.CharVar),

                //定数取得
                ["MONEYSTR"] = new MoneyStrMethod(),
                ["PRINTCPERLINE"] = new GetPrintCPerLineMethod(),
                ["PRINTCLENGTH"] = new PrintCLengthMethod(),
                ["SAVENOS"] = new GetSaveNosMethod(),
                ["GETTIME"] = new GettimeMethod(),
                ["GETTIMES"] = new GettimesMethod(),
                ["GETMILLISECOND"] = new GetmsMethod(),
                ["GETSECOND"] = new GetSecondMethod(),

                //数学関数
                ["RAND"] = new RandMethod(),
                ["MIN"] = new MaxMethod(false),
                ["MAX"] = new MaxMethod(true),
                ["ABS"] = new AbsMethod(),
                ["POWER"] = new PowerMethod(),
                ["SQRT"] = new SqrtMethod(),
                ["CBRT"] = new CbrtMethod(),
                ["LOG"] = new LogMethod(),
                ["LOG10"] = new LogMethod(10.0d),
                ["EXPONENT"] = new ExpMethod(),
                ["SIGN"] = new SignMethod(),
                ["LIMIT"] = new GetLimitMethod(),

                //変数操作系
                ["SUMARRAY"] = new SumArrayMethod(),
                ["SUMCARRAY"] = new SumArrayMethod(true),
                ["MATCH"] = new MatchMethod(),
                ["CMATCH"] = new MatchMethod(true),
                ["GROUPMATCH"] = new GroupMatchMethod(),
                ["NOSAMES"] = new NosamesMethod(),
                ["ALLSAMES"] = new AllsamesMethod(),
                ["MAXARRAY"] = new MaxArrayMethod(),
                ["MAXCARRAY"] = new MaxArrayMethod(true),
                ["MINARRAY"] = new MaxArrayMethod(false, false),
                ["MINCARRAY"] = new MaxArrayMethod(true, false),
                ["GETBIT"] = new GetbitMethod(),
                ["GETNUM"] = new GetnumMethod(),
                ["GETPALAMLV"] = new GetPalamLVMethod(),
                ["GETEXPLV"] = new GetExpLVMethod(),
                ["FINDELEMENT"] = new FindElementMethod(false),
                ["FINDLASTELEMENT"] = new FindElementMethod(true),
                ["INRANGE"] = new InRangeMethod(),
                ["INRANGEARRAY"] = new InRangeArrayMethod(),
                ["INRANGECARRAY"] = new InRangeArrayMethod(true),
                ["GETNUMB"] = new GetnumMethod(),

                ["ARRAYMSORT"] = new ArrayMultiSortMethod(),

                //文字列操作系
                ["STRLENS"] = new StrlenMethod(),
                ["STRLENSU"] = new StrlenuMethod(),
                ["SUBSTRING"] = new SubstringMethod(),
                ["SUBSTRINGU"] = new SubstringuMethod(),
                ["STRFIND"] = new StrfindMethod(false),
                ["STRFINDU"] = new StrfindMethod(true),
                ["STRCOUNT"] = new StrCountMethod(),
                ["TOSTR"] = new ToStrMethod(),
                ["TOINT"] = new ToIntMethod(),
                ["TOUPPER"] = new StrChangeStyleMethod(StrFormType.Upper),
                ["TOLOWER"] = new StrChangeStyleMethod(StrFormType.Lower),
                ["TOHALF"] = new StrChangeStyleMethod(StrFormType.Half),
                ["TOFULL"] = new StrChangeStyleMethod(StrFormType.Full),
                ["LINEISEMPTY"] = new LineIsEmptyMethod(),
                ["REPLACE"] = new ReplaceMethod(),
                ["UNICODE"] = new UnicodeMethod(),
                ["UNICODEBYTE"] = new UnicodeByteMethod(),
                ["CONVERT"] = new ConvertIntMethod(),
                ["ISNUMERIC"] = new IsNumericMethod(),
                ["ESCAPE"] = new EscapeMethod(),
                ["ENCODETOUNI"] = new EncodeToUniMethod(),
                ["CHARATU"] = new CharAtMethod(),
                ["GETLINESTR"] = new GetLineStrMethod(),
                ["STRFORM"] = new StrFormMethod(),
                ["STRJOIN"] = new JoinMethod(),

                ["GETCONFIG"] = new GetConfigMethod(true),
                ["GETCONFIGS"] = new GetConfigMethod(false),

                //html系
                ["HTML_GETPRINTEDSTR"] = new HtmlGetPrintedStrMethod(),
                ["HTML_POPPRINTINGSTR"] = new HtmlPopPrintingStrMethod(),
                ["HTML_TOPLAINTEXT"] = new HtmlToPlainTextMethod(),
                ["HTML_ESCAPE"] = new HtmlEscapeMethod(),


                //画像処理系
                ["SPRITECREATED"] = new SpriteStateMethod(),
                ["SPRITEWIDTH"] = new SpriteStateMethod(),
                ["SPRITEHEIGHT"] = new SpriteStateMethod(),
                ["SPRITEMOVE"] = new SpriteSetPosMethod(),
                ["SPRITESETPOS"] = new SpriteSetPosMethod(),
                ["SPRITEPOSX"] = new SpriteStateMethod(),
                ["SPRITEPOSY"] = new SpriteStateMethod(),

                ["CLIENTWIDTH"] = new ClientSizeMethod(),
                ["CLIENTHEIGHT"] = new ClientSizeMethod(),

                ["GETKEY"] = new GetKeyStateMethod(),
                ["GETKEYTRIGGERED"] = new GetKeyStateMethod(),
                ["MOUSEX"] = new MousePosMethod(),
                ["MOUSEY"] = new MousePosMethod(),
                ["ISACTIVE"] = new IsActiveMethod(),
                ["SAVETEXT"] = new SaveTextMethod(),
                ["LOADTEXT"] = new LoadTextMethod(),

                ["GCREATED"] = new GraphicsStateMethod(),// ("GCREATED");
                ["GWIDTH"] = new GraphicsStateMethod(),//("GWIDTH");
                ["GHEIGHT"] = new GraphicsStateMethod(),//("GHEIGHT");
                ["GGETCOLOR"] = new GraphicsGetColorMethod(),
                ["SPRITEGETCOLOR"] = new SpriteGetColorMethod(),

                ["GCREATE"] = new GraphicsCreateMethod(),
                ["GCREATEFROMFILE"] = new GraphicsCreateFromFileMethod(),
                ["GDISPOSE"] = new GraphicsDisposeMethod(),
                ["GCLEAR"] = new GraphicsClearMethod(),
                ["GFILLRECTANGLE"] = new GraphicsFillRectangleMethod(),
                ["GDRAWSPRITE"] = new GraphicsDrawSpriteMethod(),
                ["GSETCOLOR"] = new GraphicsSetColorMethod(),
                ["GDRAWG"] = new GraphicsDrawGMethod(),
                ["GDRAWGWITHMASK"] = new GraphicsDrawGWithMaskMethod(),

                ["GSETBRUSH"] = new GraphicsSetBrushMethod(),
                ["GSETFONT"] = new GraphicsSetFontMethod(),
                ["GSETPEN"] = new GraphicsSetPenMethod(),

                ["SPRITECREATE"] = new SpriteCreateMethod(),
                ["SPRITEDISPOSE"] = new SpriteDisposeMethod(),

                ["CBGSETG"] = new CBGSetGraphicsMethod(),
                ["CBGSETSPRITE"] = new CBGSetCIMGMethod(),
                ["CBGCLEAR"] = new CBGClearMethod(),

                ["CBGCLEARBUTTON"] = new CBGClearButtonMethod(),
                ["CBGREMOVERANGE"] = new CBGRemoveRangeMethod(),
                ["CBGREMOVEBMAP"] = new CBGRemoveBMapMethod(),
                ["CBGSETBMAPG"] = new CBGSetBMapGMethod(),
                ["CBGSETBUTTONSPRITE"] = new CBGSETButtonSpriteMethod(),

                ["GSAVE"] = new GraphicsSaveMethod(),
                ["GLOAD"] = new GraphicsLoadMethod(),


                ["SPRITEANIMECREATE"] = new SpriteAnimeCreateMethod(),
                ["SPRITEANIMEADDFRAME"] = new SpriteAnimeAddFrameMethod(),
                ["SETANIMETIMER"] = new SetAnimeTimerMethod(),

                #region EM_私家版_追加関数
                ["HTML_STRINGLEN"] = new HtmlStringLenMethod(),
                ["HTML_SUBSTRING"] = new HtmlSubStringMethod(),

                ["EXISTFILE"] = new ExistFileMethod(),
                ["EXISTVAR"] = new ExistVarMethod(),
                ["ISDEFINED"] = new IsDefinedMethod(),

                ["ENUMFUNCBEGINSWITH"] = new EnumNameMethod(EnumNameMethod.EType.Function, EnumNameMethod.EAction.BeginsWith),
                ["ENUMFUNCENDSWITH"] = new EnumNameMethod(EnumNameMethod.EType.Function, EnumNameMethod.EAction.EndsWith),
                ["ENUMFUNCWITH"] = new EnumNameMethod(EnumNameMethod.EType.Function, EnumNameMethod.EAction.With),
                ["ENUMVARBEGINSWITH"] = new EnumNameMethod(EnumNameMethod.EType.Variable, EnumNameMethod.EAction.BeginsWith),
                ["ENUMVARENDSWITH"] = new EnumNameMethod(EnumNameMethod.EType.Variable, EnumNameMethod.EAction.EndsWith),
                ["ENUMVARWITH"] = new EnumNameMethod(EnumNameMethod.EType.Variable, EnumNameMethod.EAction.With),
                ["ENUMMACROBEGINSWITH"] = new EnumNameMethod(EnumNameMethod.EType.Macro, EnumNameMethod.EAction.BeginsWith),
                ["ENUMMACROENDSWITH"] = new EnumNameMethod(EnumNameMethod.EType.Macro, EnumNameMethod.EAction.EndsWith),
                ["ENUMMACROWITH"] = new EnumNameMethod(EnumNameMethod.EType.Macro, EnumNameMethod.EAction.With),
                ["ENUMFILES"] = new EnumFilesMethod(),

                ["GETVAR"] = new GetVarMethod(),
                ["GETVARS"] = new GetVarsMethod(),
                ["SETVAR"] = new SetVarMethod(),

                ["VARSETEX"] = new VarSetExMethod(),
                ["ARRAYMSORTEX"] = new ArrayMultiSortExMethod(),

                ["REGEXPMATCH"] = new RegexpMatchMethod(),

                ["XML_DOCUMENT"] = new XmlDocumentMethod(XmlDocumentMethod.Operation.Create),
                ["XML_RELEASE"] = new XmlDocumentMethod(XmlDocumentMethod.Operation.Release),
                ["XML_GET"] = new XmlGetMethod(),
                ["XML_GET_BYNAME"] = new XmlGetMethod(true),
                ["XML_SET"] = new XmlSetMethod(),
                ["XML_SET_BYNAME"] = new XmlSetMethod(true),
                ["XML_EXIST"] = new XmlDocumentMethod(XmlDocumentMethod.Operation.Check),
                ["XML_TOSTR"] = new XmlToStrMethod(),
                ["XML_ADDNODE"] = new XmlAddNodeMethod(XmlAddNodeMethod.Operation.Node),
                ["XML_ADDNODE_BYNAME"] = new XmlAddNodeMethod(XmlAddNodeMethod.Operation.Node, true),
                ["XML_REMOVENODE"] = new XmlRemoveNodeMethod(XmlRemoveNodeMethod.Operation.Node),
                ["XML_REMOVENODE_BYNAME"] = new XmlRemoveNodeMethod(XmlRemoveNodeMethod.Operation.Node, true),
                ["XML_REPLACE"] = new XmlReplaceMethod(),
                ["XML_REPLACE_BYNAME"] = new XmlReplaceMethod(true),
                ["XML_ADDATTRIBUTE"] = new XmlAddNodeMethod(XmlAddNodeMethod.Operation.Attribute),
                ["XML_ADDATTRIBUTE_BYNAME"] = new XmlAddNodeMethod(XmlAddNodeMethod.Operation.Attribute, true),
                ["XML_REMOVEATTRIBUTE"] = new XmlRemoveNodeMethod(XmlRemoveNodeMethod.Operation.Attribute),
                ["XML_REMOVEATTRIBUTE_BYNAME"] = new XmlRemoveNodeMethod(XmlRemoveNodeMethod.Operation.Attribute, true),

                ["MAP_CREATE"] = new MapManagementMethod(MapManagementMethod.Operation.Create),
                ["MAP_EXIST"] = new MapManagementMethod(MapManagementMethod.Operation.Check),
                ["MAP_RELEASE"] = new MapManagementMethod(MapManagementMethod.Operation.Release),

                ["MAP_GET"] = new MapGetStrMethod(MapGetStrMethod.Operation.Get),
                ["MAP_CLEAR"] = new MapDataOperationMethod(MapDataOperationMethod.Operation.Clear),
                ["MAP_SIZE"] = new MapDataOperationMethod(MapDataOperationMethod.Operation.Size),
                ["MAP_HAS"] = new MapDataOperationMethod(MapDataOperationMethod.Operation.Has),
                ["MAP_SET"] = new MapDataOperationMethod(MapDataOperationMethod.Operation.Set),
                ["MAP_REMOVE"] = new MapDataOperationMethod(MapDataOperationMethod.Operation.Remove),
                ["MAP_GETKEYS"] = new MapGetStrMethod(MapGetStrMethod.Operation.GetKeys),

                ["MAP_TOXML"] = new MapGetStrMethod(MapGetStrMethod.Operation.ToXml),
                ["MAP_FROMXML"] = new MapFromXmlMethod(),

                ["DT_CREATE"] = new DataTableManagementMethod(DataTableManagementMethod.Operation.Create),
                ["DT_EXIST"] = new DataTableManagementMethod(DataTableManagementMethod.Operation.Check),
                ["DT_RELEASE"] = new DataTableManagementMethod(DataTableManagementMethod.Operation.Release),
                ["DT_NOCASE"] = new DataTableManagementMethod(DataTableManagementMethod.Operation.Case),

                ["DT_CLEAR"] = new DataTableManagementMethod(DataTableManagementMethod.Operation.Clear),

                ["DT_COLUMN_ADD"] = new DataTableColumnManagementMethod(DataTableColumnManagementMethod.Operation.Create),
                ["DT_COLUMN_EXIST"] = new DataTableColumnManagementMethod(DataTableColumnManagementMethod.Operation.Check),
                ["DT_COLUMN_REMOVE"] = new DataTableColumnManagementMethod(DataTableColumnManagementMethod.Operation.Remove),
                ["DT_COLUMN_LENGTH"] = new DataTableLengthMethod(DataTableLengthMethod.Operation.Column),

                ["DT_ROW_ADD"] = new DataTableRowSetMethod(DataTableRowSetMethod.Operation.Add),
                ["DT_ROW_SET"] = new DataTableRowSetMethod(DataTableRowSetMethod.Operation.Set),
                ["DT_ROW_REMOVE"] = new DataTableRowRemoveMethod(),
                ["DT_ROW_LENGTH"] = new DataTableLengthMethod(DataTableLengthMethod.Operation.Row),

                ["DT_CELL_GET"] = new DataTableCellGetMethod(DataTableCellGetMethod.Operation.Get),
                ["DT_CELL_ISNULL"] = new DataTableCellGetMethod(DataTableCellGetMethod.Operation.IsNull),
                ["DT_CELL_GETS"] = new DataTableCellGetMethod(DataTableCellGetMethod.Operation.Gets),
                ["DT_CELL_SET"] = new DataTableCellSetMethod(),

                ["DT_SELECT"] = new DataTableSelectMethod(),
                #endregion

                #region EEで追加されたやつ
                ["EXISTSOUND"] = new ExistSoundMethod(),
                ["EXISTFUNCTION"] = new ExistFunctionMethod(),
                //["GROTATE"] = new GraphicsRotateMethod(),
                ["GDRAWGWITHROTATE"] = new GraphicsDrawGWithRotateMethod(),
                ["GDRAWTEXT"] = new GraphicsDrawStringMethod(),
                ["GGETFONT"] = new GraphicsStateStrMethod(),//("GGETFONT")
                ["GGETFONTSIZE"] = new GraphicsStateMethod(),//("GGETFONTSIZE")
                ["GGETFONTSTYLE"] = new GraphicsStateMethod(),
                ["GGETTEXTSIZE"] = new GraphicsGetTextSizeMethod(),
                //["GGETBRUSH"] = new GraphicsGetBrushMethod(),
                ["GETMEMORYUSAGE"] = new GetUsingMemoryMethod(),
                ["CLEARMEMORY"] = new ClearMemoryMethod(),
                ["GETTEXTBOX"] = new GetTextBoxMethod(),
                ["SETTEXTBOX"] = new ChangeTextBoxMethod(),
                ["ERDNAME"] = new ErdNameMethod(),
                #endregion
            };


            //1823 自分の関数名を知っていた方が何かと便利なので覚えさせることにした
            foreach (var pair in methodList)
				pair.Value.SetMethodName(pair.Key);
        }

		private static readonly Dictionary<string, FunctionMethod> methodList;
		public static Dictionary<string, FunctionMethod> GetMethodList()
		{
			return methodList;
		}
	}
}
