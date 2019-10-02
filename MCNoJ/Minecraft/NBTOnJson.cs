using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MCNoJ.Minecraft
{
    public class NBTOnJson
    {
        string basePath;
        public NBTOnJson(string basePath)
        {
            this.basePath = basePath;
        }
        public NBT MakeNBT(JToken jToken)
        {
            switch (jToken.Type)
            {
                case JTokenType.Null:
                    return null;
                case JTokenType.Object:
                    Dictionary<string, NBT> objChildren = new Dictionary<string, NBT>();
                    foreach (KeyValuePair<string, JToken> child in (JObject)jToken)
                    {
                        string key = child.Key;
                        NBT childValue = MakeNBT(child.Value, ref key);
                        if (childValue != null)
                        {
                            objChildren.Add(key, childValue);
                        }
                    }
                    return new NBTCompund(objChildren);
                case JTokenType.Array:
                    List<NBT> arrChildren = new List<NBT>();
                    foreach (JToken child in (JArray)jToken)
                    {
                        NBT childValue = MakeNBT(child);
                        if (childValue != null)
                        {
                            arrChildren.Add(childValue);
                        }
                    }
                    return new NBTList(arrChildren);
                case JTokenType.Integer:
                    return new NBTInt((int)(long)((JValue)jToken).Value);
                case JTokenType.Float:
                    return new NBTDouble((double)((JValue)jToken).Value);
                case JTokenType.String:
                    return new NBTString((string)((JValue)jToken).Value);
                default:
                    return null;
            }
        }
        private NBT MakeNBT(JToken jToken, ref string key)
        {
            Match matche = Regex.Match(key, "^_([^_]+)_(.+)$");
            JTokenType jsonType = jToken.Type;
            NBT nbt = null;
            switch (matche.Groups[1].Value)
            {
                case "TAGByte":
                    nbt = TryMakeNBTBtye(jToken);
                    break;
                case "TAGShort":
                    nbt = TryMakeNBTShort(jToken);
                    break;
                case "TAGInt":
                    nbt = TryMakeNBTInt(jToken);
                    break;
                case "TAGLong":
                    nbt = TryMakeNBTLong(jToken);
                    break;
                case "TAGFloat":
                    nbt = TryMakeNBTFloat(jToken);
                    break;
                case "TAGDouble":
                    nbt = TryMakeNBTDouble(jToken);
                    break;
                case "TAGByteArray":
                    nbt = new NBTByteArray(TryMakeNBTListType(jToken, TryMakeNBTBtye));
                    break;
                case "TAGIntArray":
                    nbt = new NBTIntArray(TryMakeNBTListType(jToken, TryMakeNBTInt));
                    break;
                case "TAGLongArray":
                    nbt = new NBTLongArray(TryMakeNBTListType(jToken, TryMakeNBTLong));
                    break;
                case "RawJson":
                    nbt = TryMakeRawJsonNBT(jToken);
                    break;
                case "RawJsonArray":
                    nbt = new NBTList(TryMakeNBTListType(jToken, TryMakeRawJsonNBT));
                    break;
                case "RawJsonFile":
                    nbt = TryMakeRawJsonFileNBT(jToken);
                    break;
                case "RawJsonFileArray":
                    nbt = new NBTList(TryMakeNBTListType(jToken, TryMakeRawJsonFileNBT));
                    break;
            }
            if (nbt == null)
            {
                return MakeNBT(jToken);
            }
            else
            {
                key = matche.Groups[2].Value;
                return nbt;
            }
        }
        private NBTByte TryMakeNBTBtye(JToken jToken)
        {
            if (jToken.Type == JTokenType.Integer)
            {
                return new NBTByte((sbyte)(long)((JValue)jToken).Value);
            }
            else
            {
                throw new FormatException($"<{jToken.Path}> value must be integer(8bit signed).");
            }
        }
        private NBTShort TryMakeNBTShort(JToken jToken)
        {
            if (jToken.Type == JTokenType.Integer)
            {
                return new NBTShort((short)(long)((JValue)jToken).Value);
            }
            else
            {
                throw new FormatException($"<{jToken.Path}> value must be integer(16bit signed).");
            }
        }
        private NBTInt TryMakeNBTInt(JToken jToken)
        {
            if (jToken.Type == JTokenType.Integer)
            {
                return new NBTInt((int)(long)((JValue)jToken).Value);
            }
            else
            {
                throw new FormatException($"<{jToken.Path}> value must be integer(32bit signed).");
            }
        }
        private NBTLong TryMakeNBTLong(JToken jToken)
        {
            if (jToken.Type == JTokenType.Integer)
            {
                return new NBTLong((long)((JValue)jToken).Value);
            }
            else
            {
                throw new FormatException($"<{jToken.Path}> value must be integer(64bit signed).");
            }
        }
        private NBTFloat TryMakeNBTFloat(JToken jToken)
        {
            if (jToken.Type == JTokenType.Float || jToken.Type == JTokenType.Integer)
            {
                return new NBTFloat((float)(double)((JValue)jToken).Value);
            }
            else
            {
                throw new FormatException($"<{jToken.Path}> value must be float.");
            }
        }
        private NBTDouble TryMakeNBTDouble(JToken jToken)
        {
            if (jToken.Type == JTokenType.Float || jToken.Type == JTokenType.Integer)
            {
                return new NBTDouble((double)((JValue)jToken).Value);
            }
            else
            {
                throw new FormatException($"<{jToken.Path}> value must be float.");
            }
        }
        private NBTString TryMakeRawJsonNBT(JToken jToken)
        {
            return new NBTString(new RawJson(jToken).GetEscapedJsonText('"'));
        }
        private NBTString TryMakeRawJsonFileNBT(JToken jToken)
        {
            if (jToken.Type == JTokenType.String)
            {
                string path = (string)((JValue)jToken).Value;
                path = RelPath.GetRootedPathByFile(basePath, path);
                return new NBTString(new RawJson(path).GetEscapedJsonText('"'));
            }
            else
            {
                throw new FormatException($"<{jToken.Path}> value must be string.");
            }
        }
        private List<NBT> TryMakeNBTListType(JToken jToken, Func<JToken, NBT> tryMakeRawJsonNBT)
        {
            if (jToken.Type == JTokenType.Array)
            {
                List<NBT> arrChildren = new List<NBT>();
                foreach (JToken child in (JArray)jToken)
                {
                    NBT childValue = tryMakeRawJsonNBT(child);
                    if (childValue != null)
                    {
                        arrChildren.Add(childValue);
                    }
                }
                return arrChildren;
            }
            else
            {
                throw new FormatException($"<{jToken.Path}> value must be array.");
            }
        }
    }
}
