using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace SharpBoost {
    public static class StringExtensions {
        #region Format
        public static string F(this string format, object arg0) {
            return string.Format(format, arg0);
        }

        public static string F(this string format, object arg0, object arg1) {
            return string.Format(format, arg0, arg1);
        }

        public static string F(this string format, object arg0, object arg1, object arg2) {
            return string.Format(format, arg0, arg1, arg2);
        }

        public static string F(this string format, params object[] args) {
            return string.Format(format, args);
        }
        #endregion

        #region Regex
        public static string RegexReplace(this string input, string pattern, MatchEvaluator evaluator, RegexOptions options
             = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace) {
            if (input == null)
                throw new ArgumentNullException("input");
            if (pattern == null)
                throw new ArgumentNullException("pattern");
            if (evaluator == null)
                throw new ArgumentNullException("evaluator");

            return Regex.Replace(input, pattern, evaluator);

        }

        public static string RegexReplace(this string input, string pattern, string replacement, RegexOptions options
           = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace) {
            if (input == null)
                throw new ArgumentNullException("input");
            if (pattern == null)
                throw new ArgumentNullException("pattern");
            if (replacement == null)
                throw new ArgumentNullException("replacement");

            return Regex.Replace(input, pattern, replacement, options);
        }


        //format replace rules: $<group_number> or ${group_name}, example $1 ${mygroup} 
        public static string RegexSearch(this string input, string pattern, string format = "", RegexOptions options
            = RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace) {
            if (input == null)
                throw new ArgumentNullException("input");
            if (pattern == null)
                throw new ArgumentNullException("pattern");
            if (format == null)
                throw new ArgumentNullException("format");

            var regex = new Regex(pattern, options);
            var m = regex.Match(input);
            if (!m.Success)
                return string.Empty;

            if (string.IsNullOrEmpty(format))
                return m.Value;

            return new Regex(@"(\$(?<index>[\d]+)|(\$\{(?<name>[^}]+)\}))")
                .Replace(format, match => {
                    var indStr = match.Groups["index"].Value;
                    var name = match.Groups["name"].Value;
                    if (!string.IsNullOrEmpty(indStr)) {
                        try {
                            var index = int.Parse(indStr);
                            return m.Groups[index].Value;
                        }
                        // ReSharper disable EmptyGeneralCatchClause
                        catch { }
                        // ReSharper restore EmptyGeneralCatchClause
                    }
                    else if (!string.IsNullOrEmpty(name)) {
                        return m.Groups[name].Value;
                    }

                    return string.Empty;
                });
        }

        public static void RegexSearch(this string input, string pattern, Action<Match> evaluator, RegexOptions options
            = RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace) {
            if (input == null)
                throw new ArgumentNullException("input");
            if (pattern == null)
                throw new ArgumentNullException("pattern");
            if (evaluator == null)
                throw new ArgumentNullException("evaluator");

            var regex = new Regex(pattern, options);
            var matches = regex.Matches(input);
            foreach (var match in matches.Cast<Match>().Where(match => match.Success))
                evaluator(match);
        }

        public static bool IsMatch(this string input, string pattern, RegexOptions options
            = RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace) {
            if (input == null)
                throw new ArgumentNullException("input");
            if (pattern == null)
                throw new ArgumentNullException("pattern");

            var regex = new Regex(pattern, options);
            return regex.IsMatch(input);
        }
        #endregion

        #region WebHelper

       
        public static string HtmlEncode(this string data) {
            return HttpUtility.HtmlEncode(data);
        }

       
        public static string HtmlDecode(this string data) {
            return HttpUtility.HtmlDecode(data);
        }

        
        public static System.Collections.Specialized.NameValueCollection ParseQueryString(this string query) {
            return HttpUtility.ParseQueryString(query);
        }

        public static string UrlEncode(this string url) {
            return HttpUtility.UrlEncode(url);
        }

        
        public static string UrlDecode(this string url) {
            return HttpUtility.UrlDecode(url);
        }

        
        public static string UrlPathEncode(this string url) {
            return HttpUtility.UrlPathEncode(url);
        }
        #endregion

        #region Other
       
        public static bool IsNullOrEmpty(this string input) {
            return String.IsNullOrEmpty(input);
        }

        public static bool EqualsIgnoreCase(this string input, string other) {
            return input.Equals(other, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region Encrypt Decrypt
        
        public static string RSAEncrypt(this string stringToEncrypt, string key) {
            if (string.IsNullOrEmpty(stringToEncrypt)) {
                throw new ArgumentException("An empty string value cannot be encrypted.");
            }

            if (string.IsNullOrEmpty(key)) {
                throw new ArgumentException("Cannot encrypt using an empty key. Please supply an encryption key.");
            }

            var cspp = new System.Security.Cryptography.CspParameters {KeyContainerName = key};

            var rsa = new System.Security.Cryptography.RSACryptoServiceProvider(cspp) {PersistKeyInCsp = true};

            var bytes = rsa.Encrypt(Encoding.UTF8.GetBytes(stringToEncrypt), true);

            return BitConverter.ToString(bytes);
        }

       
        public static string RSADecrypt(this string stringToDecrypt, string key) {
            if (string.IsNullOrEmpty(stringToDecrypt)) {
                throw new ArgumentException("An empty string value cannot be encrypted.");
            }

            if (string.IsNullOrEmpty(key)) {
                throw new ArgumentException("Cannot decrypt using an empty key. Please supply a decryption key.");
            }

            var cspp = new System.Security.Cryptography.CspParameters {KeyContainerName = key};

            var rsa = new System.Security.Cryptography.RSACryptoServiceProvider(cspp) {PersistKeyInCsp = true};

            var decryptArray = stringToDecrypt.Split(new[] { "-" }, StringSplitOptions.None);
            var decryptByteArray = Array.ConvertAll(decryptArray, (s => Convert.ToByte(byte.Parse(s, System.Globalization.NumberStyles.HexNumber))));


            var bytes = rsa.Decrypt(decryptByteArray, true);

            return  Encoding.UTF8.GetString(bytes);
        }
        #endregion

        #region XmlSerialize XmlDeserialize
        
        public static string XmlSerialize<T>(this T objectToSerialise) where T : class {
            var serialiser = new XmlSerializer(typeof(T));
            string xml;
            using (var memStream = new MemoryStream()) {
                using (var xmlWriter = new XmlTextWriter(memStream, Encoding.UTF8)) {
                    serialiser.Serialize(xmlWriter, objectToSerialise);
                    xml = Encoding.UTF8.GetString(memStream.GetBuffer());
                }
            }

            // ascii 60 = '<' and ascii 62 = '>'
            xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
            xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
            return xml;
        }

        public static T XmlDeserialize<T>(this string xml) where T : class {
            var serialiser = new XmlSerializer(typeof(T));
            T newObject;

            using (var stringReader = new StringReader(xml)) {
                using (var xmlReader = new XmlTextReader(stringReader)) {
                    try {
                        newObject = serialiser.Deserialize(xmlReader) as T;
                    }
                    catch (InvalidOperationException) // String passed is not Xml, return null
                    {
                        return null;
                    }

                }
            }

            return newObject;
        }
        #endregion

        //TODO: convert to X extensions
       
    }
}
