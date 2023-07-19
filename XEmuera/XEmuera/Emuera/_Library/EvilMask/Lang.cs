
using MinorShift.Emuera;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using XEmuera.Models;

namespace EvilMask.Emuera
{
    internal sealed class Lang
    {
        public sealed class TranslatableString
        {
            public TranslatableString(string text)
            {
                this.text = text;
                this.tr = null;
            }

            public void Clear()
            {
                tr = null;
            }

            public void Set(string tr)
            {
                this.tr = tr;
            }

            private string text;
            private string tr;

            public string Text { get { return tr == null ? text : tr; } }
        }

        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
        sealed class Managed : Attribute { }

        [AttributeUsage(AttributeTargets.Class)]
        sealed class Translate : Attribute 
        {
            public Translate(string text)
            {
                String = text;
            }
            public string String { get; private set; }
        }

        [Managed]
        public sealed class UI
        {
            [Managed]
            public sealed class MainWindow
            {
                [Translate("ファイル(&F)"), Managed]
                public sealed class File
                {
                    public static string Text { get { return trClass[typeof(File)].Text; } }
                    [Managed] public static TranslatableString Restart { get; } = new TranslatableString("再起動(&R)");
                    [Managed] public static TranslatableString SaveLog { get; } = new TranslatableString("ログを保存する...(&S)");
                    [Managed] public static TranslatableString CopyLogToClipboard { get; } = new TranslatableString("ログをクリップボードにコピー(&C)");
                    [Managed] public static TranslatableString BackToTitle { get; } = new TranslatableString("タイトル画面へ戻る(&T)");
                    [Managed] public static TranslatableString ReloadAllScripts { get; } = new TranslatableString("全コードを読み直す(&C)");
                    [Managed] public static TranslatableString ReloadFolder { get; } = new TranslatableString("フォルダを読み直す(&F)");
                    [Managed] public static TranslatableString ReloadScriptFile { get; } = new TranslatableString("ファイルを読み直す(&A)");
                    [Managed] public static TranslatableString Exit { get; } = new TranslatableString("終了(&X)");
                }

                [Translate("デバッグ(&D)"), Managed]
                public sealed class Debug
                {
                    public static string Text { get { return trClass[typeof(Debug)].Text; } }
                    [Managed] public static TranslatableString OpenDebugWindow { get; } = new TranslatableString("デバッグウインドウを開く");
                    [Managed] public static TranslatableString UpdateDebugInfo { get; } = new TranslatableString("デバッグ情報の更新");
                }

                [Translate("ヘルプ(&H)"), Managed]
                public sealed class Help
                {
                    public static string Text { get { return trClass[typeof(Help)].Text; } }
                    [Managed] public static TranslatableString Config { get; } = new TranslatableString("設定(&C)");
                }

                [Managed]
                public sealed class ContextMenu
                {
                    [Managed] public static TranslatableString KeyMacro { get; } = new TranslatableString("マクロ");

                    [Translate("マクログループ"), Managed]
                    public sealed class KeyMacroGroup
                    {
                        public static string Text { get { return trClass[typeof(KeyMacroGroup)].Text; } }
                        [Managed] public static TranslatableString Group { get; } = new TranslatableString("グループ");
                    }

                    [Managed] public static TranslatableString Cut { get; } = new TranslatableString("切り取り");
                    [Managed] public static TranslatableString Copy { get; } = new TranslatableString("コピー");
                    [Managed] public static TranslatableString Paste { get; } = new TranslatableString("貼り付け");
                    [Managed] public static TranslatableString Delete { get; } = new TranslatableString("削除");
                    [Managed] public static TranslatableString Execute { get; } = new TranslatableString("実行");
                }

                [Managed]
                public sealed class MsgBox
                {
                    [Managed] public static TranslatableString InstaceExists { get; } = new TranslatableString("既に起動しています");
                    [Managed] public static TranslatableString MultiInstanceInfo { get; } = new TranslatableString("多重起動を許可する場合、emuera.configを書き換えて下さい");
                    [Managed] public static TranslatableString FolderNotFound { get; } = new TranslatableString("フォルダなし");
                    [Managed] public static TranslatableString NoCsvFolder { get; } = new TranslatableString("csvフォルダが見つかりません");
                    [Managed] public static TranslatableString NoErbFolder { get; } = new TranslatableString("erbフォルダが見つかりません");
                    [Managed] public static TranslatableString FailedCreateDebugFolder { get; } = new TranslatableString("debugフォルダの作成に失敗しました");
                    [Managed] public static TranslatableString ArgPathNotExists { get; } = new TranslatableString("与えられたファイル・フォルダは存在しません");
                    [Managed] public static TranslatableString InvalidArg{ get; } = new TranslatableString("ドロップ可能なファイルはERBファイルのみです");
                }

                [Managed] public static TranslatableString FileFilter { get; } = new TranslatableString("ERBファイル");
            }

            [Translate("ClipBoardDialog"), Managed]
            public sealed class ClipBoardDialog
            {
                public static string Text { get { return trClass[typeof(ClipBoardDialog)].Text; } }
            }

            [Translate("ConfigDialog"), Managed]
            public sealed class ConfigDialog
            {
                public static string Text { get { return trClass[typeof(ConfigDialog)].Text; } }

                [Translate("環境"), Managed]
                public sealed class Environment
                {
                    public static string Text { get { return trClass[typeof(Environment)].Text; } }
                    [Managed] public static TranslatableString UseMouse { get; } = new TranslatableString("マウスを使用する");
                    [Managed] public static TranslatableString UseMenu { get; } = new TranslatableString("メニューを使用する");
                    [Managed] public static TranslatableString UseDebugCommand { get; } = new TranslatableString("デバッグコマンドを使用する");
                    [Managed] public static TranslatableString AllowMultipleInstances { get; } = new TranslatableString("多重起動を許可する");
                    [Managed] public static TranslatableString UseKeyMacro { get; } = new TranslatableString("キーボードマクロを使用する");
                    [Managed] public static TranslatableString AutoSave { get; } = new TranslatableString("オートセーブを行なう");
                    [Managed] public static TranslatableString UseSaveFolder { get; } = new TranslatableString("セーブデータをsavフォルダ内に作成する");
                    [Managed] public static TranslatableString EnglishConfigOutput { get; } = new TranslatableString("CONFIGファイルの内容を英語で保存する");
                    [Managed] public static TranslatableString MaxLog { get; } = new TranslatableString("履歴ログの行数");
                    [Managed] public static TranslatableString InfiniteLoopAlertTime { get; } = new TranslatableString("無限ループ警告までのミリ秒");
                    [Managed] public static TranslatableString SaveDataPerPage { get; } = new TranslatableString("使用するセーブデータ数");
                    [Managed] public static TranslatableString TextEditor { get; } = new TranslatableString("関連づけるテキストエディタ");
                    [Managed] public static TranslatableString Browse { get; } = new TranslatableString("選択");

                    [Translate("コマンドライン引数"), Managed]
                    public sealed class TextEditorCommandline
                    {
                        public static string Text { get { return trClass[typeof(TextEditorCommandline)].Text; } }
                        [Managed] public static TranslatableString UserSetting { get; } = new TranslatableString("ユーザー設定");
                    }
                }


                [Translate("表示"), Managed]
                public sealed class Display
                {
                    public static string Text { get { return trClass[typeof(Display)].Text; } }
                    [Managed] public static TranslatableString TextDrawingMode { get; } = new TranslatableString("描画インターフェース");
                    [Managed] public static TranslatableString FPS { get; } = new TranslatableString("フレーム毎秒");
                    [Managed] public static TranslatableString PrintCPerLine { get; } = new TranslatableString("PRINTCを並べる数");
                    [Managed] public static TranslatableString PrintCLength { get; } = new TranslatableString("PRINTCの文字数");
                    [Managed] public static TranslatableString ButtonWrap { get; } = new TranslatableString("ボタンの途中で行を折りかえさない");
                    [Managed] public static TranslatableString EmueraLang { get; } = new TranslatableString("Emuera表示言語");
                }

                [Translate("ウィンドウ"), Managed]
                public sealed class Window
                {
                    public static string Text { get { return trClass[typeof(Window)].Text; } }
                    [Managed] public static TranslatableString WindowWidth { get; } = new TranslatableString("ウィンドウ幅");
                    [Managed] public static TranslatableString WindowHeight { get; } = new TranslatableString("ウィンドウ高さ");
                    [Managed] public static TranslatableString GetWindowSize { get; } = new TranslatableString("現在のウィンドウサイズを取得");
                    [Managed] public static TranslatableString ChangeableWindowHeight { get; } = new TranslatableString("ウィンドウの高さを可変にする");
                    [Managed] public static TranslatableString WindowMaximixed { get; } = new TranslatableString("起動時にウィンドウを最大化する");
                    [Managed] public static TranslatableString SetWindowPos { get; } = new TranslatableString("起動時のウィンドウの位置を固定する");
                    [Managed] public static TranslatableString WindowX { get; } = new TranslatableString("ウィンドウ位置X");
                    [Managed] public static TranslatableString WindowY { get; } = new TranslatableString("ウィンドウ位置Y");
                    [Managed] public static TranslatableString GetWindowPos { get; } = new TranslatableString("現在のウィンドウ位置を取得");
                    [Managed] public static TranslatableString LinesPerScroll { get; } = new TranslatableString("スクロールの行数");
                }

                [Translate("フォント"), Managed]
                public sealed class Font
                {
                    public static string Text { get { return trClass[typeof(Font)].Text; } }
                    [Managed] public static TranslatableString BackgroundColor { get; } = new TranslatableString("背景色");
                    [Managed] public static TranslatableString TextColor { get; } = new TranslatableString("文字色");
                    [Managed] public static TranslatableString HighlightColor { get; } = new TranslatableString("選択中文字色");
                    [Managed] public static TranslatableString LogHistoryColor { get; } = new TranslatableString("履歴文字色");
                    [Managed] public static TranslatableString FontName { get; } = new TranslatableString("フォント名");
                    [Managed] public static TranslatableString GetFontNames { get; } = new TranslatableString("フォント名一覧を取得");
                    [Managed] public static TranslatableString FontSize { get; } = new TranslatableString("フォントサイズ");
                    [Managed] public static TranslatableString LineHeight { get; } = new TranslatableString("一行の高さ");
                }

                [Translate("システム"), Managed]
                public sealed class System
                {
                    public static string Text { get { return trClass[typeof(System)].Text; } }
                    [Managed] public static TranslatableString Warning { get; } = new TranslatableString("※システムの項目を変化させた場合、\nERBスクリプトが正常に動作しないことがあります");
                    [Managed] public static TranslatableString IgnoreCase { get; } = new TranslatableString("大文字小文字の違いを無視する");
                    [Managed] public static TranslatableString UseRename { get; } = new TranslatableString("_Rename.csvを利用する");
                    [Managed] public static TranslatableString UseReplace { get; } = new TranslatableString("_Replace.csvを利用する");
                    [Managed] public static TranslatableString SearchSubfolder { get; } = new TranslatableString("サブディレクトリを検索する");
                    [Managed] public static TranslatableString SortFileNames { get; } = new TranslatableString("読み込み順をファイル名順にソートする");
                    [Managed] public static TranslatableString SystemFuncOverride { get; } = new TranslatableString("システム関数の上書きを許可する");
                    [Managed] public static TranslatableString SystemFuncOverrideWarn { get; } = new TranslatableString("システム関数が上書きされたとき警告を表示する");
                    [Managed] public static TranslatableString DuplicateFuncWarn { get; } = new TranslatableString("同名の非イベント関数が複数定義されたとき警告する");
                    [Managed] public static TranslatableString WSIncludesFullWidth { get; } = new TranslatableString("全角スペースをホワイトスペースに含める");
                    [Managed] public static TranslatableString ANSI { get; } = new TranslatableString("内部で使用する東アジア言語");
                }

                [Translate("システム2"), Managed]
                public sealed class System2
                {
                    public static string Text { get { return trClass[typeof(System2)].Text; } }
                    [Managed] public static TranslatableString IgnoreTripleSymbol { get; } = new TranslatableString("FORM中の三連記号を展開しない");
                    [Managed] public static TranslatableString SaveInBinary { get; } = new TranslatableString("セーブデータをバイナリ形式で保存する");
                    [Managed] public static TranslatableString SaveInUTF8 { get; } = new TranslatableString("セーブデータをUTF-8で保存する(非バイナリ時のみ)");
                    [Managed] public static TranslatableString CompressSave { get; } = new TranslatableString("セーブデータを圧縮して保存する(バイナリ時のみ)");
                    [Managed] public static TranslatableString NoAutoCompleteCVar { get; } = new TranslatableString("キャラクタ変数の引数を補完しない");
                    [Managed] public static TranslatableString DisallowUpdateCheck { get; } = new TranslatableString("UPDATECHECKを許可しない");
                    [Managed] public static TranslatableString UseERD { get; } = new TranslatableString("ERD機能を利用する");
                    [Managed] public static TranslatableString VarsizeDimConfig { get; } = new TranslatableString("VARSIZEの次元指定をERD機能に合わせる");
                    [Managed] public static TranslatableString SaveLoadExt { get; } = new TranslatableString("LOADTEXTとSAVETEXTで使える拡張子");
                }

                [Translate("互換性"), Managed]
                public sealed class Compatibility
                {
                    public static string Text { get { return trClass[typeof(Compatibility)].Text; } }
                    [Managed] public static TranslatableString Warning { get; } = new TranslatableString("※eramakerとEmueraで動作が違う、\nEmueraの過去のバージョンで動作したものが動作しない、\nなどの問題を解決するためのオプションです\n標準で問題ない場合は変更しないでください");
                    [Managed] public static TranslatableString ExecuteErrorLine { get; } = new TranslatableString("解釈不可能な行があっても実行する");
                    [Managed] public static TranslatableString NameForCallname { get; } = new TranslatableString("CALLNAMEが空文字列の時にNAMEを代入する");
                    [Managed] public static TranslatableString EramakerRAND { get; } = new TranslatableString("擬似変数RANDの仕様をeramakerに合わせる");
                    [Managed] public static TranslatableString EramakerTIMES { get; } = new TranslatableString("TIMESの計算をeramakerにあわせる");
                    [Managed] public static TranslatableString NoIgnoreCase { get; } = new TranslatableString("関数・属性については大文字小文字を無視しない");
                    [Managed] public static TranslatableString CallEvent { get; } = new TranslatableString("イベント関数のCALLを許可する");
                    [Managed] public static TranslatableString UseSPCharacters { get; } = new TranslatableString("SPキャラを使用する");
                    [Managed] public static TranslatableString ButtonWarp { get; } = new TranslatableString("ver1739以前の非ボタン折り返しを再現する");
                    [Managed] public static TranslatableString OmitArgs { get; } = new TranslatableString("ユーザー関数の全ての引数の省略を許可する");
                    [Managed] public static TranslatableString AutoTOSTR { get; } = new TranslatableString("ユーザー関数の引数に自動的にTOSTRを補完する");
                    [Managed] public static TranslatableString EramakerStandard { get; } = new TranslatableString("eramakerの仕様にする");
                    [Managed] public static TranslatableString EmueraStandard { get; } = new TranslatableString("Emuera標準仕様にする");
                }

                [Translate("解析"), Managed]
                public sealed class Debug
                {
                    public static string Text { get { return trClass[typeof(Debug)].Text; } }
                    [Managed] public static TranslatableString CompatibilityWarn { get; } = new TranslatableString("eramaker互換性に関する警告を表示する");
                    [Managed] public static TranslatableString LoadingReport { get; } = new TranslatableString("ロード時にレポートを表示する");

                    [Translate("ロード時に引数を解析する"), Managed]
                    public sealed class ReduceArgs
                    {
                        public static string Text { get { return trClass[typeof(ReduceArgs)].Text; } }
                        [Managed] public static TranslatableString Never { get; } = new TranslatableString("常に行わない");
                        [Managed] public static TranslatableString OnUpdate { get; } = new TranslatableString("更新されていれば行う");
                        [Managed] public static TranslatableString Always { get; } = new TranslatableString("常に行う");
                    }

                    [Translate("表示する最低警告レベル"), Managed]
                    public sealed class WarnLevel
                    {
                        public static string Text { get { return trClass[typeof(WarnLevel)].Text; } }
                        [Managed] public static TranslatableString Level0 { get; } = new TranslatableString("0:標準でない文法");
                        [Managed] public static TranslatableString Level1 { get; } = new TranslatableString("1:無視可能なエラー");
                        [Managed] public static TranslatableString Level2 { get; } = new TranslatableString("2:動作しないエラー");
                        [Managed] public static TranslatableString Level3 { get; } = new TranslatableString("3:致命的エラー");
                    }

                    [Managed] public static TranslatableString IgnoreUnusedFuncs { get; } = new TranslatableString("呼び出されなかった関数を無視する");

                    [Managed]
                    public sealed class WarnSetting
                    {
                        [Managed] public static TranslatableString Ignore { get; } = new TranslatableString("無視");
                        [Managed] public static TranslatableString TotalNumber { get; } = new TranslatableString("総数のみ表示する");
                        [Managed] public static TranslatableString OncePerFile { get; } = new TranslatableString("ファイル毎に一度だけ表示する");
                        [Managed] public static TranslatableString Always { get; } = new TranslatableString("表示する");
                    }

                    [Managed] public static TranslatableString FuncNotFoundWarn { get; } = new TranslatableString("関数が見つからない警告の扱い");
                    [Managed] public static TranslatableString UnusedFuncWarn { get; } = new TranslatableString("関数が呼び出されなかった警告の扱い");
                    [Managed] public static TranslatableString PlayerStandard { get; } = new TranslatableString("ユーザー向けの設定にする");
                    [Managed] public static TranslatableString DeveloperStandard { get; } = new TranslatableString("開発者向けの設定にする");
                }

                [Managed] public static TranslatableString ChangeWontTakeEffectUntilRestart { get; } = new TranslatableString("※変更は再起動するまで反映されません");
                [Managed] public static TranslatableString Save { get; } = new TranslatableString("保存");
                [Managed] public static TranslatableString SaveAndRestart { get; } = new TranslatableString("保存して再起動");
                [Managed] public static TranslatableString Cancel { get; } = new TranslatableString("キャンセル");
            }


