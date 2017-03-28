using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Diagnostics;
using System.IO;

namespace KevinMohanTurnerSWAPI
{
    public class Program
    {
        //class variable to keep track of what's been printed; only using static because of the no duplicate requirement and because it's a simple console app with 1 user
        static List<string> printHistory = new List<string>();

        public static void Main(string[] args)
        {
            try
            {
                var sanitaryInput = string.Empty;
                do
                {
                    sanitaryInput = getFilmInput();
                } while (sanitaryInput == string.Empty);

                var parsedTitle = parseFilmTitle(sanitaryInput);
                var subProperty = getSubProperty(sanitaryInput);

                if(sanitaryInput.Contains("characters") && !sanitaryInput.Contains("starships") && !sanitaryInput.Contains("planets"))
                { //characters
                    printCharacters(parsedTitle, subProperty);
                }
                else if (!sanitaryInput.Contains("characters") && sanitaryInput.Contains("starships") && !sanitaryInput.Contains("planets"))
                { //starships
                    printStarships(parsedTitle, subProperty);
                }
                else if (!sanitaryInput.Contains("characters") && !sanitaryInput.Contains("starships") && sanitaryInput.Contains("planets"))
                {//planets
                    printPlanets(parsedTitle, subProperty);
                }
                else
                {//basic error handling
                    Console.WriteLine("You may only specify characters, starships, or planets in your search but must pick one.");
                }

                Console.Read(); //keeps window open
            }
            catch (Exception ex)
            {
                var errorMessageLog = new StringBuilder();
                errorMessageLog.Capacity = 100;
                errorMessageLog.Append(String.Format("An unexpected error has occurred. Details: {0}", ex.Message));
                Console.WriteLine(errorMessageLog.ToString());
                Console.Read();
                errorMessageLog.Clear();
            }
        }

        public static string getFilmInput()
        {
            try
            {
                Console.WriteLine("\nPlease enter your film title in double quotes followed by film property followed by subproperty:\n");
                var inputFilmText = Console.ReadLine();
                if(inputFilmText != null)
                {
                    if(inputFilmText.Length > 2 && inputFilmText.Length < 100 )
                    {
                        if(!inputFilmText[0].Equals('"'))
                        {
                            throw new Exception("Please surround the film name in double quotes and try again.");
                        }
                        else if(!inputFilmText.ToLower().Contains("characters") && !inputFilmText.ToLower().Contains("starships") && !inputFilmText.ToLower().Contains("planets"))
                        {
                            throw new Exception("Please specify the property of the film you would like (characters, starships, planets).");
                        }
                    }
                    else if(inputFilmText.Length >= 100) //arbitrary film title string length being too long
                    {
                        throw new Exception("The film title was too long. Please try again.");
                    }
                }
                else
                {
                    throw new Exception("Please enter a film title.");
                }

                return inputFilmText;
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("Sorry, I didn't understand that. {0}", ex.Message));
                return string.Empty;
            }
        }

        public static async Task printCharacters(string filmTitle, string subProperty)
        {
            try
            {
                var characterAPICalls = await getFilms(filmTitle, 2);
                Task[] taskArray = new Task[characterAPICalls.Count];

                for (int i = 0; i < taskArray.Length; i++)
                {
                    taskArray[i] = await Task.Factory.StartNew(() => printCharacter(characterAPICalls[i], subProperty));
                }

                Task.WaitAll(taskArray); //wait for async calls so we can print exit at the very end.
                Console.WriteLine("\n\nPress enter to exit.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error has occurred.");
            }
        }

