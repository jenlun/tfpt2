namespace TfPt2.Tests
{
    using NUnit.Framework;

    using TFSPowerTools2;

    [TestFixture]
    public class ParseTests
    {
        private const string History = @"Changeset Change                     User              Date       Comment
--------- -------------------------- ----------------- ---------- -----------------------------------------------------------------------------------
28        edit                       XXXXXXX\mica1     10/11/2013 SBI ? - Updated for sonar coding standard compliance
21868     merge                      XXXXXXX\mica1     6/6/2013   Merge for HyperMarket wk 21 GameResult Bug Fix to Prod
21579     merge, edit                XXXXXXX\mica1     5/29/2013  Merge for week 20 Hypermarket QA deploy
19493     merge, branch              XXXXXXX\joil      3/18/2013  SBI-179 Sporting Solution Game Action/ Result Refactoring";

        private const string AnnotatedLine = @"28           public class BasketballGameActionHandler : IGameActionHandler";

        [Test]
        public void ParseHistory()
        {
            var changeSets = new ChangeSets();
            changeSets.AddHistory(History);

            Assert.AreEqual(@"28, XXXXXXX\mica1, 10/11/2013", changeSets.Get("28"));

            var transform = new TransformAnnotation(changeSets, AnnotatedLine);
            var transformed = transform.GetTransformedAnnotation();
            Assert.AreEqual(@"28, XXXXXXX\mica1, 10/11/2013           public class BasketballGameActionHandler : IGameActionHandler", transformed);
        }

        [TestFixtureSetUp]
        public void SetUp()
        {
            ChangeSets.ClearCache();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            ChangeSets.ClearCache();
        }
    }
}
