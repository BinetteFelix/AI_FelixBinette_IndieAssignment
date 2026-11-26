using SalvageSprint.Core;
using NUnit.Framework;

namespace SalvageSprint.Tests
{
    [Category("Infrastructure")]
    public sealed class NavigationRegressionTests
    {
        [Test]
        public void CompletePathWithUnknownDistanceRemainsInProgress()
        {
            Assert.That(NavigationFeedback.Classify(true, false, true, true, float.PositiveInfinity, 0.6f), Is.EqualTo(TravelStatus.Moving));
        }
        [Test]
        public void InCompletePathWithUnknownDistanceStillFails()
        {
            Assert.That(NavigationFeedback.Classify(true, false, false, true, float.PositiveInfinity, 0.6f), Is.EqualTo(TravelStatus.Failed));
        }
    }
}