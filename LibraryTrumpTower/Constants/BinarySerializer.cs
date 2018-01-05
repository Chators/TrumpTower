﻿using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace LibraryTrumpTower.Constants
{
    public static class BinarySerializer
    {
        public static string pathCurrentMapXml = "MapXml/CurrentMap.xml";
        public static string pathMapXml = "MapXml";
        public static string pathCustomMap = "MapXml/CustomMap";
        public static string pathCampagneMap = "MapXml/CampagneMap";

        public static void Serialize<T>(T obj, string name)
        {
            Directory.CreateDirectory(pathCustomMap);
            Directory.CreateDirectory(pathCampagneMap);

            var serializer = new DataContractSerializer(typeof(T));
            FileStream flux = new FileStream(pathMapXml + "/" + name, FileMode.Create);
            XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(flux, Encoding.UTF8);
            serializer.WriteObject(writer, obj);
            writer.Flush();
            flux.Close();
        }

        public static T Deserialize<T>(string path)
        {
            var serializer = new DataContractSerializer(typeof(T));
            var reader = XmlReader.Create(pathMapXml + "/CurrentMap.xml");
            return (T)serializer.ReadObject(reader);
        }

        /*
        public static T Deserialize<T>(string path)
        {
            var serializer = new DataContractSerializer(typeof(T));
            var reader = XmlReader.Create("../../../../MapXml/" + path);
            return (T)serializer.ReadObject(reader);
        }
        */
    }
}