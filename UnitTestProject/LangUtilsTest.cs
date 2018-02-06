using System;
using NUnit.Framework;
using IEIT.Reports.WebFramework.Core.Enum;
using IEIT.Reports.WebFramework.Api.Resolvers;

namespace UnitTestProject
{
    [TestFixture]
    public class LangUtilsTest
    {
        [Test]
        [TestCase(DisplayLanguage.Kazakh, ExpectedResult = "kk")]
        [TestCase(DisplayLanguage.Russian, ExpectedResult = "ru")]
        [TestCase(DisplayLanguage.English, ExpectedResult = "en")]
        [TestCase(DisplayLanguage.French, ExpectedResult = "fr")]
        [TestCase(DisplayLanguage.Italian, ExpectedResult = "it")]
        [TestCase(DisplayLanguage.Chinese, ExpectedResult = "zh")]
        [TestCase(DisplayLanguage.Japan, ExpectedResult = "ja")]
        [TestCase(DisplayLanguage.Korean, ExpectedResult = "ko")]
        [TestCase(DisplayLanguage.German, ExpectedResult = "de")]
        [TestCase(DisplayLanguage.Kyrgyz, ExpectedResult = "ky")]
        [TestCase(DisplayLanguage.Arabic, ExpectedResult = "ar")]
        [TestCase(DisplayLanguage.Mongolian, ExpectedResult = "mn")]
        [TestCase(DisplayLanguage.Tatar, ExpectedResult = "tt")]
        [TestCase(DisplayLanguage.Turkish, ExpectedResult = "tr")]
        [TestCase(DisplayLanguage.Uzbek, ExpectedResult = "uz")]
        [TestCase(DisplayLanguage.Vietnamese, ExpectedResult = "vi")]
        public string ToShortStringShouldWork(DisplayLanguage lang)
        {
            Assert.AreEqual(OldToShortString(lang), LangUtils.ToShortString(lang));
            return LangUtils.ToShortString(lang);
        }
        [Test]
        public void ShoulThrowWhenNoLang()
        {
            int someBigNumber = 1000;
            Assert.Throws<NotImplementedException>(() => {
                LangUtils.ToShortString((DisplayLanguage)someBigNumber);
            });
        }
        public static string OldToShortString(DisplayLanguage lang)
        {
            switch (lang)
            {
                default:
                    throw new NotImplementedException("Couldn't find language to get its short name!");
                case DisplayLanguage.Kazakh:
                    return "kk";
                case DisplayLanguage.Russian:
                    return "ru";
                case DisplayLanguage.English:
                    return "en";
                case DisplayLanguage.French:
                    return "fr";
                case DisplayLanguage.Italian:
                    return "it";
                case DisplayLanguage.Chinese:
                    return "zh";
                case DisplayLanguage.Japan:
                    return "ja";
                case DisplayLanguage.Korean:
                    return "ko";
                case DisplayLanguage.German:
                    return "de";
                case DisplayLanguage.Kyrgyz:
                    return "ky";
                case DisplayLanguage.Arabic:
                    return "ar";
                case DisplayLanguage.Mongolian:
                    return "mn";
                case DisplayLanguage.Tatar:
                    return "tt";
                case DisplayLanguage.Turkish:
                    return "tr";
                case DisplayLanguage.Uzbek:
                    return "uz";
                case DisplayLanguage.Vietnamese:
                    return "vi";
            }
        }
    }
}