            [Translate("ConfigDialog"), Managed]
            public sealed class DebugConfigDialog
            {
                public static string Text { get { return trClass[typeof(DebugConfigDialog)].Text; } }
                [Managed] public static TranslatableString Name { get; } = new TranslatableString("デバッグ");
                [Managed] public static TranslatableString Warning { get; } = new TranslatableString("※デバッグ関連のオプションはコマンドライン引数に-Debug\nを指定して起動した時のみ有効です");
                [Managed] public static TranslatableString OpenDebugWindowOnStartup { get; } = new TranslatableString("起動時にデバッグウインドウを表示する");
                [Managed] public static TranslatableString AlwaysOnTop { get; } = new TranslatableString("デバッグウインドウを最前面に表示する");
                [Managed] public static TranslatableString WindowWidth { get; } = new TranslatableString("デバッグウィンドウ幅");
                [Managed] public static TranslatableString WindowHeight { get; } = new TranslatableString("デバッグウィンドウ高さ");
                // [Managed] public static TranslatableString GetWindowSize { get; } = new TranslatableString("現在のウィンドウサイズを取得");
                // Lang.UI.ConfigDialog.Window.GetWindowSize
                [Managed] public static TranslatableString SetWindowPos { get; } = new TranslatableString("デバッグウィンドウ位置を指定する");
                [Managed] public static TranslatableString WindowX { get; } = new TranslatableString("デバッグウィンドウ位置X");
                [Managed] public static TranslatableString WindowY { get; } = new TranslatableString("デバッグウィンドウ位置Y");
                // [Managed] public static TranslatableString Warning { get; } = new TranslatableString("現在のウィンドウ位置を取得");
                // Lang.UI.ConfigDialog.Window.GetWindowPos
            }

            [Translate("Emuera - デバッグウインドウ"), Managed]
            public sealed class DebugDialog
            {
                public static string Text { get { return trClass[typeof(DebugDialog)].Text; } }

                //[Translate("ファイル(&F)"), Managed]
                [Managed]
                public sealed class File
                {
                    // public static string Text { get { return trClass[typeof(File)].Text; } }
                    // use Lang.UI.MainWindow.File.Text
                    [Managed] public static TranslatableString SaveWatchList { get; } = new TranslatableString("ウォッチリストの保存");
                    [Managed] public static TranslatableString LoadWatchList { get; } = new TranslatableString("ウォッチリストの読込");
                    // Managed] public static TranslatableString Close { get; } = new TranslatableString("閉じる");
                    // use Lang.UI.DebugDialog.Close.Text
                }
                [Translate("設定(&C)"), Managed]
                public sealed class Setting
                {
                    public static string Text { get { return trClass[typeof(Setting)].Text; } }
                    [Managed] public static TranslatableString Config { get; } = new TranslatableString("コンフィグ(&C)");
                }
                [Translate("変数ウォッチ"), Managed]
                public sealed class VariableWatch
                {
                    public static string Text { get { return trClass[typeof(VariableWatch)].Text; } }
                    [Managed] public static TranslatableString Object { get; } = new TranslatableString("対象");
                    [Managed] public static TranslatableString Value { get; } = new TranslatableString("値");
                }
                [Managed] public static TranslatableString StackTrace { get; } = new TranslatableString("スタックトレース");
                [Managed] public static TranslatableString Console { get; } = new TranslatableString("コンソール");
                [Managed] public static TranslatableString StayOnTop { get; } = new TranslatableString("最前面に表示");
                [Managed] public static TranslatableString UpdateData { get; } = new TranslatableString("データ更新");
                [Managed] public static TranslatableString Close { get; } = new TranslatableString("閉じる");
            }
        }

        [Managed]
        public sealed class Error
        {
            [Managed] public static TranslatableString WarnPrefix { get; } = new TranslatableString("注意: {0}の{1}行目: {2}");
            [Managed] public static TranslatableString FuncPrefix { get; } = new TranslatableString("{0}関数: {1}");

            [Managed] public static TranslatableString NotExistColorSpecifier { get; } = new TranslatableString("値をColor指定子として認識できません");
            [Managed] public static TranslatableString ContainsNonNumericCharacters { get; } = new TranslatableString("数字でない文字が含まれています");
            [Managed] public static TranslatableString InvalidSpecification { get; } = new TranslatableString("不正な指定です");
            [Managed] public static TranslatableString DoesNotMatchCdflagElements { get; } = new TranslatableString("CDFLAGの要素数とCDFLAGNAME1及びCDFLAGNAME2の要素数が一致していません");
            [Managed] public static TranslatableString TooManyCdflagElements { get; } = new TranslatableString("CDFLAGの要素数が多すぎます（CDFLAGNAME1とCDFLAGNAME2の要素数の積が100万を超えています）");
            [Managed] public static TranslatableString DuplicateErdKey { get; } = new TranslatableString("変数\"{0}\"の置き換え名前\"{1}\"の定義が重複しています。（ファイル1 - {2}）（ファイル2 - {3}）");
            [Managed] public static TranslatableString DuplicateVariableDefine { get; } = new TranslatableString("変数{0}の定義が重複しています。");
            [Managed] public static TranslatableString NotDefinedErdKey { get; } = new TranslatableString("変数\"{0}\"には\"{1}\"の定義がありません");
            [Managed] public static TranslatableString KeywordsCannotBeEmpty { get; } = new TranslatableString("キーワードを空には出来ません");
            [Managed] public static TranslatableString InvalidProhibitedVar { get; } = new TranslatableString("CanForbidでない変数\"{0}\"にIsForbidがついている");
            [Managed] public static TranslatableString CanNotSpecifiedByString { get; } = new TranslatableString("配列変数\"{0}\"の要素を文字列で指定することはできません");
            [Managed] public static TranslatableString NotDefinedKey { get; } = new TranslatableString("\"{0}\"の中に\"{1}\"の定義がありません");
            [Managed] public static TranslatableString CannotIndexSpecifiedByString { get; } = new TranslatableString("配列変数\"{0}\"の{1}番目の要素を文字列で指定することはできません");
            [Managed] public static TranslatableString UseCdflagname { get; } = new TranslatableString("CDFLAGの要素の取得にはCDFLAGNAME1又はCDFLAGNAME2を使用します");
            [Managed] public static TranslatableString NotExistKey { get; } = new TranslatableString("存在しないキーを参照しました");
            [Managed] public static TranslatableString UsedAtForPrivVar { get; } = new TranslatableString("プライベート変数\"{0}\"に対して@が使われました");
            [Managed] public static TranslatableString UsedProhibitedVar { get; } = new TranslatableString("呼び出された変数\"{0}\"は設定により使用が禁止されています");
            [Managed] public static TranslatableString CannotGetKeyNotExistRunningFunction { get; } = new TranslatableString("実行中の関数が存在しないため\"{0}\"を取得又は変更できませんでした");
            [Managed] public static TranslatableString UsedAtForGlobalVar { get; } = new TranslatableString("ローカル変数でない変数{0}に対して@が使われました");
            [Managed] public static TranslatableString InvalidAt { get; } = new TranslatableString("@の使い方が不正です");
            [Managed] public static TranslatableString CallfNonMethodFunc { get; } = new TranslatableString("#FUNCTIONが指定されていない関数\"@{0}\"をCALLF系命令で呼び出そうとしました");
            [Managed] public static TranslatableString UsedNonMethodFunc { get; } = new TranslatableString("#FUNCTIONが定義されていない関数({0}:{1}行目)を式中で呼び出そうとしました");
            [Managed] public static TranslatableString DeclaringDisable { get; } = new TranslatableString("\"{0}\"は#DISABLEが宣言されています");
            [Managed] public static TranslatableString VarNotDefinedThisFunc { get; } = new TranslatableString("変数\"{0}\"はこの関数中では定義されていません");
            [Managed] public static TranslatableString IllegalUseReservedWord { get; } = new TranslatableString("Emueraの予約語\"{0}\"が不正な使われ方をしています");
            [Managed] public static TranslatableString UseVarLikeFunc { get; } = new TranslatableString("変数名\"{0}\"が関数のように使われています");
            [Managed] public static TranslatableString UseFuncLikeVar { get; } = new TranslatableString("関数名\"{0}\"が変数のように使われています");
            [Managed] public static TranslatableString UnexpectedMacro { get; } = new TranslatableString("予期しないマクロ名\"{0}\"です");
            [Managed] public static TranslatableString UseInstructionLikeFunc { get; } = new TranslatableString("命令名\"{0}\"が関数のように使われています");
            [Managed] public static TranslatableString UseInstructionLikeVar { get; } = new TranslatableString("命令名\"{0}\"が変数のように使われています");
            [Managed] public static TranslatableString CanNotInterpreted { get; } = new TranslatableString("\"{0}\"は解釈できない識別子です");
            [Managed] public static TranslatableString AbnormalFirstOperand { get; } = new TranslatableString("三項演算子\\@の第一オペランドが異常です");
            [Managed] public static TranslatableString EmptyBrace { get; } = new TranslatableString("{{}}の中に式が存在しません");
            [Managed] public static TranslatableString EmptyPer { get; } = new TranslatableString("%%の中に式が存在しません");
            [Managed] public static TranslatableString NotSpecifiedLR { get; } = new TranslatableString("','の後にRIGHT又はLEFTがありません");
            [Managed] public static TranslatableString OtherThanLR { get; } = new TranslatableString("','の後にRIGHT又はLEFT以外の単語があります");
            [Managed] public static TranslatableString ExtraCharacterLR { get; } = new TranslatableString("RIGHT又はLEFTの後に余分な文字があります");
            [Managed] public static TranslatableString IsNotNumericBrace { get; } = new TranslatableString("{{}}の中の式が数式ではありません");
            [Managed] public static TranslatableString IsNotStringPer { get; } = new TranslatableString("%%の中の式が文字列式ではありません");
            [Managed] public static TranslatableString OoRForcekanaArg { get; } = new TranslatableString("命令FORCEKANAの引数が指定可能な範囲(0～3)を超えています");
            [Managed] public static TranslatableString MaxBarNotPositive { get; } = new TranslatableString("BARの最大値が正の値ではありません");
            [Managed] public static TranslatableString BarNotPositive { get; } = new TranslatableString("BARの長さが正の値ではありません");
            [Managed] public static TranslatableString TooLongBar { get; } = new TranslatableString("BARが長すぎます");
            [Managed] public static TranslatableString NotCloseSBrackets { get; } = new TranslatableString("'['に対応する']'が見つかりません");
            [Managed] public static TranslatableString NotCloseBrackets { get; } = new TranslatableString("'('に対応する')'が見つかりません");
            [Managed] public static TranslatableString UnexpectedBrackets { get; } = new TranslatableString("構文解析中に予期しない')'を発見しました");
            [Managed] public static TranslatableString UnexpectedSBrackets { get; } = new TranslatableString("構文解析中に予期しない']'を発見しました");
            [Managed] public static TranslatableString CannotOmitFuncArg { get; } = new TranslatableString("関数定義の引数は省略できません");
            [Managed] public static TranslatableString NoExpressionAfterEqual { get; } = new TranslatableString("'='の後に式がありません");
            [Managed] public static TranslatableString DoesNotMatchEqual { get; } = new TranslatableString("'='の前後で型が一致しません");
            [Managed] public static TranslatableString CanNotInterpretedExpression { get; } = new TranslatableString("構文を式として解釈できません");
            [Managed] public static TranslatableString ExpressionResultIsNotNumeric { get; } = new TranslatableString("式の結果が数値ではありません");
            [Managed] public static TranslatableString EmptyStream { get; } = new TranslatableString("空のストリームを渡された");
            [Managed] public static TranslatableString SBracketsFuncNotImprement { get; } = new TranslatableString("[]を使った機能はまだ実装されていません");
            [Managed] public static TranslatableString ThrowFailed { get; } = new TranslatableString("エラー投げ損ねた");
            [Managed] public static TranslatableString NoOpAfterIs { get; } = new TranslatableString("ISキーワードの後に演算子がありません");
            [Managed] public static TranslatableString NotBinaryOpAfterThis { get; } = new TranslatableString("ISキーワードの後の演算子が2項演算子ではありません");
            [Managed] public static TranslatableString NothingAfterIs { get; } = new TranslatableString("ISキーワードの後に式がありません");
            [Managed] public static TranslatableString CanNotOmitCaseArg { get; } = new TranslatableString("CASEの引数は省略できません");
            [Managed] public static TranslatableString NoExpressionAfterTo { get; } = new TranslatableString("TOキーワードの後に式がありません");
            [Managed] public static TranslatableString DuplicateTo { get; } = new TranslatableString("TOキーワードが2度使われています");
            [Managed] public static TranslatableString DoesNotMatchTo { get; } = new TranslatableString("TOキーワードの前後の型が一致していません");
            [Managed] public static TranslatableString InvalidTo { get; } = new TranslatableString("TOキーワードはここでは使用できません");
            [Managed] public static TranslatableString InvalidIs { get; } = new TranslatableString("ISキーワードはここでは使用できません");
            [Managed] public static TranslatableString UnexpectedOpInVarArg { get; } = new TranslatableString("変数の引数の読み取り中に予期しない演算子を発見しました");
            [Managed] public static TranslatableString EqualInExpression { get; } = new TranslatableString("式中で代入演算子'='が使われています(等価比較には'=='を使用してください)");
            [Managed] public static TranslatableString ComparisonOpContinuous { get; } = new TranslatableString("（構文上の注意）比較演算子が連続しています。");
            [Managed] public static TranslatableString MissingQuestion { get; } = new TranslatableString("対応する'?'のない'#'です");
            [Managed] public static TranslatableString NoContainExpressionInBrackets { get; } = new TranslatableString("かっこ\"(\"～\")\"の中に式が含まれていません");
            [Managed] public static TranslatableString UnexpectedSymbol { get; } = new TranslatableString("構文解釈中に予期しない記号\"{0}\"を発見しました");
            [Managed] public static TranslatableString TernaryBinaryError { get; } = new TranslatableString("'?'と'#'の数が正しく対応していません");
            [Managed] public static TranslatableString FailedSolveMacro { get; } = new TranslatableString("マクロ解決失敗");
            [Managed] public static TranslatableString UnrecognizedSyntax { get; } = new TranslatableString("式が異常です");
            [Managed] public static TranslatableString MultipleUnaryOp { get; } = new TranslatableString("後置の単項演算子が複数存在しています");
            [Managed] public static TranslatableString DuplicateIncrementDecrement { get; } = new TranslatableString("インクリメント・デクリメントを前置・後置両方同時に使うことはできません");
            [Managed] public static TranslatableString InsufficientExpression { get; } = new TranslatableString("式の数が不足しています");
            [Managed] public static TranslatableString EmptyFramelist { get; } = new TranslatableString("totaltime > 0なのにFrameListが空");
            [Managed] public static TranslatableString OoRLasframe { get; } = new TranslatableString("SpriteAnime:最終フレームが範囲外");
            [Managed] public static TranslatableString SpriteTimeOut { get; } = new TranslatableString("SpriteAnime:時間外参照");
            [Managed] public static TranslatableString IncrementNonVar { get; } = new TranslatableString("変数以外をインクリメントすることはできません");
            [Managed] public static TranslatableString IncrementConst { get; } = new TranslatableString("変更できない変数をインクリメントすることはできません");

            [Managed] public static TranslatableString NumericType { get; } = new TranslatableString("数値型");
            [Managed] public static TranslatableString StringType { get; } = new TranslatableString("文字列型");
            [Managed] public static TranslatableString UnknownType { get; } = new TranslatableString("不定型");
            [Managed] public static TranslatableString RefType { get; } = new TranslatableString("参照型");
            [Managed] public static TranslatableString MultidimType { get; } = new TranslatableString("多次元型");
            [Managed] public static TranslatableString CharaType { get; } = new TranslatableString("キャラクター型");
            [Managed] public static TranslatableString CanNotAppliedUnaryOp { get; } = new TranslatableString("{0}に単項演算子\"{1}\"は適用できません");
            [Managed] public static TranslatableString CanNotAppliedBinaryOp { get; } = new TranslatableString("{0}と{1}の演算に二項演算子\"{2}\"は適用できません");

