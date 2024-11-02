# 概要
JPEGファイルまたはPNGファイルから、WEBPファイルを作成する。<br>
JPEGファイルの場合はロスありWEBP、PNGファイルの場合はロス無しWEBPファイルを作成する。

# USAGE
convertToWebp \<file path or directory path\>*

# 説明
引数に複数のパス名が記述できるが、パス名がファイルか、ディレクトリかで処理が異なる。

## ファイルパスが指定された場合
同じディレクトリに、WEBPファイルを作成する。<br>
例)<br>
```
convertToWebp a.png
```
a.pngと同じディレクトリにa.webpを作成する。

## ディレクトリパスが指定された場合
指定されたディレクトリの名前の末尾に".webp"を付加したディレクトリを作成し、このディレクトリ以下にWEBPファイルを作成する。
元のディレクトリの階層構造は再現する。
例)<br>
```
convertToWebp a
```
aが以下のような構造の場合、<br>
> a/a.jpg<br>
> a/b/b.png<br>

以下のようなディレクトリとWEBPファイルが作成される。<br>
> a.webp/a.webp<br>
> a.webp/b/b.webp<br>

拡張子がJPGでもPNGでもないファイルの場合はその旨コンソールに出力する。
エラー終了したりしない。
