module("ExcelData")
local fileName = "FormatDemo"
local GetLocalizedText = DataService and DataService.GetLocalizedText

FormatDemo={
    --["header"]={key="string",intVal="int",uintVal="uint",longVal="long",ulongVal="ulong",byteVal="byte",floatVal="float",doubleVal="double",strVal="string",lstrVal="localizedstring",intArray="int[]",floatArray="float[]",stringArray="string[]",clientOnlyVal="float"}
    ["key1"]={key="key1",intVal=12,uintVal=123,longVal=-123456,ulongVal=13212321,byteVal=1,floatVal=123.456,doubleVal=567.891,strVal="Normal string.",__lKey_lstrVal="Client_1000",intArray={1, 2, 3},floatArray={1.5, 2.3, 4.5},stringArray={"Hello world", "\" Hello world 2\""},clientOnlyVal=123.456},
    ["key2"]={key="key2",intVal=13,uintVal=124,longVal=133133133,ulongVal=12321,byteVal=0,floatVal=124.456,doubleVal=568.891,strVal="\"String with quotation mark\"",__lKey_lstrVal="Client_1001",intArray={0, 7, 0},floatArray={0.5},stringArray={"Hello", "world", "Again"},clientOnlyVal=124.456},
    ["key3"]={key="key3",intVal=14,uintVal=125,longVal=434324,ulongVal=123213213,byteVal=1,floatVal=125.456,doubleVal=569.891,strVal="String with space to trim.",__lKey_lstrVal="Client_1002",intArray={2, 1010, 333},floatArray={1.5, 2.1},stringArray={"Hi, My name is Jason.", "你好，杰森。"},clientOnlyVal=125.456},
    ["key4"]={key="key4",intVal=15,uintVal=126,longVal=13214,ulongVal=123213213,byteVal=0,floatVal=126.456,doubleVal=570.891,strVal="    String with space to use.",__lKey_lstrVal="Dialog_1003",intArray={444, 1221, 0},floatArray={3.1, 4.4, 2.2},stringArray={"Hello \n World", "Hello \n World"},clientOnlyVal=126.456},
    ["key5"]={key="key5",intVal=16,uintVal=127,longVal=143,ulongVal=12321321,byteVal=1,floatVal=127.456,doubleVal=571.891,strVal="Line with explicit linefeed\n and implicit \nlinefeed",__lKey_lstrVal="Dialog_1004",intArray={11},floatArray={9.9, 22, 34},stringArray={"Hello world"},clientOnlyVal=127.456},
    ["key6"]={key="key6",intVal=17,uintVal=128,longVal=1213,ulongVal=12321321,byteVal=0,floatVal=128.456,doubleVal=572.891,strVal="你好&こんにちは&안녕하세요--CJK",__lKey_lstrVal="Dialog_1005",intArray={123, 12321, 123213, 414},floatArray={16, 17, 19},stringArray={"", ""},clientOnlyVal=128.456},
}

function InitLocalizedTexts_FormatDemo()
    if not GetLocalizedText then return end
    for k,v in pairs(FormatDemo) do
        v.lstrVal = GetLocalizedText(fileName, v.__lKey_lstrVal or "")
    end
end

InitLocalizedTexts_FormatDemo()

return FormatDemo