            [Managed] public static TranslatableString InvalidTernaryOp { get; } = new TranslatableString("三項演算子の使用法が不正です");
            [Managed] public static TranslatableString MultiplyNegativeToStr { get; } = new TranslatableString("文字列に負の値({0})を乗算しようとしました");
            [Managed] public static TranslatableString Multiply10kToStr { get; } = new TranslatableString("文字列に10000以上の値({0})を乗算しようとしました");
            [Managed] public static TranslatableString DivideByZero { get; } = new TranslatableString("0による除算が行なわれました");
            [Managed] public static TranslatableString XmlGetError { get; } = new TranslatableString("XML_GET関数:\"{0}\"の解析エラー:{1}");
            [Managed] public static TranslatableString XmlGetPathError { get; } = new TranslatableString("XML_GET関数:XPath\"{0}\"の解析エラー:{1}");
            [Managed] public static TranslatableString FirstArg { get; } = new TranslatableString("1番目の引数");
            [Managed] public static TranslatableString NotVarFunc { get; } = new TranslatableString("{0}関数:{1}が変数ではありません");
            [Managed] public static TranslatableString IsCharaVarFunc { get; } = new TranslatableString("{0}関数:{1}がキャラクタ変数です");
            [Managed] public static TranslatableString Not1DFuncArg { get; } = new TranslatableString("{0}関数:{1}番目の引数が一次元文字列配列ではありません");
            [Managed] public static TranslatableString NotDimVarFunc { get; } = new TranslatableString("{0}関数:{1}が配列変数ではありません");
            [Managed] public static TranslatableString AbnormalArray { get; } = new TranslatableString("異常な配列");
            [Managed] public static TranslatableString SetStrToInt { get; } = new TranslatableString("文字列型でない変数\"{0}\"に文字列型を代入しようとしました");
            [Managed] public static TranslatableString SetIntToStr { get; } = new TranslatableString("整数型でない変数\"{0}\"に整数値を代入しようとしました");
            [Managed] public static TranslatableString InvalidRegexArg { get; } = new TranslatableString("{0}関数: 第{1}引数が正規表現として不正です: {2}");
            [Managed] public static TranslatableString XmlParseError { get; } = new TranslatableString("{0}関数:\"{1}\"の解析エラー:{2}");
            [Managed] public static TranslatableString XmlXPathParseError { get; } = new TranslatableString("{0}関数:XPath\"{1}\"の解析エラー:{2}");
            [Managed] public static TranslatableString ReturnTypeDifferentOrNotImpelemnt { get; } = new TranslatableString("戻り値の型が違う or 未実装");
            [Managed] public static TranslatableString NotImplement { get; } = new TranslatableString("未実装");
            [Managed] public static TranslatableString EmptyRefFunc { get; } = new TranslatableString("何も参照していない関数参照\"{0}\"を呼び出しました");
            [Managed] public static TranslatableString RefFuncHasNotArg { get; } = new TranslatableString("引数のない関数参照\"{0}\"を呼び出しました");
            [Managed] public static TranslatableString AbnormalData { get; } = new TranslatableString("データ異常");
            [Managed] public static TranslatableString OoRSortKey { get; } = new TranslatableString("ソートキーが配列外を参照しています");
            [Managed] public static TranslatableString AbnormalVarDeclaration { get; } = new TranslatableString("異常な変数宣言");
            [Managed] public static TranslatableString AssignToConst { get; } = new TranslatableString("読み取り専用の変数\"{0}\"に代入しようとしました");
            [Managed] public static TranslatableString OoRCharaVar { get; } = new TranslatableString("キャラクタ配列変数\"{0}\"の第{1}引数({2})は配列の範囲外です");
            [Managed] public static TranslatableString OoRArrayShift { get; } = new TranslatableString("命令ARRAYSHIFTの第4引数({0})が配列\"{1}\"の範囲を超えています");
            [Managed] public static TranslatableString OoRArrayRemove { get; } = new TranslatableString("命令ARRAYREMOVEの第2引数({0})が配列\"{1}\"の範囲を超えています");
            [Managed] public static TranslatableString OoRArraySort { get; } = new TranslatableString("命令ARRAYSORTの第3引数({0})が配列\"{1}\"の範囲を超えています");
            [Managed] public static TranslatableString OoRCharaNum { get; } = new TranslatableString("存在しない登録キャラクタを参照しようとしました");
            [Managed] public static TranslatableString AddedUndefinedChara { get; } = new TranslatableString("定義していないキャラクタを作成しようとしました");
            [Managed] public static TranslatableString OoRDelChara { get; } = new TranslatableString("存在しない登録キャラクタ({0})を削除しようとしました");
            [Managed] public static TranslatableString DuplicateDelChara { get; } = new TranslatableString("同一の登録キャラクタ番号({0})が複数回指定されました");
            [Managed] public static TranslatableString NotExistFromCopyChara { get; } = new TranslatableString("コピー元のキャラクタが存在しません");
            [Managed] public static TranslatableString NotExistToCopyChara { get; } = new TranslatableString("コピー先のキャラクタが存在しません");
            [Managed] public static TranslatableString OoRSwapChara { get; } = new TranslatableString("存在しない登録キャラクタを入れ替えようとしました");
            [Managed] public static TranslatableString RefUndefinedChara { get; } = new TranslatableString("定義していないキャラクタを参照しようとしました");
            [Managed] public static TranslatableString OoRCstr { get; } = new TranslatableString("CSTRの参照可能範囲外を参照しました");
            [Managed] public static TranslatableString RefDoesNotExistData { get; } = new TranslatableString("存在しないデータを参照しようとしました");
            [Managed] public static TranslatableString RefOoR { get; } = new TranslatableString("参照可能範囲外を参照しました");
            [Managed] public static TranslatableString FailedCreateDataFolder { get; } = new TranslatableString("datフォルダーの作成に失敗しました");
            [Managed] public static TranslatableString NothingFileName { get; } = new TranslatableString("ファイル名が指定されていません");
            [Managed] public static TranslatableString InvalidFileName { get; } = new TranslatableString("ファイル名に不正な文字が含まれています");
            [Managed] public static TranslatableString DifferentGame { get; } = new TranslatableString("異なるゲームのセーブデータです");
            [Managed] public static TranslatableString DifferentVersion { get; } = new TranslatableString("セーブデータのバーションが異なります");
            [Managed] public static TranslatableString CorruptedSaveData { get; } = new TranslatableString("セーブデータが壊れています");
            [Managed] public static TranslatableString LoadError { get; } = new TranslatableString("読み込み中にエラーが発生しました");
            [Managed] public static TranslatableString ErrorSavingGlobalData { get; } = new TranslatableString("グローバルデータの保存中にエラーが発生しました");
            [Managed] public static TranslatableString NotExistPath { get; } = new TranslatableString("存在しないパスを指定しました");
            [Managed] public static TranslatableString DelReadOnlyFile { get; } = new TranslatableString("指定されたファイル\"{0}\"は読み込み専用のため削除できません");
            [Managed] public static TranslatableString TooMany2DCharaVarArg { get; } = new TranslatableString("キャラクタ二次元配列変数\"{0}\"の引数が多すぎます");
            [Managed] public static TranslatableString TooMany1DCharaVarArg { get; } = new TranslatableString("キャラクタ配列変数\"{0}\"の引数が多すぎます");
            [Managed] public static TranslatableString TooManyCharaVarArg { get; } = new TranslatableString("キャラクタ変数\"{0}\"の引数が多すぎます");
            //[Managed] public static TranslatableString CanNotOmit2DCharaVarArg { get; } = new TranslatableString("キャラクタ二次元配列変数\"{0}\"の引数は省略できません");
            [Managed] public static TranslatableString CanNotOmit1DCharaVarArg1 { get; } = new TranslatableString("キャラクタ配列変数\"{0}\"の引数は省略できません");
            [Managed] public static TranslatableString CanNotOmit1DCharaVarArg2 { get; } = new TranslatableString("キャラクタ配列変数\"{0}\"の引数は省略できません(コンフィグにより禁止が選択されています)");
            [Managed] public static TranslatableString CanNotOmitCharaVarArg1 { get; } = new TranslatableString("キャラクタ変数\"{0}\"の引数は省略できません");
            [Managed] public static TranslatableString CanNotOmitCharaVarArg2 { get; } = new TranslatableString("キャラクタ変数\"{0}\"の引数は省略できません(コンフィグにより禁止が選択されています)");
            [Managed] public static TranslatableString CanNotOmit3DVarArg { get; } = new TranslatableString("三次元配列変数\"{0}\"の引数は省略できません");
            [Managed] public static TranslatableString CanNotOmit2DVarArg { get; } = new TranslatableString("二次元配列変数\"{0}\"の引数は省略できません");
            [Managed] public static TranslatableString TooMany2DVarArg { get; } = new TranslatableString("二次元配列変数\"{0}\"の引数が多すぎます");
            [Managed] public static TranslatableString TooMany1DVarArg { get; } = new TranslatableString("一次元変数\"{0}\"の引数が多すぎます");
            [Managed] public static TranslatableString OmittedRandArg { get; } = new TranslatableString("RANDの引数が省略されています");
            [Managed] public static TranslatableString RandArgIsZero { get; } = new TranslatableString("RANDの引数に0が与えられています");
            [Managed] public static TranslatableString ZeroDVarHasArg { get; } = new TranslatableString("配列でない変数\"{0}\"を引数付きで呼び出しています");
            [Managed] public static TranslatableString KeywordCanNotEmpty { get; } = new TranslatableString("キーワードを空にはできません");
            [Managed] public static TranslatableString AssignToVarOoR { get; } = new TranslatableString("配列変数\"{0}\"の要素数を超えて代入しようとしました");
            [Managed] public static TranslatableString MissingVarArg { get; } = new TranslatableString("変数\"{0}\"に必要な引数が不足しています");
            [Managed] public static TranslatableString CallStrAsInt { get; } = new TranslatableString("整数型でない変数\"{0}\"を整数型として呼び出しました");
            [Managed] public static TranslatableString CallIntAsStr { get; } = new TranslatableString("文字列型でない変数\"{0}\"を文字列型として呼び出しました");
            [Managed] public static TranslatableString CallNDStrAsInt { get; } = new TranslatableString("整数型配列でない変数\"{0}\"を整数型配列として呼び出しました");
            [Managed] public static TranslatableString CallNDIntAsStr { get; } = new TranslatableString("文字列型配列でない変数\"{0}\"を文字列型配列として呼び出しました");
            [Managed] public static TranslatableString GetSize0DVar { get; } = new TranslatableString("配列型でない変数\"{0}\"の長さを取得しようとしました");
            [Managed] public static TranslatableString CallCharaVarAsVar { get; } = new TranslatableString("キャラクタ変数\"{0}\"を非キャラ変数として呼び出しました");
            [Managed] public static TranslatableString CallVarAsCharaVar { get; } = new TranslatableString("非キャラクタ変数\"{0}\"をキャラ変数として呼び出しました");
            [Managed] public static TranslatableString GetSize0DCharaVar { get; } = new TranslatableString("配列型でないキャラクタ変数\"{0}\"の長さを取得しようとしました");
            [Managed] public static TranslatableString GetSizeCharaVarWithoutDim { get; } = new TranslatableString("{0}次元配列型のキャラ変数\"{1}\"の長さを次元を指定せずに取得しようとしました");
            [Managed] public static TranslatableString GetSizeCharaVarNonExistDim { get; } = new TranslatableString("配列型変数のキャラ変数\"{0}\"の存在しない次元の長さを取得しようとしました");
            [Managed] public static TranslatableString OoRCharaVarArg { get; } = new TranslatableString("キャラクタ配列変数\"{0}\"の第{1}引数({2})はキャラ登録番号の範囲外です");
            [Managed] public static TranslatableString OoRInstructionArg { get; } = new TranslatableString("\"{0}\"命令の第{1}引数({2})は配列\"{3}\"の範囲外です");
            [Managed] public static TranslatableString GetSizeDimError { get; } = new TranslatableString("{0}次元配列型変数\"{1}\"の長さを取得しようとしました");
            [Managed] public static TranslatableString GetSizeNonExistDim { get; } = new TranslatableString("配列型変数\"{0}\"の存在しない次元の長さを取得しようとしました");
            [Managed] public static TranslatableString OoRVarArg { get; } = new TranslatableString("配列変数\"{0}\"の第{1}引数({2})は配列の範囲外です");
            [Managed] public static TranslatableString EmptyRefVar { get; } = new TranslatableString("参照型変数\"{0}\"は何も参照していません");
            [Managed] public static TranslatableString CanNotOmitRefToVar { get; } = new TranslatableString("参照先変数は省略できません");
            [Managed] public static TranslatableString CanNotRefPseudoVar { get; } = new TranslatableString("疑似変数は参照できません");
            [Managed] public static TranslatableString CanNotRefConstVar { get; } = new TranslatableString("定数は参照できません");
            [Managed] public static TranslatableString CanNotGlobalRefLocalVar { get; } = new TranslatableString("広域の参照変数はローカル変数を参照できません");
            [Managed] public static TranslatableString CanNotRefCharaVar { get; } = new TranslatableString("キャラ変数は参照できません");
            [Managed] public static TranslatableString CanNotRefDifferentType { get; } = new TranslatableString("型が異なる変数は参照できません");
            [Managed] public static TranslatableString CanNotRefDifferentDim { get; } = new TranslatableString("次元数が異なる変数は参照できません");
            [Managed] public static TranslatableString AssignToPseudoVar { get; } = new TranslatableString("擬似変数\"{0}\"に代入しようとしました");
            [Managed] public static TranslatableString GetSizePseudoVar { get; } = new TranslatableString("擬似変数\"{0}\"の長さを取得しようとしました");
            [Managed] public static TranslatableString GetDimPseudoVar { get; } = new TranslatableString("擬似変数\"{0}\"の配列を取得しようとしました");
            [Managed] public static TranslatableString RandArgIsNegative { get; } = new TranslatableString("RANDの引数に0以下の値({0})が指定されました");
            [Managed] public static TranslatableString SpriteNameAlreadyUsed { get; } = new TranslatableString("同名のリソースがすでに作成されています:{0}");
            [Managed] public static TranslatableString NotDeclaredAnimationSpriteSize { get; } = new TranslatableString("アニメーションスプライトのサイズが宣言されていません");
            [Managed] public static TranslatableString InvalidAnimationSpriteSize { get; } = new TranslatableString("アニメーションスプライトのサイズの指定が適切ではありません");
            [Managed] public static TranslatableString MissingSecondArgumentExtension { get; } = new TranslatableString("第二引数に拡張子がありません:{0}");
            [Managed] public static TranslatableString NotExistImageFile { get; } = new TranslatableString("指定された画像ファイルが見つかりませんでした:{0}");
            [Managed] public static TranslatableString FailedLoadFile { get; } = new TranslatableString("指定されたファイルの読み込みに失敗しました:{0}");
            [Managed] public static TranslatableString TooLargeImageFile { get; } = new TranslatableString("指定された画像ファイルの大きさが大きすぎます(幅及び高さを{0}px以下にすることを強く推奨します):{1}");
            [Managed] public static TranslatableString FailedCreateResource { get; } = new TranslatableString("画像リソースの作成に失敗しました:{0}");
            [Managed] public static TranslatableString SpriteCreateFromFailedResource { get; } = new TranslatableString("作成に失敗したリソースを元にスプライトを作成しようとしました:{0}");
            [Managed] public static TranslatableString SpriteSizeIsNegatibe { get; } = new TranslatableString("スプライトの高さ又は幅には正の値のみ指定できます:{0}");
            [Managed] public static TranslatableString OoRParentImage { get; } = new TranslatableString("親画像の範囲外を参照しています:{0}");
            [Managed] public static TranslatableString FrameTimeIsNegative { get; } = new TranslatableString("フレーム表示時間には正の値のみ指定できます:{0}");
            [Managed] public static TranslatableString FailedAddSpriteFrame { get; } = new TranslatableString("アニメーションスプライトのフレームの追加に失敗しました:{0}");
            [Managed] public static TranslatableString InvalidConfigName { get; } = new TranslatableString("文字列\"{0}\"は適切なコンフィグ名ではありません");
            [Managed] public static TranslatableString NotAllowGetConfigValue { get; } = new TranslatableString("コンフィグ文字列\"{0}\"の値の取得は許可されていません");
            [Managed] public static TranslatableString UseArgVarInHasNotArgFunc { get; } = new TranslatableString("関数宣言に引数変数\"{0}\"が使われていない関数中で\"{0}\"が使われています(関数の引数以外の用途に使うことは推奨されません。代わりに#DIMの使用を検討してください)");
            [Managed] public static TranslatableString UseArgVarInSystemFunc { get; } = new TranslatableString("システム関数\"{0}\"中で\"{1}\"が使われています(関数の引数以外の用途に使うことは推奨されません。代わりに#DIMの使用を検討してください)");
            [Managed] public static TranslatableString FailedOpenFile { get; } = new TranslatableString("{0}のオープンに失敗しました");
            //[Managed] public static TranslatableString LoadingFile { get; } = new TranslatableString("{0}読み込み中・・・");//こいつはエラーメッセージじゃないけどここに置いておく
            [Managed] public static TranslatableString UnexpectedError { get; } = new TranslatableString("予期しないエラーが発生しました");
            [Managed] public static TranslatableString MissingComma { get; } = new TranslatableString("\",\"が必要です");
            [Managed] public static TranslatableString CanNotInterpretVarName { get; } = new TranslatableString("{0}つ目の値を変数名として認識できません");
            [Managed] public static TranslatableString CanNotChange0DVarSize { get; } = new TranslatableString("配列変数でない変数\"{0}\"のサイズを変更できません");
            [Managed] public static TranslatableString CanNotChangeVarSize { get; } = new TranslatableString("{0}のサイズは変更できません");
            [Managed] public static TranslatableString ArrayLengthIs0 { get; } = new TranslatableString("配列長に0は指定できません（変数を使用禁止にするには配列長に負の値を指定してください）");
            [Managed] public static TranslatableString CanNotDisableVarArrayLengthIsNegative { get; } = new TranslatableString("使用禁止にできない変数に対して負の配列長が指定されています");
            [Managed] public static TranslatableString IgnoreNDData { get; } = new TranslatableString("{0}次元配列のサイズ指定に不必要なデータは無視されます");
            [Managed] public static TranslatableString LocalVarSizeCanNotLessThan1 { get; } = new TranslatableString("ローカル変数のサイズを1未満にはできません");
            [Managed] public static TranslatableString InternalVarSizeCanNotLessThan100 { get; } = new TranslatableString("ローカル変数でない一次元配列のサイズを100未満にはできません");
            [Managed] public static TranslatableString OneDVarSizeCanNotGreaterThan1M { get; } = new TranslatableString("一次元配列のサイズを1000000より大きくすることはできません");
            [Managed] public static TranslatableString VarSizeCanNotLessThan1 { get; } = new TranslatableString("配列サイズを1未満にはできません");
            [Managed] public static TranslatableString VarSizeCanNotGreaterThan1M { get; } = new TranslatableString("配列サイズを1000000より大きくすることはできません");
            [Managed] public static TranslatableString MissingVarSizeArg { get; } = new TranslatableString("{0}次元配列のサイズ指定には{0}つの数値が必要です");
            [Managed] public static TranslatableString VarSizeLimitIs1M { get; } = new TranslatableString("{0}次元配列の要素数は最大で100万個までです");
            [Managed] public static TranslatableString VarSizeAlreadyDefined { get; } = new TranslatableString("{0}の要素数は既に定義されています（上書きします）");
            [Managed] public static TranslatableString DifferentVarProhibitSetting { get; } = new TranslatableString("\"{0}\"と\"{1}\"の禁止設定が異なります（使用禁止を解除します）");
            [Managed] public static TranslatableString DifferentVarSize { get; } = new TranslatableString("\"{0}\"と\"{1}\"の要素数が異なります（大きい方に合わせます）");
            [Managed] public static TranslatableString InappropriatePalamJuelPalamname { get; } = new TranslatableString("PALAMとJUELとPALAMNAMEの要素数が不適切です");
            [Managed] public static TranslatableString PalamnameSizeLessThanJuelSize { get; } = new TranslatableString("PALAMNAMEの要素数がJUELより少なくなっています（JUELに合わせます）");
            [Managed] public static TranslatableString DuplicateCharaDefine1 { get; } = new TranslatableString("番号{0}のキャラが複数回定義されています(SPキャラとして定義するには互換性オプション「SPキャラを使用する」をONにしてください)");
            [Managed] public static TranslatableString DuplicateCharaDefine2 { get; } = new TranslatableString("番号{0}のキャラが複数回定義されています");
            [Managed] public static TranslatableString StartedComma { get; } = new TranslatableString("\",\"で始まっています");
            [Managed] public static TranslatableString CharaNoDefinedTwice { get; } = new TranslatableString("番号が二重に定義されました");
            [Managed] public static TranslatableString CanNotConvertToInt { get; } = new TranslatableString("\"{0}\"を整数値に変換できません");
            [Managed] public static TranslatableString StartedDataBeforeCharaNo { get; } = new TranslatableString("番号が定義される前に他のデータが始まりました");
            [Managed] public static TranslatableString ProgramError { get; } = new TranslatableString("プログラムミス");
            [Managed] public static TranslatableString IsProhibitedVar { get; } = new TranslatableString("\"{0}\"は禁止設定された変数です");
            [Managed] public static TranslatableString OoRArray { get; } = new TranslatableString("{0}は配列の範囲外です");
            [Managed] public static TranslatableString MissingSecondIdentifier { get; } = new TranslatableString("二つ目の識別子がありません");
            [Managed] public static TranslatableString MissingThirdIdentifier { get; } = new TranslatableString("三つ目の識別子がありません");
            [Managed] public static TranslatableString VarKeyAreadyDefined { get; } = new TranslatableString("\"{0}\"の\"{1}\"番目の要素は既に定義されています(上書きします)");
            [Managed] public static TranslatableString FirstValueCanNotConvertToInt { get; } = new TranslatableString("一つ目の値を整数値に変換できません");
            [Managed] public static TranslatableString ProhibitedArrayName { get; } = new TranslatableString("禁止設定された名前配列です");
            [Managed] public static TranslatableString CanNotReadAmountOfMoney { get; } = new TranslatableString("金額が読み取れません");
            [Managed] public static TranslatableString SaveCodeIs0 { get; } = new TranslatableString("コード:0のセーブデータはいかなるコードのスクリプトからも読めるデータとして扱われます");
            [Managed] public static TranslatableString CanNotReadVersion { get; } = new TranslatableString("バージョン指定を読み取れなかったので処理を省略します");
            [Managed] public static TranslatableString RequireLaterEmuera { get; } = new TranslatableString("このバリアント動作させるにはVer.{0}以降のバージョンのEmueraが必要です");
            [Managed] public static TranslatableString SomethingErrorInGamebase { get; } = new TranslatableString("GAMEBASE.CSVの読み込み中にエラーが発生したため、読みこみを中断します");
            [Managed] public static TranslatableString Instruction { get; } = new TranslatableString("命令:");
            [Managed] public static TranslatableString MissingArg { get; } = new TranslatableString("引数が設定されていません");
            [Managed] public static TranslatableString NotEnoughArguments { get; } = new TranslatableString("引数が足りません");
            [Managed] public static TranslatableString TooManyArg { get; } = new TranslatableString("引数が多すぎます");
            [Managed] public static TranslatableString CanNotRecognizeArg { get; } = new TranslatableString("第{0}引数を認識できません");
            [Managed] public static TranslatableString IncorrectArg { get; } = new TranslatableString("第{0}引数の型が正しくありません");
            [Managed] public static TranslatableString ArgIsNotVariable { get; } = new TranslatableString("第{0}引数に変数以外を指定することはできません");
            [Managed] public static TranslatableString ArgIsConst { get; } = new TranslatableString("第{0}引数に変更できない変数を指定することはできません");
            [Managed] public static TranslatableString AbnormalSpecification { get; } = new TranslatableString("異常な指定");
            [Managed] public static TranslatableString CanNotOmitArg { get; } = new TranslatableString("第{0}引数を省略することはできません");
            [Managed] public static TranslatableString DifferentArgsCount { get; } = new TranslatableString("引数の数が正しくありません");
            [Managed] public static TranslatableString ArgIsNotRealNumber { get; } = new TranslatableString("第{0}引数が実数値ではありません（常に0と解釈されます）");
            [Managed] public static TranslatableString WrongFormat { get; } = new TranslatableString("書式が間違っています");
            [Managed] public static TranslatableString ArgIsStrVar { get; } = new TranslatableString("第{0}引数を文字列型にすることはできません");
            [Managed] public static TranslatableString MissingArgAfterComma { get; } = new TranslatableString("\',\'の後ろに引数がありません。");
            [Managed] public static TranslatableString ArgIsNotRequired { get; } = new TranslatableString("引数は不要です");
            [Managed] public static TranslatableString TransparentUnsupported { get; } = new TranslatableString("無色透明(Transparent)は色として指定できません");
            [Managed] public static TranslatableString InvalidColorName { get; } = new TranslatableString("指定された色名\"{0}\"は無効な色名です");
            [Managed] public static TranslatableString ArgIsNotArrayVar { get; } = new TranslatableString("第{0}引数に配列でない変数を指定することはできません");
            [Managed] public static TranslatableString ExtraCharacterAfterArg { get; } = new TranslatableString("引数の後に余分な文字があります");
            [Managed] public static TranslatableString ArgIsNotCharaVar { get; } = new TranslatableString("第{0}引数はキャラクタ変数でなければなりません");
            [Managed] public static TranslatableString ArgIsNot1DVar { get; } = new TranslatableString("第{0}引数に１次元配列もしくは配列型キャラクタ変数以外を指定することはできません");
            [Managed] public static TranslatableString IsNotForwardBack { get; } = new TranslatableString("第{0}引数にソート方法指定子（FORWARD or BACK）以外が指定されています");
            [Managed] public static TranslatableString ArgIsNotNumber { get; } = new TranslatableString("第{0}引数が数値ではありません");
            [Managed] public static TranslatableString NotSpecifiedFuncName { get; } = new TranslatableString("関数名が指定されていません");
            [Managed] public static TranslatableString CanNotReadLeft { get; } = new TranslatableString("代入文の左辺の読み取りに失敗しました");
            [Managed] public static TranslatableString LeftHasExtraComma { get; } = new TranslatableString("代入文の左辺に余分な','があります");
            [Managed] public static TranslatableString LeftIsNotVar { get; } = new TranslatableString("代入文の左辺に変数以外を指定することはできません");
            [Managed] public static TranslatableString LeftIsConst { get; } = new TranslatableString("代入文の左辺に変更できない変数を指定することはできません");
            [Managed] public static TranslatableString InvalidOpWithInt { get; } = new TranslatableString("整数型の代入に演算子\"{0}\"は使用できません");
            [Managed] public static TranslatableString InvalidOpWithIncrement { get; } = new TranslatableString("インクリメント行でインクリメント以外の処理が定義されています");
            [Managed] public static TranslatableString InvalidOpWithDecrement { get; } = new TranslatableString("デクリメント行でデクリメント以外の処理が定義されています");
            [Managed] public static TranslatableString CanNotReadRight { get; } = new TranslatableString("代入文の右辺の読み取りに失敗しました");
            [Managed] public static TranslatableString CanNotContainMultipleValue { get; } = new TranslatableString("複合代入演算では右辺に複数の値を含めることはできません");
            [Managed] public static TranslatableString CanNotOmitRight { get; } = new TranslatableString("代入式の右辺の値は省略できません");
            [Managed] public static TranslatableString CanNotAssignStrToInt { get; } = new TranslatableString("数値型変数に文字列は代入できません");
            [Managed] public static TranslatableString StrAssignIsPrihibited { get; } = new TranslatableString("文字列代入は禁止されています（'=を用いるかコンフィグオプションを変えてください)");
            [Managed] public static TranslatableString CanNotAssignIntToStr { get; } = new TranslatableString("文字列変数に数値型は代入できません");
            [Managed] public static TranslatableString RightHasExtraComma { get; } = new TranslatableString("代入文の右辺に余分な','があります");
            [Managed] public static TranslatableString InvalidAssignmentOp { get; } = new TranslatableString("代入式に使用できない演算子が使われました");
            [Managed] public static TranslatableString IgnoreArgBecauseNotInt { get; } = new TranslatableString("第2引数は整数型ではないため、無視されます");
            [Managed] public static TranslatableString OmittedArg1 { get; } = new TranslatableString("省略できない引数が省略されています。Emueraは0を補います");
            [Managed] public static TranslatableString OmittedArg2 { get; } = new TranslatableString("省略できない引数が省略されています。Emueraは0を補いますがeramakerの動作は不定です");
            [Managed] public static TranslatableString CanNotUseRepeat { get; } = new TranslatableString("COUNTが使用禁止変数になっているため、REPEATは使用できません");
            [Managed] public static TranslatableString RepeatCountLessthan0 { get; } = new TranslatableString("0回以下のREPEATです。(eramakerではエラーになります)");
            [Managed] public static TranslatableString ArgLessThan0 { get; } = new TranslatableString("引数に0以下の値が渡されています(この行は何もしません)");
            [Managed] public static TranslatableString ArgIsNegativeValue { get; } = new TranslatableString("引数に負の値が渡されています(結果は不定です)");
            [Managed] public static TranslatableString ReturnArgIsVar { get; } = new TranslatableString("RETURNの引数に変数が渡されています(eramaker：常に0を返します)");
            [Managed] public static TranslatableString ReturnArgIsFormula { get; } = new TranslatableString("RETURNの引数に数式が渡されています(eramaker：Emueraとは異なる値を返します)");
            [Managed] public static TranslatableString ArgIsFormula { get; } = new TranslatableString("\"{0}\"の引数に複数の値が与えられています(eramaker：非対応です)");
            [Managed] public static TranslatableString CharaVarCanNotSpecifiedArg { get; } = new TranslatableString("第{0}引数にキャラクタ変数を指定することはできません");
            [Managed] public static TranslatableString DifferentArgType { get; } = new TranslatableString("第{0}引数の型が違います");
            [Managed] public static TranslatableString ArgIsOoRBit { get; } = new TranslatableString("第{0}引数({1})がビット範囲(0～63)を超えています");
            [Managed] public static TranslatableString SpecifiedConst { get; } = new TranslatableString("値を変更できない変数\"{0}\"が指定されました");
            [Managed] public static TranslatableString CanNotSetthirdLaterArg { get; } = new TranslatableString("対象となる変数\"{0}\"の要素を省略する場合には第3引数以降を設定できません");
            [Managed] public static TranslatableString IgnoreThirdLaterArg { get; } = new TranslatableString("第３引数以降は1次元配列以外では無視されます");
            [Managed] public static TranslatableString NotMatchTwoArg { get; } = new TranslatableString("2つの引数の型が一致していません");
            [Managed] public static TranslatableString ArgIs2DVar { get; } = new TranslatableString("第１引数に二次元配列の変数を指定することはできません");
            [Managed] public static TranslatableString IgnoreFourthLaterArg { get; } = new TranslatableString("第４引数以降は1次元配列以外では無視されます");
            [Managed] public static TranslatableString InvalidSetcolorArgCount { get; } = new TranslatableString("SETCOLORの引数の数が不正です(SETCOLORの引数は1個もしくは3個です)");
            [Managed] public static TranslatableString ArgIsRequiredNonCharaArrayVar { get; } = new TranslatableString("第{0}引数は非キャラ型の1次元配列変数でなければなりません");
            [Managed] public static TranslatableString NotMatchFirstAndThirdVar { get; } = new TranslatableString("第１引数と第３引数の型が違います");
            [Managed] public static TranslatableString CanNotSaveCharaVar { get; } = new TranslatableString("キャラクタ変数\"{0}\"はセーブできません(キャラクタ変数のSAVEにはSAVECHARAを使用します)");
            [Managed] public static TranslatableString CanNotSavePrivVar { get; } = new TranslatableString("プライベート変数\"{0}\"はセーブできません");
            [Managed] public static TranslatableString CanNotSaveLocalVar { get; } = new TranslatableString("ローカル変数\"{0}\"はセーブできません");
            [Managed] public static TranslatableString CanNotSaveConstVar { get; } = new TranslatableString("値を変更できない変数はセーブできません");
            [Managed] public static TranslatableString CanNotSavePseudoVar { get; } = new TranslatableString("疑似変数はセーブできません");
            [Managed] public static TranslatableString CanNotSaveRefVar { get; } = new TranslatableString("参照型変数はセーブできません");
            [Managed] public static TranslatableString DuplicateVarSave { get; } = new TranslatableString("変数\"{0}\"を二度以上保存しようとしています");
            [Managed] public static TranslatableString NotPositiveCharaNo { get; } = new TranslatableString("キャラ登録番号は正の値でなければなりません");
            [Managed] public static TranslatableString CharaNoOverInt32 { get; } = new TranslatableString("キャラ登録番号が32bit符号付整数の上限を超えています");
            [Managed] public static TranslatableString DuplicateCharaSave { get; } = new TranslatableString("キャラ登録番号\"{0}\"を二度以上保存しようとしています");
            [Managed] public static TranslatableString ArgIsNotRef { get; } = new TranslatableString("第{0}引数は関数参照か参照型変数でなければなりません");
            [Managed] public static TranslatableString NotDefinedUserFunc { get; } = new TranslatableString("式中関数\"{0}\"が見つかりません");
            [Managed] public static TranslatableString CanNotRefFunc { get; } = new TranslatableString("#FUNCTION(S)属性を持たない関数\"{0}\"は参照できません");
            [Managed] public static TranslatableString NotDefinedVar { get; } = new TranslatableString("変数\"{0}\"が見つかりません");
            [Managed] public static TranslatableString ArraycopyArgIsNotDefined { get; } = new TranslatableString("ARRAYCOPY命令の第{0}引数\"{1}\"は変数名として存在しません");
            [Managed] public static TranslatableString ArraycopyArgIsNotArray { get; } = new TranslatableString("ARRAYCOPY命令の第{0}引数\"{1}\"は配列変数ではありません");
            [Managed] public static TranslatableString ArraycopyArgIsCharaVar { get; } = new TranslatableString("ARRAYCOPY命令の第{0}引数\"{1}\"はキャラクタ変数です（対応していません）");
            [Managed] public static TranslatableString ArraycopyArgIsConst { get; } = new TranslatableString("ARRAYCOPY命令の第{0}引数\"{1}\"は値を変更できない変数です");
            [Managed] public static TranslatableString DifferentArraycopyArgsDim { get; } = new TranslatableString("ARRAYCOPY命令の2つの引数の次元が異なります");
            [Managed] public static TranslatableString DifferentArraycopyArgsType { get; } = new TranslatableString("ARRAYCOPY命令の２つの配列変数の型が一致していません");
            [Managed] public static TranslatableString MissingArgAfterColon { get; } = new TranslatableString("変数の:の後に引数がありません");
            [Managed] public static TranslatableString AbnormalPrint { get; } = new TranslatableString("PRINT異常");
            [Managed] public static TranslatableString AbnormalPrintdata { get; } = new TranslatableString("PRINTDATA異常");
            [Managed] public static TranslatableString InvalidArg { get; } = new TranslatableString("引数が正しくありません");
            [Managed] public static TranslatableString NotDefinedFunc { get; } = new TranslatableString("指定された関数名\"@{0}\"は存在しません");
            [Managed] public static TranslatableString SPCharaConfigIsOff { get; } = new TranslatableString("SPキャラ関係の機能は標準では使用できません(互換性オプション「SPキャラを使用する」をONにしてください)");
            [Managed] public static TranslatableString OoRCvarsetArg { get; } = new TranslatableString("命令CVARSETの第{0}引数({1})がキャラクタの範囲外です");
            [Managed] public static TranslatableString CvarsetArgIsNotCharaVar { get; } = new TranslatableString("命令CVARSETにキャラクタ変数でない変数\"{0}\"が渡されました");
            [Managed] public static TranslatableString OoRSavecharaArg { get; } = new TranslatableString("SAVECHARAの第{0}引数の値がキャラ登録番号の範囲を超えています");
            [Managed] public static TranslatableString DuplicateCharaNo { get; } = new TranslatableString("同一のキャラ登録番号({0})が複数回指定されました");
            [Managed] public static TranslatableString ArgIsOoRColorCode { get; } = new TranslatableString("第{0}引数が色を表す整数の範囲外です");
            [Managed] public static TranslatableString ArgIsOoR { get; } = new TranslatableString("引数の値が適切な範囲外です");
            [Managed] public static TranslatableString AwaitArgIsNegative { get; } = new TranslatableString("AWAIT命令:負の値({0})が指定されました");
            [Managed] public static TranslatableString AwaitArgIsOver10Seconds { get; } = new TranslatableString("AWAIT命令:10秒以上の待機時間({0}ms)が指定されました");
            [Managed] public static TranslatableString CanNotUseInstructionInfunc { get; } = new TranslatableString("\"@{0}\"中で{1}命令を実行することはできません");
            [Managed] public static TranslatableString NothingAfterSif { get; } = new TranslatableString("SIF文の次の行がありません");
            [Managed] public static TranslatableString FuncCanNotAfterSif { get; } = new TranslatableString("SIF文の次の行を\"{0}\"文にすることはできません");
            [Managed] public static TranslatableString LabelCanNotAfterSif { get; } = new TranslatableString("SIF文の次の行をラベル行にすることはできません");
            [Managed] public static TranslatableString EmptyAfterSif { get; } = new TranslatableString("SIF文の次の行が空行またはコメント行です(eramaker:SIF文は意味を失います)");
            [Managed] public static TranslatableString AbnormalContinue { get; } = new TranslatableString("異常なCONTINUE");
            [Managed] public static TranslatableString CanNotUseReturnf { get; } = new TranslatableString("RETURNFは#FUNCTION(S)以外では使用できません");
            [Managed] public static TranslatableString ReturnfStrInIntFunc { get; } = new TranslatableString("#FUNCTIONで始まる関数の戻り値に文字列型が指定されました");
            [Managed] public static TranslatableString ReturnfIntInStrFunc { get; } = new TranslatableString("#FUCNTIONSで始まる関数の戻り値に数値型が指定されました");
            [Managed] public static TranslatableString CanNotUseCallevent { get; } = new TranslatableString("EVENT関数中にCALLEVENT命令は使用できません");
            [Managed] public static TranslatableString NotDefinedLabelName { get; } = new TranslatableString("指定されたラベル名\"${0}\"は現在の関数内に存在しません");
            [Managed] public static TranslatableString InvalidLabelName { get; } = new TranslatableString("指定されたラベル名\"${0}\"は無効な$ラベル行です");
            [Managed] public static TranslatableString NotStartedSharpLineInHeader { get; } = new TranslatableString("ヘッダーの中に#で始まらない行があります");
            [Managed] public static TranslatableString CanNotInterpretSharpLine { get; } = new TranslatableString("解釈できない#行です");
            [Managed] public static TranslatableString UnknownPreprocessorInSharpLine { get; } = new TranslatableString("\"#{0}\"は解釈できないプリプロセッサです");
            [Managed] public static TranslatableString MissingReplacementSource { get; } = new TranslatableString("置換元の識別子がありません");
            [Managed] public static TranslatableString FuncMacroArgIs0 { get; } = new TranslatableString("関数型マクロの引数を0個にすることはできません");
            [Managed] public static TranslatableString WrongFormatReplacementSource { get; } = new TranslatableString("置換元の引数指定の書式が間違っています");
            [Managed] public static TranslatableString DuplicateCharacterReplcaementSource { get; } = new TranslatableString("置換元の引数に同じ文字が2回以上使われています");
            [Managed] public static TranslatableString MissingSubstitution { get; } = new TranslatableString("置換先の式がありません");
            [Managed] public static TranslatableString CanNotDeclaredFuncMacro { get; } = new TranslatableString("関数型マクロは宣言できません");
            [Managed] public static TranslatableString UnexpectedErrorFrom { get; } = new TranslatableString("予期しないエラーが発生しました:{0}");
            [Managed] public static TranslatableString HasTooManyArg { get; } = new TranslatableString("\"{0}\"に余分な引数があります");
            [Managed] public static TranslatableString DuplicateSkipstart { get; } = new TranslatableString("[SKIPSTART]が重複して使用されています");
            [Managed] public static TranslatableString MissingArguments { get; } = new TranslatableString("\"{0}\"に引数がありません");
            [Managed] public static TranslatableString IsInvalid { get; } = new TranslatableString("不適切な{0}です");
            [Managed] public static TranslatableString UnexpectedSkipend { get; } = new TranslatableString("[SKIPSTART]と対応しない[SKIPEND]です");
            [Managed] public static TranslatableString UnexpectedMacroEndif { get; } = new TranslatableString("対応する[IF]のない[ENDIF]です");
            [Managed] public static TranslatableString UnrecognizedPreprosessor { get; } = new TranslatableString("認識できないプリプロセッサです");
            [Managed] public static TranslatableString TheresNo { get; } = new TranslatableString("[{0}]がありません");
            [Managed] public static TranslatableString InvalidSBrackets { get; } = new TranslatableString("[]の使い方が不正です");
            [Managed] public static TranslatableString IgnoreAfterPreprosessor { get; } = new TranslatableString("[{0}]の後ろは無視されます。");
            [Managed] public static TranslatableString InvalidSharp { get; } = new TranslatableString("関数宣言の直後以外で#行が使われています");
            [Managed] public static TranslatableString FuncIsAlreadyDefined { get; } = new TranslatableString("関数\"@{0}\"は既に定義({1}の{2}行目)されています");
            [Managed] public static TranslatableString LabelIsAlreadyDefined { get; } = new TranslatableString("ラベル名\"${0}\"は既に同じ関数内({1}の{2}行目)で使用されています");
            [Managed] public static TranslatableString LineBeforeFunc { get; } = new TranslatableString("関数が定義されるより前に行があります");
            [Managed] public static TranslatableString FuncArgError { get; } = new TranslatableString("関数\"@{0}\"の引数のエラー:{1}");
            [Managed] public static TranslatableString CalledFailedFunc { get; } = new TranslatableString("ロード時に解析に失敗した関数が呼び出されました");
            [Managed] public static TranslatableString EventFuncHasArg { get; } = new TranslatableString("イベント関数\"@{0}\"に引数は設定できません");
            [Managed] public static TranslatableString SystemFuncHasArg { get; } = new TranslatableString("システム関数\"@{0}\"に引数は設定できません");
            [Managed] public static TranslatableString WrongArgFormat { get; } = new TranslatableString("引数の書式が間違っています");
            [Managed] public static TranslatableString CanNotEmptyFuncSBrackets { get; } = new TranslatableString("関数定義の[]内の引数は空にできません");
            [Managed] public static TranslatableString CanNotOmitFuncDefineArg { get; } = new TranslatableString("関数定義の引数は省略できません");
            [Managed] public static TranslatableString FuncDefineArgOnlyConst { get; } = new TranslatableString("関数定義の[]内の引数は定数のみ指定できます");
            [Managed] public static TranslatableString ArgCanOnlyAssignableVar { get; } = new TranslatableString("関数定義の引数には代入可能な変数を指定してください");
            [Managed] public static TranslatableString ArgHasNotSubscript { get; } = new TranslatableString("関数定義の参照型でない引数\"{0}\"に添え字が指定されていません");
            [Managed] public static TranslatableString ArgSubscriptOnlyConst { get; } = new TranslatableString("関数定義の引数の添え字には定数を指定してください");
            [Managed] public static TranslatableString DuplicateArg { get; } = new TranslatableString("第{0}引数\"{1}\"はすでに第{2}引数として宣言されています");
            [Managed] public static TranslatableString ArgCanOnlyConst { get; } = new TranslatableString("引数の初期値には定数のみを指定できます");
            [Managed] public static TranslatableString ArgCanOnlyPrivVar { get; } = new TranslatableString("引数の初期値を定義できるのは\"ARG\"、\"ARGS\"またはプライベート変数のみです");
            [Managed] public static TranslatableString RefArgCanNotInitialize { get; } = new TranslatableString("参照渡しの引数に初期値は定義できません");
            [Managed] public static TranslatableString NotMatchTypeArgAndInitialValue { get; } = new TranslatableString("引数の型と初期値の型が一致していません");
            [Managed] public static TranslatableString BeNotFuncCheckBecauseUseCallform { get; } = new TranslatableString("CALLFORM系命令が使われたため、呼び出されない関数のチェックは行われません。");
            [Managed] public static TranslatableString FuncNeverCalled { get; } = new TranslatableString("関数\"@{0}\"は定義されていますが一度も呼び出されません");
            [Managed] public static TranslatableString UndefinedFunctions { get; } = new TranslatableString("・定義が見つからなかった関数: 他のファイルで定義されている場合はこの警告は無視できます");
            [Managed] public static TranslatableString GeneralFunc { get; } = new TranslatableString("　○一般関数:");
            [Managed] public static TranslatableString Occurrences { get; } = new TranslatableString("回");
            [Managed] public static TranslatableString SentenceFunc { get; } = new TranslatableString("　○文中関数:");
            [Managed] public static TranslatableString IgnoredFuncNeverCalled { get; } = new TranslatableString("警告Lv1:定義された関数が一度も呼び出されていない事に関する警告を{0}件無視しました");
            [Managed] public static TranslatableString IgnoredUndefinedFuncCall { get; } = new TranslatableString("警告Lv2:定義されていない関数を呼び出した事に関する警告を{0}件無視しました");
            [Managed] public static TranslatableString TotalFunc { get; } = new TranslatableString("非コメント行数:{0}, 全関数合計:{1}, 被呼出関数合計:{2}");
            [Managed] public static TranslatableString OverWriteSystemFuncWarn1 { get; } = new TranslatableString("＊＊＊＊＊警告＊＊＊＊＊");
            [Managed] public static TranslatableString OverWriteSystemFuncWarn2 { get; } = new TranslatableString("  システム関数\"{0}\"がユーザー定義関数によって上書きされています");
            [Managed] public static TranslatableString OverWriteSystemFuncWarn3 { get; } = new TranslatableString(" 上記の関数を利用するスクリプトは意図通りに動かない可能性があります");
            [Managed] public static TranslatableString OverWriteSystemFuncWarn4 { get; } = new TranslatableString("  ※この警告は該当する式中関数を利用しているEmuera専用スクリプト向けの警告です。");
            [Managed] public static TranslatableString OverWriteSystemFuncWarn5 { get; } = new TranslatableString("  eramaker用のスクリプトの動作には影響しません。");
            [Managed] public static TranslatableString OverWriteSystemFuncWarn6 { get; } = new TranslatableString("  今後この警告が不要ならばコンフィグの「システム関数が上書きされたとき警告を表示する」をOFFにして下さい。");
            [Managed] public static TranslatableString OverWriteSystemFuncWarn7 { get; } = new TranslatableString("＊＊＊＊＊＊＊＊＊＊＊＊");
            [Managed] public static TranslatableString FuncAnalysisError { get; } = new TranslatableString("\"@{0}\"の解析中にエラー:{1}");
            [Managed] public static TranslatableString CanNotUseInUserFunc { get; } = new TranslatableString("\"{0}\"命令は#FUNCTION中で使うことはできません");
            [Managed] public static TranslatableString CanNotLabelDefineInSyntax { get; } = new TranslatableString("\"{0}\"構文中に$ラベルを定義することはできません");
            [Managed] public static TranslatableString InvalidInstructionInSyntax { get; } = new TranslatableString("\"{0}\"構文に使用できない命令\"{1}\"が含まれています");
            [Managed] public static TranslatableString OutsideSelectcase { get; } = new TranslatableString("SELECTCASE構文の分岐の外に命令\"{0}\"が含まれています");
            [Managed] public static TranslatableString NestedRepeat { get; } = new TranslatableString("REPEAT文が入れ子にされています（無限ループの恐れがあります）");
            [Managed] public static TranslatableString RepeatInsideFor { get; } = new TranslatableString("カウンタ変数にCOUNT:{0}を用いたFOR文の中でREPEATが呼び出されています（無限ループの恐れがあります）");
            [Managed] public static TranslatableString InvalidLoopInstruction { get; } = new TranslatableString("REPEAT, FOR, WHILE, DOの中以外で\"{0}\"文が使われました");
            [Managed] public static TranslatableString InvalidElse { get; } = new TranslatableString("IF～ENDIFの外で\"{0}\"文が使われました");
            [Managed] public static TranslatableString InvalidElseAfterElse { get; } = new TranslatableString("ELSE文より後で\"{0}\"文が使われました");
            [Managed] public static TranslatableString UnexpectedEndif { get; } = new TranslatableString("対応するIFの無いENDIF文です");
            [Managed] public static TranslatableString InstructionNotClosed { get; } = new TranslatableString("{0}文に対応する{1}がない状態で{2}文に到達しました");
            [Managed] public static TranslatableString InvalidCaseAfterCaseelse { get; } = new TranslatableString("CASEELSE文より後で\"{0}\"文が使われました");
            [Managed] public static TranslatableString UnexpectedEndselect { get; } = new TranslatableString("対応するSELECTCASEの無いENDSELECT文です");
            [Managed] public static TranslatableString NotMatchCaseTypeAndSelectcaseType { get; } = new TranslatableString("CASEの引数の型がSELECTCASEと一致しません");
            [Managed] public static TranslatableString MissingCorresponding { get; } = new TranslatableString("対応する\"{0}\"の無い\"{1}\"文です");
            [Managed] public static TranslatableString MissingTryc { get; } = new TranslatableString("対応するTRYC系命令がありません");
            [Managed] public static TranslatableString UnexpectedEndcatch { get; } = new TranslatableString("対応するCATCHのないENDCATCHです");
            [Managed] public static TranslatableString NestedPrintdata { get; } = new TranslatableString("PRINTDATA系命令が入れ子にされています");
            [Managed] public static TranslatableString StrdataInsidePrintdata { get; } = new TranslatableString("PRINTDATA系命令の中にSTRDATA系命令が含まれています");
            [Managed] public static TranslatableString NestedStrdata { get; } = new TranslatableString("STRDATA命令が入れ子にされています");
            [Managed] public static TranslatableString PrintdataInsideStrdata { get; } = new TranslatableString("STRDATA系命令の中にPRINTDATA系命令が含まれています");
            [Managed] public static TranslatableString UnexpectedDatalist { get; } = new TranslatableString("対応するPRINTDATA系命令のないDATALISTです");
            [Managed] public static TranslatableString UnexpectedEndlist { get; } = new TranslatableString("対応するDATALISTのないENDLISTです");
            [Managed] public static TranslatableString DatalistDataIsMissing { get; } = new TranslatableString("DATALIST命令に表示データが与えられていません（このDATALISTは空文字列を表示します）");
            [Managed] public static TranslatableString MissingPrintdata { get; } = new TranslatableString("対応するPRINTDATA系命令のない\"{0}\"です");
            [Managed] public static TranslatableString MissingPrintdataStrdata { get; } = new TranslatableString("対応するPRINTDATA系命令もしくはSTRDATAのない\"{0}\"です");
            [Managed] public static TranslatableString InstructionDataIsMissing { get; } = new TranslatableString("\"{0}\"命令に表示データがありません（この命令は無視されます）");
            [Managed] public static TranslatableString DatalistNotClosed { get; } = new TranslatableString("DATALISTが閉じられていません");
            [Managed] public static TranslatableString NestedTrycalllist { get; } = new TranslatableString("TRYCALLLIST系命令が入れ子にされています");
            [Managed] public static TranslatableString MissingTrycalllist { get; } = new TranslatableString("対応するTRYCALLLIST系命令のない\"{0}\"です");
            [Managed] public static TranslatableString InvalidInstructionInTrycalllist { get; } = new TranslatableString("TRYCALLLIST系命令中に無効な\"{0}\"が存在します");
            [Managed] public static TranslatableString TrygotolistToSBrackets { get; } = new TranslatableString("TRYGOTOLISTの呼び出し対象に[～～]が設定されています");
            [Managed] public static TranslatableString TrygotolistTargetHasArg { get; } = new TranslatableString("TRYGOTOLISTの呼び出し対象に引数が設定されています");
            [Managed] public static TranslatableString NestedNoskip { get; } = new TranslatableString("NOSKIP系命令が入れ子にされています");
            [Managed] public static TranslatableString MissingNoskip { get; } = new TranslatableString("対応するNOSKIP系命令のない\"{0}\"です");
            [Managed] public static TranslatableString DefaultError { get; } = new TranslatableString("ディフォルトエラー（Emuera設定漏れ）");
            [Managed] public static TranslatableString UseSingleUserFunc { get; } = new TranslatableString("式中関数では#SINGLEは機能しません");
            [Managed] public static TranslatableString UsableSingleEventFunc { get; } = new TranslatableString("イベント関数以外では#SINGLEは機能しません");
            [Managed] public static TranslatableString DuplicateSingle { get; } = new TranslatableString("#SINGLEが重複して使われています");
            [Managed] public static TranslatableString OnlyWithSingle { get; } = new TranslatableString("#ONLYが指定されたイベント関数では#SINGLEは機能しません");
            [Managed] public static TranslatableString UseLaterUserFunc { get; } = new TranslatableString("式中関数では#LATERは機能しません");
            [Managed] public static TranslatableString UsableLaterEventFunc { get; } = new TranslatableString("イベント関数以外では#LATERは機能しません");
            [Managed] public static TranslatableString DuplicateLater { get; } = new TranslatableString("#LATERが重複して使われています");
            [Managed] public static TranslatableString OnlyWithLater { get; } = new TranslatableString("#ONLYが指定されたイベント関数では#LATERは機能しません");
            [Managed] public static TranslatableString PriWithLater { get; } = new TranslatableString("#PRIと#LATERが重複して使われています(この関数は2度呼ばれます)");
            [Managed] public static TranslatableString UsePriUserFunc { get; } = new TranslatableString("式中関数では#PRIは機能しません");
            [Managed] public static TranslatableString UsablePriEventFunc { get; } = new TranslatableString("イベント関数以外では#PRIは機能しません");
            [Managed] public static TranslatableString DuplicatePri { get; } = new TranslatableString("#PRIが重複して使われています");
            [Managed] public static TranslatableString OnlyWithPri { get; } = new TranslatableString("#ONLYが指定されたイベント関数では#PRIは機能しません");
            [Managed] public static TranslatableString UseOnlyUserFunc { get; } = new TranslatableString("式中関数では#ONLYは機能しません");
            [Managed] public static TranslatableString UsableOnlyEventFunc { get; } = new TranslatableString("イベント関数以外では#ONLYは機能しません");
            [Managed] public static TranslatableString DuplicateOnly { get; } = new TranslatableString("#ONLYが重複して使われています");
            [Managed] public static TranslatableString AlreadyDeclaredOnly { get; } = new TranslatableString("このイベント関数\"@{0}\"にはすでに#ONLYが宣言されています（この関数は実行されません）");
            [Managed] public static TranslatableString BeIgnorePri { get; } = new TranslatableString("このイベント関数には#PRIが宣言されていますが無視されます");
            [Managed] public static TranslatableString BeIgnoreLater { get; } = new TranslatableString("このイベント関数には#LATERが宣言されていますが無視されます");
            [Managed] public static TranslatableString BeIgnoreSingle { get; } = new TranslatableString("このイベント関数には#SINGLEが宣言されていますが無視されます");
            [Managed] public static TranslatableString CanNotDeclaredBeginNumberFunction { get; } = new TranslatableString("\"#{0}\"属性は関数名が数字で始まる関数には指定できません");
            [Managed] public static TranslatableString FuncNameBeginNumber { get; } = new TranslatableString("関数名が数字で始まっています");
            [Managed] public static TranslatableString AlreadySharpDeclared { get; } = new TranslatableString("関数\"{0}\"にはすでに#{1}が宣言されています(この行は無視されます)");
            [Managed] public static TranslatableString AlreadyDeclaredSharpFunction { get; } = new TranslatableString("関数\"{0}\"にはすでに#FUNCTIONが宣言されています");
            [Managed] public static TranslatableString AlreadyDeclaredSharpFunctions { get; } = new TranslatableString("関数\"{0}\"にはすでに#FUNCTIONSが宣言されています");
            [Managed] public static TranslatableString UseSharpInSystemFunc { get; } = new TranslatableString("システム関数に#{0}が指定されています");
            [Managed] public static TranslatableString SharpHasNotValidValue { get; } = new TranslatableString("#{0}の後に有効な数値が指定されていません");
            [Managed] public static TranslatableString EventFuncIgnoreSpecified { get; } = new TranslatableString("イベント関数では#{0}による{1}のサイズ指定は無視されます");
            [Managed] public static TranslatableString LocalsizeLessThan1 { get; } = new TranslatableString("#{0}に0以下の値({1)が与えられました。設定は無視されます");
            [Managed] public static TranslatableString TooManyLocalsize { get; } = new TranslatableString("#{0}に大きすぎる値({1})が与えられました。設定は無視されます");
            [Managed] public static TranslatableString LocalIsProhibited { get; } = new TranslatableString("#{0}が指定されていますが変数{1}は使用禁止されています");
            [Managed] public static TranslatableString DuplicateLocalsize { get; } = new TranslatableString("この関数にはすでに#LOCALSIZEが定義されています。（以前の定義は無視されます）");
            [Managed] public static TranslatableString DuplicateLocalssize { get; } = new TranslatableString("この関数にはすでに#LOCALSSIZEが定義されています。（以前の定義は無視されます）");
            [Managed] public static TranslatableString VarNameAlreadyUsed { get; } = new TranslatableString("変数名\"{0}\"は既に使用されています");
            [Managed] public static TranslatableString ExtraCharacterAfterSharp { get; } = new TranslatableString("#の識別子の後に余分な文字があります");
            [Managed] public static TranslatableString InvalidFunc { get; } = new TranslatableString("関数名が不正であるか存在しません");
            [Managed] public static TranslatableString LabelHasArg { get; } = new TranslatableString("$で始まるラベルに引数が設定されています");
            [Managed] public static TranslatableString StartedPlusButNotIncrement { get; } = new TranslatableString("行が\'+\'から始まっていますが、インクリメントではありません");
            [Managed] public static TranslatableString StartedMinusButNotDecrement { get; } = new TranslatableString("行が\'-\'から始まっていますが、デクリメントではありません");
            [Managed] public static TranslatableString InvalidCharacterAfterInstruction1 { get; } = new TranslatableString("命令で行が始まっていますが、命令の直後に半角スペース・タブ以外の文字が来ています(この警告はシステムオプション「{0}」により無視できます)");
            [Managed] public static TranslatableString InvalidCharacterAfterInstruction2 { get; } = new TranslatableString("命令で行が始まっていますが、命令の直後に半角スペース・タブ以外の文字が来ています");
            [Managed] public static TranslatableString CanNotInterpretedLine { get; } = new TranslatableString("解釈できない行です");
            [Managed] public static TranslatableString Use2EqualToAssign { get; } = new TranslatableString("代入演算子に\"==\"が使われています");
            [Managed] public static TranslatableString CalleventToNonEventFunc { get; } = new TranslatableString("イベント関数でない関数\"@{0}\"({1}:{2}行目)に対しEVENT呼び出しが行われました");
            [Managed] public static TranslatableString CallToEventFunc { get; } = new TranslatableString("イベント関数\"@{0}\"に対し通常のCALLが行われました(このエラーは互換性オプション「{1}」により無視できます)");
            [Managed] public static TranslatableString CallToUserFunc { get; } = new TranslatableString("#FUCNTION(S)が定義された関数\"@{0}\"({1}:{2}行目)に対し通常のCALLが行われました");
            [Managed] public static TranslatableString CanNotOmitRefArg { get; } = new TranslatableString("\"@{0}\"の{1}番目の引数は参照渡しのため省略できません");
            [Managed] public static TranslatableString RequireArrayBecauseRefArg { get; } = new TranslatableString("\"@{0}\"の{1}番目の引数は参照渡しのための配列変数でなければなりません");
            [Managed] public static TranslatableString NumberOfArg { get; } = new TranslatableString("\"@{0}\"の{1}番目の引数:{2}");
            [Managed] public static TranslatableString CanNotOmitArgWithMessage { get; } = new TranslatableString("\"@{0}\"の{1}番目の引数は省略できません(この警告は互換性オプション「{2}」により無視できます)");
            [Managed] public static TranslatableString CanNotConvertStrToInt { get; } = new TranslatableString("\"@{0}\"の{1}番目の引数を文字列型から整数型に変換できません");
            [Managed] public static TranslatableString CanNotConvertIntToStr { get; } = new TranslatableString("\"@{0}\"の{1}番目の引数を整数型から文字列型に変換できません(この警告は互換性オプション「{2}」により無視できます)");
            [Managed] public static TranslatableString CalltrainArgMoreThanSelectcom { get; } = new TranslatableString("CALLTRAIN命令の引数の値がSELECTCOMの要素数を超えています");
            [Managed] public static TranslatableString SelectExitInfiniteLoopMB { get; } = new TranslatableString("無限ループの疑いにより強制終了が選択されました");
            [Managed] public static TranslatableString OverflowFuncStack { get; } = new TranslatableString("関数の呼び出しスタックが溢れました(無限に再帰呼び出しされていませんか？)");
            [Managed] public static TranslatableString FuncEndError { get; } = new TranslatableString("関数の終端でエラーが発生しました:{0}");
            [Managed] public static TranslatableString FuncEndEmueraError { get; } = new TranslatableString("関数の終端でEmueraのエラーが発生しました:{0}");
            [Managed] public static TranslatableString FuncEndUnexpectedError { get; } = new TranslatableString("関数の終端で予期しないエラーが発生しました:{0}");
            [Managed] public static TranslatableString ErrorFileAndLine { get; } = new TranslatableString("{0}の{1}行目で");
            [Managed] public static TranslatableString ErrorFile { get; } = new TranslatableString("{0}で");
            [Managed] public static TranslatableString HasThrow { get; } = new TranslatableString("{0}THROWが発生しました");
            [Managed] public static TranslatableString HasError { get; } = new TranslatableString("{0}エラーが発生しました:{1}");
            [Managed] public static TranslatableString ThrowMessage { get; } = new TranslatableString("THROW内容：{0}");
            [Managed] public static TranslatableString ErrorMessage { get; } = new TranslatableString("エラー内容：{0}");
            [Managed] public static TranslatableString ErrorInFunc { get; } = new TranslatableString("現在の関数：@{0}（{1}の{2}行目）");
            [Managed] public static TranslatableString FuncCallStack { get; } = new TranslatableString("関数呼び出しスタック：");
            [Managed] public static TranslatableString ErrorFuncStack { get; } = new TranslatableString("{0}の{1}行目（関数@{2}内）");
            [Managed] public static TranslatableString HasEmueraError { get; } = new TranslatableString("{0}Emueraのエラーが発生しました:{1}");
            [Managed] public static TranslatableString HasUnexpectedError { get; } = new TranslatableString("{0}予期しないエラーが発生しました:{1}");
            [Managed] public static TranslatableString SkipdispInputError1 { get; } = new TranslatableString("表示スキップ中にデフォルト値を持たないINPUTに遭遇しました");
            [Managed] public static TranslatableString SkipdispInputError2 { get; } = new TranslatableString("INPUTに必要な処理をNOSKIP～ENDNOSKIPで囲むか、SKIPDISP 0～SKIPDISP 1で囲ってください");
            [Managed] public static TranslatableString SkipdispInputError3 { get; } = new TranslatableString("無限ループに入る可能性が高いため実行を終了します");
            [Managed] public static TranslatableString DoFailedLine { get; } = new TranslatableString("読込に失敗した行が実行されました。エラーの詳細は読込時の警告を参照してください。");
            [Managed] public static TranslatableString OoRPickupcharaArg { get; } = new TranslatableString("命令PICKUPCHARAの第{0}引数にキャラリストの範囲外の値({1})が与えられました");
            [Managed] public static TranslatableString CanNotUseOutsideSystemtitle { get; } = new TranslatableString("@SYSTEM_TITLE以外でこの命令を使うことはできません");
            [Managed] public static TranslatableString SavedataArgIsNegative { get; } = new TranslatableString("SAVEDATAの引数に負の値({0})が指定されました");
            [Managed] public static TranslatableString TooLargeSavedataArg { get; } = new TranslatableString("SAVEDATAの引数({0})が大きすぎます");
            [Managed] public static TranslatableString SavetextContainNewLineCharacter { get; } = new TranslatableString("SAVEDATAのセーブテキストに改行文字が与えられました（セーブデータが破損するため改行文字は使えません）");
            [Managed] public static TranslatableString UnexpectedErrorInSavedata { get; } = new TranslatableString("SAVEDATA命令によるセーブ中に予期しないエラーが発生しました");
            [Managed] public static TranslatableString PowerResultNonNumeric { get; } = new TranslatableString("累乗結果が非数値です");
            [Managed] public static TranslatableString PowerResultInfinite { get; } = new TranslatableString("累乗結果が無限大です");
            [Managed] public static TranslatableString PowerResultOverflow { get; } = new TranslatableString("累乗結果({0})が64ビット符号付き整数の範囲外です");
            [Managed] public static TranslatableString VarsTypeDifferent { get; } = new TranslatableString("入れ替える変数の型が異なります");
            [Managed] public static TranslatableString UnknownVarType { get; } = new TranslatableString("不明な変数型です");
            [Managed] public static TranslatableString SetcolorArgLessThan0 { get; } = new TranslatableString("SETCOLORの引数に0未満の値が指定されました");
            [Managed] public static TranslatableString SetcolorArgOver255 { get; } = new TranslatableString("SETCOLORの引数に255を超える値が指定されました");
            [Managed] public static TranslatableString InvalidAlignment { get; } = new TranslatableString("ALIGNMENTのキーワード\"{0}\"は未定義です");
            [Managed] public static TranslatableString MissingEndnoskip { get; } = new TranslatableString("対応するENDNOSKIPのないNOSKIPです");
            [Managed] public static TranslatableString IsUsableOnly1DVar { get; } = new TranslatableString("{0}は1次元配列および配列型キャラクタ変数のみに対応しています");
            [Managed] public static TranslatableString tooLongEncodetouniArg { get; } = new TranslatableString("ENCODETOUNIの引数が長すぎます（現在{0}文字。最大{1}文字まで）");
            [Managed] public static TranslatableString AssertArgIs0 { get; } = new TranslatableString("ASSERT文の引数が0です");
            [Managed] public static TranslatableString LoaddataArgIsNegative { get; } = new TranslatableString("LOADDATAの引数に負の値({0})が指定されました");
            [Managed] public static TranslatableString TooLargeLoaddataArg { get; } = new TranslatableString("LOADDATAの引数({0})が大きすぎます");
            [Managed] public static TranslatableString LoadCorruptedData { get; } = new TranslatableString("不正なデータをロードしようとしました");
            [Managed] public static TranslatableString UnexpectedErrorInLoaddata { get; } = new TranslatableString("ファイルのロード中に予期しないエラーが発生しました");
            [Managed] public static TranslatableString CanNotUseDotrainHere { get; } = new TranslatableString("DOTRAIN命令をこの位置で実行することはできません");
            [Managed] public static TranslatableString DotrainArgLessThan0 { get; } = new TranslatableString("DOTRAIN命令に0未満の値が渡されました");
            [Managed] public static TranslatableString DotrainArgOverTrainnameArray { get; } = new TranslatableString("DOTRAIN命令にTRAINNAMEの配列数以上の値が渡されました");
            [Managed] public static TranslatableString UndefinedFunc { get; } = new TranslatableString("未定義の関数です");
            [Managed] public static TranslatableString InvalidBeginArg { get; } = new TranslatableString("BEGINのキーワード\"{0}\"は未定義です");
            [Managed] public static TranslatableString CalleventBeforeFinishEvent { get; } = new TranslatableString("EVENT関数の解決前にCALLEVENT命令が行われました");
            [Managed] public static TranslatableString FuncIsNotFound { get; } = new TranslatableString("関数\"@{0}\"が見つかりません");
            [Managed] public static TranslatableString InvalidValue { get; } = new TranslatableString("無効な値です");
            [Managed] public static TranslatableString ExecutedCom { get; } = new TranslatableString("＜コマンド連続実行：{0}/{1}＞");
            [Managed] public static TranslatableString CouldNotExecuteCom { get; } = new TranslatableString("コマンドを実行できませんでした");
            [Managed] public static TranslatableString AutoSaveError1 { get; } = new TranslatableString("オートセーブ中に予期しないエラーが発生しました");
            [Managed] public static TranslatableString AutoSaveError2 { get; } = new TranslatableString("オートセーブをスキップします");
            [Managed] public static TranslatableString NotEnoughMoney { get; } = new TranslatableString("お金が足りません。");
            [Managed] public static TranslatableString OutOfStock { get; } = new TranslatableString("売っていません。");
            [Managed] public static TranslatableString UnexpectedSaveError { get; } = new TranslatableString("セーブ中に予期しないエラーが発生しました");
            [Managed] public static TranslatableString NoData { get; } = new TranslatableString("データがありません");
            [Managed] public static TranslatableString UnexpectedScriptEnd { get; } = new TranslatableString("予期しないスクリプト終端です");
            [Managed] public static TranslatableString CanNotSpecifiedKeyword { get; } = new TranslatableString("{0}中では{1}キーワードは指定できません");
            [Managed] public static TranslatableString NotIdentifierAfterKeyword { get; } = new TranslatableString("{0}の後に有効な識別子が指定されていません");
            [Managed] public static TranslatableString NotIdentifierArg { get; } = new TranslatableString("識別子の後に引数定義がありません");
            [Managed] public static TranslatableString RefArgIsNotArray { get; } = new TranslatableString("REF引数は配列変数でなければなりません");
            [Managed] public static TranslatableString RefArrayCanNotMoreThan4 { get; } = new TranslatableString("REF引数は4次元以上の配列にできません");
            [Managed] public static TranslatableString ExtraCharacterAfterDeclaration { get; } = new TranslatableString("宣言の後に余分な文字があります");
            [Managed] public static TranslatableString UnexpectedToken { get; } = new TranslatableString("引数の解析中に予期しないトークン\"{0}\"を発見しました");
            [Managed] public static TranslatableString ArgParsingError { get; } = new TranslatableString("引数の解析中にエラーが発生しました");
            [Managed] public static TranslatableString CanNotSpecifiedWith { get; } = new TranslatableString("{0}と{1}キーワードは同時に指定できません");
            [Managed] public static TranslatableString DuplicateKeyword { get; } = new TranslatableString("{0}キーワードが二重に指定されています");
            [Managed] public static TranslatableString CanNotUseKeywordGlobalVar { get; } = new TranslatableString("広域変数の宣言に{0}キーワードは指定できません");
            [Managed] public static TranslatableString CanNotUseKeywordLocalVar { get; } = new TranslatableString("ローカル変数の宣言に{0}キーワードは指定できません");
            [Managed] public static TranslatableString NotVarAfterKeyword { get; } = new TranslatableString("{0}の後に有効な変数名が指定されていません");
            [Managed] public static TranslatableString ConstHasNotInitialValue { get; } = new TranslatableString("CONSTキーワードが指定されていますが初期値が設定されていません");
            [Managed] public static TranslatableString HasNotExpressionAfterComma { get; } = new TranslatableString("カンマの後に有効な定数式が指定されていません");
            [Managed] public static TranslatableString CanNotSizedRef { get; } = new TranslatableString("参照型変数にはサイズを指定できません(サイズを省略するか0を指定してください)");
            [Managed] public static TranslatableString OoRDefinable { get; } = new TranslatableString("ユーザー定義変数のサイズは1以上1000000以下でなければなりません");
            [Managed] public static TranslatableString UnexpectedOp { get; } = new TranslatableString("予期しない演算子を発見しました");
            [Managed] public static TranslatableString CanNotSetInitialValue { get; } = new TranslatableString("{0}変数には初期値を設定できません");
            [Managed] public static TranslatableString ArrayVarCanNotOmitInitialValue { get; } = new TranslatableString("配列の初期値は省略できません");
            [Managed] public static TranslatableString InitialValueMoreThanArraySize { get; } = new TranslatableString("初期値の数が配列のサイズを超えています");
            [Managed] public static TranslatableString ConstInitialValueDifferentArraySize { get; } = new TranslatableString("定数の初期値の数が配列のサイズと一致しません");
            [Managed] public static TranslatableString InitialValueOnlyConst { get; } = new TranslatableString("配列の初期値には定数のみ指定できます");
            [Managed] public static TranslatableString NotMatchVarTypeAndInitialValue { get; } = new TranslatableString("変数の型と初期値の型が一致していません");
            [Managed] public static TranslatableString CanNotDeclareConstArray { get; } = new TranslatableString("CONSTキーワードが指定された変数を多次元配列にはできません");
            [Managed] public static TranslatableString CharaVarCanNotDeclareMoreThan3D { get; } = new TranslatableString("3次元以上のキャラ型変数を宣言することはできません");
            [Managed] public static TranslatableString VarCanNotDeclareMoreThan4D { get; } = new TranslatableString("4次元以上の配列変数を宣言することはできません");
            [Managed] public static TranslatableString StrVarrRequiredBinaryOption { get; } = new TranslatableString("文字列型の多次元配列変数にSAVEDATAフラグを付ける場合には「バイナリ型セーブ」オプションが必須です");
            [Managed] public static TranslatableString CharaStrRequiredBinaryOption { get; } = new TranslatableString("キャラ型変数にSAVEDATAフラグを付ける場合には「バイナリ型セーブ」オプションが必須です");
            [Managed] public static TranslatableString ForceQuitAndRestartError { get; } = new TranslatableString("FORCE_QUIT_AND_RESTARTが連続実行されました");
            [Managed] public static TranslatableString ProgramStatusError { get; } = new TranslatableString("emueraのエラー：プログラムの状態を特定できません");
            [Managed] public static TranslatableString FailedOpenEditor { get; } = new TranslatableString("エディタを開くことができませんでした");
            [Managed] public static TranslatableString CanNotInputTimerWait { get; } = new TranslatableString("タイマー系命令の待ち時間中はコマンドを入力できません");
            [Managed] public static TranslatableString CanNotInputScriptRunning { get; } = new TranslatableString("スクリプト実行中はコマンドを入力できません");
            [Managed] public static TranslatableString CanNotUseDebugWindow { get; } = new TranslatableString("デバッグウインドウは-Debug引数付きで起動したときのみ使えます");
            [Managed] public static TranslatableString CanNotUseDebugCommand { get; } = new TranslatableString("デバッグコマンドを使用できない設定になっています");
            [Managed] public static TranslatableString InvalidDebugCommand { get; } = new TranslatableString("デバッグコマンドで使用できるのは代入文か命令文だけです");
            [Managed] public static TranslatableString CanNotUseFlowInstruction { get; } = new TranslatableString("フロー制御命令は使用できません");
            [Managed] public static TranslatableString CanNotUseInstruction { get; } = new TranslatableString("{0}命令は使用できません");
            [Managed] public static TranslatableString CanNotUseWhenError { get; } = new TranslatableString("エラー発生時はこの機能は使えません");
            [Managed] public static TranslatableString CanNotUseWhenInitialize { get; } = new TranslatableString("初期化中はこの機能は使えません");
            [Managed] public static TranslatableString Warning1 { get; } = new TranslatableString("警告Lv{0}:{1}:{2}行目:{3}");
            [Managed] public static TranslatableString Warning2 { get; } = new TranslatableString("警告Lv{0}:{1}:{2}");
            [Managed] public static TranslatableString Warning3 { get; } = new TranslatableString("警告Lv{0}:{1}");
            [Managed] public static TranslatableString EmptyDrawline { get; } = new TranslatableString("空文字列によるDRAWLINEが行われました");
            [Managed] public static TranslatableString TextAfterP { get; } = new TranslatableString("</p>の後にテキストがあります");
            [Managed] public static TranslatableString TextAfterNobr { get; } = new TranslatableString("</nobr>の後にテキストがあります");
            [Managed] public static TranslatableString NotFoundCloseTag { get; } = new TranslatableString("コメント終了タグ\"-->\"がみつかりません");
            [Managed] public static TranslatableString NotFoundTerminateTag { get; } = new TranslatableString("タグ終端'>'が見つかりません");
            [Managed] public static TranslatableString TagIsNotClosed { get; } = new TranslatableString("閉じられていないタグがあります");
            [Managed] public static TranslatableString CanNotUsePosWithoutNobr { get; } = new TranslatableString("<nobr>が設定されていない行ではpos属性は使用できません");
            [Managed] public static TranslatableString CanOnlyUsePosAssignmentLR { get; } = new TranslatableString("alignがleftでない行ではpos属性は使用できません");
            [Managed] public static TranslatableString MissingSemicolon { get; } = new TranslatableString("'&'に対応する';'がみつかりません");
            [Managed] public static TranslatableString ContinuouslyAndSemicolon { get; } = new TranslatableString("'&'と';'が連続しています");
            [Managed] public static TranslatableString InvalidCharacterReference { get; } = new TranslatableString("\"&{0};\"は適切な文字参照ではありません");
            [Managed] public static TranslatableString OoRUnicodeHtml { get; } = new TranslatableString("\"&{0};\"はUnicodeの範囲外です(サロゲートペアは使えません)");
            [Managed] public static TranslatableString UnexpectedCloseTag { get; } = new TranslatableString("</{0}>の前に<{0}>がありません");
            [Managed] public static TranslatableString CanNotInterpretCloseTag { get; } = new TranslatableString("終了タグ</{0}>は解釈できません");
            [Managed] public static TranslatableString AttributeSetToTag { get; } = new TranslatableString("<{0}>タグにに属性が設定されています");
            [Managed] public static TranslatableString DuplicateTag { get; } = new TranslatableString("<{0}>が二重に使われています");
            [Managed] public static TranslatableString TagIsNotBegin { get; } = new TranslatableString("<{0}>が行頭以外で使われています");
            [Managed] public static TranslatableString TagHasNotAttribute { get; } = new TranslatableString("<{0}>タグに属性が設定されていません");
            [Managed] public static TranslatableString CanNotInterpretPAttribute { get; } = new TranslatableString("<p>タグの属性名{0}は解釈できません");
            [Managed] public static TranslatableString CanNotInterpretAttribute { get; } = new TranslatableString("属性値{0}は解釈できません");
            [Managed] public static TranslatableString DuplicateAttribute { get; } = new TranslatableString("<{0}>タグに{1}属性が2度以上指定されています");
            [Managed] public static TranslatableString AttributeCanNotInterpretNum { get; } = new TranslatableString("<{0}>タグの{1}属性の属性値が数値として解釈できません");
            [Managed] public static TranslatableString CanNotInterpretAttributeName { get; } = new TranslatableString("<{0}>タグの属性名{1}は解釈できません");
            [Managed] public static TranslatableString NotSetAttribute { get; } = new TranslatableString("<{0}>タグに{1}属性が設定されていません");
            [Managed] public static TranslatableString NestedButtonTag { get; } = new TranslatableString("<button>又は<nonbutton>が入れ子にされています");
            [Managed] public static TranslatableString NestedClearbuttonTag { get; } = new TranslatableString("<clearbutton>が入れ子にされています");
            [Managed] public static TranslatableString ClearbuttonAttributeCanNotInterpretNum { get; } = new TranslatableString("<{0}>タグに{1}属性の値\"{2}\"は解釈できません");
            [Managed] public static TranslatableString HtmlTagError { get; } = new TranslatableString("html文字列\"{0}\"のタグ解析中にエラーが発生しました");
            [Managed] public static TranslatableString RequireColorCode { get; } = new TranslatableString("色を表す単語又は#RRGGBB値が必要です");
            [Managed] public static TranslatableString OoRColorValue { get; } = new TranslatableString("{0}は適切な色指定の範囲外です");
            [Managed] public static TranslatableString CanNotInterpretNumValue { get; } = new TranslatableString("{0}は数値として解釈できません");
            [Managed] public static TranslatableString InvalidColorName2 { get; } = new TranslatableString("指定された色名\"{0}\"は無効な色名です(16進数で色を指定する場合には数値の前に#が必要です)");
            [Managed] public static TranslatableString BufferOverFlow { get; } = new TranslatableString("※※※バッファーの文字数が2000字(全角1000字)を超えています。これ以降は表示できません※※※");
            [Managed] public static TranslatableString CanNotUseFuncCurrentVer { get; } = new TranslatableString("この機能は現バージョンでは使えません");
            [Managed] public static TranslatableString AbnormalFileData { get; } = new TranslatableString("ファイルデータ型異常");
            [Managed] public static TranslatableString AbnormalBinaryData { get; } = new TranslatableString("バイナリデータの異常");
            [Managed] public static TranslatableString InvalidStream { get; } = new TranslatableString("無効なストリームです");
            [Managed] public static TranslatableString NoStrToRead { get; } = new TranslatableString("読み取るべき文字列がありません");
            [Managed] public static TranslatableString NoNumToRead { get; } = new TranslatableString("読み取るべき数値がありません");
            [Managed] public static TranslatableString CanNotInterpretNum { get; } = new TranslatableString("数値として認識できません");
            [Managed] public static TranslatableString InvalidArray { get; } = new TranslatableString("無効な配列が渡されました");
            [Managed] public static TranslatableString UnexpectedSaveDataEnd { get; } = new TranslatableString("予期しないセーブデータの終端です");
            [Managed] public static TranslatableString InvalidSaveDataFormat { get; } = new TranslatableString("セーブデータの形式が不正です");
            [Managed] public static TranslatableString NotSupportStringArray2D { get; } = new TranslatableString("StringArray2Dのロードには対応していません");
            [Managed] public static TranslatableString UnexpectedContinuationEnd { get; } = new TranslatableString("予期しない行連結終端記号'}'が見つかりました");
            [Managed] public static TranslatableString CharacterAfterContinuation { get; } = new TranslatableString("行連結始端記号'{'の行に'{'以外の文字を含めることはできません");
            [Managed] public static TranslatableString NotCloseLineContinuation { get; } = new TranslatableString("行連結始端記号'{'が使われましたが終端記号'}'が見つかりません");
            [Managed] public static TranslatableString CharacterAfterContinuationEnd { get; } = new TranslatableString("行連結終端記号'}'の行に'}'以外の文字を含めることはできません");
            [Managed] public static TranslatableString UnexpectedContinuation { get; } = new TranslatableString("予期しない行連結始端記号'{'が見つかりました");
            [Managed] public static TranslatableString OoRInt64 { get; } = new TranslatableString("\"{0}\"は64ビット符号付き整数の範囲を超えています");
            [Managed] public static TranslatableString CanNotUseBinaryNotate { get; } = new TranslatableString("二進法表記の中で使用できない文字が使われています");
            [Managed] public static TranslatableString LineBeginsIllegalCharacter { get; } = new TranslatableString("不正な文字で行が始まっています");
            [Managed] public static TranslatableString MacroOverLimit { get; } = new TranslatableString("マクロの展開数が1文あたりの上限値{0}を超えました(自己参照・循環参照のおそれ)");
            [Managed] public static TranslatableString MacroIsNotAvailable { get; } = new TranslatableString("マクロ{0}はこの文脈では使用できません(1単語に置き換えるマクロのみが使用できます)");
            [Managed] public static TranslatableString UnexpectedFullWidthSpace { get; } = new TranslatableString("予期しない全角スペースを発見しました(この警告はシステムオプション「{0}」により無視できます)");
            [Managed] public static TranslatableString MissingCharacterAfterEscape { get; } = new TranslatableString("エスケープ文字\\の後に文字がありません");
            [Managed] public static TranslatableString UnexpectedEqual { get; } = new TranslatableString("予期しない代入演算子'='を発見しました(等価比較には'=='を使用してください)");
            [Managed] public static TranslatableString CanNotRecognizedOp { get; } = new TranslatableString("'{0}'は演算子として認識できません");
            [Managed] public static TranslatableString CanNotRecognizedAssignOp { get; } = new TranslatableString("\"{0}\"は代入演算子として認識できません");
            [Managed] public static TranslatableString UnexpectedCharacter { get; } = new TranslatableString("字句解析中に予期しない文字'{0}'を発見しました");
            [Managed] public static TranslatableString EmptyTwoSBrackets { get; } = new TranslatableString("空の[[]]です");
            [Managed] public static TranslatableString MissingTwoSBrackets { get; } = new TranslatableString("対応する\"]]\"のない\"[[\"です");
            [Managed] public static TranslatableString CanNotRenameKey { get; } = new TranslatableString("字句解析中に置換(rename)できない符号{0}を発見しました");
            [Managed] public static TranslatableString NotClosed { get; } = new TranslatableString("\"{0}\"が閉じられていません");
            [Managed] public static TranslatableString MacroHasNotArg { get; } = new TranslatableString("関数形式のマクロ{0}に引数がありません");
            [Managed] public static TranslatableString WrongMacroUsage { get; } = new TranslatableString("関数形式のマクロ{0}の用法が正しくありません");
            [Managed] public static TranslatableString MacroDifferentArgCount { get; } = new TranslatableString("関数形式のマクロ{0}の引数の数が正しくありません");
            [Managed] public static TranslatableString CanNotOmitMacroArg { get; } = new TranslatableString("関数形式のマクロ{0}の引数を省略することはできません");
            [Managed] public static TranslatableString NotFoundCorresponding { get; } = new TranslatableString("\'{0}\'が使われましたが対応する\'{1}\'が見つかりません");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            [Managed] public static TranslatableString CannotRecommendCallLocalVar { get; } = new TranslatableString("コード中でローカル変数を@付きで呼ぶことは推奨されません(代わりに*.ERHファイルの利用を検討してください)");

