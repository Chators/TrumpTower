using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTrumpTower.Spawns.Dijkstra
{
    class User
    {
        string LastName { get; set; }
        string FirstName { get; set; }
        string Mail { get; set; }
        public Vector2 _position;
        // Le tableau ici représente les ARRETES
        public Dictionary<string, Relationship> CircleOfRelationships { get; private set; }

        public User(string lastName, string firstName, string mail, Vector2 position)
        {
            LastName = lastName;
            FirstName = firstName;
            Mail = mail;
            _position = position;
            CircleOfRelationships = new Dictionary<string, Relationship>();
        }

        /* Permet d'ajouter une relation entre 2 utilisateurs
         * Entree : - Un user qui est l'utilisateur à ajouter
         *          - Le type de la relation entre ses 2 utilisateurs
         * Sortie : Rien
         */
        public void AddRelationship(User user)
        {
            // On verifie d'abord si les deux utilisateurs sont déjà en relation
            bool isRelated = (CircleOfRelationships.ContainsKey(user.CompleteName)) ? true : false;

            // Si ils sont déjà en relation, on modifie leur relation (arrête)
            if (isRelated)
            {
            }
            // Sinon on crée une relation (arrête)
            else
            {
                Relationship relationship = new Relationship(this, user);
                // On ajoute dans les tableaux respectif de chacun la relation
                CircleOfRelationships.Add(user.CompleteName, relationship);
                user.CircleOfRelationships.Add(CompleteName, relationship);
            }
        }
        public string CompleteName => LastName + "" + FirstName;

        public List<User> OnSFaitUnPtitDijkstra(Dictionary<string, User> allUser, User targetUser)
        {
            bool isConnexe = false;

            List<List<User>> userConnexe = allUser[CompleteName].GetAllContactsWithDepth(new List<User> { allUser[CompleteName] }, new List<User>(), new List<User>(), new List<List<User>>());
            // On cherche si l'utilisateur est connexe
            for (int i = 0; i < userConnexe.Count; i++)
            {
                List<User> userTmp = userConnexe[i];
                for (int y = 0; y < userTmp.Count; y++)
                {
                    if (userTmp[y] == targetUser)
                    {
                        isConnexe = true;
                        break;
                    }

                }
            }
            Console.WriteLine();

            if (isConnexe)
            {
                Dictionary<User, bool> visited = new Dictionary<User, bool>();
                for (int i = 0; i < userConnexe.Count; i++)
                {
                    for (int j = 0; j < userConnexe[i].Count; j++)
                        visited.Add(userConnexe[i][j], false);
                }
                visited[allUser[CompleteName]] = false;

                Dictionary<User, int> weight = new Dictionary<User, int>();
                for (int i = 0; i < userConnexe.Count; i++)
                {
                    for (int j = 0; j < userConnexe[i].Count; j++)
                        weight.Add(userConnexe[i][j], -1);
                }
                weight[allUser[CompleteName]] = 0;

                Dictionary<User, User> antecedent = new Dictionary<User, User>();
                for (int i = 0; i < userConnexe.Count; i++)
                {
                    for (int j = 0; j < userConnexe[i].Count; j++)
                        antecedent.Add(userConnexe[i][j], null);
                }
                antecedent[allUser[CompleteName]] = null;

                User userDeparture = allUser[CompleteName];
                User userTarget = allUser[targetUser.CompleteName];



                return allUser[CompleteName].GetTheShortestWay(visited, weight, antecedent, userDeparture, userDeparture, userTarget);
            }
            else
            {
                return null;
            }
        }

        private List<List<User>> GetAllContactsWithDepth(List<User> notVisited, List<User> notVisitedMoreDepth, List<User> visited, List<List<User>> arrayOfArrayWithDepth, int depthLevel = 0)
        {
            User userCandidat;

            // On retire le sommet des non visités et on le rajoute dans visité
            notVisited.Remove(this);
            visited.Add(this);

            foreach (Relationship relationship in CircleOfRelationships.Values)
            {
                userCandidat = (relationship.Users[0] == this) ? relationship.Users[1] : relationship.Users[0];
                bool isUnknown = !notVisited.Contains(userCandidat) && !visited.Contains(userCandidat) && !notVisitedMoreDepth.Contains(userCandidat);
                if (isUnknown) notVisitedMoreDepth.Add(userCandidat);
            }

            // Les personnes ne sont pas en contact
            if (notVisited.Count == 0 && notVisitedMoreDepth.Count == 0) return arrayOfArrayWithDepth;
            // On continue à chercher
            else
            {
                if (notVisited.Count == 0 && notVisitedMoreDepth.Count > 0)
                {
                    //arrayOfArrayWithDepth[depthLevel].Add(notVisitedMoreDepth);
                    notVisited = notVisitedMoreDepth;

                    Console.WriteLine("Tous les contacts de pronfondeur " + (depthLevel + 1) + " sont : ");
                    arrayOfArrayWithDepth.Add(new List<User>());
                    for (int i = 0; i < notVisitedMoreDepth.Count; i++)
                    {
                        arrayOfArrayWithDepth[depthLevel].Add(notVisitedMoreDepth[i]);
                        Console.WriteLine("- " + notVisitedMoreDepth[i].CompleteName);
                    }

                    notVisitedMoreDepth = new List<User>();
                    depthLevel++;
                }
                return notVisited[0].GetAllContactsWithDepth(notVisited, notVisitedMoreDepth, visited, arrayOfArrayWithDepth, depthLevel);

            }
        }

        private List<User> GetTheShortestWay(Dictionary<User, bool> visited, Dictionary<User, int> weight, Dictionary<User, User> antecedent, User currentUser, User userDeparture, User userTarget)
        {
            List<User> CreatePath()
            {
                currentUser = userTarget;

                List<User> path = new List<User>();

                path.Add(currentUser);

                while (currentUser != userDeparture)
                {
                    Console.WriteLine(antecedent);
                    path.Add(antecedent[currentUser]);
                    currentUser = antecedent[currentUser];
                }

                path.Reverse();
                return path;
            }

            User userMoreShortest = userDeparture;

            // Tant que le plus petit sommet n'est pas le sommet d'arrivé !
            do
            {
                // On regarde tous les sommets adjacents pour modifier le poids
                foreach (Relationship relationship in currentUser.CircleOfRelationships.Values)
                {
                    User userCandidat = (relationship.Users[0] == currentUser) ? relationship.Users[1] : relationship.Users[0];
                    // Si on n'a pas déjà visité le candidat
                    if (!visited[userCandidat])
                    {
                        if (weight[userCandidat] > weight[currentUser] + 1 || weight[userCandidat] == -1)
                        {
                            weight[userCandidat] = weight[currentUser] + 1;
                            antecedent[userCandidat] = currentUser;
                        }
                    }
                }

                // On marque le sommet comme visité
                visited[currentUser] = true;

                currentUser = null;
                foreach (User candidatUser in visited.Keys)
                {
                    // On cherche le prochain sommet à visité
                    if (!visited[candidatUser] && weight[candidatUser] != -1)
                    {
                        if (currentUser == null || weight[candidatUser] < weight[currentUser])
                            currentUser = candidatUser;
                    }

                    // On cherche le plus petit sommet
                    if (weight[candidatUser] > weight[userMoreShortest])
                        userMoreShortest = candidatUser;
                }

                // On vérifie si tous les sommets sont true
                bool allNodesTrue = true;
                foreach (bool isVisited in visited.Values)
                {
                    if (!isVisited) allNodesTrue = false;
                }
                if (allNodesTrue) userMoreShortest = userTarget;

            } while (userMoreShortest != userTarget);

            return CreatePath();
        }
    }
}
