using CommandLine;
using MCNoJ.Minecraft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Windows;

namespace MCNoJ
{
    class Program
    {
        [STAThreadAttribute]
        static void Main(string[] args)
        {
            CommandLine.ParserResult<Options> result = Parser.Default.ParseArguments<Options>(args);
            if (result.Tag == ParserResultType.Parsed)
            {
                var options = (Parsed<Options>)result;
                string inputFile = Path.GetFullPath(options.Value.InputFile);
                JToken json;
                string output;
                NBT nbt;
                NBTOnJson jsonToNBT = new NBTOnJson(inputFile);
                try
                {
                    //read input json
                    try
                    {
                        using (StreamReader sr = new StreamReader(inputFile))
                        using (JsonTextReader jr = new JsonTextReader(sr))
                        {
                            json = JToken.ReadFrom(jr);
                        }
                    }
                    catch (JsonReaderException e)
                    {
                        string errMsg = "";
                        errMsg += $"Json file error in <{inputFile}>\n";
                        errMsg += "=========Error details=========\n";
                        errMsg += $"{e.Message}\n";
                        errMsg += "===============================";
                        throw new JsonReaderException(errMsg);
                    }
                    //make nbt
                    try
                    {
                        nbt = jsonToNBT.MakeNBT(json["nbt"]);
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine(e.Message);
                        return;
                    }
                    if (nbt == null)
                    {
                        nbt = NBTCompund.Empty;
                    }
                    //setting output format
                    output = nbt.ToString();
                    if (!options.Value.OnlyNbt)
                    {
                        string command = (string)(JValue)json["command"];
                        output = command.Replace("$nbt$", output);
                    }
                    //setting output file
                    string outputFile;
                    string outputFileJson = (string)(JValue)json["output"];
                    string outputFileCommandLine = options.Value.OutputFile;

                    if (outputFileJson == null && outputFileCommandLine == null) {
                        //not defined output file
                        Console.WriteLine(output);
                    } else {
                        if (outputFileJson != null && outputFileCommandLine == null)
                        {
                            //defined output file in only json
                            outputFile = RelPath.GetRootedPathByFile(inputFile, outputFileJson);
                        }
                        else
                        {
                            //defined output file commandline
                            outputFile = Path.GetFullPath(options.Value.OutputFile);
                        }
                        using (StreamWriter sw = new StreamWriter(outputFile))
                        {
                            sw.WriteLine(output);
                        }
                        Console.WriteLine("Write file successful.");
                    }
                    if (options.Value.Clipboard)
                    {
                        Clipboard.SetText(output);
                        Console.WriteLine("Copyed output to clipboard.");

                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
                catch (JsonReaderException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }
        }
    }
    class Options
    {
        /**********************
         * exefile [option] <inputfile>
         *      オプション全省略の場合コンソール出力
         *      
         * option
         *  -o <outputfile>     出力をファイルに出力
         *  -c                  出力をクリップボードにコピー
         *  -n                  NBTのみ出力
         *********************/
        [Value(1, Required = true, MetaName = "intput file")]
        public string InputFile { get; set; }
        [Option('o', "output", Required = false, HelpText = "output file")]
        public string OutputFile { get; set; }
        [Option('c', "clipboard", Required = false, HelpText = "coly output to clip board")]
        public bool Clipboard { get; set; }
        [Option('n', "onlynbt", Required = false, HelpText = "output only NBT text")]
        public bool OnlyNbt { get; set; }
    }
}
