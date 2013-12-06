namespace TFSPowerTools2
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text.RegularExpressions;

    public class ChangeSets
    {
        private static readonly string TempFilePath = Path.GetTempPath() + "changesets.tmp";
        private Dictionary<int, ChangeSetDetails> csd = new Dictionary<int, ChangeSetDetails>();

        public ChangeSets()
        {
            this.RestoreSavedState();
        }

        public static void ClearCache()
        {
            File.Delete(TempFilePath);
        }

        public string Get(string stringKey)
        {
            return this.csd[int.Parse(stringKey)].ToString();
        }

        public void AddHistory(string history)
        {
            this.BuildChangeSetDetails(history);
            this.SaveState();
        }

        private void SaveState()
        {
            using (new GlobalLock(2000))
            {
                using (Stream stream = File.Open(TempFilePath, FileMode.Create))
                {
                    var bformatter = new BinaryFormatter();

                    bformatter.Serialize(stream, this.csd);
                }
            }
        }

        private void RestoreSavedState()
        {
            using (new GlobalLock(2000))
            {
                try
                {
                    using (var stream = File.Open(TempFilePath, FileMode.Open))
                    {
                        var bformatter = new BinaryFormatter();

                        this.csd = (Dictionary<int, ChangeSetDetails>)bformatter.Deserialize(stream);
                    }
                }
                catch (FileNotFoundException)
                {
                    // ignore first time
                }
            }
        }

        private void BuildChangeSetDetails(string history)
        {
            using (var reader = new StringReader(history))
            {
                // skip 2 lines
                reader.ReadLine();
                reader.ReadLine();

                var regex = new Regex(@"(\d+)\s+.+\s([\w\\]+).+\D(\d{1,2}/\d{1,2}/\d{4}).+");

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var m = regex.Match(line);

                    int changesetId = int.Parse(m.Groups[1].Value);

                    ChangeSetDetails cs;
                    var keyExists = this.csd.TryGetValue(changesetId, out cs);
                    if (!keyExists)
                    {
                        string username = m.Groups[2].Value;
                        string timestamp = m.Groups[3].Value;
                        cs = new ChangeSetDetails(changesetId, timestamp, username);

                        this.csd[changesetId] = cs;
                    }
                }
            }
        }

        [Serializable]
        private class ChangeSetDetails
        {
            private readonly int changeSetId;

            private readonly string timeStamp;

            private readonly string username;

            public ChangeSetDetails(int changeSetId, string timeStamp, string username)
            {
                this.changeSetId = changeSetId;
                this.timeStamp = timeStamp;
                this.username = username;
            }

            public override string ToString()
            {
                return string.Format("{0}, {1}, {2}", this.changeSetId, this.username, this.timeStamp);
            }
        }
    }
}