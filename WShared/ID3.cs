using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

using NS_AppConfig;
#if ID3LIB
using ID3Sharp;
#endif

namespace NS_ID3
{
    /***************************************************************************
    SPECIFICATION: 
    CREATED:       09.05.2009
    LAST CHANGE:   09.05.2009
    ***************************************************************************/
    public class ID3
    {
        static private ID3 instance; 

        private static readonly string[] Genres = 
        {
            "Blues",
            "Classic Rock",
            "Country",
            "Dance",
            "Disco",
            "Funk",
            "Grunge",
            "Hip-Hop",
            "Jazz",
            "Metal",
            "New Age",
            "Oldies",
            "Other",
            "Pop",
            "R&B",
            "Rap",
            "Reggae",
            "Rock",
            "Techno",
            "Industrial",
            "Alternative",
            "Ska",
            "Death Metal",
            "Pranks",
            "Soundtrack",
            "Euro-Techno",
            "Ambient",
            "Trip-Hop",
            "Vocal",
            "Jazz+Funk",
            "Fusion",
            "Trance",
            "Classical",
            "Instrumental",
            "Acid",
            "House",
            "Game",
            "Sound Clip",
            "Gospel",
            "Noise",
            "AlternRock",
            "Bass",
            "Soul",
            "Punk",
            "Space",
            "Meditative",
            "Instrumental Pop",
            "Instrumental Rock",
            "Ethnic",
            "Gothic",
            "Darkwave",
            "Techno-Industrial",
            "Electronic",
            "Pop-Folk",
            "Eurodance",
            "Dream",
            "Southern Rock",
            "Comedy",
            "Cult",
            "Gangsta",
            "Top 40",
            "Christian Rap",
            "Pop/Funk",
            "Jungle",
            "Native American",
            "Cabaret",
            "New Wave",
            "Psychadelic",
            "Rave",
            "Showtunes",
            "Trailer",
            "Lo-Fi",
            "Tribal",
            "Acid Punk",
            "Acid Jazz",
            "Polka",
            "Retro",
            "Musical",
            "Rock & Roll",
            "Hard Rock",
            "Folk",
            "Folk-Rock",
            "National Folk",
            "Swing",
            "Fast Fusion",
            "Bebob",
            "Latin",
            "Revival",
            "Celtic",
            "Bluegrass",
            "Avantgarde",
            "Gothic Rock",
            "Progressive Rock",
            "Psychedelic Rock",
            "Symphonic Rock",
            "Slow Rock",
            "Big Band",
            "Chorus",
            "Easy Listening",
            "Acoustic",
            "Humour",
            "Speech",
            "Chanson",
            "Opera",
            "Chamber Music",
            "Sonata",
            "Symphony",
            "Booty Bass",
            "Primus",
            "Porn Groove",
            "Satire",
            "Slow Jam",
            "Club",
            "Tango",
            "Samba",
            "Folklore",
            "Ballad",
            "Power Ballad",
            "Rhythmic Soul",
            "Freestyle",
            "Duet",
            "Punk Rock",
            "Drum Solo",
            "A capella",
            "Euro-House",
            "Dance Hall"
        };

        public enum TagKind
        {
            TK_TITLE,
            TK_ARTIST,
            TK_ALBUM,
            TK_YEAR,
            TK_COMMENT,
            TK_TRACKNR,
            TK_GENRE,
            TK_NROF
        };

        public class TAG
        {
            public TAG(bool sl, string nm, string tg)
            {
                Selected = sl;
                Name     = nm;
                Tag      = tg;
                ColWith  = 80;
            }
            public bool     Selected;
            public string   Name;
            public string   Tag;
            public int      ColWith;
        };

