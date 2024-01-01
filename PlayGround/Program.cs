using H3Hacker.Utility;
using System.Text;

System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
var encodingGBK = Encoding.GetEncoding("GBK");
var encodingUnicode = Encoding.Unicode;
Console.WriteLine(encodingGBK.GetBytes("查尔斯爵士").ToStringByEncoding());
Console.WriteLine(encodingUnicode.GetBytes("查尔斯爵士").ToStringByEncoding());