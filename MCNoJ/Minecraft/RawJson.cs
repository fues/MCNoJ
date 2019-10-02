using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace MCNoJ.Minecraft
{
    public class RawJson
    {
        JToken origJson;
        public RawJson(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                using (JsonTextReader jr = new JsonTextReader(sr))
                {
                    this.origJson = JToken.ReadFrom(jr);
                }
            }
            catch (JsonReaderException e)
            {
                string errMsg = "";
                errMsg += $"Json file error in <{path}>\n";
                errMsg += "=========Error details=========\n";
                errMsg += $"{e.Message}\n";
                errMsg += "===============================";
                throw new JsonReaderException(errMsg);
            }
        }
        public RawJson(JToken json)
        {
            this.origJson = json;
        }
        public string GetEscapedJsonText(char containedChar)
        {
            string escapedJsonText;
            escapedJsonText = JsonConvert.SerializeObject(origJson).Replace("\\", "\\\\");
            if (containedChar == '"')
            {
                return escapedJsonText.Replace("\"", "\\\"");
            }
            else if(containedChar == '\'')
            {
                return escapedJsonText.Replace("'", "\\'");
            }
            else
            {
                return escapedJsonText;
            }
        }
    }
}
