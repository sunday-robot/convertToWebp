JPEGファイルまたはPNGファイルから、WEBPファイルを作成する。
JPEGファイルの場合はロスありWEBP、PNGファイルの場合はロス無しWEBPファイルを作成する。

USAGE:
convertToWebp <file or directory path>*

ファイルパスが指定された場合は同じディレクトリに、WEBPファイルを作成する。
例)
convertToWebp a.png
a.pngと同じディレクトリにa.webpを作成する。

ディレクトリが指定されて場合は、指定されたディレクトリの名前の末尾に".webp"を付加したディレクトリを作成し、このディレクトリ以下にWEBPファイルを作成する。
元のディレクトリの階層構造は再現する。


convertToWebp a

aが以下のような構造の場合、
a/a.jpg
a/b/b.png

以下のようなディレクトリが作成され、WEBPファイルが作成される。
a.webp/a.webp
a.webp/b/b.webp

拡張子がJPGでもPNGでもないファイルの場合はその旨コンソールに出力する。
エラー終了したりしない。
