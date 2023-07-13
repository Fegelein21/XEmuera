==============================================================
= Readme:
=	Emuera1824+v11+webp+Secure
=
=	注意: 今回の仕様変更品は、全環境で動作する保証がありません
=		以下注意書きをよく読んでください
=		2021/03/19 ver 0.5.0 by M
==============================================================

●概要
 このFixは、東etoマン氏のEmuera1829+v9+WebP_fixtest0001 として投稿された
 起動時エラー改善パッチ品をもとに、webp画像のGCREATEFROMFILE 対応を行い安定化させ、
 妊）|дﾟ)の中の人私家改造版 Emuera1824+v11 とマージしたものです。
 テスト品のトロイ誤検知のため一旦ひっこめたものに、Emuera セキュリティ仕様を
 変更したものをUpします。

●ライセンス
 Emuera本体側の改変に関しては本体のライセンスに準じます。
 WebPWrapper.cs 側に関しては同様に WebpWrapper の MITライセンス に準じます。
 libwebp については、添付 libwebp に添付された Google のライセンス文に準じます。

●開発環境
  Windows 10 Home 21H1 (OS ビルド 19043.899)
  Visual Studio 2019 Community ver 16.9.2 ((c)Microsoft) 
  Microsoft(R) C/C++ Optimizing Compiler Version 19.28.29913
  TortoiseGit 2.11.3 (20210107-b33cf0f) / Git for windows 2.31.0.windows.1
  neo-ConfuserEx 1.0.0
	それぞれ 各ソフトウエアのコピーライト及びライセンスに準じます。

●同梱内容
★ Docs/Emuera_readme.txt
	Emuera本家のreadme。よく読んで。
★ Docs/libwebp/COPYING
	libwebp ライブラリライセンス by Google.
★ Docs/私家改造版Emuera_readme.txt
	妊）|дﾟ)の中の人私家改造版 Emuera1824+v11のreadme。よく読んで。
★ Docs/WebP追加対応版Readme.txt
	がめら氏によるWebp追加対応版Emueraパッチ readme。よく読んで
★ Docs/Emuera1824+v9+WebP_fixtest0001.txt
	東etoマン氏によるEmuera1829+v9+WebP_fixtest0001版Emueraパッチ readme。よく読んで
☆ Readme for Emuera1824+v11+webp+Secure.txt
	このファイルです。
☆ src1824+v11+webp+test+Secure.7z
☆ Docs/Updates_fromv0.2.0Tov0.5.0.diff
	ソース＆変更箇所。変更は多岐に渡るので、diffで確認を。
☆ Docs/libwebp/libwebp_imageio_patch.diff
	/sdl オプション付きでビルドする際に処置したパッチ
☆ Docs/libwebp/libwebp_resource.diff
	パッチを当てた libwebp.dll であることを表すリソース。
☆ 参考: SpriteTest.webp
	eraTWのタイトル画面上に csv登録していない webpファイルを GCREATEFROMFILE() で
	読み込み、SPRITECREATE() → CBGSETSPRITE() で表示させてみた実験例。
☆ Emuera1824+v11+webp_PreCompiled_Please_Readme.7z
	プリコンパイル済みバイナリ。ビルドしなおしたので、敏感なアンチウイルスソフトから警告が出ると思います。
	■Emueraはセキュリティ対応したものと、従来通りの非対応のもの2種類を同梱しています。
	  一般ユーザはファイル名に Secure とついたほうを推奨します。
	  非対応品は非Windowsユーザか、非力なPCやアンチウイルスソフトに怒られた人向けです。
	■libwebp_x??.dll は古いライブラリ関数を呼んでいるため、信頼性重視オプションでパッチ処置付き
	  リビルド(スタック＆バッファチェック:/sdl /Gs /GS)。ファイルバージョンに" (Patched)"が付きました。
	■鯖に配慮して暗号化済み、パスワードは Emuera です。

