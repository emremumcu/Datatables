// https://quicktype.io/
// https://restcountries.com/v2/all
// https://restcountries.com/v3/all

namespace Datatables.AppData
{
    using Datatables.Models;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading.Tasks;

    public class CountryDB
    {
        private List<Country> countries;

        private async Task ReadCountryData()
        {
            if (System.IO.File.Exists("countries.bin"))
            {
                countries = DeSerializeData<List<Country>>("countries.bin");
            }
            else
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://restcountries.com/v2/all"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        countries = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Country>>(apiResponse);
                    }
                }

                SerializeData<List<Country>>(countries, "countries.bin");
            }
        }

        private void SerializeData<T>(T obj, string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
#pragma warning disable SYSLIB0011
            bf.Serialize(fs, obj);
#pragma warning restore SYSLIB0011
            fs.Close();
        }

        private T DeSerializeData<T>(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
#pragma warning disable SYSLIB0011
            T obj = (T)bf.Deserialize(fs);
#pragma warning restore SYSLIB0011
            fs.Close();
            return obj;
        }

        public async Task<List<Country>> GetCountries()
        {
            if (countries == null) await ReadCountryData();
            return countries;            
        }
    }
}
