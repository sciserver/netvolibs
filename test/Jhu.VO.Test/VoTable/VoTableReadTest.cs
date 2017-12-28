using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.VO.VoTable
{
    [TestClass]
    public class VoTableReadTest : TestClassBase
    {
        private void ReadTableDataTestHelper(string filename, int[] rows, object[][] gt)
        {
            VoTable vt;
            VoTableResource res;

            var file = GetTestFilePath(filename);
            vt = new VoTable(file, FileAccess.Read);
            vt.ReadHeaderAsync().Wait();

            int q = 0;

            while ((res = vt.ReadNextResource()) != null)
            {
                res.ReadHeaderAsync().Wait();
                var values = new object[res.Columns.Count];

                if (gt != null && q == 0)
                {
                    Assert.AreEqual(gt[0].Length, res.Columns.Count);
                }
                
                int r = 0;
                while (res.ReadNextRowAsync(values, 0).Result)
                {
                    if (gt != null && q == 0)
                    {
                        for (int i = 0; i < res.Columns.Count; i++)
                        {
                            Assert.AreEqual(gt[r][i], values[i]);
                        }
                    }
                    r++;
                }

                res.ReadToFinishAsync().Wait();
                res.ReadFooterAsync().Wait();

                Assert.AreEqual(rows[q], r);
            }

            vt.ReadFooterAsync().Wait();
        }

        [TestMethod]
        public void ReadTableDataAllPrimitiveTest()
        {
            var gt = new object[][]
            {
                new object [] {
                    true, new SharpFitsIO.Bit(true), (byte)1, (short)1, 1, 1L, "a", "ő","almafa","körtefa","szilvafa","tréfa",
                    0.1234567f, 0.1234567891,
                    new SharpFitsIO.SingleComplex(12.434f, 54.23f),
                    new SharpFitsIO.DoubleComplex(32.343, 34.12)
                },
                new object[] {
                    null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null
                }
            };
            ReadTableDataTestHelper(@"test\files\votable\votable_tabledata_allprimitive.xml", new[] { 2 }, gt);
        }

        [TestMethod]
        public void ReadTableDataAllPrimitiveMagicNullsTest()
        {
            var gt = new object[][]
            {
                new object [] {
                    true, new SharpFitsIO.Bit(true), (byte)1, (short)1, 1, 1L, "a", "ő","almafa","körtefa","szilvafa","tréfa",
                    0.1234567f, 0.1234567891,
                    new SharpFitsIO.SingleComplex(12.434f, 54.23f),
                    new SharpFitsIO.DoubleComplex(32.343, 34.12)
                },
                new object[] {
                    null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null
                }
            };
            ReadTableDataTestHelper(@"test\files\votable\votable_tabledata_allprimitive_magic.xml", new[] { 2 }, gt);
        }
    }
}
