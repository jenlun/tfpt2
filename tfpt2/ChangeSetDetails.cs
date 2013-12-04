namespace TFSPowerTools2
{
    using System;

    [Serializable]
    internal class ChangeSetDetails
    {
        public int ChangeSetId { get; set; }

        public string Username { get; set; }

        public DateTime TimeStamp { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", this.ChangeSetId, this.Username, this.TimeStamp.ToShortDateString());
        }
    }
}