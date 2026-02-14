# 概要
JPEGファイルまたはPNGファイルから、WEBPファイルを作成する。<br>
JPEGファイルの場合はロスありWEBP、PNGファイルの場合はロス無しWEBPファイルを作成する。

# USAGE
convertToWebp \<file path or directory path\>*

# 説明
拡張子が.JPGあるいは.PNGのファイルを、WEBPファイルに変換する。元のファイルはゴミ箱に移動させる。
引数にディレクトリが指定された場合は、そのディレクトリ配下のすべてのファイルを処理対象とする。
拡張子が.JPGでも.PNGでもないファイルについては何もしない。
