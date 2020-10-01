using ApiHelper.Models;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;

namespace ApiHelper.Processor
{
    public class DogApiProcessor
    {

        public static async Task­ <List<DogModels>> LoadBreedList()
        {
            ///TODO : À compléter LoadBreedList
            /// Attention le type de retour n'est pas nécessairement bon
            /// J'ai mis quelque chose pour avoir une base
            /// TODO : Compléter le modèle manquant
            ///

            string url;

            url = "https://dog.ceo/api/breeds/list/all";


            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    List<DogModels> dogModels = new List<DogModels>();
                    BreedsModel result = await response.Content.ReadAsAsync<BreedsModel>();

                   
                    var familles = result.Breeds.Keys.ToList();

                    foreach(var breed in familles)
                    {
                        DogModels race = new DogModels();
                        race.Nom = breed;
                        dogModels.Add(race);
                    }


                    return dogModels;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            
        }

        public static async Task<DogModels> GetImageUrl(string breed)
        {
            string url;

            url = $"https://dog.ceo/api/breed/{breed}/images/random";


            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    
                    DogModels result = await response.Content.ReadAsAsync<DogModels>();

                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

            
        }

        

    }
}