            [Managed] public static TranslatableString LabelNameMissing { get; } = new TranslatableString("ラベル名がありません");
            [Managed] public static TranslatableString LabelContainsOtherThanUnderline { get; } = new TranslatableString("ラベル名\"{0}\"に\"_\"以外の記号が含まれています");
            [Managed] public static TranslatableString LabelStartedHalfDigit { get; } = new TranslatableString("ラベル名\"{0}\"が半角数字から始まっています");
            [Managed] public static TranslatableString LabelConflictReservedWord1 { get; } = new TranslatableString("関数名\"{0}\"はEmueraの予約語と衝突しています。Emuera専用構文の構文解析に支障をきたす恐れがあります");
            [Managed] public static TranslatableString LabelConflictReservedWord2 { get; } = new TranslatableString("関数名\"{0}\"はEmueraの予約語です");
            [Managed] public static TranslatableString LabelOverwriteInternalExpression { get; } = new TranslatableString("関数名\"{0}\"はEmueraの式中関数を上書きします");
            [Managed] public static TranslatableString LabelNameAlreadyUsedInternalExpression { get; } = new TranslatableString("関数名\"{0}\"はEmueraの式中関数名として使われています");
            [Managed] public static TranslatableString LabelNameAlreadyUsedInternalVariable { get; } = new TranslatableString("関数名\"{0}\"はEmueraの変数で使われています");
            [Managed] public static TranslatableString LabelNameAlreadyUsedInternalInstruction { get; } = new TranslatableString("関数名\"{0}\"はEmueraの変数もしくは命令で使われています");
            [Managed] public static TranslatableString LabelNameAlreadyUsedMacro { get; } = new TranslatableString("関数名\"{0}\"はマクロに使用されています");
            [Managed] public static TranslatableString LabelNameAlreadyUsedRefFunction { get; } = new TranslatableString("関数名\"{0}\"は参照型関数の名称に使用されています");

