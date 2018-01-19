using LibraryTrumpTower.Constants;
using MySql.Data.MySqlClient;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TrumpTower.LibraryTrumpTower;

namespace Menu.BDD
{
    public static class Bdd
    {
        /* Permet de récuperer tous les noms de map dans la BDD return List<string> */
        public static List<string> GetAllNameOfMap()
        {
            /* Permet de reintialiser les noms de map dans le fichier texte sur le serveur*/
            string page = "http://trumptower.heberge-tech.fr:2232/getMap.php";
            RestClient client = new RestClient(page);
            RestRequest requete = new RestRequest();
            client.Execute<string>(requete);

            /* Recuperation des noms de map et mise en forme */
            page = "http://trumptower.heberge-tech.fr:2232/maps.txt";
            client = new RestClient(page);
            requete = new RestRequest();
            Task<IRestResponse<string>> task = client.Execute<string>(requete);

            string allNameMap = task.Result.Content;
            List<string> nameMap = new List<string>();
            string currentName = "";
            for (int i = 0; i < allNameMap.Length; i++)
            {
                if (allNameMap[i] == Convert.ToChar(";"))
                {
                    nameMap.Add(currentName);
                    currentName = "";
                }
                else
                    currentName += allNameMap[i];
            }

            return nameMap;
        }

        /* Permet de récuperer tous les dates dans la BDD return List<string> */
        public static List<string> GetAllDate()
        {
            /* Permet de reintialiser les noms de map dans le fichier texte sur le serveur*/
            string page = "http://trumptower.heberge-tech.fr:2232/getDate.php";
            RestClient client = new RestClient(page);
            RestRequest requete = new RestRequest();
            client.Execute<string>(requete);

            /* Recuperation des noms de map et mise en forme */
            page = "http://trumptower.heberge-tech.fr:2232/date.txt";
            client = new RestClient(page);
            requete = new RestRequest();
            Task<IRestResponse<string>> task = client.Execute<string>(requete);

            string allNameMap = task.Result.Content;
            List<string> nameMap = new List<string>();
            string currentName = "";
            for (int i = 0; i < allNameMap.Length; i++)
            {
                if (allNameMap[i] == Convert.ToChar(";"))
                {
                    nameMap.Add(currentName);
                    currentName = "";
                }
                else
                    currentName += allNameMap[i];
            }

            return nameMap;
        }

        /* Permet de récuperer tous les noms d'auteur dans la BDD return List<string> */
        public static List<string> GetAllAuthor()
        {
            /* Permet de reintialiser les noms de map dans le fichier texte sur le serveur*/
            string page = "http://trumptower.heberge-tech.fr:2232/getAutor.php";
            RestClient client = new RestClient(page);
            RestRequest requete = new RestRequest();
            client.Execute<string>(requete);

            /* Recuperation des noms de map et mise en forme */
            page = "http://trumptower.heberge-tech.fr:2232/auteur.txt";
            client = new RestClient(page);
            requete = new RestRequest();
            Task<IRestResponse<string>> task = client.Execute<string>(requete);

            string allNameMap = task.Result.Content;
            List<string> nameMap = new List<string>();
            string currentName = "";
            for (int i = 0; i < allNameMap.Length; i++)
            {
                if (allNameMap[i] == Convert.ToChar(";"))
                {
                    nameMap.Add(currentName);
                    currentName = "";
                }
                else
                    currentName += allNameMap[i];
            }

            return nameMap;
        }

        /* Permet d'upload une map */
        public static void UploadMap(string pseudo, string nameMap, string mot_clefs, string description, string difficult)
        {
            string page = "http://trumptower.heberge-tech.fr:2232/uploadXML.php";
            RestClient client = new RestClient(page);
            RestRequest requete = new RestRequest();
            requete.Method = Method.POST;
            requete.AddParameter("pseudo", pseudo);
            requete.AddParameter("nomMap", nameMap);
            requete.AddParameter("mot_clefs", mot_clefs);
            requete.AddParameter("description", description);
            requete.AddParameter("difficult", difficult);
            string pathMap = BinarySerializer.pathCustomMap + "/" + nameMap + ".xml";
            requete.AddFile("map", new FileStream(pathMap, FileMode.Open), nameMap + ".xml");
            client.Execute<string>(requete);
        }

        /* Permet de download une map */
        public static void DownLoadMap(string name)
        {
            var page = "http://trumptower.heberge-tech.fr:2232/uploadedMap/" + name + ".txt";

            // on passe l'url de base
            RestClient client = new RestClient(page);
            RestRequest requete = new RestRequest();

            Task<IRestResponse<string>> task = client.Execute<string>(requete);
            string mapXML = task.Result.Content;

            Directory.CreateDirectory(BinarySerializer.pathCustomMap);
            Directory.CreateDirectory(BinarySerializer.pathCampagneMap);
            FileStream flux = new FileStream(BinarySerializer.pathCustomMap + "/" + name + ".xml", FileMode.Create);
            StreamWriter writer = new StreamWriter(flux);
            writer.WriteLine(mapXML);
            writer.Flush();
            flux.Close();
        }
    }
}
