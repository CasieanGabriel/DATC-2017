using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace T1.Classes
{
    class Function
    {
        public static string MAINURL = "http://datc-rest.azurewebsites.net";

        #region MENU
        public static void ShowMainMenu()
        {
            string menuList = "\n1- Show Breweries\n" +
                "2- Add new beer\n" + 
                 "0- Exit\n";
            Console.WriteLine(menuList);
        }

        public static int SelectBreweryIDMenu(ref int chosenBreweryID)
        {
            int option = 0;
            string menuList = "\nPlease choose a brewery\n" +
                 "1- Show brewery beers\n" +
                 "0- Back\n";
            Console.WriteLine(menuList);
            option = Convert.ToInt32(Console.ReadLine());
            if (option == 1)
            {
                Console.WriteLine("Insert a brewery ID");
                chosenBreweryID = Convert.ToInt32(Console.ReadLine());
            }
            else
                if (option == 0) // go back
                return -1;
            return option;
        }

        public static int ShowReviewMenu(ref int chosenBeerID)
        {
            int option = 0;
            string menuList = "\nIf you want to continue select review\n" +
                 "1- Show Review\n" +
                 "0- Back\n";
            Console.WriteLine(menuList);
            option = Convert.ToInt32(Console.ReadLine());
            if( option == 1)
            {
                Console.WriteLine("Select a beer ID ");
                chosenBeerID = Convert.ToInt32(Console.ReadLine());
            }
            if (option == 0)
                return -1;
            return option;
        }

        #endregion


        #region SelectedOptions

        public static void FirstOptionSelected( int option,  JObject mainObj)
        {
            switch (option)
            {
                case 1:
                    int breweryopt = 0;
                    List<Breweries> breweriesList = new List<Breweries>();
                    breweriesList = GetBrieweriesDataFromJSON(mainObj);
                    ShowAllBreweries(breweriesList);

                    do
                    {
                        int chosenBreweryID = -1;
                        breweryopt = SelectBreweryIDMenu(ref chosenBreweryID);
                        if (breweryopt == -1)
                            break;
                        else
                        {
                            if (breweryopt != 0)
                            {
                                SecondOptionsSelected(breweryopt,chosenBreweryID,breweriesList);
                            }
                        }
                    }
                    while (breweryopt != 0);

                    break;
                case 2:
                    PostNewBeer(MAINURL, "/beers");
                    break;
                default:
                    break;
            }
        }

        public static void SecondOptionsSelected(int option, int breweryID ,List<Breweries>breweriesList)
        {
            switch(option)
            {
                case 1:
                    JObject jobject = null;
                    string href = "";
                    int sw = 0;
                    foreach (var item in breweriesList)
                    {
                        if (item.Id == breweryID)
                        {
                            href = item.Href;
                            sw++;
                        }
                    }
                    if (sw == 0)
                    {
                        Console.WriteLine("Does not exist :( Try again");
                        break;
                    }
                    else
                    {
                        int opt = 0;
                        jobject = GetJSONFromURL(MAINURL, href);
                        List<Beers> beersList = new List<Beers>();
                        beersList = GetBeersDataFromJSON(jobject);
                        ShowAllBeers(beersList);
                        do
                        {
                            Console.WriteLine("Do you want to continue? If yes, select if you want to go to style or more info about the beers\n");
                            Console.WriteLine("1- Style\n2- More Info\n0- Back");
                            opt = Convert.ToInt32(Console.ReadLine());
                            if (opt == 1 || opt == 2 || opt == 0)
                            {
                                StyleOrMoreInfoSelected(opt, beersList);
                            }
                            else
                            {
                                Console.WriteLine("Wrong option!");
                            }
                        }
                        while (opt != 0);
                    }
                    break;
                case 2:
                  
                default:
                    Console.WriteLine("Wrong Option");
                    break;
            }
        }

        public static void StyleOrMoreInfoSelected(int option,List<Beers> beersList)
        {
            JObject jobject = null;
            string href = "";
            int sw = 0;
            int beerID = -1;
            switch (option)
            {
                case 1:
                    // style

                    Console.WriteLine("Please choose beer ID");
                    beerID = Convert.ToInt32(Console.ReadLine());
                    foreach(var item in beersList)
                    {
                        if (item.Id == beerID)
                        {
                            href = item.StyleLink;
                            sw++;
                        }
                    }
                    if(sw == 0)
                    {
                        Console.WriteLine("Beer ID could not be found. Try again");
                   //     StyleOrMoreInfoSelected(1, beersList);
                    }
                    else
                    {
                        jobject = GetJSONFromURL(MAINURL, href);
                        List<Beers> styleBeers = new List<Beers>(); // styles/ID/beers
                        href = jobject["_links"]["beers"]["href"].ToString();
                        jobject = GetJSONFromURL(MAINURL, href);
                        styleBeers = GetStyleFromJSON(jobject);
                        ShowAllBeers(styleBeers, true);
                    }

                    break;
                case 2:
                    // more info
                    Console.WriteLine("Please choose beer ID");
                    beerID = Convert.ToInt32(Console.ReadLine());
                    foreach (var item in beersList)
                    {
                        if (item.Id == beerID)
                        {
                            href = item.SelfLink;
                            sw++;
                        }
                    }
                    if (sw == 0)
                    {
                        Console.WriteLine("Beer ID could not be found. Try again");
                    //    StyleOrMoreInfoSelected(2, beersList);
                    }
                    else
                    {
                        int opt = 0;
                        jobject = GetJSONFromURL(MAINURL, href);
                        List<Beers> moreInfoBeersList = new List<Beers>();
                        moreInfoBeersList = GetReviewBeerOrMoreInfoFromJSON(jobject);
                        ShowAllBeers(moreInfoBeersList,false,false,true);

                        opt = ShowReviewMenu(ref beerID);
                        if (opt == -1)
                            break;
                        else
                            ReviewOptionSelected(opt, beerID, moreInfoBeersList, href);
                    }
                    break;
                default:
                    break;
            }
        }

        public static void ReviewOptionSelected(int option, int chosenBeerID ,List<Beers> beersList, string self_href = "")
        {
            switch( option )
            {
                case 1:
                    string href = "";
                    foreach( var item in beersList)
                    {
                        if (item.Id == chosenBeerID)
                            href = item.ReviewLink;
                    }
                    if (href == "")
                        Console.WriteLine("Selected beer, does not have a review link or chosen beer ID does not exist");
                    else
                    {
                        JObject jobject = null;
                        jobject = GetJSONFromURL(MAINURL, href);
                        List<Beers> reviewBeerList = new List<Beers>();
                        reviewBeerList = GetReviewBeerOrMoreInfoFromJSON(jobject);
                        ShowAllBeers(reviewBeerList,false,true,false);
                    }
                    break;
                case 2:
                    break;
                default:
                    break;
            }
        }

        #endregion


        #region ShowDataToConsole


        public static void ShowAllBreweries(List<Breweries>breweriesList)
        {
            Console.WriteLine("\nBreweriy List");
            Console.WriteLine("ID\t \tName");
            foreach (var item in breweriesList)
            {
                string formatItemString = string.Empty;
                formatItemString += item.Id.ToString() + "\t";
                formatItemString += item.Name.ToString() + " ";
                
                Console.WriteLine(formatItemString);
            }
            Console.WriteLine("\n-------------------------------------------------");
        }

        public static void ShowAllBeers(List<Beers> beersList, bool showStyle = false, bool showReview = false, bool showMoreInfo = false)
        {
            if (showStyle)
                Console.WriteLine("\n\t\t\tStyle For Selected Beer\n\n");
            else
                if (showReview)
                Console.WriteLine("\n\t\t\tReview For Beer: \n\n");
            else
                if (showMoreInfo)
                Console.WriteLine("\n\t\t\tMore info: \n\n");
            else
                Console.WriteLine("\n\t\t\tBeers List\n\n");
            foreach (var item in beersList)
            {
                string formatItemString = string.Empty;
                formatItemString += "Beer ID:        " + item.Id.ToString() + "\n";
                formatItemString += "Beer Name ->    " + item.Name.ToString() + " \n";

                if (item.BreweryId != null)
                    formatItemString += "Brewery ID ->   "+  item.BreweryId.ToString() + " \n";
                if (item.BreweryName != null)
                    formatItemString += "Brewery Name -> " + item.BreweryName.ToString() + " \n";
                if (item.StyleId != null)
                    formatItemString += "Syle ID ->      " + item.StyleId.ToString() + " \n";
                if (item.StyleName != null)
                    formatItemString += "Style Name ->   "+ item.StyleName.ToString() + " \n";
                formatItemString += "--------------------";
                Console.WriteLine(formatItemString);
            }
            Console.WriteLine("\n-------------------------------------------------");
        }

        #endregion 


        #region TransformJSON_ToList

        public static List<Breweries> GetBrieweriesDataFromJSON(JObject mainObj)
        {
            List<Breweries> breweriesList = new List<Breweries>();
            try
            {
                JObject brw = (JObject)mainObj["_embedded"];
                var brwery = brw["brewery"];
                int i = 0;
                while(brwery[i] != null)
                {
                    JObject currentObject = (JObject)brwery[i];
                    Breweries b = new Breweries();
                    b.Id = Convert.ToInt16(currentObject["Id"]);
                    b.Name = currentObject["Name"].ToString();
                    if (currentObject["_links"]["beers"]["href"] != null)
                    {
                        b.Href = currentObject["_links"]["beers"]["href"].ToString();
                    }
                    breweriesList.Add(b);
                    b = null;
                    i++;
                }
            }
            catch (Exception exp){  }
            return breweriesList;
        }

        public static List<Beers> GetBeersDataFromJSON(JObject mainObj)
        {
            List<Beers> beersList = new List<Beers>();
            try
            {
                JObject br = (JObject)mainObj["_embedded"];
                var beers = br["beer"];
                int i = 0;
                while (beers[i] != null)
                {
                    try
                    {
                        JObject currentObj = (JObject)beers[i];
                        Beers b = new Beers();
                        if (currentObj["Id"] != null)
                            b.Id = Convert.ToInt32(currentObj["Id"]);
                        if (currentObj["Name"] != null)
                            b.Name = currentObj["Name"].ToString();
                        if (currentObj["BreweryId"] != null)
                            b.BreweryId = Convert.ToInt32(currentObj["BreweryId"]);
                        if (currentObj["BreweryName"] != null)
                            b.BreweryName = currentObj["BreweryName"].ToString();
                        if (currentObj["StyleId"] != null)
                            b.StyleId = Convert.ToInt32(currentObj["StyleId"]);
                        if (currentObj["StyleName"] != null)
                            b.StyleName = currentObj["StyleName"].ToString();
                        if (currentObj["_links"]["style"]["href"] != null)
                            b.StyleLink = currentObj["_links"]["style"]["href"].ToString();
                        if (currentObj["_links"]["self"]["href"] != null)
                            b.SelfLink = currentObj["_links"]["self"]["href"].ToString();
                        try
                        {
                            // this is because not all have reviews attached
                            if (currentObj["_links"]["review"]["href"] != null)
                                b.ReviewLink = currentObj["_links"]["review"]["href"].ToString();
                        }
                        catch { }
                        beersList.Add(b);
                    }
                    catch { }
                    i++;
                }
            }
            catch { }
            return beersList;
        }

        public static List<Beers> GetReviewBeerOrMoreInfoFromJSON(JObject mainObj)
        {
            List<Beers> beersStyleList = new List<Beers>();
            try
            {
                if (mainObj != null)
                {
                    JObject currentObj = (JObject)mainObj;
                    Beers b = new Beers();
                    if (currentObj["Id"] != null)
                        b.Id = Convert.ToInt32(currentObj["Id"]);
                    if (currentObj["Name"] != null)
                        b.Name = currentObj["Name"].ToString();
                    if (currentObj["BreweryId"] != null)
                        b.BreweryId = Convert.ToInt32(currentObj["BreweryId"]);
                    if (currentObj["BreweryName"] != null)
                        b.BreweryName = currentObj["BreweryName"].ToString();
                    if (currentObj["StyleId"] != null)
                        b.StyleId = Convert.ToInt32(currentObj["StyleId"]);
                    if (currentObj["StyleName"] != null)
                        b.StyleName = currentObj["StyleName"].ToString();
                    if (currentObj["_links"]["style"]["href"] != null)
                        b.StyleLink = currentObj["_links"]["style"]["href"].ToString();
                    beersStyleList.Add(b);
                }
            }
            catch ( Exception exp )
            {
                var a = exp.Message;
            }
            return beersStyleList;
        }

        public static List<Beers> GetStyleFromJSON(JObject mainObj)
        {
            List<Beers> beersStyleList = new List<Beers>();
            var embedded = mainObj["_embedded"]["beer"];
            int i = 0;
            try
            {
                while (embedded[i] != null)
                {
                    try
                    {
                        JObject currentObj = (JObject)embedded[i];
                        Beers b = new Beers();
                        if (currentObj["Id"] != null)
                            b.Id = Convert.ToInt32(currentObj["Id"]);
                        if (currentObj["Name"] != null)
                            b.Name = currentObj["Name"].ToString();
                        if (currentObj["BreweryId"] != null)
                            b.BreweryId = Convert.ToInt32(currentObj["BreweryId"]);
                        if (currentObj["BreweryName"] != null)
                            b.BreweryName = currentObj["BreweryName"].ToString();
                        if (currentObj["StyleId"] != null)
                            b.StyleId = Convert.ToInt32(currentObj["StyleId"]);
                        if (currentObj["StyleName"] != null)
                            b.StyleName = currentObj["StyleName"].ToString();
                        if (currentObj["_links"]["style"]["href"] != null)
                            b.StyleLink = currentObj["_links"]["style"]["href"].ToString();
                        beersStyleList.Add(b);
                    }
                    catch { }
                    i++;
                }
            }
            catch (Exception exp) { }
            return beersStyleList;
        }


        #endregion


        #region GetJSONObjectFromURL

        public static JObject GetJSONFromURL(string url, string href)
        {
            JObject jsonConvertedData = null;
            try
            {

                string newurl = url + href;
                var request = WebRequest.Create(newurl);
                string text = string.Empty;
                request.ContentType = "application/hal+json";
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        text = sr.ReadToEnd();
                    }
                }
                jsonConvertedData = (JObject)JsonConvert.DeserializeObject(text);
              //  Console.WriteLine(jsonConvertedData);
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error on getting beers: \n" + exp.Message);
            }
            return jsonConvertedData;
        }


        #endregion


        #region Post
        

        public static void PostNewBeer(string url, string href)
        {
            string newUrl = url + href;
            Beers b = new Beers();

            Console.WriteLine("Dati ID: ");
            b.Id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Dati Numele: ");
            b.Name = Console.ReadLine();
            StringContent content = new StringContent(JsonConvert.SerializeObject(b));


            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
            var clientResponse = client.PostAsync(newUrl, content).Result;
            Console.WriteLine(clientResponse);
        }


        #endregion

    }
}