            [Managed] public static TranslatableString VarContainsOtherThanUnderline { get; } = new TranslatableString("変数名\"{0}\"に\"_\"以外の記号が含まれています");
            [Managed] public static TranslatableString VarConflictReservedWord { get; } = new TranslatableString("変数名\"{0}\"はEmueraの予約語です");
            [Managed] public static TranslatableString VarNameAlreadyUsedInternalInstruction { get; } = new TranslatableString("変数名\"{0}\"はEmueraの変数もしくは命令で使われています");
            [Managed] public static TranslatableString VarNameAlreadyUsedInternalVariable { get; } = new TranslatableString("変数名\"{0}\"はEmueraの変数名として使われています");
            [Managed] public static TranslatableString VarNameAlreadyUsedMacro { get; } = new TranslatableString("変数名\"{0}\"はマクロに使用されています");
            [Managed] public static TranslatableString VarNameAlreadyUsedGlobalVariable { get; } = new TranslatableString("変数名\"{0}\"はユーザー定義の広域変数名に使用されています");
            [Managed] public static TranslatableString VarNameAlreadyUsedRefFunction { get; } = new TranslatableString("変数名\"{0}\"は参照型関数の名称に使用されています");
            [Managed] public static TranslatableString VarStartedHalfDigit { get; } = new TranslatableString("変数名\"{0}\"が半角数字から始まっています");

