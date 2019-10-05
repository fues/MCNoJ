# Minecraft NBT on Json

[English version are here.](./readme.md)

Json記法でNBT(正確にはSNBT)が記述できるようにしたCUIのツールです。

## 特徴

- 煩雑なNBTを好きなテキストエディタでフォーマットしながら書ける
- [Raw Json](https://minecraft.gamepedia.com/Commands#Raw_JSON_text)を直接・外部ファイル指定して書ける
- NBTのみ出力・コマンド化して出力も可能
- 結果を自動でクリップボードにコピーすることも可能
- bat等で自動化も容易?

## 使用例

[長いのでこちら](readme_demo/demoJP.md)


## コマンドライン

```sh
MCNoJ.exe [オプション...] <入力ファイル>
```

|オプション|説明|
|:-|:-|
| `-o <出力ファイル>`|結果をファイルに出力(入力ファイル内の記述を上書きする)|
| `-c`             |結果をクリップボードにコピー|
| `-n`             |NBTのみ出力|

-o オプションと入力ファイルどちらにも出力ファイルを書かない場合、標準出力に結果が出ます。

## 入力ファイルのフォーマット

```Json
{
    "command" : "give...$nbt$...",
    "output" : "xxx.mcfunction",
    "nbt":{
        ...
    }
}
```

- `command` : **必須** 文字列。 コマンドを入力する。`$nbt$`で囲まれたところに結果のNBTが入る
- `output` : **任意** 文字列。 出力ファイル。絶対パスまたは入力ファイルからの相対パス
- `nbt` : **必須** ここにNBT情報を記述する。

### 特殊記法

jsonのキーの名称は基本的にそのままNBTのキーに変換されますが、NBTにしか無い型やRawJsonは以下のルールで記述します。

[NBTの型についてはこちら](https://minecraft.gamepedia.com/NBT_format)

#### NBTの型のキーは 文字列のアンダーバーを除き、最初と最後にアンダーバーをつけたものを最初につける

つまり `TAG_Short`型の`DeathTime`を12345にする場合

``` json
{
    "_TAGShort_DeathTime" : 12345
}
```

となる

例外として`TAG_Int`と`TAG_Double`はそのままの形で記述できる

#### Raw Jsonを直接書く場合はキーの前に_RawJson_または_RawJsonArray_をつける

例としてアイテムのdisplayタグを記述した。

```json
{
    "display":{
        "_RawJson_Name":{
            "text":"Item Name",
        },
        "_RawJsonArray_Lore":[
            {"text":"line 1"},
            {"text":"line 2"}
        ]
    }
}
```

#### Raw Jsonを別ファイルに書く場合はキーの前に_RawJsonFile_または_RawJsonFileArray_をつける

ファイル名は絶対パス、もしくは入力ファイルを基準とした相対パスで指定する。

上と同じくdisplayタグを記述した。

```json
{
    "display":{
        "_RawJsonFile_Name":"example.json",
        "_RawJsonFileArray_Lore":[
            "example2.json",
            "example3.json"
        ]
    }
}
```