●変更内容:
▼Ver 0.5.0 2021/03/19
・CHANGE: 起動時にフォルダの「セキュリティ権限チェック」を実施、トロイ活動に利用しにくくした
	書き込み・動作の権限チェック、及び CSV/ERBフォルダ有無チェックを行い、NGなら即終了するように。
	※現時点では WINDOWS Homeの標準インストール環境 固有になっている可能性があり、エラー検証が
	  十分にできていません。
	  (NAS含む)ネットワーク上やリムーバブルドライブに置いた場合など、また
	  MONO等では不具合があるかもしれません。これ以上は有識者改善求む。
		_Library/Sys.cs
		Program.cs
・CHANGE: emuera.config と debug.config の書き込みは、上記許可がないと行わないように。
		その他のファイル出力機構には触れていません。
		Config/Config.cs
		Config/ConfigData.cs
		Forms/ConfigDialog.cs
		Forms/DebugConfigDialog.cs
・ADD: 難読化＆セキュリティ用アトリビュートを付与 (Secureでない方は、従来品と差がほぼないはず)
		Properties/Obfuscations.cs
		各名前空間・クラス・関数
・FIX: WebpWrapperで、利用していないエンコードや出力・メモリバッファー操作系を殺した。
	今までも、元々コードとしては生成されていないはず？
		_Library/WebpWrapper.cs
・CHANGE: libwebp_*.dll のコンパイルオプション変更、些細なセキュリティ処置パッチ処置
・CHANGE: プリコンパイル済みEmueraバイナリ、上記の通り難読化＆セキュリティ処置済み品と、未処置品の2種を同梱。

▼Ver 0.2.0 2021/03/07
・FIx: Emueraスレ バグ報告指摘 デバッグモード時パス異常typo修正
	(VS2019 16.9の Rosyn ではなぜか警告なしにコンパイル通っちゃうのは何？)
		Program.cs 56行目修正
・Fix: GCREATEFROMFILE / SPRITE系 でも webp 読み込み対応OK、安定増しました。
	一応テストしました…まだ怪しかったら後免
		Creater.Method.cs
・UPDATE: 少し改善
	コードのメンテナンス性が悪いと Visual Studio に怒られリファクタリング。
	確かに、一年塩漬けにしたくらい読みづらいなので…
	まぁ、大本が拾いものらしいけれど
		WebpWrapper.cs

▼Ver 0.1.0
  Based on Emuera1829+v9+WebP_fixtest0001 by 東etoマン氏
  Based on Emuera1824+v11 by 妊）|дﾟ)の中の人

・ADD:    WebPライブラリに 専用Exeptionクラス WebPExeption を用意
・CHANGE: WebPのLoadで、ファイル読み込み時アクセスエラーは3回までリトライするように
・FIX:    Webpファイルかどうかの判定変更。
          パスに".webp"が含まれているかどうか、から拡張子が".webp"かどうかに変更
          (パスにwebpが入っていたら、拡張子じゃなくてもwebpラッパーが呼ばれてた…)
・CHANGE: 受け手の AppContents.cs で WebPExeption を処理。エラー時の処置を分離
・ADD:    Creator.Method.cs でも WebPライブラリを呼ぶように
          (これでGREATE系命令でwebpファイルが使えるかも？
          ただ、恥ずかしながらGREATE系命令群のテストスクリプトがわからず、未テストです)
・CHANGE: パス区切り文字(\)・パス区切り補助文字(/)を、ハードコードではなく
          .NET Framework から参照するように (別Forkで4.8化してる都合上…)
	Config/Config.cs
	Config/ConfigData.cs
	ContentAppContents.cs
	Program.cs
	Sys.cs

●Comment:
1)	どうやら、識別子・リソース・ハードコードされている文字列にCJK文字列が含まれている
	比率が高いとトロイ扱いとして反応しやすいようです。検出器を作っている人たちが欧米の
	人たちだからね、しょうがないね

2)	ファイル書き込み機能に何らかの制限をつけるべきですね。
	現状フリーハンドだから、トロイと変わりない…

[EOF]