            [Managed] public static TranslatableString MacroContainsOtherThanUnderline { get; } = new TranslatableString("マクロ名\"{0}\"に\"_\"以外の記号が含まれています");
            [Managed] public static TranslatableString MacroConflictReservedWord { get; } = new TranslatableString("マクロ名\"{0}\"はEmueraの予約語です");
            [Managed] public static TranslatableString MacroNameAlreadyUsedInternalInstruction { get; } = new TranslatableString("マクロ名\"{0}\"はEmueraの変数もしくは命令で使われています");
            [Managed] public static TranslatableString MacroNameAlreadyUsedInternalVariable { get; } = new TranslatableString("マクロ名\"{0}\"はEmueraの変数名として使われています");
            [Managed] public static TranslatableString MacroNameAlreadyUsedMacro { get; } = new TranslatableString("マクロ名\"{0}\"はマクロに使用されています");
            [Managed] public static TranslatableString MacroNameAlreadyUsedGlobalVariable { get; } = new TranslatableString("マクロ名\"{0}\"はユーザー定義の広域変数名に使用されています");
            [Managed] public static TranslatableString MacroNameAlreadyUsedRefFunction { get; } = new TranslatableString("マクロ名\"{0}\"は参照型関数の名称に使用されています");


            [Managed] public static TranslatableString ArgCanNotBeNull { get; } = new TranslatableString("{0}関数: 第{1}引数は省略できません");
            [Managed] public static TranslatableString ArgIsNotCharacterVar { get; } = new TranslatableString("{0}関数: 第{1}引数の変数がキャラクタ変数ではありません");
            [Managed] public static TranslatableString InvalidArgType { get; } = new TranslatableString("{0}関数: 第{1}引数の型が正しくありません");
            [Managed] public static TranslatableString ArgIsNotStr { get; } = new TranslatableString("{0}関数: 第{1}引数は文字列ではありません");
            [Managed] public static TranslatableString ArgIsNotInt { get; } = new TranslatableString("{0}関数: 第{1}引数は整数ではありません");
            [Managed] public static TranslatableString ArgIsNotVar { get; } = new TranslatableString("{0}関数: 第{1}引数は変数ではありません");
            [Managed] public static TranslatableString ArgIsNotStrVar { get; } = new TranslatableString("{0}関数: 第{1}引数は文字列型変数ではありません");
            [Managed] public static TranslatableString ArgIsNotIntVar { get; } = new TranslatableString("{0}関数: 第{1}引数は整数型変数ではありません");
            [Managed] public static TranslatableString ArgIsNotArray { get; } = new TranslatableString("{0}関数: 第{1}引数は配列変数ではありません");
            [Managed] public static TranslatableString ArgIsNotStrArray { get; } = new TranslatableString("{0}関数: 第{1}引数は文字列型配列変数ではありません");
            [Managed] public static TranslatableString ArgIsNotIntArray { get; } = new TranslatableString("{0}関数: 第{1}引数は整数型配列変数ではありません");
            [Managed] public static TranslatableString ArgIsNotNDArray { get; } = new TranslatableString("{0}関数: 第{1}引数は{2}次元配列変数ではありません");
            [Managed] public static TranslatableString ArgIsNotNDStrArray { get; } = new TranslatableString("{0}関数: 第{1}引数は文字列型{2}次元配列変数ではありません");
            [Managed] public static TranslatableString ArgIsNotNDIntArray { get; } = new TranslatableString("{0}関数: 第{1}引数は整数型{2}次元配列変数ではありません");
            [Managed] public static TranslatableString TooManyFuncArgs { get; } = new TranslatableString("{0}関数: 引数が多すぎます");
            [Managed] public static TranslatableString NotEnoughArgs { get; } = new TranslatableString("{0}関数: 少なくとも{1}つの引数が必要です");
            [Managed] public static TranslatableString ArgsCountNotMatches { get; } = new TranslatableString("{0}関数: {1}つの引数が必要ですが，{2}つが与えられています");
            [Managed] public static TranslatableString ArgsNotNeeded { get; } = new TranslatableString("{0}関数: 引数の必要がありません");
            [Managed] public static TranslatableString NotValidArgs { get; } = new TranslatableString("{0}関数: 引数がどの書式にも合わせていません | {1}");
            [Managed] public static TranslatableString NotValidArgsReason { get; } = new TranslatableString("書式{0}: {1}");

