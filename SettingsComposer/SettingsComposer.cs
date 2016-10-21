using BorrehSoft.Utensils.Collections.Settings;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Composing
{
    public class SettingsComposer
    {
        private CodeDomProvider CsharpFormatter;
        public string IndentDeepener { get; private set; }

        public SettingsComposer(string indentDeepener = "   ")
        {
            this.IndentDeepener = indentDeepener;
            this.CsharpFormatter = CodeDomProvider.CreateProvider("CSharp");
        }

        public void SettingsToFile(Settings settings, string file)
        {
            using (StreamWriter writer = new StreamWriter(file))
            {
                SettingsToWriter(settings, writer);
            }
        }

        private void SettingsToWriter(Settings settings, StreamWriter writer, string currentIndent = "")
        {
            writer.WriteLine('{');

            foreach (KeyValuePair<string, object> item in settings.Dictionary)
            {
                AssignmentToWriter(item, writer, currentIndent + this.IndentDeepener);
            }

            writer.Write(currentIndent);
            writer.Write('}');
        }

        private void AssignmentToWriter(KeyValuePair<string, object> item, StreamWriter writer, string currentIndent)
        {
            writer.Write(currentIndent);
            writer.Write(item.Key);
            writer.Write(" = ");

            ValueToWriter(item.Value, writer, currentIndent);

            writer.WriteLine(';');
        }

        private void ValueToWriter(object value, StreamWriter writer, string indent)
        {
            Settings potentialSubsettings = value as Settings;
            IEnumerable<object> potentialArray = value as IEnumerable<object>;

            if (potentialSubsettings != null)
            {
                SettingsToWriter(potentialSubsettings, writer, indent);
            }
            else if (potentialArray != null)
            {
                ArrayToWriter(potentialArray, writer, indent);
            }
            else if (value is string)
            {
                this.CsharpFormatter.GenerateCodeFromExpression(
                    new CodePrimitiveExpression(value as string), writer, null);
            }
            else
            {
                writer.Write(value.ToString());
            }
        }

        private void ArrayToWriter(IEnumerable<object> potentialArray, StreamWriter writer, string indent)
        {
            writer.WriteLine('[');
            foreach (object value in potentialArray)
            {                
                ValueToWriter(value, writer, indent + this.IndentDeepener);
                writer.WriteLine(',');
            }

            writer.Write(indent);            
            writer.Write("]");
        }
    }
}
