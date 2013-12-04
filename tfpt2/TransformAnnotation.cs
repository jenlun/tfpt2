namespace TFSPowerTools2
{
    using System;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;

    public class TransformAnnotation
    {
        private readonly string annotateOutput;

        private readonly ChangeSets changeSets;

        public TransformAnnotation(ChangeSets changeSets, string annotateOutput)
        {
            this.changeSets = changeSets;
            this.annotateOutput = annotateOutput;
            this.ValidateChangeSets();
        }

        public string GetTransformedAnnotation()
        {
            var sb = new StringBuilder();
            //TODO: simplify, ^(\d{1,5}) and replace everything at once, not line by line.
            var regex = new Regex(@"^(\d{1,5})(.*$)");
            using (var reader = new StringReader(this.annotateOutput))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var s = regex.Replace(line, m => this.changeSets.Get(m.Groups[1].Value) + m.Groups[2].Value + Environment.NewLine);
                    sb.Append(s);
                }
            }

            return sb.ToString();
        }

        private void ValidateChangeSets()
        {
            this.GetTransformedAnnotation();
        }

    }
}