            [Managed] public static TranslatableString IsNotVar { get; } = new TranslatableString("\"{0}\"が変数ではありません");
            [Managed] public static TranslatableString IsNotInt { get; } = new TranslatableString("\"{0}\"が整数型ではありません");
            [Managed] public static TranslatableString IsNotStr { get; } = new TranslatableString("\"{0}\"が文字列型ではありません");

            [Managed] public static TranslatableString SPCharacterFeatureDisabled { get; } = new TranslatableString("SPキャラ関係の機能は標準では使用できません(互換性オプション「SPキャラを使用する」をONにしてください)");
            [Managed] public static TranslatableString CharacterIndexOutOfRange { get; } = new TranslatableString("{0}関数: 第{1}引数({2})はキャラクタ位置の範囲外です");
            [Managed] public static TranslatableString NotVariableName { get; } = new TranslatableString("{0}関数: 第{1}引数(\"{2}\")が変数名ではありません");
            [Managed] public static TranslatableString ArgIsNegative { get; } = new TranslatableString("{0}関数: 第{1}引数に負の値({2})が指定されました");
            [Managed] public static TranslatableString ArgIsNotMoreThan0 { get; } = new TranslatableString("{0}関数: 第{1}引数に0以下の値({2})が指定されました");
            [Managed] public static TranslatableString ArgIsTooLarge { get; } = new TranslatableString("{0}関数: 第{1}引数({2})が大きすぎます");
            [Managed] public static TranslatableString FuncDeprecated { get; } = new TranslatableString("関数{0}()は推奨されません。代わりに関数{1}()を使用してください");
            [Managed] public static TranslatableString ArgIsOutOfRange { get; } = new TranslatableString("{0}関数: 第{1}引数({2})が{3}から{4}の範囲外です");
            [Managed] public static TranslatableString ArgIsOutOfRangeExcept { get; } = new TranslatableString("{0}関数: 第{1}引数({2})が{3}から{4}({5}を除く)の範囲外です");
            [Managed] public static TranslatableString InvalidFormat { get; } = new TranslatableString("{0}関数: 第{1}引数の書式指定が間違っています");
            [Managed] public static TranslatableString NegativeMaximum { get; } = new TranslatableString("{0}関数: 最大値に0以下の値({1})が指定されました");
            [Managed] public static TranslatableString MaximumLowerThanMinimum { get; } = new TranslatableString("{0}関数: 最大値に最小値以下の値({1})が指定されました");

