using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;

namespace BetterEmulatorSkins
{
    class EmulatorSkin
    {
        static string skinPath = Path.Combine(Helper.ProgramFilesx86(), "Microsoft XDE", "1.0");
        static string skinImagesPath = Path.Combine(skinPath, "Livven");
        static string folder = "BetterEmulatorSkins";

        bool isDefault;
        string folderName;
        string fileName;
        string[] info = new string[3];
        string[] coordinates = new string[2];
        
        public EmulatorSkin()
        {
            isDefault = true;
            Name = "Default";
        }
        public EmulatorSkin(string name)
        {
            Name = name;
            folderName = Path.Combine(folder, Name);
            fileName = name.Replace(' ', '_') + "_Skin_x.png";
            using (StreamReader file = new StreamReader(Path.Combine(folderName, "_Info")))
            {
                for (int i = 0; i < info.Length; i++)
                {
                    info[i] = file.ReadLine();
                    info[i] = info[i].Substring(info[i].IndexOf(": ") + 2);
                }
                coordinates = info[0].Split(',');
            }
        }

        public string Name { get; private set; }
        public string PreviewImage { get { return isDefault ? Path.Combine(skinPath, "WM7_Skin_Up.png") : Path.Combine("pack://siteoforigin:,,,/", UpImageSource); } }

        string UpImageSource { get { return Path.Combine(folderName, "Up.png"); } }
        string DownImageSource { get { return Path.Combine(folderName, "Down.png"); } }
        string MaskImageSource { get { return Path.Combine(folderName, "Mask.png"); } }
        string UpImage { get { return fileName.Replace("_x.", "_Up."); } }
        string DownImage { get { return fileName.Replace("_x.", "_Down."); } }
        string MaskImage { get { return fileName.Replace("_x.", "_Mask."); } }

        int PosX { get { return Convert.ToInt32(coordinates[0]); } }
        int PosY { get { return Convert.ToInt32(coordinates[1]); } }
        public string Author { get { return info[1]; } }
        public string Website { get { return info[2]; } }
        public Visibility CreditsVisible { get { return isDefault ? Visibility.Hidden : Visibility.Visible; } }

        public void Change()
        {
            if (isDefault)
            {
                SetXML(62, 101, "WM7_Skin_Up.png", "WM7_Skin_Down.png", "WM7_Skin_Mask.png", "default");
            }
            else
            {
                CopyImage(UpImageSource, UpImage);
                CopyImage(DownImageSource, DownImage);
                CopyImage(MaskImageSource, MaskImage);
                SetXML(PosX, PosY, Path.Combine("Livven", UpImage), Path.Combine("Livven", DownImage), Path.Combine("Livven", MaskImage), Name);
            }
        }
        static void SetXML(int posX, int posY, string upImage, string downImage, string maskImage, string skin)
        {
            string xmlFile = Path.Combine(skinPath, "WM7_Skin.xml");
            var skinXML = new XmlDocument();
            skinXML.Load(xmlFile);
            var attributes = skinXML.SelectSingleNode("skin/view").Attributes;
            attributes.GetNamedItem("displayPosX").Value = posX.ToString();
            attributes.GetNamedItem("displayPosY").Value = posY.ToString();
            attributes.GetNamedItem("normalImage").Value = upImage;
            attributes.GetNamedItem("downImage").Value = downImage;
            attributes.GetNamedItem("mappingImage").Value = maskImage;
            skinXML.Save(xmlFile);
            App.Current.MainWindow.Title = folder + " (changed to " + skin + ")";
        }
        static void CopyImage(string source, string image)
        {
            if (!Directory.Exists(skinImagesPath))
                Directory.CreateDirectory(skinImagesPath);
            try
            {
                File.Copy(source, Path.Combine(skinImagesPath, image), true);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public static ObservableCollection<EmulatorSkin> GetSkins()
        {
            var skins = new ObservableCollection<EmulatorSkin>();
            skins.Add(new EmulatorSkin());
            string[] skinNames = Directory.GetDirectories(folder);
            for (int i = 0; i < skinNames.Length; i++)
            {
                skins.Add(new EmulatorSkin(Path.GetFileName(skinNames[i])));
            }
            return skins;
        }
    }
}
