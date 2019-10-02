# 使用例

特に意味があるコマンドではないけど特殊記法を全部記載した例です。

[readmeに戻る](../readmeJP.md)

## 用意するもの

- 入力ファイル : [a.json](../MCNoJ/debug_files/a.json)
- 外部RawJson読み込み用 : [page1.json](../MCNoJ/debug_files/page1.json)
- 外部RawJson読み込み用 : [page2.json](../MCNoJ/debug_files/page2.json)

## コマンド

入力ファイルと同じ場所で実行

```sh
<実行ファイル> -c a.json
```

(-c は結果をクリップボードにコピーする)

## 結果

入力ファイル

```Json
{
    "command": "give @p minecraft:pufferfish$nbt$ 3",
    "nbt": {
        "_TAGByte_byteex": 123,
        "_TAGByteArray_bytearrex": [
            1,
            2,
            3
        ],
        "_TAGInt_intex1": 456,
        "intex2": 4567,
        "_TAGIntArray_intarrex": [
            4,
            5,
            6
        ],
        "_TAGShort_shortex": 789,
        "_TAGLong_longex": 101112,
        "_TAGLongArray_longarrex": [
            10,
            11,
            12
        ],
        "_TAGFloat_floatex": 1.2345678910111213,
        "_TAGDouble_doubleex1": 1.2345678910111213,
        "doubleex2": 3.14159265358979323,
        "stringex": "string!!!!!!",
        "_RawJson_rjex": [{
            "aaa": "asdf",
            "bbb": "ffff"
        }],
        "_RawJsonArray_rjaex": [{
            "aaa": "asdf",
            "bbb": "ffff"
        }],
        "_RawJsonFile_rjfex": "page1.json",
        "_RawJsonFileArray_rjfaex": [
            "page1.json",
            "page2.json"
        ]
    }
}
```

↓
(結果を手動で改行・インデント挿入))

``` mcc
give @p minecraft:pufferfish{
    byteex:123b,
    bytearrex:[
        B;1b,
        2b,
        3b
    ],
    intex1:456,
    intex2:4567,
    intarrex:[
        I;4,
        5,
        6
    ],
    shortex:789s,
    longex:101112l,
    longarrex:[
        L;10l,
        11l,
        12l
    ],
    floatex:1.234568f,
    doubleex1:1.23456789101112d,
    doubleex2:3.14159265358979d,
    stringex:"string!!!!!!",
    rjex:"[{\"aaa\":\"asdf\",\"bbb\":\"ffff\"}]",
    rjaex:[
        "{\"aaa\":\"asdf\",\"bbb\":\"ffff\"}"
    ],
    rjfex:"[{\"text\":\"page1\\n\",\"color\":\"reset\"},{\"text\":\"---page1\\n\",\"italic\":true}]",
    rjfaex:[
        "[{\"text\":\"page1\\n\",\"color\":\"reset\"},{\"text\":\"---page1\\n\",\"italic\":true}]",
        "[{\"text\":\"page2\\n\",\"color\":\"reset\"},{\"text\":\"---page2\\n\",\"italic\":true}]"
    ]
} 3
```