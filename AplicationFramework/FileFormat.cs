/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WDToolbox.Data.Text;

namespace WDToolbox.AplicationFramework
{
    /// <summary>
    /// Handles the information associated with a file format.
    /// Formats compare to 0 if the name field is the same
    /// </summary>
    public class FileFormat : IComparable<FileFormat>, IComparable<string>, IEquatable<FileFormat>
    {
        //-----------------------------------------------------------------------------------------------------
        // Predefined Formats
        //-----------------------------------------------------------------------------------------------------
        public static readonly FileFormat Text_ASCII = new FileFormat("text", new string[] { "txt" });

        //images
        #region Images
        public static readonly FileFormat Image_Bitmap = FromString("Windows Bitmap (*.bmp)");
        public static readonly FileFormat Image_DrHalo = FromString("Dr*. Halo (*.cut)");
        public static readonly FileFormat Image_MultiPCX = FromString("Multi-PCX (*.dcx)");
        public static readonly FileFormat Image_Dicom = FromString("Dicom (*.dicom, *.dcm)");
        public static readonly FileFormat Image_DirectDraw = FromString("DirectDraw Surface (*.dds)");
        public static readonly FileFormat Image_OpenEXR = FromString("OpenEXR (*.exr)");
        public static readonly FileFormat Image_fits = FromString("Flexible Image Transport System (*.fits, *.fit)");
        public static readonly FileFormat Image_ftx = FromString("Heavy Metal: FAKK 2 (*.ftx)");
        public static readonly FileFormat Image_hdr = FromString("Radiance High Dynamic (*.hdr)");
        public static readonly FileFormat Image_MacIcon = FromString("Macintosh icon (*.icns)");
        public static readonly FileFormat Image_WinIcon = FromString("Windows icon/cursor (*.ico, *.cur)");
        public static readonly FileFormat Image_IFF = FromString("Interchange File Format (*.iff)");
        public static readonly FileFormat Image_IWI = FromString("Infinity Ward Image (*.iwi)");
        public static readonly FileFormat Image_GIF = FromString("Graphics Interchange Format (*.gif)");
        public static readonly FileFormat Image_Jpeg = FromString("Image_ (*.jpg, *.jpe, *.Image_)");
        public static readonly FileFormat Image_Jpeg2k = FromString("Image_ 2000 (*.jp2, *.jp2k)");
        public static readonly FileFormat Image_LBM = FromString("Interlaced Bitmap (*.lbm)");
        public static readonly FileFormat Image_LIF = FromString("Homeworld texture (*.lif)");
        public static readonly FileFormat Image_MDL = FromString("Half-Life Model (*.mdl)");
        public static readonly FileFormat Image_MP3Art = FromString("MPEG-1 Audio Layer 3 (*.mp3)");
        public static readonly FileFormat Image_Palette = FromString("Palette (*.pal)");
        public static readonly FileFormat Image_PCD = FromString("Kodak PhotoCD (*.pcd)");
        public static readonly FileFormat Image_PCX = FromString("ZSoft PCX (*.pcx)");
        public static readonly FileFormat Image_PIC = FromString("Softimage PIC (*.pic)");
        public static readonly FileFormat Image_PNG = FromString("Portable Network Graphics (*.png)");
        public static readonly FileFormat Image_Anymap = FromString("Portable Anymap (*.pbm, *.pgm, *.pnm, *.pnm)");
        public static readonly FileFormat Image_PIX = FromString("Alias | Wavefront (*.pix)");
        public static readonly FileFormat Image_PhotoShop = FromString("Adobe PhotoShop (*.psd)");
        public static readonly FileFormat Image_PaintShopPro = FromString("PaintShop Pro (*.psp)");
        public static readonly FileFormat Image_Pixar = FromString("Pixar (*.pxr)");
        public static readonly FileFormat Image_RawData = FromString("Raw data (*.raw)");
        public static readonly FileFormat Image_ROT = FromString("Homeworld 2 Texture (*.rot)");
        public static readonly FileFormat Image_SiliconGraphics = FromString("Silicon Graphics (*.sgi, *.bw, *.rgb, *.rgba)");
        public static readonly FileFormat Image_CreativeTexture = FromString("Creative Assembly Texture (*.texture)");
        public static readonly FileFormat Image_Targa = FromString("Truevision Targa (*.tga)");
        public static readonly FileFormat Image_TIFF = FromString("Tagged Image File Format (*.tif, *.tiff)");
        public static readonly FileFormat Image_GamecubeTexture = FromString("Gamecube Texture (*.tpl)");
        public static readonly FileFormat Image_UnrealTexture = FromString("Unreal Texture (*.utx)");
        public static readonly FileFormat Image_Quake2Texture = FromString("Quake 2 Texture (*.wal)");
        public static readonly FileFormat Image_ValveTexture = FromString("Valve Texture Format (*.vtf)");
        public static readonly FileFormat Image_HDPhoto = FromString("HD Photo (*.wdp, *.hdp)");
        public static readonly FileFormat Image_XPixel = FromString("X Pixel Map (*.xpm)");
        #endregion

        #region source code
        public static readonly FileFormat SourceCode_CandCPP = FromString("C Source Files(*.h, *.hpp, *.c, *.cpp)");
        public static readonly FileFormat SourceCode_C_Header = FromString("C Header(*.h)");
        public static readonly FileFormat SourceCode_C_Source = FromString("C Source File(*.c)");
        public static readonly FileFormat SourceCode_CPP_Header = FromString("C++ Header(*.h, *.hpp)");
        public static readonly FileFormat SourceCode_CPP_Source = FromString("C++ Source File(*.cpp)");
        #endregion

