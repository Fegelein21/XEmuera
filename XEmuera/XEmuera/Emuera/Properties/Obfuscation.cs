/// <summary>
/// コンパイラによる セキュリティ対策の設定ファイル
/// </summary>
#if DEBUG
#else
// 標準コンパイラで対応
[assembly: System.Runtime.CompilerServices.SuppressIldasmAttribute()]
#endif