            [Managed] public static TranslatableString ResultIsNaN { get; } = new TranslatableString("{0}関数: 計算結果が非数値です");
            [Managed] public static TranslatableString ResultIsInfinity { get; } = new TranslatableString("{0}関数: 計算結果が無限大です");
            [Managed] public static TranslatableString ResultIsOutOfTheRangeOfInt64 { get; } = new TranslatableString("{0}関数: 計算結果({1})が64ビット符号付き整数の範囲外です");

            [Managed] public static TranslatableString CharacterRangeInvalid { get; } = new TranslatableString("{0}関数: 範囲指定がキャラクタ配列の範囲を超えています({1}～{2})");
            [Managed] public static TranslatableString InvalidUnicode { get; } = new TranslatableString("{0}関数: 制御文字に対応する値(0x{1:X})が渡されました");
            [Managed] public static TranslatableString ArgShouldBeSpecificValue { get; } = new TranslatableString("{0}関数: 第{1}引数は{2}のいずれかでなければなりません");

            [Managed] public static TranslatableString EncodeToUni2ndArgError { get; } = new TranslatableString("{0}関数: 第2引数({1})が第1引数の文字列({2})の文字数を超えています");
            [Managed] public static TranslatableString ArgIsEmptyString { get; } = new TranslatableString("{0}関数: 第{1}引数が空文字列です");

            [Managed] public static TranslatableString InvalidFormString { get; } = new TranslatableString("{0}関数: 文字列\"{1}\"の展開エラー: {2}");
            [Managed] public static TranslatableString UnexectedFormStringErr { get; } = new TranslatableString("{0}関数: 文字列\"{1}\"の展開処理中にエラーが発生しました");

            [Managed] public static TranslatableString InvalidType { get; } = new TranslatableString("{0}関数: 型が違います ({1}関数を使用してください)");
            [Managed] public static TranslatableString GIdIsNegative { get; } = new TranslatableString("{0}関数: GraphicsIDに負の値({1})が指定されました");
            [Managed] public static TranslatableString GIdIsTooLarge { get; } = new TranslatableString("{0}関数: GraphicsIDの値({1})が大きすぎます");
            [Managed] public static TranslatableString InvalidColorARGB { get; } = new TranslatableString("{0}関数: ColorARGB引数に不適切な値(0x{1:X})が指定されました");
            [Managed] public static TranslatableString InvalidColorMatrix { get; } = new TranslatableString("{0}関数: ColorMatrixの指定された要素({1}, {2})が不適切であるか5x5に足りていません");
            [Managed] public static TranslatableString GDIPlusOnly { get; } = new TranslatableString("{0}関数: 描画オプションがWINAPIの時には使用できません");
            [Managed] public static TranslatableString GParamIsNegative { get; } = new TranslatableString("{0}関数: Graphicsの{1}に0以下の値({2})が指定されました");
            [Managed] public static TranslatableString GParamTooLarge { get; } = new TranslatableString("{0}関数: Graphicsの{1}に{2}以上の値({3})が指定されました");
            [Managed] public static TranslatableString ImgRefOutOfRange { get; } = new TranslatableString("{0}関数: 画像の範囲外が指定されています");

            [Managed] public static TranslatableString MinInt64CanNotApplyABS { get; } = new TranslatableString("{0}関数: 符号付き64bit整数の最小値({1})に対して絶対値を取ることはできません");
            
            [Managed] public static TranslatableString UnsupportedType { get; } = new TranslatableString("{0}関数: 対応していない型が指定されています");
            [Managed] public static TranslatableString ArgsNotFitExpr { get; } = new TranslatableString("{0}関数: 引数の数({1})が{2}+{3}nではありません");
            [Managed] public static TranslatableString DTLackOfNamedColumn { get; } = new TranslatableString("{0}関数: DataTable(\"{1}\")に\"{2}\"を名前とした列がありません");
            [Managed] public static TranslatableString DTInvalidDataType{ get; } = new TranslatableString("{0}関数: DataTable(\"{1}\")の\"{2}\"列に違う型の値を指定しようとしています");
            [Managed] public static TranslatableString DTCanNotEditIdColumn { get; } = new TranslatableString("{0}関数: DataTable(\"{1}\")の\"id\"列は変更できません");
        }
        [Managed]
        public sealed class MessageBox
        {
            [Managed] public static TranslatableString ConfigError { get; } = new TranslatableString("設定のエラー");
            [Managed] public static TranslatableString TooSmallFontSize { get; } = new TranslatableString("フォントサイズが小さすぎます(8が下限)");
            [Managed] public static TranslatableString LineHeightLessThanFontSize { get; } = new TranslatableString("行の高さがフォントサイズより小さいため、フォントサイズと同じ高さと解釈されます");
            [Managed] public static TranslatableString TooSmallDisplaySaveData { get; } = new TranslatableString("表示するセーブデータ数が少なすぎます(20が下限)");
            [Managed] public static TranslatableString TooLargeDisplaySaveData { get; } = new TranslatableString("表示するセーブデータ数が多すぎます(80が上限)");
            [Managed] public static TranslatableString TooSmallLogSize { get; } = new TranslatableString("ログ表示行数が少なすぎます(500が下限)");
            [Managed] public static TranslatableString FolderCreationFailure { get; } = new TranslatableString("フォルダ作成失敗");
            [Managed] public static TranslatableString FailedCreateSavFolder { get; } = new TranslatableString("savフォルダの作成に失敗しました");
            [Managed] public static TranslatableString SavFolderCreated { get; } = new TranslatableString("savフォルダを作成しました\n現在のデータをsavフォルダ内に移動しますか？");
            [Managed] public static TranslatableString DataTransfer { get; } = new TranslatableString("データ移動");
            [Managed] public static TranslatableString MissingSavFolder { get; } = new TranslatableString("savフォルダは作成されましたが見つかりません\n削除しましたか？");
            [Managed] public static TranslatableString DataTransferFailure { get; } = new TranslatableString("データ移動失敗");
            [Managed] public static TranslatableString FailedMoveSavFiles { get; } = new TranslatableString("savファイルの移動に失敗しました");
            [Managed] public static TranslatableString UpdateCheck { get; } = new TranslatableString("アップデートチェック");
            [Managed] public static TranslatableString NewVersionAvailable { get; } = new TranslatableString("新しいバージョン（{0}）が公開されています。URLを開きますか？\nリンク先:{1}");
            [Managed] public static TranslatableString ConfigFileError { get; } = new TranslatableString("コンフィグファイルに異常があります\nEmueraを終了しますか？");
            [Managed] public static TranslatableString ReplaceFileError { get; } = new TranslatableString("_Replace.csvに異常があります\nEmueraを終了しますか？");
            [Managed] public static TranslatableString ReplaceError { get; } = new TranslatableString("_Replace.csvエラー");
            [Managed] public static TranslatableString InfiniteLoop { get; } = new TranslatableString("無限ループの可能性があります");
            [Managed] public static TranslatableString TooLongLoop { get; } = new TranslatableString("現在、{0}の{1}行目を実行中です。\n最後の入力から{3}ミリ秒経過し{2}行が実行されました。\n処理を中断し強制終了しますか？");
            [Managed] public static TranslatableString ForceQuitAndRestart { get; } = new TranslatableString("FORCE_QUIT_AND_RESTARTが入力待ちを挟まず連続実行されました。再起動せず終了しますか？");
            [Managed] public static TranslatableString IllegalFontError { get; } = new TranslatableString("Emueraの表示処理中に不適正なフォントを検出しました\n描画処理を続行できないため強制終了します");
            [Managed] public static TranslatableString IllegalFont { get; } = new TranslatableString("フォント不適正");
            [Managed] public static TranslatableString FailedOutputLogError { get; } = new TranslatableString("ログの出力に失敗しました");
            [Managed] public static TranslatableString FailedOutputLog { get; } = new TranslatableString("ログ出力失敗");
            [Managed] public static TranslatableString CanNotOutputToParentDirectory { get; } = new TranslatableString("ログ出力先に親ディレクトリは指定できません");
            [Managed] public static TranslatableString CanOnlyOutputToSubDirectory { get; } = new TranslatableString("ログファイルは実行ファイル以下のディレクトリにのみ保存できます");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");

        }
        [Managed]
        public sealed class KeyMacro
        {
            [Managed] public static TranslatableString SetMacroGroup { get; } = new TranslatableString("マクログループ{0}に設定");
            [Managed] public static TranslatableString MacroKeyF { get; } = new TranslatableString("マクロキーF{0}:");
            [Managed] public static TranslatableString GMacroKeyF { get; } = new TranslatableString("G{0}:マクロキーF{1}:");

        }
        [Managed]
        public sealed class SystemLine
        {
            [Managed] public static TranslatableString LoadingFile { get; } = new TranslatableString("{0}読み込み中・・・");
            [Managed] public static TranslatableString ElapsedTimeLoad { get; } = new TranslatableString("経過時間:{0}ms:{1}読み込み中・・・");
            [Managed] public static TranslatableString ElapsedTime { get; } = new TranslatableString("経過時間:{0}ms");
            [Managed] public static TranslatableString BuildingUserFunc { get; } = new TranslatableString("ユーザー定義関数のリストを構築中・・・");
            [Managed] public static TranslatableString CheckingSyntax { get; } = new TranslatableString("スクリプトの構文チェック中・・・");
            [Managed] public static TranslatableString LoadComplete { get; } = new TranslatableString("ロード完了");
            [Managed] public static TranslatableString SelectExitConfigMB { get; } = new TranslatableString("コンフィグファイルに異常があり、終了が選択されたため処理を終了しました");
            [Managed] public static TranslatableString ResourceReadError { get; } = new TranslatableString("リソースフォルダ読み込み中に異常が発見されたため処理を終了します");
            [Managed] public static TranslatableString LoadingMacro { get; } = new TranslatableString("macro.txt読み込み中・・・");
            [Managed] public static TranslatableString LoadingReplace { get; } = new TranslatableString("_Replace.csv読み込み中・・・");
            [Managed] public static TranslatableString SelectExitReplaceMB { get; } = new TranslatableString("_Replace.csvに異常があり、終了が選択されたため処理を終了しました");
            [Managed] public static TranslatableString LoadingRename { get; } = new TranslatableString("_Rename.csv読み込み中・・・");
            [Managed] public static TranslatableString MissingRename { get; } = new TranslatableString("csv\\_Rename.csvが見つかりません");
            [Managed] public static TranslatableString GamebaseError { get; } = new TranslatableString("GAMEBASE.CSVの読み込み中に問題が発生したため処理を終了しました");
            [Managed] public static TranslatableString ErhLoadingError { get; } = new TranslatableString("ERHの読み込み中にエラーが発生したため処理を終了しました");
            [Managed] public static TranslatableString DebugTraceCall { get; } = new TranslatableString("CALL :@{0}:{1}:{2}行目");
            [Managed] public static TranslatableString DebugTraceJump { get; } = new TranslatableString("JUMP :@{0}:{1}:{2}行目");
            [Managed] public static TranslatableString AnalysisCompleted { get; } = new TranslatableString("ファイル解析終了：Analysis.logに出力します");
            [Managed] public static TranslatableString PressEnterOrClick { get; } = new TranslatableString("エンターキーもしくはクリックで終了します");
            [Managed] public static TranslatableString ExitBecauseCanNotInterpreted1 { get; } = new TranslatableString("ERBコードに解釈不可能な行があるためEmueraを終了します");
            [Managed] public static TranslatableString ExitBecauseCanNotInterpreted2 { get; } = new TranslatableString("※互換性オプション「{0}」により強制的に動作させることができます");
            [Managed] public static TranslatableString ExitBecauseCanNotInterpreted3 { get; } = new TranslatableString("emuera.logにログを出力します");
            [Managed] public static TranslatableString SaveQuestion { get; } = new TranslatableString("何番にセーブしますか？");
            [Managed] public static TranslatableString LoadQuestion { get; } = new TranslatableString("何番をロードしますか？");
            [Managed] public static TranslatableString DisplaySaveSlot { get; } = new TranslatableString("[{0, 2}] セーブデータ{0, 2}～{1, 2}を表示");
            [Managed] public static TranslatableString DoYouOverwrite { get; } = new TranslatableString("既にデータが存在します。上書きしますか？");
            [Managed] public static TranslatableString Yes { get; } = new TranslatableString("[0] はい");
            [Managed] public static TranslatableString No { get; } = new TranslatableString("[1] いいえ");
            [Managed] public static TranslatableString Remaining { get; } = new TranslatableString("残り ");
            [Managed] public static TranslatableString Processing { get; } = new TranslatableString("*実行中の行");
            [Managed] public static TranslatableString FileNone { get; } = new TranslatableString("ファイル名:なし");
            [Managed] public static TranslatableString LineFuncNone { get; } = new TranslatableString("行番号:なし 関数名:なし");
            [Managed] public static TranslatableString FileName { get; } = new TranslatableString("ファイル名:{0}");
            [Managed] public static TranslatableString LineFuncName { get; } = new TranslatableString("行番号:{0} 関数名:{1}");
            [Managed] public static TranslatableString FuncCallStack { get; } = new TranslatableString("*スタックトレース");
            [Managed] public static TranslatableString ReloadingErb { get; } = new TranslatableString("ERB再読み込み中……");
            [Managed] public static TranslatableString ReloadCompleted { get; } = new TranslatableString("再読み込み完了");
            [Managed] public static TranslatableString LogFileHasBeenCreated { get; } = new TranslatableString("※※※ログファイルを{0}に出力しました※※※");
            [Managed] public static TranslatableString MinusWontWork { get; } = new TranslatableString("整数型最小値({0})は-を取っても値は変化しません");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");

        }

        static public void LoadLanguageFile()
        {
            if (currentLang != null && currentLang == LanguageModel.Language) return;
            foreach (var pair in trItems) pair.Value.Clear();
            if (Directory.Exists(langDir))
            {
                foreach (var path in Directory.EnumerateFiles(langDir, "emuera.*.xml", SearchOption.TopDirectoryOnly))
                {
                    XmlDocument xml = new XmlDocument();
                    try
                    {
                        xml.Load(path);
                    }
                    catch
                    {
                        continue;
                    }
                    var node = xml.SelectSingleNode("/lang/name");
                    if (node != null)
                    {
                        var langName = node.InnerText.Trim();
                        if (langName.IndexOf('\n')<0 && !langList.ContainsKey(langName))
                        {
                            langList.Add(langName, path);
                            var fontName = node.InnerText.Trim();
                        }
                        if (LanguageModel.Language == langName)
                        {
                            loadLangXML(xml);
                            currentLang = langName;
                        }
                    }
                }
            }
            langNames = new string[langList.Count];
            langList.Keys.CopyTo(langNames, 0);
        }
        static void loadLangXML(XmlDocument xml)
        {
            var fnode = xml.SelectSingleNode("/lang/mfont");
            if (fnode != null)
                MFont = fnode.InnerText.Trim();
            else
                MFont = "MS UI Gothic";
            var nodes = xml.SelectNodes("/lang/tr");
            for (int i = 0; i < nodes.Count; i++)
            {
                var attr = nodes[i].Attributes["id"];
                if (attr != null && trItems.ContainsKey(attr.Value))
                    trItems[attr.Value].Set(nodes[i].InnerText);
            }
        }
        static public void ReloadLang()
        {
            if (LanguageModel.Language == string.Empty)
            {
                foreach (var item in trItems) item.Value.Clear();
                return;
            }
            if (langList.ContainsKey(LanguageModel.Language))
            {
                var path = langList[LanguageModel.Language];
                XmlDocument xml = new XmlDocument();
                try
                {
                    xml.Load(path);
                }
                catch
                {
                    return;
                }
                loadLangXML(xml);
            }
        }
        static public string[] GetLangList()
        {
            return langNames;
        }

        static public void GenerateDefaultLangFile()
        {
            if (!Directory.Exists(langDir))
                Directory.CreateDirectory(langDir);
            FileStream fs = new FileStream(langDir + "emuera-default-lang.xml", FileMode.Create);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            XmlWriter writer = XmlWriter.Create(fs, settings);
            XmlDocument xml = new XmlDocument();
            var root = xml.CreateElement("lang");
            xml.AppendChild(root);
            var name = xml.CreateElement("name");
            name.InnerText = "日本語";
            root.AppendChild(name);
            foreach (var item in trItems)
            {
                var tr = xml.CreateElement("tr");
                var id = xml.CreateAttribute("id");
                id.Value = item.Key;
                tr.Attributes.Append(id);
                tr.AppendChild(xml.CreateCDataSection(item.Value.Text));
                root.AppendChild(tr);
            }
            xml.WriteTo(writer);
            writer.Flush();
        }

        static readonly string langDir = GameFolderModel.Instance.Path + Path.DirectorySeparatorChar + "lang";
        static string currentLang;
        static readonly Dictionary<string, string> langList = new Dictionary<string, string>();
        public static string MFont { get; private set; }
        static string[] langNames;
        static readonly Dictionary<string, TranslatableString> trItems = new Dictionary<string, TranslatableString>();
        static readonly Dictionary<Type, TranslatableString> trClass = new Dictionary<Type, TranslatableString>();

        static Lang()
        {
            queryManagedClass(typeof(Lang), string.Empty, trItems);
        }

        static void queryManagedClass(Type t, string addr, Dictionary<string, TranslatableString> trItems)
        {
            if (addr.Length > 0) addr += '.';
            foreach (var nt in t.GetNestedTypes())
            {
                string tr = null;
                bool managed = false;
                foreach (var attr in nt.GetCustomAttributes(false))
                {
                    if (attr is Managed) managed = true;
                    else if (attr is Translate trAttr) tr = trAttr.String;
                }
                if (managed)
                {
                    if (tr != null)
                    {
                        var item = new TranslatableString(tr);
                        trItems.Add(addr + nt.Name, item);
                        trClass.Add(nt, item);
                    }
                    foreach (var prop in nt.GetProperties())
                    {
                        if (prop.PropertyType == typeof(TranslatableString))
                        {
                            foreach (var pattr in prop.GetCustomAttributes(false))
                            {
                                if (pattr is Managed)
                                {
                                    trItems.Add(addr + nt.Name + '.' + prop.Name, prop.GetValue(null, null) as TranslatableString);
                                }
                            }
                        }
                    }
                    queryManagedClass(nt, addr + nt.Name, trItems);
                }
            }
        }
    }
}
