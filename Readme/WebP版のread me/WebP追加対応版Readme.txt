パッチ作成者　がめら

◆概要
resourcesでPNGを利用して画像を表示するバリアントが増えてきたため、
非可逆圧縮でも透過が使えてロスレスでもPNGよりサイズが縮む事がある最新のWebPフォーマットをEmueraに追加しました。
C#用WebPライブラリは下記のGitHubから拝借しました（MITライセンスをlicense.txtに追記済み）
https://codeload.github.com/JosePineiro/WebP-wrapper/zip/master

◆使い方
resourcesに拡張子「.webp」の画像を置き、PNGと同じくCSVで指定するだけです。
内部でEmueraが使う通常のBitmapクラスに変換されているため、バリアント側の対応は画像の置き換えとCSV書き換え以外不要です。
Google公式のcwebpエンコーダでフォルダ内の全PNGファイルをWebPに置き換えるバッチファイルも用意しました。

まずはGoogleのページからWindows版cwebp.exeをダウンロードしてください。https://developers.google.com/speed/webp/download?hl=ja
そしてcwebpを同じフォルダに置くか環境変数を通してバッチファイルを実行してください。

ちなみにeraTW4.750のresources直下のPNGでテストした所、

オリジナルPNG：28.3MB
非可逆：15.85MB
ロスレス風：17.2MB
ロスレス：25.4MB

という結果になりました。
更に、ロスレス風の最高圧縮設定では劣化もあるものの、非可逆よりも高画質でオリジナルの半分の14.2MBになりました。
なので巨大なバリアントだと10MB以上容量を抑える事が出来ると思います。

◆ライセンス
WebP対応におけるEmuera追加部分のライセンスはフリーです。
WebPWrapper.csはMITライセンスとなるのでlicense.txtを参照してください。

◆更新履歴
2020年6月4日: 初版
2020年6月6日: libwebp_x64.dllが無いなど何らかの理由でWebP読み込みが失敗した場合のエラーメッセージを追加。