        public static async Task printCharacter(string character, string subProperty)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    using (var asyncData = await client.GetAsync(new Uri(character)))
                    {
                        string result = await asyncData.Content.ReadAsStringAsync();
                        var resultObject = JsonConvert.DeserializeObject<SWAPICharactersModel>(result);
                        switch (subProperty.Trim())
                        {
                            case "name":
                                if (!printHistory.Contains(resultObject.name))
                                {
                                    printHistory.Add(resultObject.name);
                                    Console.WriteLine(resultObject.name);
                                }
                                break;
                            case "height":
                                if (!printHistory.Contains(resultObject.height))
                                {
                                    printHistory.Add(resultObject.height);
                                    Console.WriteLine(resultObject.height);
                                }
                                break;
                            case "mass":
                                if (!printHistory.Contains(resultObject.mass))
                                {
                                    printHistory.Add(resultObject.mass);
                                    Console.WriteLine(resultObject.mass);
                                }
                                break;
                            case "hair_color":
                                if (!printHistory.Contains(resultObject.hair_color))
                                {
                                    printHistory.Add(resultObject.hair_color);
                                    Console.WriteLine(resultObject.hair_color);
                                }
                                break;
                            case "skin_color":
                                if (!printHistory.Contains(resultObject.skin_color))
                                {
                                    printHistory.Add(resultObject.skin_color);
                                    Console.WriteLine(resultObject.skin_color);
                                }
                                break;
                            case "eye_color":
                                if (!printHistory.Contains(resultObject.eye_color))
                                {
                                    printHistory.Add(resultObject.eye_color);
                                    Console.WriteLine(resultObject.eye_color);
                                }
                                break;
                            case "birth_year":
                                if (!printHistory.Contains(resultObject.birth_year))
                                {
                                    printHistory.Add(resultObject.birth_year);
                                    Console.WriteLine(resultObject.birth_year);
                                }
                                break;
                            case "gender":
                                if (!printHistory.Contains(resultObject.gender))
                                {
                                    printHistory.Add(resultObject.gender);
                                    Console.WriteLine(resultObject.gender);
                                }
                                break;
                            case "homeworld":
                                if (!printHistory.Contains(resultObject.homeworld))
                                {
                                    printHistory.Add(resultObject.homeworld);
                                    Console.WriteLine(resultObject.homeworld);
                                }
                                break;
                            default:
                                if (!printHistory.Contains(resultObject.name))
                                {
                                    printHistory.Add(resultObject.name);
                                    Console.WriteLine(resultObject.name);
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error has occurred.");
            }
        }

        public static async Task printStarships(string filmTitle, string subProperty)
        {
            try
            {
                var starshipsAPICalls = await getFilms(filmTitle, 1);
                Task[] taskArray = new Task[starshipsAPICalls.Count];

                for (int i = 0; i < taskArray.Length; i++)
                {
                    taskArray[i] = await Task.Factory.StartNew(() => printStarship(starshipsAPICalls[i], subProperty));
                }

                Task.WaitAll(taskArray);
                Console.WriteLine("\n\nPress enter to exit.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error has occurred.");
            }
        }

        public static async Task printStarship(string starship, string subProperty)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    using (var asyncData = await client.GetAsync(new Uri(starship)))
                    {
                        string result = await asyncData.Content.ReadAsStringAsync();
                        var resultObject = JsonConvert.DeserializeObject<SWAPIStarshipsModel>(result);
                        switch (subProperty.Trim())
                        {
                            case "name":
                                if (!printHistory.Contains(resultObject.name))
                                {
                                    printHistory.Add(resultObject.name);
                                    //if you want to see the millisecond timing between console writelines:
                                    //Console.WriteLine(DateTime.Now.Millisecond + " (ms)  -   " + resultObject.name);
                                    Console.WriteLine(resultObject.name);
                                }
                                break;
                            case "model":
                                if (!printHistory.Contains(resultObject.model))
                                {
                                    printHistory.Add(resultObject.model);
                                    Console.WriteLine(resultObject.model);
                                }
                                break;
                            case "manufacturer":
                                if (!printHistory.Contains(resultObject.manufacturer))
                                {
                                    printHistory.Add(resultObject.manufacturer);
                                    Console.WriteLine(resultObject.manufacturer);
                                }
                                break;
                            case "cost_in_credits":
                                if (!printHistory.Contains(resultObject.cost_in_credits))
                                {
                                    printHistory.Add(resultObject.cost_in_credits);
                                    Console.WriteLine(resultObject.cost_in_credits);
                                }
                                break;
                            case "length":
                                if (!printHistory.Contains(resultObject.length))
                                {
                                    printHistory.Add(resultObject.length);
                                    Console.WriteLine(resultObject.length);
                                }
                                break;
                            case "max_atmosphering_speed":
                                if (!printHistory.Contains(resultObject.max_atmosphering_speed))
                                {
                                    printHistory.Add(resultObject.max_atmosphering_speed);
                                    Console.WriteLine(resultObject.max_atmosphering_speed);
                                }
                                break;
                            case "crew":
                                if (!printHistory.Contains(resultObject.crew))
                                {
                                    printHistory.Add(resultObject.crew);
                                    Console.WriteLine(resultObject.crew);
                                }
                                break;
                            case "passengers":
                                if (!printHistory.Contains(resultObject.passengers))
                                {
                                    printHistory.Add(resultObject.passengers);
                                    Console.WriteLine(resultObject.passengers);
                                }
                                break;
                            case "cargo_capacity":
                                if (!printHistory.Contains(resultObject.cargo_capacity))
                                {
                                    printHistory.Add(resultObject.cargo_capacity);
                                    Console.WriteLine(resultObject.cargo_capacity);
                                }
                                break;
                            case "consumables":
                                if (!printHistory.Contains(resultObject.consumables))
                                {
                                    printHistory.Add(resultObject.consumables);
                                    Console.WriteLine(resultObject.consumables);
                                }
                                break;
                            case "hyperdrive_rating":
                                if (!printHistory.Contains(resultObject.hyperdrive_rating))
                                {
                                    printHistory.Add(resultObject.hyperdrive_rating);
                                    Console.WriteLine(resultObject.hyperdrive_rating);
                                }
                                break;
                            case "MGLT":
                                if (!printHistory.Contains(resultObject.MGLT))
                                {
                                    printHistory.Add(resultObject.MGLT);
                                    Console.WriteLine(resultObject.MGLT);
                                }
                                break;
                            case "starship_class":
                                if (!printHistory.Contains(resultObject.starship_class))
                                {
                                    printHistory.Add(resultObject.starship_class);
                                    Console.WriteLine(resultObject.starship_class);
                                }
                                break;
                            default:
                                if (!printHistory.Contains(resultObject.name))
                                {
                                    printHistory.Add(resultObject.name);
                                    Console.WriteLine(resultObject.name);
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error has occurred.");
            }
        }

        public static async Task printPlanets(string filmTitle, string subProperty)
        {
            try
            {
                var planetAPICalls = await getFilms(filmTitle, 3);
                Task[] taskArray = new Task[planetAPICalls.Count];

                for (int i = 0; i < taskArray.Length; i++)
                {
                    taskArray[i] = await Task.Factory.StartNew(() => printPlanet(planetAPICalls[i], subProperty));
                }

                Task.WaitAll(taskArray);
                Console.WriteLine("\n\nPress enter to exit.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error has occurred.");
            }
        }

        public static async Task<string> printPlanet(string planet, string subProperty)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    using (var asyncData = await client.GetAsync(new Uri(planet)))
                    {
                        string result = await asyncData.Content.ReadAsStringAsync();
                        var resultObject = JsonConvert.DeserializeObject<SWAPIPlanetsModel>(result);
                        switch (subProperty.Trim())
                        {
                            case "name":
                                if (!printHistory.Contains(resultObject.name))
                                {
                                    printHistory.Add(resultObject.name);
                                    Console.WriteLine(resultObject.name);
                                }
                                break;
                            case "rotation_period":
                                if (!printHistory.Contains(resultObject.rotation_period))
                                {
                                    printHistory.Add(resultObject.rotation_period);
                                    Console.WriteLine(resultObject.rotation_period);
                                }
                                break;
                            case "orbital_period":
                                if (!printHistory.Contains(resultObject.orbital_period))
                                {
                                    printHistory.Add(resultObject.orbital_period);
                                    Console.WriteLine(resultObject.orbital_period);
                                }
                                break;
                            case "diameter":
                                if (!printHistory.Contains(resultObject.diameter))
                                {
                                    printHistory.Add(resultObject.diameter);
                                    Console.WriteLine(resultObject.diameter);
                                }
                                break;
                            case "climate":
                                if (!printHistory.Contains(resultObject.climate))
                                {
                                    printHistory.Add(resultObject.climate);
                                    Console.WriteLine(resultObject.climate);
                                }
                                break;
                            case "gravity":
                                if (!printHistory.Contains(resultObject.gravity))
                                {
                                    printHistory.Add(resultObject.gravity);
                                    Console.WriteLine(resultObject.gravity);
                                }
                                break;
                            case "terrain":
                                if (!printHistory.Contains(resultObject.terrain))
                                {
                                    printHistory.Add(resultObject.terrain);
                                    Console.WriteLine(resultObject.terrain);
                                }
                                break;
                            case "surface_water":
                                if (!printHistory.Contains(resultObject.surface_water))
                                {
                                    printHistory.Add(resultObject.surface_water);
                                    Console.WriteLine(resultObject.surface_water);
                                }
                                break;
                            case "population":
                                if (!printHistory.Contains(resultObject.population))
                                {
                                    printHistory.Add(resultObject.population);
                                    Console.WriteLine(resultObject.population);
                                }
                                break;
                            default:
                                if (!printHistory.Contains(resultObject.name))
                                {
                                    printHistory.Add(resultObject.name);
                                    Console.WriteLine(resultObject.name);
                                }
                                break;
                        }
                    }
                }
                return printHistory[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error has occurred.");
                return string.Empty;
            }
        }
        public static async Task<List<string>> getFilms(string filmTitle, int callingProperty)
        {
            try
            {
                var films = new List<SWAPIFilmModel>();
                int incr = 0;
                bool matched = false;
                string filmUrlById;
                var film = new SWAPIFilmModel();

                while (!matched)
                {
                    filmUrlById = String.Format("http://swapi.co/api/films/{0}", incr);
                    
                    using (var client = new HttpClient())
                    {
                        using (var asyncData = await client.GetAsync(new Uri(filmUrlById)))
                        {
                            string result = await asyncData.Content.ReadAsStringAsync();
                            film = JsonConvert.DeserializeObject<SWAPIFilmModel>(result);
                            if(film != null)
                            {
                                if(film.title != null)
                                {
                                    if (film.title.Equals(filmTitle))
                                    {
                                        matched = true;
                                    }
                                }
                            }
                        }
                    }

                    incr++;
                }

                if(callingProperty == (int)CallingProperty.Characters)
                {
                    return film.characters;
                }
                else if (callingProperty == (int)CallingProperty.Starships)
                {
                    return film.starships;
                }
                else if (callingProperty == (int)CallingProperty.Planets)
                {
                    return film.planets;
                }
                else
                {
                    return new List<string>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error has occurred.");
                return new List<string>();
            }
        }

        public static string parseFilmTitle(string inputText)
        {
            var inputList = inputText.Split('"');
            return inputList[1].ToString().Trim();
        }

        public static string getSubProperty(string inputText)
        {
            return inputText.Substring(inputText.LastIndexOf(' '), inputText.Length - inputText.LastIndexOf(' '));
        }
    }
}