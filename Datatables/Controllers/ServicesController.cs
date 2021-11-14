// https://quicktype.io/
// https://restcountries.com/v2/all
// https://restcountries.com/v3/all

namespace Datatables.Controllers
{
    using Datatables.AppData;
    using Datatables.Models;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    public class ServicesController : Controller
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

        public async Task<IActionResult> CountriesCC()
        {
            if (countries == null) await ReadCountryData();
            var result = new { data = countries };
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Countries(string requestType)
        {
            if(requestType == "ClientSideProcessing")
            {
                if (countries == null) await ReadCountryData();
                var fullResult = new { data = countries };
                return Ok(fullResult);
            }
            else if (requestType == "ServerSideProcessing")
            {
                // Form Values
                var requestedPage = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();                
                var length = Request.Form["length"].FirstOrDefault();
                var sortColIndex = Request.Form["order[0][column]"].FirstOrDefault();
                var sortColName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                var searchRegexp = Request.Form["search[regex]"].FirstOrDefault();
                var search = Request.Form["search"].FirstOrDefault();

                // Skip-Take Calculations
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int fullCount = 0;

                countries = await new CountryDB().GetCountries();
                
                fullCount = countries.Count;


                //Sorting  
                if (!(string.IsNullOrEmpty(sortColName) && string.IsNullOrEmpty(sortDirection)))
                {
                    // order the data using sortColumn and sortColumnDirection
                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    //filter data using searchValue;
                }                

                countries = countries.Skip(skip).Take(pageSize).ToList();

                // https://datatables.net/manual/server-side

                var ordersData = new
                {
                    draw = requestedPage, // cast to integer for security
                    recordsFiltered = fullCount,//Total records, after filtering (i.e. the total number of records after filtering has been applied - not just the number of records being returned for this page of data).
                    recordsTotal = fullCount, //Total records, before filtering (i.e. the total number of records in the database)
                    data = countries,//The data to be displayed in the table. This is an array of data source objects, one for each row, which will be used by DataTables. Note that this parameter's name can be changed using the ajax option's dataSrc property.
                    // error = "" // Optional: If an error occurs during the running of the server-side processing script, you can inform the user of this error by passing back the error message to be displayed using this parameter. Do not include if there is no error.
                };

                return Json(ordersData);
            }
            else
            {
                return BadRequest();
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

    }

    public static class Binary
    {
        public static byte[] ObjectToByteArray(object objData)
        {
            if (objData == null) return default;
            return Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(objData, GetJsonSerializerOptions()));
        }

        public static T ByteArrayToObject<T>(byte[] byteArray)
        {
            if (byteArray == null || !byteArray.Any()) return default;
            return System.Text.Json.JsonSerializer.Deserialize<T>(byteArray, GetJsonSerializerOptions());
        }

        private static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions()
            {
                PropertyNamingPolicy = null,
                WriteIndented = true,
                AllowTrailingCommas = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
        }
    }
}


/*
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var customerData = (from tempcustomer in context.Customers select tempcustomer);
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.FirstName.Contains(searchValue) 
                                                || m.LastName.Contains(searchValue) 
                                                || m.Contact.Contains(searchValue) 
                                                || m.Email.Contains(searchValue) );
                }
                recordsTotal = customerData.Count();
                var data = customerData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
 
 */
/*
 
 <table id="myTable" class="display" style="width:100%">
    <thead>
        <tr>
            <th>Col1</th>
            <th>Col2</th>
            <th>Col3</th>
            <th>Col4</th>
            <th>Col5</th>
            <th>Col6</th>
            <th>Col7</th>
            <th>Col8</th>
        </tr>
    </thead>
</table>


@section scriptSection {

    <script>

    $('#myTable').DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "orderMulti": false,
        paging: true,
        //searching: { regex: true },
        ajax: {
            url: '@Url.Action("ServerSide", "DataTables")',
            type: "POST",
            //contentType: "application/json",
            datatype: "json",
            //data: function (d) {
            //    return JSON.stringify(d);
            //}
            data: {
                ajaxParam: "ajaxParamVal"
            }
        },
        //"columnDefs": [{
        //    "targets": [0],
        //    "visible": false,
        //    "searchable": false
        //}],
        dataSrc: 'data',
        "columns": [
            { "data": "OrderId", "name": "OrderId", "autoWidth": true },
            { "data": "OrderDate", "name": "OrderDate", "autoWidth": true },
            { "data": "ProductName", "name": "ProductName", "autoWidth": true },
            { "data": "Price", "name": "Price", "autoWidth": true },
            { "data": "Country", "name": "Country", "autoWidth": true },
            {
                "render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/DemoGrid/Edit/' + full.CustomerID + '">Edit</a>'; }
            },
            {
                data: null,
                render: function (data, type, row) {
                    return "<a href='#' class='btn btn-danger' onclick=DeleteData('" + row.CustomerID + "'); >Delete</a>";
                }
            },
            {
                "render": function (data, row) { return "<a href='#' class='btn btn-danger' onclick=DeleteCustomer('" + row.id + "'); >Delete</a>"; }
            },
        ]
    });

    </script>

}
 
 */

