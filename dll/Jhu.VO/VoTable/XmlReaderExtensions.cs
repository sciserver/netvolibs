using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Jhu.VO.VoTable
{
    public static class XmlReaderExtensions
    {
        private static HashSet<string> GetTagSet(string[] tagnames)
        {
            var tagset = new HashSet<string>(VoTable.Comparer);

            if (tagnames != null)
            {
                for (int i = 0; i < tagnames.Length; i++)
                {
                    tagset.Add(tagnames[i]);
                }
            }

            return tagset;
        }

        public static async Task<bool> MoveAfterStartAsync(this XmlReader reader, params string[] tagnames)
        {
            var tagset = GetTagSet(tagnames);

            while (!reader.EOF)
            {
                if (reader.NodeType == XmlNodeType.Element &&
                    tagset.Contains(reader.Name))
                {
                    var empty = reader.IsEmptyElement;
                    await reader.ReadAsync();
                    return empty;
                }

                await reader.ReadAsync();
            }

            throw Error.InvalidFormat();
        }

        public static async Task<bool> MoveAfterEndAsync(this XmlReader reader, params string[] tagnames)
        {
            var tagset = GetTagSet(tagnames);

            while (!reader.EOF)
            {
                if (reader.NodeType == XmlNodeType.Element &&
                    reader.IsEmptyElement &&
                    tagset.Contains(reader.Name))
                {
                    await reader.ReadAsync();
                    return true;
                }
                else if (reader.NodeType == XmlNodeType.EndElement &&
                    tagset.Contains(reader.Name))
                {
                    await reader.ReadAsync();
                    return false;
                }

                await reader.ReadAsync();
            }

            throw Error.InvalidFormat();
        }

        public static bool IsEndElement(this XmlReader reader, params string[] tagnames)
        {
            var tagset = GetTagSet(tagnames);

            reader.MoveToContent();

            return (reader.NodeType == XmlNodeType.Element && reader.IsEmptyElement ||
                    reader.NodeType == XmlNodeType.EndElement) &&
                    tagset.Contains(reader.Name);
        }
    }
}