        public static TAG[] TagMask;

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       09.05.2009
        LAST CHANGE:   09.05.2009
        ***************************************************************************/
        private ID3()
        {
            TagMask = new TAG[(int)TagKind.TK_NROF]
            {
                new TAG( true , "Title",   "TIT2" ),
                new TAG( false, "Artist",  "TPE1" ),
                new TAG( true , "Album",   "TALB" ),
                new TAG( false, "Year",    "TYER" ),
                new TAG( false, "Comment", "COMM" ),
                new TAG( true , "Track No","TRCK" ),
                new TAG( false, "Genre",   "TCON" )
            };
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       11.05.2009
        LAST CHANGE:   11.05.2009
        ***************************************************************************/
        public static void Serialize( ref AppSettings a_rConf )
        {
            if( a_rConf.IsReading )
            {
                foreach( TAG tg in TagMask )
                {
                    tg.Selected = (bool)a_rConf.Deserialize();
                    tg.ColWith  = (int) a_rConf.Deserialize();
                }
            }
            else
            {
                foreach( TAG tg in TagMask )
                {
                    a_rConf.Serialize(tg.Selected);
                    a_rConf.Serialize(tg.ColWith);
                }
            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       10.05.2009
        LAST CHANGE:   10.05.2009
        ***************************************************************************/
        public static ID3 GetInstance()
        {
            if (instance == null)
            {
                instance = new ID3();
            }

            return instance;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       10.05.2009
        LAST CHANGE:   10.05.2009
        ***************************************************************************/
        public static void SelectAll(bool a_Sel)
        {
            foreach(TAG tg in TagMask)
            {
                tg.Selected = a_Sel;
            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       08.05.2009
        LAST CHANGE:   18.05.2009
        ***************************************************************************/
        static public string[] ReadID3Tags( string a_Fname )
        {
            List<string> ret = new List<string>();

            try
            {
                const int ID3_BLOCKSZ = 128;
                FileStream rd = new FileStream( a_Fname, FileMode.Open, FileAccess.Read );

                long pos = rd.Length - (long)ID3_BLOCKSZ;
                byte[] barr = new byte[ID3_BLOCKSZ];

                if ( pos < 0 )
                {
                    for (int i=0; i<(int)TagKind.TK_NROF; i++)  ret.Add("-");
                    return ret.ToArray();
                }

                rd.Position = pos;

                int cnt = rd.Read( barr, 0, ID3_BLOCKSZ );  // ID3_BLOCKSZ );

                int idx = 0;
                CutString( barr, ref idx, 3 );              // TAG has to be skipped
                ret.Add( CutString( barr, ref idx, 30 ) );  //  TITLE
                ret.Add( CutString( barr, ref idx, 30 ) );  //  ARTIST
                ret.Add( CutString( barr, ref idx, 30 ) );  //  ALBUM
                ret.Add( CutString( barr, ref idx, 4 ) );   //  YEAR
                ret.Add( CutString( barr, ref idx, 29 ) );  //  COMMENT
                                                              
                ret.Add( barr[idx++].ToString("000") );     // Track No
                
                int genre = barr[idx];

                if ( genre < Genres.Length )  ret.Add( Genres[genre] );
                else                          ret.Add( "" );

                for( int i = 0; i < ret.Count - 2; i++ )
                {
                    for( int j = 0; j < ret[i].Length; j++ )
                    {
                        char c = ret[i][j];
                        if( c < ' ' || c > '~' )
                        {
                            ret[i] = ret[i].Remove( j );
                            break;
                        }
                    }
                }

                rd.Close();
            }
            catch( Exception ex )
            {
                MessageBox.Show( ex.Message, "Error in reading ID3 Tags" );
            }

            return ret.ToArray();
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       18.05.2009
        LAST CHANGE:   18.05.2009
        ***************************************************************************/
        #if ID3LIB
        static public string[] ReadID3TagsLib( string a_Fname )
        {
            List<string> ret = new List<string>();

            try
            {
                FileStream rd = new FileStream( a_Fname, FileMode.Open, FileAccess.Read );

                ID3Tag id3Tag = null;

                if( ID3v2Tag.HasTag( rd ) )
                {
                    id3Tag = new ID3v2Tag();
                    id3Tag = ID3v2Tag.ReadTag( rd );
                }
                else if( ID3v1Tag.HasTag( rd ) )
                {
                    id3Tag = new ID3v1Tag();
                    id3Tag = ID3v1Tag.ReadTag( rd );
                }

                if( null != id3Tag )
                {
                    ret.Add( id3Tag.Title );
                    ret.Add( id3Tag.Artist );
                    ret.Add( id3Tag.Album );
                    ret.Add( id3Tag.Year );
                    ret.Add( id3Tag.Comments );
                    int tn = id3Tag.TrackNumber; 
                    if (tn < 0) ret.Add( "---" );
                    else        ret.Add( tn.ToString( "000" ) );
                    ret.Add( id3Tag.Genre );
                }

                rd.Close();
            }
            catch( Exception )
            {
                ret.Clear();
                for (int i=0; i<(int)TagKind.TK_NROF; i++) ret.Add("-");
            }

            for(int i=0; i<ret.Count; i++)
            {
                if (ret[i]==null) ret[i]="---";
            }
            return ret.ToArray();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       24.05.2009
        LAST CHANGE:   24.05.2009
        ***************************************************************************/
        static public void WriteID3Tag( string a_FName, TagKind a_TgKind, string a_Tag )
        {
            try
            {
                FileStream rd = new FileStream( a_FName, FileMode.Open, FileAccess.ReadWrite );

                ID3Tag      id3Tag  = null;
                ID3Versions id3Vers = ID3Versions.None;

                if( ID3v2Tag.HasTag( rd ) )
                {
                    id3Tag  = new ID3v2Tag();
                    id3Tag  = ID3v2Tag.ReadTag( rd );
                    id3Vers = ID3Versions.V2_4;
                }
                else if( ID3v1Tag.HasTag( rd ) )
                {
                    id3Tag  = new ID3v1Tag();
                    id3Tag  = ID3v1Tag.ReadTag( rd );
                    id3Vers = ID3Versions.V1_1;
                }

                if( null != id3Tag )
                {
                    switch( a_TgKind )
                    {
                        case TagKind.TK_TITLE:   id3Tag.Title       = a_Tag; break;
                        case TagKind.TK_ARTIST:  id3Tag.Artist      = a_Tag; break;
                        case TagKind.TK_ALBUM:   id3Tag.Album       = a_Tag; break;
                        case TagKind.TK_YEAR:    id3Tag.Year        = a_Tag; break;
                        case TagKind.TK_COMMENT: id3Tag.Comments    = a_Tag; break;
                        case TagKind.TK_TRACKNR: id3Tag.TrackNumber = int.Parse(a_Tag); break;
                        case TagKind.TK_GENRE:   id3Tag.Genre       = a_Tag; break;
                    }

                    id3Tag.WriteTag(rd,id3Vers);
                }

                rd.Close();
            }
            catch( Exception e)
            {
                MessageBox.Show(e.Message,"Error in writing ID3 Tag");
            }

        }

        #endif

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       09.05.2009
        LAST CHANGE:   09.05.2009
        ***************************************************************************/
        static private string CutString( byte[] barr, ref int idx, int cnt )
        {
            char[] cut = new char[cnt];

            Array.Copy( barr, idx, cut, 0, cnt );

            idx += cnt;

            return new string( cut );
        }
    }
}
