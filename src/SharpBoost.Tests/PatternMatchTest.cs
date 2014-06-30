using System;
using SharpBoost.FunProg;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharpBoost.Tests {
    [TestClass]
    public class PatternMatchTest {
        [TestMethod]
        public void TestFluentSyntax() {
            var matcher = Lambda.F<string, int>(str =>
                str.MatchOf<string, int>()
                    .Case("abc", -1)
                    .Case("bcd")(s => s.Length + 1)
                    .Case(s => !String.IsNullOrEmpty(s))(s => s.Length)
                    .Default(0)
                    .ProcessMatch());

            Assert.AreEqual(-1, matcher("abc"));
            Assert.AreEqual(4, matcher("bcd"));
            Assert.AreEqual(3, matcher("cde"));
            Assert.AreEqual(0, matcher(String.Empty));
            Assert.AreEqual(0, matcher(null));

        }

        [TestMethod]
        public void TestCollectionSyntax() {
            var matcher = Lambda.F<string, int>(str =>
                str.MatchOf(new PatternMatch<string, int> {
                    {"abc", -1},
                    {"bcd", s => s.Length + 1},
                    {s => !String.IsNullOrEmpty(s), s => s.Length},
                    0
                }));

            Assert.AreEqual(-1, matcher("abc"));
            Assert.AreEqual(4, matcher("bcd"));
            Assert.AreEqual(3, matcher("cde"));
            Assert.AreEqual(0, matcher(String.Empty));
            Assert.AreEqual(0, matcher(null));
        }
    }
}
