using System.IO;
using System.Linq;
using System.Text;

namespace Disassembler
{
    public class DisassemblyExporter
    {
        public DisassemblyExporter(DisassemblyResult result, string typeName)
        {
            this.result = result;
            this.typeName = typeName;
        }

        public void Export(StreamWriter writer)
        {
            writer.WriteLine("<!DOCTYPE html><html lang='en'><head><meta charset='utf-8' />");
            writer.WriteLine($"<title>Disassembly of {typeName}</title>");
            writer.WriteLine(CssDefinition);
            writer.WriteLine("</head>");
            writer.WriteLine("<body>");

            writer.WriteLine("<table>");
            writer.WriteLine("<tbody>");

            var methodNameToNativeCode = result.Methods
                .Where(method => string.IsNullOrEmpty(method.Problem))
                .ToDictionary(method => method.Name, method => method.NativeCode);

            foreach (var method in result.Methods.Where(method => string.IsNullOrEmpty(method.Problem)))
            {
                // I am using NativeCode as the id to avoid any problems with special characters like <> in html ;)
                writer.WriteLine(
                    $"<tr><th colspan=\"2\" id=\"{method.NativeCode}\" style=\"text-align: left;\">{FormatMethodAddress(method.NativeCode)} {method.Name}</th><th></th></tr>");

                // there is no need to distinguish the maps visually if there is only one type of code
                var diffTheMaps = method.Maps.SelectMany(map => map.Instructions).Select(ins => ins.GetType()).Distinct().Count() > 1;

                bool evenMap = true;
                foreach (var map in method.Maps)
                {
                    foreach (var instruction in map.Instructions)
                    {
                        writer.WriteLine($"<tr class=\"{(evenMap && diffTheMaps ? "evenMap" : string.Empty)}\">");
                        writer.WriteLine($"<td><pre><code>{instruction.TextRepresentation}</pre></code></td>");

                        if (!string.IsNullOrEmpty(instruction.Comment) && methodNameToNativeCode.TryGetValue(instruction.Comment, out var id))
                        {
                            writer.WriteLine($"<td><a href=\"#{id}\">{GetShortName(instruction.Comment)}</a></td>");
                        }
                        else
                        {
                            writer.WriteLine($"<td>{instruction.Comment}</td>");
                        }

                        writer.WriteLine("</tr>");
                    }

                    evenMap = !evenMap;
                }

                writer.WriteLine("<tr><td colspan=\"{2}\">&nbsp;</td></tr>");
            }

            foreach (var withProblems in result.Methods
                .Where(method => !string.IsNullOrEmpty(method.Problem))
                .GroupBy(method => method.Problem))
            {
                writer.WriteLine($"<tr><td colspan=\"{2}\"><b>{withProblems.Key}</b></td></tr>");
                foreach (var withProblem in withProblems)
                {
                    writer.WriteLine($"<tr><td colspan=\"{2}\">{withProblem.Name}</td></tr>");
                }
                writer.WriteLine("<tr><td colspan=\"{2}\"></td></tr>");
            }

            writer.WriteLine("</tbody></table></body></html>");
        }

        private static string GetShortName(string fullMethodSignature)
        {
            var bracketIndex = fullMethodSignature.IndexOf('(');
            var withoutArguments = fullMethodSignature.Remove(bracketIndex);
            var methodNameIndex = withoutArguments.LastIndexOf('.') + 1;

            return withoutArguments.Substring(methodNameIndex);
        }

        // we want to get sth like "00007ffb`a90f4560"
        internal static string FormatMethodAddress(ulong nativeCode)
        {
            if (nativeCode == default(ulong))
                return string.Empty;

            var buffer = new StringBuilder(nativeCode.ToString("x"));

            if (buffer.Length > 8) // 64 bit address
            {
                buffer.Insert(buffer.Length - 8, '`');

                while (buffer.Length < 8 + 1 + 8)
                    buffer.Insert(0, '0');
            }
            else // 32 bit
            {
                while (buffer.Length < 8)
                    buffer.Insert(0, '0');
            }

            return buffer.ToString();
        }

        private readonly DisassemblyResult result;
        private readonly string typeName;

        internal const string CssDefinition = @"
<style type=""text/css"">
	table { border-collapse: collapse; display: block; width: 100%; overflow: auto; font-family: Verdana }
	th { padding: 6px 13px; border: 1px solid #ddd; }
    td { padding: 2px; }
    td > pre { margin: 0; }
	tr { background-color: #fff; border-top: 1px solid #ccc; }
	tr:nth-child(even) { background: #f8f8f8; }
</style>";
    }
}