using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace SupportWheelOfFate.Core.Generator
{
    public interface IEngineersGenerator
    {
        IImmutableList<(string, string)> Generate(int count = 10);
    }

    public class EngineersGenerator : IEngineersGenerator
    {
        #region Names
        private const char Comma = ',';
        private const string RawFirstNames = "Aiken,Alcott,Alder,Aldrich,Alfred,Allard,Allston,Alton,Alvertos,Alvin,Arledge,Arley,Arlo,Armstrong,Arundel,Ashley,Athelstan,Averill,Awarnach,Ballard,Bancroft,Barclay,Barnett,Beacher,Beardsley,Bede,Beldon,Bentley,Birch,Blake,Booker,Booth,Borden,Bradley,Brandon,Brent,Brewster,Brigham,Brinley,Brock,Bromley,Brook,Buckley,Bud,Burgess,Burne,Afton,Agrona,Aida,Aislinn,Alcott,Alden,Alvina,Arantxa,Ariana,Arleigh,Ashley,Aspen,Audrey,Avon,Bailey,Beverly,Blaine,Blossom,Blythe,Brea,Brenda,Brook,Buffy,Cady,Cameron,Chelsea,Corliss,Courtney,Demelza,Eartha,Edda,Edith,Ethel,Farrah,Fern,Fiona,Godiva,Golda,Halsey,Harmony,Hazel,Hedwig,Hertha,Hollace,Holly,Hope,Idina,Isolda,Ivy,Jocelyn,Pagination";
        private const string RawLastNames = "Earlson,Edgarson,Edmundson,Edselson,Edwardson,Edwinson,Egertonson,Elderson,Eldonson,Eldridgeson,Elmerson,Eltonson,Emersonson,Erskineson,Esmondson,Fairfaxson,Farleyson,Farrellson,Fieldingson,Fordson,Fullerson,Fultonson,Gilfordson,Goldmanson,Gordonson,Gowerson,Grayson,Haddenson,Hadleyson,Hagleyson,Halbertson,Haleyson,Hallson,Hallamson,Halseyson,Hamiltonson,Hannibalson,Hardyson,Harlanson,Harmonson,Harryson,Hastingsson,Hawkson,Hawthorneson,Haydenson,Hayesson,Haywoodson,Hedleyson,Hendrickson,Henleyson,Murphy,Kelly,O'Sullivan,Walsh,Smith,O'Brien,Byrne,Ryan,O'Connor,O'Neill,O'Reilly,Doyle,McCarthy,Gallagher,O'Doherty,Kennedy,Lynch,Murray,Quinn,Moore,McLoughlin,O'Carroll,Connolly,Daly,O'Connell,Wilson,Dunne,Brennan,Burke,Collins,Campbell,Clarke,Johnston,Hughes,O'Farrell,Fitzgerald,Brown,Martin,Maguire,Nolan,Flynn,Thompson,O'Callaghan,O'Donnell,Duffy,O'Mahony,Boyle,Healy,O'Shea,White,Sweeney,Hayes,Kavanagh,Power,McGrath,Moran,Brady,Stewart,Casey,Foley,Fitzpatrick,O'Leary,McDonnell,MacMahon,Donnelly,Regan,Donovan,Burns,Flanagan,Mullan,Barry,Kane,Robinson,Cunningham,Griffin,Kenny,Sheehan,Ward,Whelan,Lyons,Reid,Graham,Higgins,Cullen,Keane,King,Maher,MacKenna,Bell,Scott,Hogan,O'Keeffe,Magee,MacNamara,MacDonald,MacDermott,Molony,O'Rourke,Buckley,O'Dwyer";
        #endregion

        private static readonly Func<string, IImmutableList<string>> GetIListOf = s => s.Split(Comma).ToImmutableList();
        private static readonly Lazy<IImmutableList<string>> FirstNames = new Lazy<IImmutableList<string>>(() => GetIListOf(RawFirstNames));  
        private static readonly Lazy<IImmutableList<string>> LastNames = new Lazy<IImmutableList<string>>(() => GetIListOf(RawLastNames));

        public IImmutableList<(string, string)> Generate(int count = 10)
        {
            if (count < 1 || count > LastNames.Value.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(count), $"Count {count} is out of last names range of 1..{LastNames.Value.Count}");
            }
            
            if (count < 1 || count > FirstNames.Value.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(count), $"Count {count} is out of first names range of 1..{FirstNames.Value.Count}");
            }

            var firstNames = FirstNames.Value.Shuffle().Take(count);
            var lastNames = LastNames.Value.Shuffle().Take(count);
            return firstNames.Zip(lastNames, (x, y) => (x, y)).ToImmutableList();
        }
        
    }
}