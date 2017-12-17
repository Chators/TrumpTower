using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace LibraryTrumpTower.Constants
{
    public static class BinarySerializer
    {
        public static void Serialize<T>(T obj, string path)
        {
            var serializer = new DataContractSerializer(typeof(T));
            FileStream flux = new FileStream(path, FileMode.Create);
            XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(flux, Encoding.UTF8);
            serializer.WriteObject(writer, obj);
            writer.Flush();
            flux.Close();
        }

        public static T Deserialize<T>(string path)
        {
            var serializer = new DataContractSerializer(typeof(T));
            var reader = XmlReader.Create(path);
            return (T)serializer.ReadObject(reader);
        }
    }
}