namespace TFSPowerTools2
{
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
            var regex = new Regex(@"^(\d{1,5})", RegexOptions.Multiline);
            return regex.Replace(this.annotateOutput, match => this.changeSets.Get(match.Groups[1].Value));
        }

        private void ValidateChangeSets()
        {
            this.GetTransformedAnnotation();
        }
    }
}