        #region plain text
        public static readonly FileFormat Text_TXT = FromString("Text File (*.txt)");
        #endregion

        public static readonly FileFormat[] All_Images = new FileFormat[]
                            {Image_Bitmap, Image_DrHalo, Image_MultiPCX, Image_Dicom, Image_DirectDraw, Image_OpenEXR, 
                             Image_fits, Image_ftx, Image_hdr, Image_MacIcon, Image_WinIcon, Image_IFF, Image_IWI, 
                             Image_GIF, Image_Jpeg, Image_Jpeg2k, Image_LBM, Image_LIF, Image_MDL, Image_MP3Art, 
                             Image_Palette, Image_PCD, Image_PCX, Image_PIC, Image_PNG, Image_Anymap, Image_PIX, 
                             Image_PhotoShop, Image_PaintShopPro, Image_Pixar, Image_RawData, Image_ROT, 
                             Image_SiliconGraphics, Image_CreativeTexture, Image_Targa, Image_TIFF, 
                             Image_GamecubeTexture, Image_UnrealTexture, Image_Quake2Texture, Image_ValveTexture, 
                             Image_HDPhoto, Image_XPixel};

        public static readonly FileFormat[] Common_Images = new FileFormat[] { Image_Bitmap, Image_Jpeg, Image_PNG};

        //tables
        public static readonly FileFormat Table_CSV = FromString("Comma Separated Values (*.csv)");
        public static readonly FileFormat Table_excel = FromString("Excel Spreadsheet (*.xls, *.xlsx)");
        public static readonly FileFormat Table_excelOpen = FromString("Open XML Excel Spreadsheet 2000 (*.xlsx)");
        public static readonly FileFormat[] Common_Tables = new FileFormat[] { Table_CSV, Table_excel };


        //-----------------------------------------------------------------------------------------------------
        // Instance Data
        //-----------------------------------------------------------------------------------------------------
        /// <summary>
        /// Lower case without * or .
        /// </summary>
        readonly string[] extensions;
        readonly string name;

        //-----------------------------------------------------------------------------------------------------
        // Constructors
        //-----------------------------------------------------------------------------------------------------
        /// <summary>
        /// Creat a new file format
        /// </summary>
        /// <param name="name"></param>
        /// <param name="extensions">With or without . and *</param>
        public FileFormat(string name, IList<string> extensions)
        {
            this.extensions = new string[extensions.Count];
            for(int i=0; i<this.extensions.Length; i++)
            {
                this.extensions[i] = formatExtension(extensions[i]);
            }
            this.name = name;
        }

        private string formatExtension(string ext)
        {
            return ext.ToLower().Trim().Trim(".*".ToArray());
        }

        //-----------------------------------------------------------------------------------------------------
        // Code
        //-----------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="desc">example: "Jpeg Image (*.jpg, *.jpeg)"</param>
        /// <returns></returns>
        public static FileFormat FromString(string desc)
        {
            string[] tokens = desc.Split("(".ToCharArray());
            if (tokens.Length == 2)
            {
                string name = tokens[0].Trim();
                string[] formats = tokens[1].Replace(")", "").Split("*.,;".ToCharArray());
                List<string> parsedFormats = new List<string>();
                foreach (string format in formats)
                {
                    string f = format.Trim("-*,. \t;()".ToCharArray()).ToLower();
                    if (!string.IsNullOrWhiteSpace(f))
                    {
                        parsedFormats.Add(f);
                    }
                }

                return new FileFormat(name, parsedFormats);
            }
            return null; //only here on error
        }
    
        public int CompareTo(FileFormat other)
        {
            return (this.name.CompareTo(other.name));
        }

        public int CompareTo(string other)
        {
            return (this.name.CompareTo(other));
        }

        public static string MakeFileDialogeString(IList<FileFormat> formats, bool withAllFiles, string allFormatsHeading)
        {
            StringBuilder sb = new StringBuilder();
            if(allFormatsHeading != null)
            {
                List<string> wildCards = new List<string>();
                foreach(FileFormat format in formats)
                {
                    wildCards.AddRange(format.extensionsAsWildCards());
                }
                sb.Append(String.Format("{0} ({1})|{2}", allFormatsHeading, TextHelper.list(wildCards, ", "), TextHelper.list(wildCards, ";")));
            }
            foreach(FileFormat format in formats)
            {
                sb.Append("|"+format.MakeFileDialogeString());
            }
            if(withAllFiles)
            {
                sb.Append("|All files (*.*)|*.*");
            }
            return sb.ToString().Trim("|".ToCharArray());
        }

        public string MakeFileDialogeString()
        {
            //eg: "Office Files|*.doc;*.xls;*.ppt"
            string[] wildCards = extensionsAsWildCards();
            return String.Format("{0} ({1})|{2}", name, TextHelper.list(wildCards, ", "), TextHelper.list(wildCards, ";"));
        }

        public string[] extensionsAsWildCards()
        {
            string[] eaw = new string[extensions.Length];
            for (int i = 0; i < eaw.Length; i++)
            {
                eaw[i] = "*." + extensions[i].Trim();
            }
            return eaw;
        }

        public  static FileFormat GetFormat(IEnumerable<FileFormat> formants, string name)
        {
            return formants.FirstOrDefault(F => F.name.ToLower().Trim() == name.Trim().ToLower());
        }

        public bool Equals(FileFormat other)
        {
            return this.CompareTo(other) == 0;
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public bool Match(string fileName)
        {
            string test = formatExtension(Path.GetExtension(fileName) ?? "");
            foreach (string ext in this.extensions)
            {
                if (ext.Equals(test))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
