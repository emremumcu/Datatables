using System;
using System.Collections.Generic;

namespace Datatables.Models
{
    [Serializable]
    public partial class Country
    {
        public string Name { get; set; }
        public List<string> TopLevelDomain { get; set; }
        public string Alpha2Code { get; set; }
        public string Alpha3Code { get; set; }
        public List<string> CallingCodes { get; set; }
        public string Capital { get; set; }
        public List<string> AltSpellings { get; set; }
        public string Subregion { get; set; }
        public string Region { get; set; }
        public long Population { get; set; }
        public List<string> Latlng { get; set; }
        public string Demonym { get; set; }
        public string Area { get; set; }
        public List<string> Timezones { get; set; }
        public List<string> Borders { get; set; }
        public string NativeName { get; set; }
        public string NumericCode { get; set; }
        public Flags Flags { get; set; }
        public List<Currency> Currencies { get; set; }
        public List<Language> Languages { get; set; }
        public Translations Translations { get; set; }
        public Uri Flag { get; set; }
        public List<RegionalBloc> RegionalBlocs { get; set; }
        public string Cioc { get; set; }
        public bool Independent { get; set; }
    }

    [Serializable]
    public partial class Currency
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
    }

    [Serializable]
    public partial class Flags
    {
        public Uri Svg { get; set; }
        public Uri Png { get; set; }
    }

    [Serializable]
    public partial class Language
    {
        public string Iso6391 { get; set; }
        public string Iso6392 { get; set; }
        public string Name { get; set; }
        public string NativeName { get; set; }
    }

    [Serializable]
    public partial class RegionalBloc
    {
        public string Acronym { get; set; }
        public string Name { get; set; }
    }

    [Serializable]
    public partial class Translations
    {
        public string Br { get; set; }
        public string Pt { get; set; }
        public string Nl { get; set; }
        public string Hr { get; set; }
        public string Fa { get; set; }
        public string De { get; set; }
        public string Es { get; set; }
        public string Fr { get; set; }
        public string Ja { get; set; }
        public string It { get; set; }
        public string Hu { get; set; }
    }
}
